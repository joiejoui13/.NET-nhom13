import codecs

content = '''using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private OrderRepository _orderRepo = new OrderRepository();
        private ProductRepository _productRepo = new ProductRepository();
        
        private List<Order> _orders = new List<Order>();
        private List<Product> _products = new List<Product>();
        
        private Order selectedOrder = null;
        private bool isEditing = false;
        private bool isAddingNew = false;
        private List<OrderDetail> currentDetails = new List<OrderDetail>();

        private bool defaultToPOS = false;

        public ucOrderManagement(bool defaultToPOS = false)
        {
            InitializeComponent();
            this.defaultToPOS = defaultToPOS;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
        }

        private async void ucOrderManagement_Load(object sender, EventArgs e)
        {
            await InitializeProductsAsync();
            await LoadOrdersGridAsync();

            SetEditState(false);

            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }

            if (defaultToPOS && tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadProductsSelectionGrid();
                LoadCurrentDetailsGrid();
            }
        }

        private async Task InitializeProductsAsync()
        {
            var prods = await _productRepo.GetAllAsync();
            _products = prods.ToList();
        }

        private async Task LoadOrdersGridAsync(List<Order> dataSource = null)
        {
            try {
                dgvOrders.Rows.Clear();
                var list = dataSource;
                if (list == null) {
                    var items = await _orderRepo.GetAllAsync();
                    list = items.ToList();
                    _orders = list;
                }
                foreach (var order in list)
                {
                    dgvOrders.Rows.Add(
                        order.MaHoaDon,
                        order.TenKhachHang ?? order.MaKhachHang,
                        order.TenNguoiDung ?? order.MaNguoiDung,
                        order.TongTien.ToString("N0") + " d",
                        order.GiamGia.ToString("N0") + " d",
                        order.HinhThucThanhToan,
                        order.TrangThai,
                        order.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                        "Ðon bán hàng", // LoaiHoaDon hardcoded for now or use actual logic
                        "" // LyDoHuy
                    );
                }
            } catch (Exception ex) {
                MessageBox.Show("L?i t?i danh sách hóa don: " + ex.Message);
            }
        }

        private async void SelectOrderRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvOrders.Rows.Count) return;

            dgvOrders.ClearSelection();
            dgvOrders.Rows[rowIndex].Selected = true;

            string orderId = dgvOrders.Rows[rowIndex].Cells[0].Value?.ToString();
            selectedOrder = _orders.FirstOrDefault(o => o.MaHoaDon == orderId);

            if (selectedOrder != null)
            {
                await PopulateOrderDetailsAsync(selectedOrder);
            }
        }

        private async Task PopulateOrderDetailsAsync(Order order)
        {
            txtMaHoaDon.Text = order.MaHoaDon;
            cboLoaiHoaDon.Text = "Ðon bán hàng";
            txtMaKhachHang.Text = order.MaKhachHang;
            txtTenNguoiDung.Text = order.MaNguoiDung;
            txtTongTien.Text = order.TongTien.ToString("N0") + " d";
            txtGiamGia.Text = order.GiamGia.ToString("N0") + " d";
            txtHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtNgayTao.Text = order.NgayTao.ToString("dd/MM/yyyy HH:mm");
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = "";

            // Load Details Grid from DB
            dgvOrderDetails.Rows.Clear();
            var details = await _orderRepo.GetDetailsAsync(order.MaHoaDon);
            foreach (var item in details)
            {
                dgvOrderDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " d",
                    item.ThanhTien.ToString("N0") + " d"
                );
            }

            UpdateConvertToSalesState();
        }

        private void UpdateConvertToSalesState() { }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            cboLoaiHoaDon.Enabled = editing;
            cboTrangThai.Enabled = editing;

            txtLyDoHuy.ReadOnly = !editing || cboTrangThai.Text != "Ðã hu?";

            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnAdd.Location = new Point(15, 460);
            btnEdit.Location = new Point(115, 460);
            btnDelete.Location = new Point(215, 460);
            btnSave.Location = new Point(15, 510);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 510);
            btnCancel.Size = new Size(140, 36);

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;

            UpdateConvertToSalesState();
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isEditing)
            {
                bool isCanceled = cboTrangThai.Text == "Ðã hu?";
                txtLyDoHuy.ReadOnly = !isCanceled;
                if (!isCanceled) txtLyDoHuy.Text = "";
                else txtLyDoHuy.Focus();
            }
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing) SelectOrderRow(e.RowIndex);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            // not implemented keyword fully in text box yet, assuming using some form data
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadOrdersGridAsync();
            SetEditState(false);
            if (dgvOrders.Rows.Count > 0) SelectOrderRow(0);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            isEditing = true;

            txtMaHoaDon.Text = "T? d?ng";
            txtMaKhachHang.Text = "KH001";
            txtTenNguoiDung.Text = "ND001";
            txtNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            cboLoaiHoaDon.Text = "Ðon bán hàng";
            cboTrangThai.Text = "Ðã hoàn thành";
            txtLyDoHuy.Text = "";
            txtTongTien.Text = "0 d";
            txtGiamGia.Text = "0 d";
            txtHinhThucThanhToan.Text = "Ti?n m?t";

            currentDetails.Clear();

            SetEditState(true);

            if (tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadProductsSelectionGrid();
                LoadCurrentDetailsGrid();
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng ch?n m?t don hàng d? ch?nh s?a!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Ðã hoàn thành" || selectedOrder.TrangThai == "Ðã hu?")
            {
                MessageBox.Show("Không th? ch?nh s?a don hàng dã hoàn thành ho?c dã hu?!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);

            var details = await _orderRepo.GetDetailsAsync(selectedOrder.MaHoaDon);
            currentDetails = details.ToList();

            if (tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadProductsSelectionGrid();
                LoadCurrentDetailsGrid();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedOrder == null) return;
            if (selectedOrder.TrangThai == "Ðã hu?" || selectedOrder.TrangThai == "Ðã hoàn thành")
            {
                MessageBox.Show("Không th? h?y don hàng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("B?n có ch?c ch?n mu?n h?y don hàng này không? T?n kho s? du?c hoàn tr?.", "Xác nh?n h?y don", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try {
                    bool res = await _orderRepo.DeleteOrderTransactionAsync(selectedOrder.MaHoaDon);
                    if (res) {
                        MessageBox.Show("Ðã h?y don hàng thành công!");
                        await LoadOrdersGridAsync();
                    }
                } catch(Exception ex) {
                    MessageBox.Show("L?i: " + ex.Message);
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (isAddingNew)
            {
                if (currentDetails.Count == 0)
                {
                    MessageBox.Show("Ðon hàng ph?i có ít nh?t m?t s?n ph?m!", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    var newOrder = new Order
                    {
                        MaKhachHang = txtMaKhachHang.Text,
                        MaNguoiDung = txtTenNguoiDung.Text,
                        TongTien = currentDetails.Sum(d => d.ThanhTien),
                        GiamGia = 0,
                        HinhThucThanhToan = txtHinhThucThanhToan.Text,
                        TrangThai = cboTrangThai.Text,
                        NgayTao = DateTime.Now
                    };

                    int id = await _orderRepo.AddAsync(newOrder);
                    string generatedMaHoaDon = id.ToString(); // Or fetch properly

                    foreach(var d in currentDetails) {
                        d.MaHoaDon = generatedMaHoaDon;
                        await _orderRepo.AddDetailAsync(d);
                    }

                    MessageBox.Show("T?o don hàng m?i thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch(Exception ex) {
                    MessageBox.Show("L?i luu hóa don: " + ex.Message);
                    return;
                }
            }
            else
            {
                // Update order not fully supported in repo yet, skipping or assuming handled
                MessageBox.Show("Tính nang c?p nh?t chua du?c h? tr? hoàn toàn ? Repo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            isAddingNew = false;
            SetEditState(false);
            await LoadOrdersGridAsync();
            if (dgvOrders.Rows.Count > 0) SelectOrderRow(0);
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            SetEditState(false);
            if (selectedOrder != null) await PopulateOrderDetailsAsync(selectedOrder);
        }

        private void btnConvertToInvoice_Click(object sender, EventArgs e) { }

        // ========================================================
        // TAB 2 EVENTS (PRODUCT SELECTION / POS)
        // ========================================================

        private void LoadProductsSelectionGrid(List<Product> dataSource = null)
        {
            dgvProductsSelection.Rows.Clear();
            var list = dataSource ?? _products;
            foreach (var prod in list)
            {
                dgvProductsSelection.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.GiaBan.ToString("N0") + " d"
                );
            }
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            string keyword = txtProductSearch.Text.Trim().ToLower();
            var filtered = _products.Where(p =>
                string.IsNullOrEmpty(keyword) ||
                p.MaSanPham.ToLower().Contains(keyword) ||
                p.TenSanPham.ToLower().Contains(keyword)
            ).ToList();
            LoadProductsSelectionGrid(filtered);
        }

        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dgvProductsSelection.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var prod = _products.FirstOrDefault(p => p.MaSanPham == id);
                if (prod != null)
                {
                    txtSelMaSP.Text = prod.MaSanPham;
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

                    tabSelectionContainer.SelectedTab = tabProductDetail;
                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
                }
            }
        }

        private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                if (item != null)
                {
                    txtSelMaSP.Text = item.MaSanPham;
                    txtSelTenSP.Text = item.TenSanPham;
                    txtSelSoLuong.Text = item.SoLuong.ToString();
                    txtSelGiaNhap.Text = item.DonGia.ToString();

                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
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
                    item.TenSanPham,
                    item.SoLuong.ToString("N0"),
                    item.DonGia.ToString("N0") + " d",
                    item.ThanhTien.ToString("N0") + " d"
                );
            }
            lblTotalAmount.Text = $"T?NG TI?N T?M TÍNH: {total.ToString("N0")} d";
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nh?n nút THÊM ho?c S?A ? Tab 1 tru?c khi ch?nh s?a s?n ph?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng ch?n m?t s?n ph?m t? danh sách tru?c!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("S? lu?ng ph?i là s? nguyên duong l?n hon 0!", "L?i nh?p li?u", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtSelGiaNhap.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Ðon giá ph?i l?n hon ho?c b?ng 0!", "L?i nh?p li?u", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong = qty;
                existing.DonGia = price;
                existing.ThanhTien = qty * price;
            }
            else
            {
                currentDetails.Add(new OrderDetail
                {
                    MaSanPham = id,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    DonGia = price,
                    ThanhTien = qty * price
                });
            }

            LoadCurrentDetailsGrid();
            btnResetCartForm_Click(this, EventArgs.Empty);
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (!isEditing) return;

            string id = txtSelMaSP.Text;
            var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (item != null)
            {
                currentDetails.Remove(item);
                LoadCurrentDetailsGrid();
                btnResetCartForm_Click(this, EventArgs.Empty);
            }
        }

        private void btnResetCartForm_Click(object sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
        }

        private void btnPOSSearch_Click(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private async void btnPOSRefresh_Click(object sender, EventArgs e)
        {
            btnResetCartForm_Click(sender, e);
            await InitializeProductsAsync();
            LoadProductsSelectionGrid();
        }

        private void tabSelectionContainer_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnSelectProduct_Click(object sender, EventArgs e) { }

        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (tabMain != null && tabPhieuXuat != null) tabMain.SelectedTab = tabPhieuXuat;
        }

        private void lblHinhThucThanhToan_Click(object sender, EventArgs e) { }
        private void pnlPOSTop_Paint(object sender, PaintEventArgs e) { }
        private void txtSelMaSP_TextChanged(object sender, EventArgs e) { }
        private void lblNgayTao_Click(object sender, EventArgs e) { }
    }
}
'''
with codecs.open('AssignmentApp/GUI/UserControls/Sales/ucOrderManagement.cs', 'w', 'utf-8-sig') as f:
    f.write(content)
