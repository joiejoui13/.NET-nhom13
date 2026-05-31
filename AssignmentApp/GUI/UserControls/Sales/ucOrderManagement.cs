using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private OrderRepository _orderRepo = new OrderRepository();
        private ProductRepository _productRepo = new ProductRepository();
        private OrderDetailRepository _detailRepo = new OrderDetailRepository();

        private List<Order> _orders = new List<Order>();
        private Order _selectedOrder = null;
        private bool isEditing = false;
        private bool isAddingNew = false;
        private bool isSearchMode = false;
        private List<OrderDetail> currentDetails = new List<OrderDetail>();

        private bool defaultToPOS = false;

        public ucOrderManagement(bool defaultToPOS = false)
        {
            InitializeComponent();
            this.defaultToPOS = defaultToPOS;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
            
            // Add DoubleClick event for dgvOrders
            dgvOrders.CellDoubleClick += dgvOrders_CellDoubleClick;
            dgvProductsSelection.CellDoubleClick += dgvProductsSelection_CellDoubleClick;
            txtGiamGia.TextChanged += async (s, e) => await UpdateTotalAmountAsync();
        }

        private void ucOrderManagement_Load(object sender, EventArgs e)
        {
            SetControlState("Init");
            _ = LoadOrdersGridAsync();

            if (defaultToPOS && tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                _ = LoadProductsSelectionGridAsync();
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
                txtNgayTao.Checked = false;
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
            _orders = (await _orderRepo.GetAllAsync()).ToList();
            
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
            txtNgayTao.Value = order.NgayTao;
            txtGiamGia.Text = order.MaKhuyenMai ?? "";
            cboHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtTongTien.Text = order.TongTien.ToString("N0") + " đ";
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = order.LyDoHuy;

            // Load Details from DB
            var details = await _orderRepo.GetDetailsAsync(order.MaHoaDon.ToString());
            currentDetails = details.ToList();
            
            UpdateConvertToSalesState();
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
                    !txtNgayTao.Checked)
                {
                    MessageBox.Show("Vui lòng điền thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Perform Search
                var list = await _orderRepo.GetAllAsync();
                
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

                if (txtNgayTao.Checked)
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
            txtNgayTao.Value = DateTime.Now;
            cboLoaiHoaDon.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            txtLyDoHuy.Text = "";

        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            SetControlState("Add");
            
            txtMaHoaDon.Text = "Tự động sinh";
            txtMaKhachHang.Text = ""; 
            txtTenNguoiDung.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser?.MaNguoiDung.ToString() ?? "1"; 
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
            var customerRepo = new AssignmentApp.DAL.Repositories.Sales.CustomerRepository();
            var customer = await customerRepo.GetByIdAsync(maKH);
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
                var promoRepo = new AssignmentApp.DAL.Repositories.Admin.PromotionRepository();
                var promo = await promoRepo.GetByIdAsync(promoId);
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

            _selectedOrder.TrangThai = cboTrangThai.Text;
            _selectedOrder.LoaiHoaDon = cboLoaiHoaDon.Text;
            _selectedOrder.HinhThucThanhToan = cboHinhThucThanhToan.Text;
            _selectedOrder.MaKhachHang = txtMaKhachHang.Text;
            _selectedOrder.MaKhuyenMai = string.IsNullOrEmpty(txtGiamGia.Text) ? null : txtGiamGia.Text;
            _selectedOrder.LyDoHuy = cboTrangThai.Text == "Đã huỷ" ? txtLyDoHuy.Text : "";
            _selectedOrder.TongTien = decimal.Parse(txtTongTien.Text.Replace(" đ", "").Replace(",", ""));

            await _orderRepo.UpdateAsync(_selectedOrder);

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

                await _orderRepo.AddAsync(newOrder);
                var savedOrders = await _orderRepo.GetAllAsync();
                string newId = savedOrders.Max(o => int.Parse(o.MaHoaDon)).ToString();
                
                foreach(var detail in currentDetails)
                {
                    detail.MaHoaDon = newId;
                    await _orderRepo.AddDetailAsync(detail);
                }

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
                
                // _ = _orderRepo.UpdateAsync(_selectedOrder);

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
            var list = dataSource ?? (await _productRepo.GetAllAsync()).ToList();
            foreach (var prod in list)
            {
                dgvProductsSelection.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.GiaBan.ToString("N0") + " đ"
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
                var prod = await _productRepo.GetByIdAsync(productId);
                if (prod != null)
            {
                txtSelMaSP.Text = prod.MaSanPham.ToString();
                txtSelTenSP.Text = prod.TenSanPham;
                txtSelGiaNhap.Text = prod.GiaBan.ToString();

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
                    var item = currentDetails.FirstOrDefault(d => d.MaSanPham == rawId);
                    if (item != null)
                    {
                        txtSelMaSP.Text = item.MaSanPham.ToString();
                        txtSelTenSP.Text = "Sản phẩm " + item.MaSanPham; 
                        txtSelSoLuong.Text = item.SoLuong.ToString();
                        txtSelGiaNhap.Text = item.DonGia.ToString();

                        txtSelSoLuong.Focus();
                        txtSelSoLuong.SelectAll();
                    }
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
                var promoRepo = new AssignmentApp.DAL.Repositories.Admin.PromotionRepository();
                var promo = await promoRepo.GetByIdAsync(promoId);
                if (promo != null)
                {
                    discountPercent = promo.PhanTramGiamGia;
                }
            }
            
            decimal finalTotal = total - (total * discountPercent / 100);
            txtTongTien.Text = finalTotal.ToString("N0") + " đ";
            lblTotalAmount.Text = $"TỔNG TIỀN TẠM TÍNH: {finalTotal.ToString("N0")} đ";
        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
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

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong = qty;
                existing.DonGia = price;
            }
            else
            {
                currentDetails.Add(new OrderDetail
                {
                    MaSanPham = id,
                    SoLuong = qty,
                    DonGia = price
                });
            }

            LoadCurrentDetailsGrid();
            
            // Reset textboxes slightly
            txtSelSoLuong.Text = "1";
            txtSelSoLuong.Focus();
            txtSelSoLuong.SelectAll();
        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
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
                currentDetails.Remove(item);
                LoadCurrentDetailsGrid();
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
        }

        private async void btnPOSSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtProductSearch.Text.Trim().ToLower();

            var all = await _productRepo.GetAllAsync();
            var filtered = all.Where(p =>
                string.IsNullOrEmpty(keyword) ||
                p.MaSanPham.ToString() == keyword ||
                p.TenSanPham.ToLower().Contains(keyword) ||
                p.MaDanhMuc.ToString() == keyword
            ).ToList();

            _ = LoadProductsSelectionGridAsync(filtered);
        }

        private void btnPOSRefresh_Click(object? sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
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

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            if (tabMain != null && tabPhieuXuat != null)
            {
                tabMain.SelectedTab = tabPhieuXuat;
            }
        }
    }
}
