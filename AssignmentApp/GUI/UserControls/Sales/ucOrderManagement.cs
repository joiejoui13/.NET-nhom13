using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.BLL.Services.Warehouse;
using AssignmentApp.BLL.Services.Admin;
using Microsoft.Extensions.DependencyInjection;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IPromotionService _promoService;

        private List<Order> _orders = new List<Order>();
        private Order _selectedOrder = null;
        private bool isEditing = false;
        private bool isAddingNew = false;
        private bool isSearchMode = false;
        private bool isPOSSearchMode = false;
        private List<OrderDetail> currentDetails = new List<OrderDetail>();
        private bool isCartModified = false;

        private bool defaultToPOS = false;

        public ucOrderManagement(bool defaultToPOS = false)
        {
            InitializeComponent();
            _orderService = Program.ServiceProvider.GetRequiredService<IOrderService>();
            _productService = Program.ServiceProvider.GetRequiredService<IProductService>();
            _customerService = Program.ServiceProvider.GetRequiredService<ICustomerService>();
            _promoService = Program.ServiceProvider.GetRequiredService<IPromotionService>();
            this.defaultToPOS = defaultToPOS;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
            
            // Add DoubleClick event for dgvOrders
            dgvOrders.CellDoubleClick += dgvOrders_CellDoubleClick;
            dgvProductsSelection.CellDoubleClick += dgvProductsSelection_CellDoubleClick;
            txtGiamGia.TextChanged += async (s, e) => await UpdateTotalAmountAsync();
            txtGiamGia.TextChanged += async (s, e) => await UpdateTotalAmountAsync();
            txtNgayTao.ValueChanged += txtNgayTao_ValueChanged;
            tabMain.Selecting += tabMain_Selecting;
            guna2Button4.Click += guna2Button4_Click;
            guna2Button3.Click += guna2Button3_Click;
            btnAddToCart.Click += btnAddToCart_Click;
            btnRemoveFromCart.Click += btnRemoveFromCart_Click;
            btnBackToReceipt.Click += btnBackToReceipt_Click;
            if (btnXuatbaocao != null) btnXuatbaocao.Click += btnXuatbaocao_Click;
        }


        private void tabMain_Selecting(object? sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabChonSanPham)
            {
                if (_selectedOrder == null && !isAddingNew)
                {
                    MessageBox.Show("Vui lòng chọn một đơn hàng hoặc ấn Thêm mới để thao tác chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void SetCartButtonsState(string state, bool isModified = false)
        {
            btnAddToCart.Enabled = false;
            guna2Button4.Enabled = false; // Sửa
            btnRemoveFromCart.Enabled = false; // Xóa
            guna2Button3.Enabled = false; // Bỏ qua
            btnBackToReceipt.Enabled = false; // Lưu thay đổi

            // Đóng tất cả textbox mặc định
            txtSelMaSP.Enabled = false;
            txtSelTenSP.Enabled = false;
            txtSelGiaNhap.Enabled = false;
            txtSelSoLuong.Enabled = false;
            txtSelMaDanhMuc.Enabled = false;
            txtSelTenDanhMuc.Enabled = false;

            // Kiểm tra trạng thái đơn hàng (đóng các nút nếu không phải Chờ xử lý và không phải tạo mới)
            bool isReadOnly = _selectedOrder != null && _selectedOrder.TrangThai != "Chờ xử lý" && !isAddingNew;
            if (isReadOnly)
            {
                return;
            }

            // Nếu đơn hàng có thể sửa, nút Lưu/Trở về luôn hiển thị để quay lại hoặc lưu lại thay đổi (nếu có)
            btnBackToReceipt.Enabled = true;

            if (state == "SelectingAvailable")
            {
                btnAddToCart.Enabled = true;
                guna2Button3.Enabled = true;
                txtSelSoLuong.Enabled = true; // Chỉ mở số lượng
            }
            else if (state == "SelectingCart")
            {
                guna2Button4.Enabled = true;
                btnRemoveFromCart.Enabled = true;
                guna2Button3.Enabled = true;
                txtSelSoLuong.Enabled = true; // Chỉ mở số lượng
            }
        }

        private void txtNgayTao_ValueChanged(object? sender, EventArgs e)
        {
            if (txtNgayTao.Format == DateTimePickerFormat.Custom && txtNgayTao.CustomFormat == " ")
            {
                txtNgayTao.Format = DateTimePickerFormat.Short;
                txtNgayTao.CustomFormat = null;
            }
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và phím điều khiển (như Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ucOrderManagement_Load(object sender, EventArgs e)
        {
            // Wire up real-time validation events
            if (txtTongTien != null) txtTongTien.KeyPress += NumericOnly_KeyPress;
            if (txtGiamGia != null) txtGiamGia.KeyPress += NumericOnly_KeyPress;
            if (txtSelSoLuong != null) txtSelSoLuong.KeyPress += NumericOnly_KeyPress;
            if (txtSelGiaNhap != null) txtSelGiaNhap.KeyPress += NumericOnly_KeyPress;

            SetControlState("Init");
            _ = LoadOrdersGridAsync();
            _ = LoadProductsSelectionGridAsync();
            SetCartButtonsState("Init");

            if (defaultToPOS && tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadCurrentDetailsGrid();
            }
        }

        private void SetControlState(string mode)
        {
            // Modes: "Init", "View", "Add", "Edit", "Search"
            
            bool isTextBoxReadonly = true;
            
            if (mode == "Init")
            {
                ClearInputs();
                isTextBoxReadonly = true;
                
                btnAdd.Enabled = true;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                isSearchMode = false;
                isAddingNew = false;
                isEditing = false;
            }
            else if (mode == "View")
            {
                isTextBoxReadonly = true;
                
                btnAdd.Enabled = false;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = true;
                isSearchMode = false;
                isAddingNew = false;
                isEditing = false;
            }
            else if (mode == "Add" || mode == "Edit")
            {
                isTextBoxReadonly = false;
                
                btnAdd.Enabled = false;
                btnSearch.Enabled = false;
                btnRefresh.Enabled = true; // Can still click refresh to reset
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                
                isAddingNew = (mode == "Add");
                isEditing = true;
                isSearchMode = false;
            }
            else if (mode == "Search")
            {
                ClearInputs();
                isTextBoxReadonly = false;
                
                isSearchMode = true;
                isAddingNew = false;
                isEditing = false;
                txtNgayTao.Format = DateTimePickerFormat.Custom;
                txtNgayTao.CustomFormat = " ";
            }

            if (mode == "Search")
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }

            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            if (btnXuatbaocao != null) btnXuatbaocao.Visible = (mode == "View" && _selectedOrder != null);

            txtMaKhachHang.Enabled = !isTextBoxReadonly;
            txtTenNguoiDung.Enabled = !isTextBoxReadonly;
            cboHinhThucThanhToan.Enabled = !isTextBoxReadonly;
            txtNgayTao.Enabled = !isTextBoxReadonly;
            txtGiamGia.Enabled = !isTextBoxReadonly;
            txtTongTien.Enabled = !isTextBoxReadonly;
            
            cboLoaiHoaDon.Enabled = !isTextBoxReadonly;
            cboTrangThai.Enabled = !isTextBoxReadonly;

            // Mã hóa đơn always unclickable except in Search mode
            if (mode == "Search")
                txtMaHoaDon.Enabled = true;
            else
                txtMaHoaDon.Enabled = false;


            // Lý do hủy
            txtLyDoHuy.Enabled = !isTextBoxReadonly && cboTrangThai.Text == "Đã huỷ";
            
            UpdateConvertToSalesState();
        }

        private async Task LoadOrdersGridAsync()
        {
            dgvOrders.Rows.Clear();
            _orders = (await _orderService.GetAllOrdersAsync()).ToList();
            
            foreach (var order in _orders)
            {
                dgvOrders.Rows.Add(
                    order.MaHoaDon,
                    order.MaKhachHang, 
                    order.MaNguoiDung,
                    order.TongTien.ToString("N0") + " đ",
                    order.MaKhuyenMai ?? "",
                    order.HinhThucThanhToan,
                    order.TrangThai,
                    order.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    order.LoaiHoaDon,
                    order.LyDoHuy
                );
            }
        }

        private void SelectOrderRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvOrders.Rows.Count) return;

            dgvOrders.ClearSelection();
            dgvOrders.Rows[rowIndex].Selected = true;

            string orderId = dgvOrders.Rows[rowIndex].Cells[0].Value?.ToString() ?? "";
            _selectedOrder = _orders.FirstOrDefault(o => o.MaHoaDon == orderId);

            if (_selectedOrder != null)
            {
                _ = PopulateOrderDetailsAsync(_selectedOrder);
                SetControlState("View");

                if (_selectedOrder.TrangThai != "Đã hoàn thành" && _selectedOrder.TrangThai != "Đã huỷ")
                {
                    cboTrangThai.Enabled = true;
                    cboHinhThucThanhToan.Enabled = true;
                    txtGiamGia.Enabled = true;
                    txtMaKhachHang.Enabled = true;
                    cboLoaiHoaDon.Enabled = true;
                }
            }
        }

        private async Task PopulateOrderDetailsAsync(Order order)
        {
            txtMaHoaDon.Text = order.MaHoaDon.ToString();
            cboLoaiHoaDon.Text = order.LoaiHoaDon;
            txtMaKhachHang.Text = order.MaKhachHang.ToString();
            txtTenNguoiDung.Text = order.MaNguoiDung?.ToString();
            txtNgayTao.Format = DateTimePickerFormat.Short;
            txtNgayTao.CustomFormat = null;
            txtNgayTao.Value = order.NgayTao;
            txtGiamGia.Text = order.MaKhuyenMai ?? "";
            cboHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtTongTien.Text = order.TongTien.ToString("N0") + " đ";
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = order.LyDoHuy;

            var details = await _orderService.GetOrderDetailsAsync(order.MaHoaDon.ToString());
            currentDetails = details.ToList();
            
            lblPOSTitle.Text = $"MÃ HÓA ĐƠN: {order.MaHoaDon}";
            LoadCurrentDetailsGrid();

            UpdateConvertToSalesState();

            // Reset tab 2 và cập nhật trạng thái các nút
            btnResetCartForm_Click(null, EventArgs.Empty);
            SetCartButtonsState("Init", false);
        }

        private void UpdateConvertToSalesState()
        {
            // Placeholder for UI Logic
        }

        private void cboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboTrangThai.Enabled)
            {
                bool isCanceled = cboTrangThai.Text == "Đã huỷ";
                txtLyDoHuy.Enabled = isCanceled;
                if (!isCanceled)
                {
                    txtLyDoHuy.Text = "";
                }
                else
                {
                    txtLyDoHuy.Focus();
                }
            }
        }

        private void dgvOrders_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (isAddingNew || isEditing)
                {
                    MessageBox.Show("Vui lòng Bỏ qua hoặc Lưu lại trước khi chọn dữ liệu khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!isSearchMode)
                {
                    SelectOrderRow(e.RowIndex);
                }
            }
        }

        private void dgvOrders_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (isAddingNew || isEditing)
                {
                    MessageBox.Show("Vui lòng Bỏ qua hoặc Lưu lại trước khi chọn dữ liệu khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!isSearchMode)
                {
                    SelectOrderRow(e.RowIndex);
                    if (tabMain != null && tabChonSanPham != null)
                    {
                        tabMain.SelectedTab = tabChonSanPham;
                        _ = LoadProductsSelectionGridAsync();
                        LoadCurrentDetailsGrid();
                    }
                }
            }
        }

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            if (!isSearchMode)
            {
                SetControlState("Search");
                MessageBox.Show("Đã bật chế độ Tìm kiếm!\n\n1. Vui lòng gõ thông tin cần tìm vào các ô trống (VD: Mã Hóa Đơn, Trạng Thái...).\n2. Nhấn nút TÌM KIẾM một lần nữa để lọc dữ liệu.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text) && 
                    string.IsNullOrWhiteSpace(txtMaKhachHang.Text) &&
                    string.IsNullOrWhiteSpace(txtTenNguoiDung.Text) &&
                    string.IsNullOrWhiteSpace(txtTongTien.Text) &&
                    string.IsNullOrWhiteSpace(txtLyDoHuy.Text) &&
                    string.IsNullOrWhiteSpace(cboTrangThai.Text) &&
                    string.IsNullOrWhiteSpace(cboLoaiHoaDon.Text) &&
                    string.IsNullOrWhiteSpace(txtGiamGia.Text) &&
                    string.IsNullOrWhiteSpace(cboHinhThucThanhToan.Text) &&
                    (txtNgayTao.Format == DateTimePickerFormat.Custom && txtNgayTao.CustomFormat == " "))
                {
                    MessageBox.Show("Vui lòng điền thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Perform Search
                var list = await _orderService.GetAllOrdersAsync();
                
                if (!string.IsNullOrEmpty(txtMaHoaDon.Text))
                    list = list.Where(o => o.MaHoaDon.Contains(txtMaHoaDon.Text, StringComparison.OrdinalIgnoreCase));
                
                if (!string.IsNullOrEmpty(txtMaKhachHang.Text))
                    list = list.Where(o => o.MaKhachHang != null && o.MaKhachHang.Contains(txtMaKhachHang.Text, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(txtGiamGia.Text))
                    list = list.Where(o => o.MaKhuyenMai != null && o.MaKhuyenMai.Contains(txtGiamGia.Text, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(cboHinhThucThanhToan.Text))
                    list = list.Where(o => o.HinhThucThanhToan != null && o.HinhThucThanhToan.Contains(cboHinhThucThanhToan.Text, StringComparison.OrdinalIgnoreCase));
                
                if (!string.IsNullOrEmpty(cboTrangThai.Text))
                    list = list.Where(o => o.TrangThai == cboTrangThai.Text);
                
                if (!string.IsNullOrEmpty(txtTenNguoiDung.Text))
                    list = list.Where(o => o.MaNguoiDung.Contains(txtTenNguoiDung.Text, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(txtTongTien.Text))
                {
                    string amtStr = txtTongTien.Text.Replace(" đ", "").Replace(",", "");
                    if (decimal.TryParse(amtStr, out decimal tongTien))
                    {
                        list = list.Where(o => o.TongTien == tongTien);
                    }
                }

                if (!string.IsNullOrEmpty(txtLyDoHuy.Text))
                    list = list.Where(o => o.LyDoHuy != null && o.LyDoHuy.Contains(txtLyDoHuy.Text, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(cboLoaiHoaDon.Text))
                    list = list.Where(o => o.LoaiHoaDon == cboLoaiHoaDon.Text);

                if (!(txtNgayTao.Format == DateTimePickerFormat.Custom && txtNgayTao.CustomFormat == " "))
                    list = list.Where(o => o.NgayTao.Date == txtNgayTao.Value.Date);

                _orders = list.ToList();
                dgvOrders.Rows.Clear();
                foreach (var order in _orders)
                {
                    dgvOrders.Rows.Add(
                        order.MaHoaDon,
                        order.MaKhachHang,
                        order.MaNguoiDung,
                        order.TongTien.ToString("N0") + " đ",
                        order.MaKhuyenMai ?? "",
                        order.HinhThucThanhToan,
                        order.TrangThai,
                        order.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                        order.LoaiHoaDon,
                        order.LyDoHuy
                    );
                }
                
                MessageBox.Show($"Tìm thấy {_orders.Count} kết quả.", "Kết quả Tìm kiếm");
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            SetControlState("Init");
            _ = LoadOrdersGridAsync();
        }

        private void ClearInputs()
        {
            txtMaHoaDon.Text = "";
            txtMaKhachHang.Text = "";
            txtTenNguoiDung.Text = "";
            txtTongTien.Text = "";
            txtGiamGia.Text = "";
            cboHinhThucThanhToan.SelectedIndex = -1;
            txtNgayTao.Format = DateTimePickerFormat.Short;
            txtNgayTao.CustomFormat = null;
            txtNgayTao.Value = DateTime.Now;
            cboLoaiHoaDon.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            txtLyDoHuy.Text = "";
            lblPOSTitle.Text = "MÃ HÓA ĐƠN: ";
            _selectedOrder = null;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            SetControlState("Add");
            
            txtMaHoaDon.Text = "Tự động sinh";
            txtMaKhachHang.Text = ""; 
            txtTenNguoiDung.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser?.MaNguoiDung.ToString() ?? "1"; 
            txtNgayTao.Format = DateTimePickerFormat.Short;
            txtNgayTao.CustomFormat = null;
            txtNgayTao.Value = DateTime.Now;
            cboLoaiHoaDon.Text = "Đơn bán hàng";
            cboTrangThai.Text = "Chờ xử lý";
            txtLyDoHuy.Text = "";
            txtTongTien.Text = "0 đ";
            txtGiamGia.Text = "";
            cboHinhThucThanhToan.Text = "Tiền mặt";
            
            txtNgayTao.Enabled = false;
            txtTenNguoiDung.Enabled = false;
            
            currentDetails.Clear();
            LoadCurrentDetailsGrid();
            lblPOSTitle.Text = "MÃ HÓA ĐƠN: (Đang tạo mới)";
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedOrder.TrangThai == "Đã hoàn thành" || _selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Không thể chỉnh sửa đơn hàng đã hoàn thành hoặc đã huỷ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaKhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtMaKhachHang.Text, out int maKH))
            {
                MessageBox.Show("Mã khách hàng phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var customer = await _customerService.GetCustomerByIdAsync(maKH);
            if (customer == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtGiamGia.Text))
            {
                if (!int.TryParse(txtGiamGia.Text, out int promoId))
                {
                    MessageBox.Show("Mã khuyến mãi phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                var promo = await _promoService.GetPromotionByIdAsync(promoId);
                if (promo == null)
                {
                    MessageBox.Show("Mã khuyến mãi không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (cboTrangThai.Text == "Đã huỷ" && string.IsNullOrWhiteSpace(txtLyDoHuy.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDoHuy.Focus();
                return;
            }

            string oldStatus = _selectedOrder.TrangThai;
            _selectedOrder.TrangThai = cboTrangThai.Text;
            _selectedOrder.LoaiHoaDon = cboLoaiHoaDon.Text;
            _selectedOrder.HinhThucThanhToan = cboHinhThucThanhToan.Text;
            _selectedOrder.MaKhachHang = txtMaKhachHang.Text;
            _selectedOrder.MaKhuyenMai = string.IsNullOrEmpty(txtGiamGia.Text) ? null : txtGiamGia.Text;
            _selectedOrder.LyDoHuy = cboTrangThai.Text == "Đã huỷ" ? txtLyDoHuy.Text : "";
            _selectedOrder.TongTien = decimal.Parse(txtTongTien.Text.Replace(" đ", "").Replace(",", ""));

            await _orderService.UpdateOrderStatusAsync(_selectedOrder, oldStatus);

            MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetControlState("Init");
            _ = LoadOrdersGridAsync();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedOrder == null) return;
            
            if (_selectedOrder.TrangThai == "Đã hoàn thành" || _selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Không thể hủy đơn hàng đã hoàn thành hoặc đã bị hủy từ trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            cboTrangThai.Text = "Đã huỷ";
            txtLyDoHuy.Enabled = true;
            txtLyDoHuy.Focus();
            MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng vào ô trống, sau đó nhấn SỬA để lưu lại!", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (isAddingNew)
            {
                var newOrder = new Order
                {
                    MaKhachHang = txtMaKhachHang.Text,
                    MaNguoiDung = txtTenNguoiDung.Text,
                    TongTien = decimal.Parse(txtTongTien.Text.Replace(" đ", "").Replace(",", "")),
                    MaKhuyenMai = string.IsNullOrEmpty(txtGiamGia.Text) ? null : txtGiamGia.Text,
                    GiamGia = 0,
                    HinhThucThanhToan = cboHinhThucThanhToan.Text,
                    TrangThai = cboTrangThai.Text,
                    LoaiHoaDon = cboLoaiHoaDon.Text,
                    LyDoHuy = "",
                    NgayTao = DateTime.Now
                };

                await _orderService.CreateOrderAsync(newOrder, currentDetails);
                MessageBox.Show("Tạo đơn hàng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            SetControlState("Init");
            _ = LoadOrdersGridAsync();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            SetControlState("Init");
            _ = LoadOrdersGridAsync();
        }

        private void btnConvertToInvoice_Click(object? sender, EventArgs e)
        {
            if (_selectedOrder == null) return;

            if (_selectedOrder.LoaiHoaDon != "Đơn đặt hàng")
            {
                MessageBox.Show("Chỉ có thể chuyển đổi Đơn đặt hàng thành Đơn bán hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Xác nhận chuyển Đơn đặt hàng #{_selectedOrder.MaHoaDon} thành Đơn bán hàng (Hóa đơn)?\nThao tác này sẽ cập nhật tồn kho và tạo giao dịch bán hàng tương ứng.", "Xác nhận chuyển đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                _selectedOrder.LoaiHoaDon = "Đơn bán hàng";
                _selectedOrder.TrangThai = "Đã hoàn thành"; 
                
                // _ = _orderService.ConvertToInvoiceAsync(_selectedOrder);

                MessageBox.Show("Đã chuyển đổi Đơn đặt hàng thành Đơn bán hàng thành công!\nTồn kho tương ứng đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                SetControlState("Init");
                _ = LoadOrdersGridAsync();
            }
        }

        // ========================================================
        // TAB 2 EVENTS (PRODUCT SELECTION / POS)
        // ========================================================

        private async Task LoadProductsSelectionGridAsync(List<Product>? dataSource = null)
        {
            dgvProductsSelection.Rows.Clear();

            var list = dataSource ?? (await _productService.GetAllProductsAsync()).ToList();
            foreach (var prod in list)
            {
                dgvProductsSelection.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.GiaBan.ToString("N0") + " đ",
                    prod.MaDanhMuc,
                    prod.SoLuongTon,
                    prod.TrangThai
                );
            }
        }

        private void txtProductSearch_TextChanged(object? sender, EventArgs e)
        {
            // Do not search automatically, user wants to use btnPOSSearch
        }

        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvProductsSelection.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(rawId))
                {
                    _ = PopulateProductToPOSAsync(rawId);
                    tabSelectionContainer.SelectedTab = tabProductDetail;
                    isPOSSearchMode = false;
                    SetCartButtonsState("SelectingAvailable", isCartModified);
                }
            }
        }

        private void dgvProductsSelection_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tabSelectionContainer.SelectedTab = tabProductDetail;
            }
        }

        private async Task PopulateProductToPOSAsync(string id)
        {
            if (int.TryParse(id, out int productId))
            {
                var prod = await _productService.GetProductByIdAsync(productId);
                if (prod != null)
            {
                txtSelMaSP.Text = prod.MaSanPham.ToString();
                txtSelTenSP.Text = prod.TenSanPham;
                txtSelGiaNhap.Text = prod.GiaBan.ToString();
                txtSelMaDanhMuc.Text = prod.MaDanhMuc.ToString();
                txtSelTenDanhMuc.Text = prod.TenDanhMuc ?? "";

                lblProductDetailDesc.Text = $"Mã SP: {prod.MaSanPham}\n" +
                                            $"Tên SP: {prod.TenSanPham}\n" +
                                            $"Danh mục: {prod.MaDanhMuc}\n" +
                                            $"Giá bán: {prod.GiaBan:N0} đ\n" +
                                            $"Số lượng tồn: {prod.SoLuongTon}\n" +
                                            $"Trạng thái: {prod.TrangThai}\n" +
                                            $"Ngày tạo: {prod.NgayTao:dd/MM/yyyy}\n\n" +
                                            $"Mô tả:\n{prod.MoTa}";

                var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                if (existing != null)
                {
                    txtSelSoLuong.Text = existing.SoLuong.ToString();
                    txtSelGiaNhap.Text = existing.DonGia.ToString();
                }
                else
                {
                    txtSelSoLuong.Text = "1";
                }

                txtSelSoLuong.Focus();
                txtSelSoLuong.SelectAll();
                
                if (!string.IsNullOrEmpty(prod.Anh) && File.Exists(prod.Anh))
                {
                    try
                    {
                        byte[] bytes = File.ReadAllBytes(prod.Anh);
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            if (picProductDetail.Image != null) picProductDetail.Image.Dispose();
                            picProductDetail.Image = Image.FromStream(ms);
                        }
                    }
                    catch { picProductDetail.Image = null; }
                }
                else
                {
                    if (picProductDetail.Image != null) picProductDetail.Image.Dispose();
                    picProductDetail.Image = null;
                }
            }
        }
    }

        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(rawId))
                {
                    _ = PopulateProductToPOSAsync(rawId);
                    tabSelectionContainer.SelectedTab = tabProductDetail;
                    SetCartButtonsState("SelectingCart", isCartModified);
                }
            }
        }

        private void LoadCurrentDetailsGrid()
        {
            dgvCurrentDetails.Rows.Clear();
            decimal total = 0;
            foreach (var item in currentDetails)
            {
                total += item.ThanhTien;
                dgvCurrentDetails.Rows.Add(
                    item.MaSanPham,
                    "Sản phẩm " + item.MaSanPham,
                    item.SoLuong.ToString("N0"),
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
            lblTotalAmount.Text = $"TỔNG TIỀN TẠM TÍNH: {total.ToString("N0")} đ";
            _ = UpdateTotalAmountAsync();
        }

        private async Task UpdateTotalAmountAsync()
        {
            decimal total = currentDetails.Sum(d => d.ThanhTien);
            int discountPercent = 0;
            if (int.TryParse(txtGiamGia.Text, out int promoId))
            {
                
                var promo = await _promoService.GetPromotionByIdAsync(promoId);
                if (promo != null)
                {
                    discountPercent = promo.PhanTramGiamGia;
                }
            }
            
            decimal finalTotal = total - (total * discountPercent / 100);
            txtTongTien.Text = finalTotal.ToString("N0") + " đ";
            lblTotalAmount.Text = $"TỔNG TIỀN TẠM TÍNH: {total.ToString("N0")} đ";
        }

        private async void btnAddToCart_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            if (!decimal.TryParse(txtSelGiaNhap.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn hoặc bằng 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            if (int.TryParse(id, out int productId))
            {
                var prod = await _productService.GetProductByIdAsync(productId);
                if (prod != null)
                {
                    var existingCheck = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                    int newTotalQty = (existingCheck != null ? existingCheck.SoLuong : 0) + qty;
                    if (newTotalQty > prod.SoLuongTon)
                    {
                        MessageBox.Show($"Số lượng vượt quá tồn kho!\nTồn kho hiện tại: {prod.SoLuongTon}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong += qty;
                existing.ThanhTien = existing.SoLuong * existing.DonGia;
            }
            else
            {
                currentDetails.Add(new OrderDetail
                {
                    MaSanPham = id,
                    SoLuong = qty,
                    DonGia = price,
                    ThanhTien = qty * price
                });
            }

            LoadCurrentDetailsGrid();
            
            if (!isAddingNew && _selectedOrder != null)
            {
                await UpdateTotalAmountAsync();
                if (decimal.TryParse(txtTongTien.Text.Replace(" đ", "").Replace(",", ""), out decimal newTotal))
                    _selectedOrder.TongTien = newTotal;
                    
                await _orderService.UpdateOrderCartAsync(_selectedOrder, currentDetails);
                _ = LoadProductsSelectionGridAsync();
                MessageBox.Show("Đã thêm sản phẩm và cập nhật tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetCartButtonsState("Init", false);
            }
            else
            {
                isCartModified = true;
                SetCartButtonsState("Init", isCartModified);
            }
            
            btnResetCartForm_Click(null, EventArgs.Empty);
        }

        private async void guna2Button4_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id)) return;
            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                if (int.TryParse(txtSelSoLuong.Text, out int qty) && qty > 0)
                {
                    if (int.TryParse(id, out int productId))
                    {
                        var prod = await _productService.GetProductByIdAsync(productId);
                        if (prod != null && qty > prod.SoLuongTon)
                        {
                            MessageBox.Show($"Số lượng vượt quá tồn kho!\nTồn kho hiện tại: {prod.SoLuongTon}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    existing.SoLuong = qty;
                    existing.ThanhTien = existing.SoLuong * existing.DonGia;
                }
                if (decimal.TryParse(txtSelGiaNhap.Text, out decimal price) && price >= 0)
                {
                    existing.DonGia = price;
                    existing.ThanhTien = existing.SoLuong * existing.DonGia;
                }
                LoadCurrentDetailsGrid();
                
                if (!isAddingNew && _selectedOrder != null)
                {
                    await UpdateTotalAmountAsync();
                    if (decimal.TryParse(txtTongTien.Text.Replace(" đ", "").Replace(",", ""), out decimal newTotal))
                        _selectedOrder.TongTien = newTotal;
                        
                    await _orderService.UpdateOrderCartAsync(_selectedOrder, currentDetails);
                    _ = LoadProductsSelectionGridAsync();
                    MessageBox.Show("Đã cập nhật số lượng và tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetCartButtonsState("Init", false);
                }
                else
                {
                    isCartModified = true;
                    SetCartButtonsState("Init", isCartModified);
                }
                btnResetCartForm_Click(null, EventArgs.Empty);
            }
        }

        private void guna2Button3_Click(object? sender, EventArgs e)
        {
            btnResetCartForm_Click(null, EventArgs.Empty);
            SetCartButtonsState("Init", isCartModified);
        }

        private async void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (item != null)
            {
                if (!isAddingNew && _selectedOrder != null)
                {
                    var confirm = MessageBox.Show("Hóa đơn đã được lưu! Xóa sản phẩm này sẽ trực tiếp khôi phục số lượng tồn kho. Bạn có chắc chắn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes) return;
                }

                currentDetails.Remove(item);
                LoadCurrentDetailsGrid();
                
                if (!isAddingNew && _selectedOrder != null)
                {
                    await UpdateTotalAmountAsync();
                    if (decimal.TryParse(txtTongTien.Text.Replace(" đ", "").Replace(",", ""), out decimal newTotal))
                        _selectedOrder.TongTien = newTotal;
                        
                    await _orderService.UpdateOrderCartAsync(_selectedOrder, currentDetails);
                    _ = LoadProductsSelectionGridAsync();
                    MessageBox.Show("Đã xóa sản phẩm và khôi phục tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetCartButtonsState("Init", false);
                }
                else
                {
                    isCartModified = true;
                    SetCartButtonsState("Init", isCartModified);
                }
                btnResetCartForm_Click(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Sản phẩm này chưa được thêm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
            txtSelMaDanhMuc.Text = "";
            txtSelTenDanhMuc.Text = "";
            dgvProductsSelection.ClearSelection();
            dgvCurrentDetails.ClearSelection();
            tabSelectionContainer.SelectedTab = tabListProducts;
        }

        private async void btnPOSSearch_Click(object? sender, EventArgs e)
        {
            if (!isPOSSearchMode)
            {
                isPOSSearchMode = true;
                btnResetCartForm_Click(null, EventArgs.Empty);
                SetCartButtonsState("Init", isCartModified);
                
                txtSelMaSP.Enabled = true;
                txtSelTenSP.Enabled = true;
                txtSelGiaNhap.Enabled = true;
                txtSelSoLuong.Enabled = true;
                txtSelMaDanhMuc.Enabled = true;
                txtSelTenDanhMuc.Enabled = true;

                // Mở khóa ReadOnly để cho phép gõ phím
                txtSelMaSP.ReadOnly = false;
                txtSelTenSP.ReadOnly = false;
                txtSelGiaNhap.ReadOnly = false;
                txtSelSoLuong.ReadOnly = false;
                txtSelMaDanhMuc.ReadOnly = false;
                txtSelTenDanhMuc.ReadOnly = false;

                MessageBox.Show("Đã bật chế độ Tìm kiếm!\n\nVui lòng gõ thông tin cần tìm vào các ô trống rồi nhấn TÌM KIẾM lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSelMaSP.Text) && 
                string.IsNullOrWhiteSpace(txtSelTenSP.Text) &&
                string.IsNullOrWhiteSpace(txtSelGiaNhap.Text) &&
                string.IsNullOrWhiteSpace(txtSelSoLuong.Text) &&
                string.IsNullOrWhiteSpace(txtSelMaDanhMuc.Text) &&
                string.IsNullOrWhiteSpace(txtSelTenDanhMuc.Text) &&
                string.IsNullOrWhiteSpace(txtProductSearch.Text))
            {
                MessageBox.Show("Vui lòng điền thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string keyword = txtProductSearch.Text.Trim().ToLower();
            string maSP = txtSelMaSP.Text.Trim().ToLower();
            string tenSP = txtSelTenSP.Text.Trim().ToLower();
            string maDM = txtSelMaDanhMuc.Text.Trim().ToLower();
            string tenDM = txtSelTenDanhMuc.Text.Trim().ToLower();

            var all = await _productService.GetAllProductsAsync();
            var filtered = all.Where(p =>
                (string.IsNullOrEmpty(keyword) ||
                 p.MaSanPham.ToString() == keyword ||
                 p.TenSanPham.ToLower().Contains(keyword) ||
                 p.MaDanhMuc.ToString() == keyword ||
                 (p.TenDanhMuc != null && p.TenDanhMuc.ToLower().Contains(keyword))) &&
                (string.IsNullOrEmpty(maSP) || p.MaSanPham.ToString() == maSP) &&
                (string.IsNullOrEmpty(tenSP) || p.TenSanPham.ToLower().Contains(tenSP)) &&
                (string.IsNullOrEmpty(maDM) || p.MaDanhMuc.ToString() == maDM) &&
                (string.IsNullOrEmpty(tenDM) || (p.TenDanhMuc != null && p.TenDanhMuc.ToLower().Contains(tenDM)))
            ).ToList();

            _ = LoadProductsSelectionGridAsync(filtered);
            dgvProductsSelection.ClearSelection();
            tabSelectionContainer.SelectedTab = tabListProducts;
        }

        private void btnPOSRefresh_Click(object? sender, EventArgs e)
        {
            isPOSSearchMode = false;
            txtProductSearch.Text = "";
            txtSelMaSP.ReadOnly = true;
            txtSelTenSP.ReadOnly = true;
            txtSelGiaNhap.ReadOnly = true;
            txtSelSoLuong.ReadOnly = true;
            txtSelMaDanhMuc.ReadOnly = true;
            txtSelTenDanhMuc.ReadOnly = true;
            btnResetCartForm_Click(null, EventArgs.Empty);
            SetCartButtonsState("Init", isCartModified);
            _ = LoadProductsSelectionGridAsync();
        }

        private void tabSelectionContainer_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabSelectionContainer.SelectedTab == tabProductDetail)
            {
                picProductDetail.Image = null;
            }
        }

        private void btnSelectProduct_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (!string.IsNullOrEmpty(id))
            {
                tabSelectionContainer.SelectedTab = tabListProducts;
                txtSelSoLuong.Focus();
                txtSelSoLuong.SelectAll();
            }
        }

        private async void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            
            if (_selectedOrder != null && isCartModified)
            {
                decimal total = currentDetails.Sum(d => d.ThanhTien);
                decimal finalTotal = total;
                if (!string.IsNullOrEmpty(txtGiamGia.Text) && txtGiamGia.Text.StartsWith("KM"))
                {
                    string percentStr = txtGiamGia.Text.ToUpper().Replace("KM", "");
                    if (decimal.TryParse(percentStr, out decimal dp))
                    {
                        finalTotal = total - (total * dp / 100);
                    }
                }
                
                await _orderService.UpdateOrderCartAsync(_selectedOrder, currentDetails);
                
                MessageBox.Show("Lưu thay đổi giỏ hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isCartModified = false;
                SetCartButtonsState("Init", false);
            }

            if (tabMain != null && tabPhieuXuat != null)
            {
                tabMain.SelectedTab = tabPhieuXuat;
                _ = LoadOrdersGridAsync();
            }
        }

        private void btnXuatbaocao_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"HoaDon_{_selectedOrder.MaHoaDon}_{DateTime.Now:yyyyMMdd}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            sw.Write('\uFEFF');
                            
                            sw.WriteLine("--- THÔNG TIN HÓA ĐƠN ---");
                            sw.WriteLine($"Mã Hóa Đơn:,\"{_selectedOrder.MaHoaDon}\"");
                            sw.WriteLine($"Khách Hàng:,\"{txtMaKhachHang.Text}\"");
                            sw.WriteLine($"Người Lập:,\"{txtTenNguoiDung.Text}\"");
                            sw.WriteLine($"Ngày Tạo:,\"{_selectedOrder.NgayTao:dd/MM/yyyy HH:mm}\"");
                            sw.WriteLine($"Tổng Tiền:,\"{_selectedOrder.TongTien:N2}\"");
                            sw.WriteLine($"Thanh Toán:,\"{_selectedOrder.HinhThucThanhToan}\"");
                            sw.WriteLine($"Trạng Thái:,\"{_selectedOrder.TrangThai}\"");
                            
                            sw.WriteLine();
                            sw.WriteLine("--- CHI TIẾT SẢN PHẨM ---");
                            ExportDataGridViewToCsv(dgvCurrentDetails, sw);
                        }
                        MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportDataGridViewToCsv(DataGridView dgv, System.IO.StreamWriter sw)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;
            
            // Lấy header
            List<string> headers = new List<string>();
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) headers.Add($"\"{col.HeaderText}\"");
            }
            sw.WriteLine(string.Join(",", headers));
            
            // Lấy dòng
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                List<string> cells = new List<string>();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        object val = row.Cells[col.Index].Value;
                        string text = "";
                        if (val != null)
                        {
                            if (val is DateTime dt) text = dt.ToString("dd/MM/yyyy HH:mm");
                            else if (val is decimal dec) text = dec.ToString("F2");
                            else text = val.ToString();
                        }
                        cells.Add($"\"{text}\"");
                    }
                }
                sw.WriteLine(string.Join(",", cells));
            }
        }
    }
}




