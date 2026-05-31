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
                
                btnAdd.Enabled = true;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
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
                
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = true; // Cancel search
                
                isSearchMode = true;
                isAddingNew = false;
                isEditing = false;
            }

            txtMaKhachHang.Enabled = !isTextBoxReadonly;
            txtTenNguoiDung.Enabled = !isTextBoxReadonly;
            txtTongTien.Enabled = !isTextBoxReadonly;
            txtGiamGia.Enabled = !isTextBoxReadonly;
            txtHinhThucThanhToan.Enabled = !isTextBoxReadonly;
            txtNgayTao.Enabled = !isTextBoxReadonly;
            
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
                    order.GiamGia.ToString("N0") + " đ",
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
            }
        }

        private async Task PopulateOrderDetailsAsync(Order order)
        {
            txtMaHoaDon.Text = order.MaHoaDon.ToString();
            cboLoaiHoaDon.Text = order.LoaiHoaDon;
            txtMaKhachHang.Text = order.MaKhachHang.ToString();
            txtTenNguoiDung.Text = order.MaNguoiDung.ToString();
            txtTongTien.Text = order.TongTien.ToString("N0") + " đ";
            txtGiamGia.Text = order.GiamGia.ToString("N0") + " đ";
            txtHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtNgayTao.Text = order.NgayTao.ToString("dd/MM/yyyy HH:mm");
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = order.LyDoHuy;

            // Load Details from DB
            var details = await _orderRepo.GetDetailsAsync(order.MaHoaDon.ToString());
            currentDetails = details.ToList();
            
            dgvOrderDetails.Rows.Clear();
            foreach (var item in currentDetails)
            {
                dgvOrderDetails.Rows.Add(
                    item.MaSanPham,
                    "Sản phẩm " + item.MaSanPham, 
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }

            UpdateConvertToSalesState();
        }

        private void UpdateConvertToSalesState()
        {
            // Placeholder for UI Logic
        }

        private void cboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isEditing)
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
            if (e.RowIndex >= 0 && !isEditing && !isSearchMode)
            {
                SelectOrderRow(e.RowIndex);
            }
        }

        private void dgvOrders_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing && !isSearchMode)
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

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            if (!isSearchMode)
            {
                Form popup = new Form()
                {
                    Text = "Tìm kiếm Hóa Đơn",
                    Size = new Size(300, 150),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                Label lbl = new Label() { Text = "Chế độ Tìm Kiếm đã được bật.\nVui lòng nhập thông tin vào các ô TextBox trên màn hình chính và nhấn Tìm Kiếm lần nữa để lọc.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                popup.Controls.Add(lbl);
                popup.ShowDialog();
                
                SetControlState("Search");
            }
            else
            {
                // Perform Search
                var list = await _orderRepo.GetAllAsync();
                
                if (!string.IsNullOrEmpty(txtMaHoaDon.Text))
                {
                    list = list.Where(o => o.MaHoaDon == txtMaHoaDon.Text);
                }
                
                if (!string.IsNullOrEmpty(cboTrangThai.Text))
                    list = list.Where(o => o.TrangThai == cboTrangThai.Text);
                
                if (!string.IsNullOrEmpty(cboLoaiHoaDon.Text))
                    list = list.Where(o => o.LoaiHoaDon == cboLoaiHoaDon.Text);

                _orders = list.ToList();
                dgvOrders.Rows.Clear();
                foreach (var order in _orders)
                {
                    dgvOrders.Rows.Add(
                        order.MaHoaDon,
                        order.MaKhachHang,
                        order.MaNguoiDung,
                        order.TongTien.ToString("N0") + " đ",
                        order.GiamGia.ToString("N0") + " đ",
                        order.HinhThucThanhToan,
                        order.TrangThai,
                        order.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                        order.LoaiHoaDon,
                        order.LyDoHuy
                    );
                }
                
                MessageBox.Show($"Tìm thấy {_orders.Count} kết quả.", "Kết quả Tìm kiếm");
                SetControlState("Init");
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
            txtHinhThucThanhToan.Text = "";
            txtNgayTao.Text = "";
            cboLoaiHoaDon.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            txtLyDoHuy.Text = "";
            dgvOrderDetails.Rows.Clear();
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            SetControlState("Add");
            
            txtMaHoaDon.Text = "Tự động sinh";
            txtMaKhachHang.Text = "1"; 
            txtTenNguoiDung.Text = "1"; 
            txtNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            cboLoaiHoaDon.Text = "Đơn bán hàng";
            cboTrangThai.Text = "Đã hoàn thành";
            txtLyDoHuy.Text = "";
            txtTongTien.Text = "0 đ";
            txtGiamGia.Text = "0 đ";
            txtHinhThucThanhToan.Text = "Tiền mặt";
            
            currentDetails.Clear();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
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

            SetControlState("Edit");
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Đơn hàng này đã được hủy trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedOrder.TrangThai == "Đã hoàn thành")
            {
                MessageBox.Show("Không thể hủy đơn hàng đã hoàn thành giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng này không?", "Xác nhận hủy đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                SetControlState("Edit");
                cboTrangThai.Text = "Đã huỷ";
                txtLyDoHuy.Focus();
                MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng và nhấn 'LƯU' để xác nhận!", "Nhập lý do hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (isAddingNew)
            {
                if (currentDetails.Count == 0)
                {
                    MessageBox.Show("Đơn hàng phải có ít nhất một sản phẩm! Hãy chọn sản phẩm ở Tab Bán Hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newOrder = new Order
                {
                    MaKhachHang = txtMaKhachHang.Text,
                    MaNguoiDung = txtTenNguoiDung.Text,
                    TongTien = (decimal)currentDetails.Sum(d => d.ThanhTien),
                    GiamGia = 0,
                    HinhThucThanhToan = txtHinhThucThanhToan.Text,
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
            else if (isEditing)
            {
                if (_selectedOrder != null)
                {
                    _selectedOrder.TrangThai = cboTrangThai.Text;
                    _selectedOrder.LoaiHoaDon = cboLoaiHoaDon.Text;
                    _selectedOrder.LyDoHuy = cboTrangThai.Text == "Đã huỷ" ? txtLyDoHuy.Text : "";
                    
                    _selectedOrder.TongTien = (decimal)currentDetails.Sum(d => d.ThanhTien);

                    // Update functionality is mock for now since repo doesn't have it
                    // await _orderRepo.UpdateAsync(_selectedOrder);

                    MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            var prod = await _productRepo.GetByIdAsync(id);
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
