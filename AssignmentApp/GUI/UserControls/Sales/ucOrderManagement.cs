using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        // Mock data structures
        public class MockOrder
        {
            public int MaHoaDon { get; set; }
            public int MaKhachHang { get; set; }
            public string? TenKhachHang { get; set; }
            public string? TenNguoiDung { get; set; }
            public double TongTien { get; set; }
            public double GiamGia { get; set; }
            public string? HinhThucThanhToan { get; set; }
            public string? TrangThai { get; set; }
            public string? LoaiHoaDon { get; set; }
            public string? LyDoHuy { get; set; }
            public DateTime NgayTao { get; set; }
            public List<MockOrderDetail> Details { get; set; } = new List<MockOrderDetail>();
        }

        public class MockOrderDetail
        {
            public int MaSanPham { get; set; }
            public string? TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien => SoLuong * DonGia;
        }

        private List<MockOrder> mockOrders = new List<MockOrder>();
        private MockOrder selectedOrder = null;
        private bool isEditing = false;
        private bool isAddingNew = false;
        private List<MockOrderDetail> currentDetails = new List<MockOrderDetail>();

        private bool defaultToPOS = false;

        public ucOrderManagement(bool defaultToPOS = false)
        {
            InitializeComponent();
            this.defaultToPOS = defaultToPOS;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
        }

        private void ucOrderManagement_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            InitializeProducts();
            LoadOrdersGrid();
            cboFilterStatus.SelectedIndex = 0; // "Tất cả"
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

        private void InitializeMockData()
        {
            if (mockOrders.Count > 0) return;

            // Order 1: Completed Sales Order
            var order1 = new MockOrder
            {
                MaHoaDon = 1,
                MaKhachHang = 3,
                TenKhachHang = "Nguyễn Văn Học Sinh",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 694000,
                GiamGia = 0,
                HinhThucThanhToan = "Tiền mặt",
                TrangThai = "Đã hoàn thành",
                LoaiHoaDon = "Đơn bán hàng",
                LyDoHuy = "",
                NgayTao = DateTime.Now.AddDays(-2)
            };
            order1.Details.Add(new MockOrderDetail { MaSanPham = 12, TenSanPham = "Máy tính Casio FX-580VN X", SoLuong = 1, DonGia = 680000 });
            order1.Details.Add(new MockOrderDetail { MaSanPham = 4, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuong = 1, DonGia = 9000 });
            order1.Details.Add(new MockOrderDetail { MaSanPham = 1, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuong = 1, DonGia = 5000 });

            // Order 2: Processing Order (Purchase Order)
            var order2 = new MockOrder
            {
                MaHoaDon = 2,
                MaKhachHang = 2,
                TenKhachHang = "Công ty CP ABC",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 1360000,
                GiamGia = 240000,
                HinhThucThanhToan = "Chuyển khoản",
                TrangThai = "Chờ xử lý",
                LoaiHoaDon = "Đơn đặt hàng",
                LyDoHuy = "",
                NgayTao = DateTime.Now.AddHours(-4)
            };
            order2.Details.Add(new MockOrderDetail { MaSanPham = 7, TenSanPham = "Giấy in Double A A4 70gsm", SoLuong = 20, DonGia = 80000 });

            // Order 3: Canceled Order
            var order3 = new MockOrder
            {
                MaHoaDon = 3,
                MaKhachHang = 1,
                TenKhachHang = "Trường THPT X",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 45000,
                GiamGia = 0,
                HinhThucThanhToan = "Tiền mặt",
                TrangThai = "Đã huỷ",
                LoaiHoaDon = "Đơn bán hàng",
                LyDoHuy = "Khách thấy đắt nên không mua nữa",
                NgayTao = DateTime.Now.AddDays(-1)
            };
            order3.Details.Add(new MockOrderDetail { MaSanPham = 3, TenSanPham = "Bút máy Hồng Hà Nét Hoa", SoLuong = 1, DonGia = 45000 });

            mockOrders.Add(order1);
            mockOrders.Add(order2);
            mockOrders.Add(order3);
        }

        private void LoadOrdersGrid(List<MockOrder> dataSource = null)
        {
            dgvOrders.Rows.Clear();
            var list = dataSource ?? mockOrders;
            foreach (var order in list)
            {
                dgvOrders.Rows.Add(
                    order.MaHoaDon,
                    order.TenKhachHang,
                    order.TenNguoiDung,
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

            int orderId = Convert.ToInt32(dgvOrders.Rows[rowIndex].Cells[0].Value);
            selectedOrder = mockOrders.FirstOrDefault(o => o.MaHoaDon == orderId);

            if (selectedOrder != null)
            {
                PopulateOrderDetails(selectedOrder);
            }
        }

        private void PopulateOrderDetails(MockOrder order)
        {
            txtMaHoaDon.Text = order.MaHoaDon.ToString();
            cboLoaiHoaDon.Text = order.LoaiHoaDon;
            txtMaKhachHang.Text = order.TenKhachHang;
            txtTenNguoiDung.Text = order.TenNguoiDung;
            txtTongTien.Text = order.TongTien.ToString("N0") + " đ";
            txtGiamGia.Text = order.GiamGia.ToString("N0") + " đ";
            txtHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtNgayTao.Text = order.NgayTao.ToString("dd/MM/yyyy HH:mm");
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = order.LyDoHuy;

            // Load Details Grid
            dgvOrderDetails.Rows.Clear();
            foreach (var item in order.Details)
            {
                dgvOrderDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }

            // Update Convert to sales button visibility/enabled state
            UpdateConvertToSalesState();
        }

        private void UpdateConvertToSalesState()
        {
            if (selectedOrder != null && !isEditing)
            {
                bool isPreOrder = selectedOrder.LoaiHoaDon == "Đơn đặt hàng";
                bool isCanceledOrCompleted = selectedOrder.TrangThai == "Đã huỷ" || selectedOrder.TrangThai == "Đã hoàn thành";
                
                btnConvertToInvoice.Enabled = isPreOrder && !isCanceledOrCompleted;
                btnConvertToInvoice.Visible = isPreOrder;
            }
            else
            {
                btnConvertToInvoice.Enabled = false;
                btnConvertToInvoice.Visible = isEditing ? false : true;
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Enable/disable combo boxes
            cboLoaiHoaDon.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // LyDoHuy is only editable in Edit mode when TrangThai is "Đã huỷ"
            txtLyDoHuy.ReadOnly = !editing || cboTrangThai.Text != "Đã huỷ";

            // Make all buttons visible at all times
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // Position them statically side-by-side
            btnAdd.Location = new Point(15, 460);
            btnEdit.Location = new Point(115, 460);
            btnDelete.Location = new Point(215, 460);

            btnSave.Location = new Point(15, 510);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 510);
            btnCancel.Size = new Size(140, 36);

            // Enable/disable based on editing state
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;

            UpdateConvertToSalesState();
        }

        private void cboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isEditing)
            {
                bool isCanceled = cboTrangThai.Text == "Đã huỷ";
                txtLyDoHuy.ReadOnly = !isCanceled;
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
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectOrderRow(e.RowIndex);
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            string statusFilter = cboFilterStatus.Text;

            var filtered = mockOrders.Where(o =>
            {
                bool matchesKeyword = string.IsNullOrEmpty(keyword) || 
                                     o.MaHoaDon.ToString().Contains(keyword) ||
                                     o.TenKhachHang.ToLower().Contains(keyword) ||
                                     o.TenNguoiDung.ToLower().Contains(keyword);

                bool matchesStatus = statusFilter == "Tất cả" || o.TrangThai == statusFilter;

                return matchesKeyword && matchesStatus;
            }).ToList();

            LoadOrdersGrid(filtered);

            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
            else
            {
                selectedOrder = null;
                ClearInputs();
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboFilterStatus.SelectedIndex = 0;
            LoadOrdersGrid();
            SetEditState(false);
            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
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
            btnConvertToInvoice.Enabled = false;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            isAddingNew = true;
            isEditing = true;
            
            txtMaHoaDon.Text = (mockOrders.Count > 0 ? mockOrders.Max(o => o.MaHoaDon) + 1 : 1).ToString();
            txtMaKhachHang.Text = "Khách bán lẻ";
            txtTenNguoiDung.Text = "Thu Ngân 1";
            txtNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            cboLoaiHoaDon.Text = "Đơn bán hàng";
            cboTrangThai.Text = "Đã hoàn thành";
            txtLyDoHuy.Text = "";
            txtTongTien.Text = "0 đ";
            txtGiamGia.Text = "0 đ";
            txtHinhThucThanhToan.Text = "Tiền mặt";
            
            currentDetails.Clear();
            
            SetEditState(true);
            
            if (tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadProductsSelectionGrid();
                LoadCurrentDetailsGrid();
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã hoàn thành" || selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Không thể chỉnh sửa đơn hàng đã hoàn thành hoặc đã huỷ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);

            currentDetails = selectedOrder.Details.Select(d => new MockOrderDetail
            {
                MaSanPham = d.MaSanPham,
                TenSanPham = d.TenSanPham,
                SoLuong = d.SoLuong,
                DonGia = d.DonGia
            }).ToList();

            if (tabMain != null && tabChonSanPham != null)
            {
                tabMain.SelectedTab = tabChonSanPham;
                LoadProductsSelectionGrid();
                LoadCurrentDetailsGrid();
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Đơn hàng này đã được hủy trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã hoàn thành")
            {
                MessageBox.Show("Không thể hủy đơn hàng đã hoàn thành giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng này không?", "Xác nhận hủy đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                SetEditState(true);
                cboTrangThai.Text = "Đã huỷ";
                txtLyDoHuy.Focus();
                MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng và nhấn 'LƯU THAY ĐỔI' để xác nhận!", "Nhập lý do hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (isAddingNew)
            {
                if (currentDetails.Count == 0)
                {
                    MessageBox.Show("Đơn hàng phải có ít nhất một sản phẩm! Hãy chọn sản phẩm ở Tab 2.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int id = int.Parse(txtMaHoaDon.Text);
                var newOrder = new MockOrder
                {
                    MaHoaDon = id,
                    MaKhachHang = 1,
                    TenKhachHang = txtMaKhachHang.Text,
                    TenNguoiDung = txtTenNguoiDung.Text,
                    TongTien = currentDetails.Sum(d => d.ThanhTien),
                    GiamGia = 0,
                    HinhThucThanhToan = txtHinhThucThanhToan.Text,
                    TrangThai = cboTrangThai.Text,
                    LoaiHoaDon = cboLoaiHoaDon.Text,
                    LyDoHuy = "",
                    NgayTao = DateTime.Now,
                    Details = currentDetails.Select(d => new MockOrderDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        DonGia = d.DonGia
                    }).ToList()
                };

                mockOrders.Add(newOrder);
                selectedOrder = newOrder;
                MessageBox.Show("Tạo đơn hàng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedOrder != null)
                {
                    selectedOrder.TrangThai = cboTrangThai.Text;
                    selectedOrder.LoaiHoaDon = cboLoaiHoaDon.Text;
                    selectedOrder.LyDoHuy = cboTrangThai.Text == "Đã huỷ" ? txtLyDoHuy.Text : "";
                    selectedOrder.Details = currentDetails.Select(d => new MockOrderDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        DonGia = d.DonGia
                    }).ToList();
                    selectedOrder.TongTien = currentDetails.Sum(d => d.ThanhTien);

                    MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadOrdersGrid();
            
            if (selectedOrder != null)
            {
                int index = mockOrders.IndexOf(selectedOrder);
                if (index >= 0 && index < dgvOrders.Rows.Count)
                {
                    SelectOrderRow(index);
                }
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            SetEditState(false);
            if (selectedOrder != null)
            {
                PopulateOrderDetails(selectedOrder);
            }
        }

        private void btnConvertToInvoice_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null) return;

            if (selectedOrder.LoaiHoaDon != "Đơn đặt hàng")
            {
                MessageBox.Show("Chỉ có thể chuyển đổi Đơn đặt hàng thành Đơn bán hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Xác nhận chuyển Đơn đặt hàng #{selectedOrder.MaHoaDon} thành Đơn bán hàng (Hóa đơn)?\nThao tác này sẽ cập nhật tồn kho và tạo giao dịch bán hàng tương ứng.", "Xác nhận chuyển đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                selectedOrder.LoaiHoaDon = "Đơn bán hàng";
                selectedOrder.TrangThai = "Đã hoàn thành"; // Convert turns into active sales invoice
                
                MessageBox.Show("Đã chuyển đổi Đơn đặt hàng thành Đơn bán hàng thành công!\nTồn kho tương ứng đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadOrdersGrid();
                
                // Re-select row
                int index = mockOrders.IndexOf(selectedOrder);
                if (index >= 0 && index < dgvOrders.Rows.Count)
                {
                    SelectOrderRow(index);
                }
            }
        }

        // ========================================================
        // TAB 2 EVENTS (PRODUCT SELECTION / POS)
        // ========================================================
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public string DanhMuc { get; set; } = "";
            public double GiaBan { get; set; }
            public int TonKho { get; set; }
        }

        private List<MockProduct> mockProducts = new List<MockProduct>();

        private void InitializeProducts()
        {
            mockProducts.Clear();
            mockProducts.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", DanhMuc = "Máy tính cầm tay", GiaBan = 680000, TonKho = 10 });
            mockProducts.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", DanhMuc = "Sổ - Vở", GiaBan = 9000, TonKho = 100 });
            mockProducts.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", DanhMuc = "Bút các loại", GiaBan = 5000, TonKho = 500 });
            mockProducts.Add(new MockProduct { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", DanhMuc = "Giấy in - photo", GiaBan = 80000, TonKho = 50 });
            mockProducts.Add(new MockProduct { MaSanPham = 7, TenSanPham = "Giấy in Double A A4 80gsm", DanhMuc = "Giấy in - photo", GiaBan = 95000, TonKho = 30 });
            mockProducts.Add(new MockProduct { MaSanPham = 12, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", DanhMuc = "Bút các loại", GiaBan = 5000, TonKho = 200 });
        }

        private void LoadProductsSelectionGrid(List<MockProduct>? dataSource = null)
        {
            dgvProductsSelection.Rows.Clear();
            var list = dataSource ?? mockProducts;
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
            FilterProducts();
        }

        private void FilterProducts()
        {
            string keyword = txtProductSearch.Text.Trim().ToLower();

            var filtered = mockProducts.Where(p =>
                string.IsNullOrEmpty(keyword) ||
                p.MaSanPham.ToString() == keyword ||
                p.TenSanPham.ToLower().Contains(keyword)
            ).ToList();

            LoadProductsSelectionGrid(filtered);
        }

        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvProductsSelection.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (int.TryParse(rawId, out int id))
                {
                    var prod = mockProducts.FirstOrDefault(p => p.MaSanPham == id);
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

                        tabSelectionContainer.SelectedTab = tabProductDetail;

                        txtSelSoLuong.Focus();
                        txtSelSoLuong.SelectAll();
                    }
                }
            }
        }

        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (int.TryParse(rawId, out int id))
                {
                    var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                    if (item != null)
                    {
                        txtSelMaSP.Text = item.MaSanPham.ToString();
                        txtSelTenSP.Text = item.TenSanPham;
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
            double total = 0;
            foreach (var item in currentDetails)
            {
                total += item.ThanhTien;
                dgvCurrentDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong.ToString("N0"),
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
            lblTotalAmount.Text = $"TỔNG TIỀN TẠM TÍNH: {total.ToString("N0")} đ";
        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút THÊM hoặc SỬA ở Tab 1 trước khi chỉnh sửa sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(rawId) || !int.TryParse(rawId, out int id))
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

            if (!double.TryParse(txtSelGiaNhap.Text, out double price) || price < 0)
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
                currentDetails.Add(new MockOrderDetail
                {
                    MaSanPham = id,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    DonGia = price
                });
            }

            LoadCurrentDetailsGrid();
            btnResetCartForm_Click(this, EventArgs.Empty);
        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút THÊM hoặc SỬA ở Tab 1 trước khi chỉnh sửa sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(rawId) || !int.TryParse(rawId, out int id))
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

        private void btnPOSSearch_Click(object? sender, EventArgs e)
        {
            string maSp = txtSelMaSP.Text.Trim().ToLower();
            string tenSp = txtSelTenSP.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(maSp) && string.IsNullOrEmpty(tenSp))
            {
                LoadProductsSelectionGrid();
                return;
            }

            var filtered = mockProducts.Where(p =>
                (string.IsNullOrEmpty(maSp) || p.MaSanPham.ToString().ToLower().Contains(maSp)) &&
                (string.IsNullOrEmpty(tenSp) || p.TenSanPham.ToLower().Contains(tenSp))
            ).ToList();

            LoadProductsSelectionGrid(filtered);
        }

        private void btnPOSRefresh_Click(object? sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
            LoadProductsSelectionGrid();
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
            string rawId = txtSelMaSP.Text;
            if (!string.IsNullOrEmpty(rawId) && int.TryParse(rawId, out int id))
            {
                var prod = mockProducts.FirstOrDefault(p => p.MaSanPham == id);
                if (prod != null)
                {
                    txtSelMaSP.Text = prod.MaSanPham.ToString();
                    txtSelTenSP.Text = prod.TenSanPham;
                    txtSelGiaNhap.Text = prod.GiaBan.ToString();
                    txtSelSoLuong.Text = "1";
                    tabSelectionContainer.SelectedTab = tabListProducts;
                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
                }
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
