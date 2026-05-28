using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucStockIn : UserControl
    {
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public string DanhMuc { get; set; } = "";
            public double GiaNhap { get; set; }
        }

        public class MockStockInDetail
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double GiaNhap { get; set; }
            public double ThanhTien => SoLuong * GiaNhap;
        }

        public class MockStockInReceipt
        {
            public int MaPhieuNhap { get; set; }
            public string NguoiTao { get; set; } = "";
            public DateTime NgayNhap { get; set; }
            public string TrangThai { get; set; } = "Chờ xử lý";
            public List<MockStockInDetail> Details { get; set; } = new List<MockStockInDetail>();
        }

        private List<MockProduct> mockProducts = new List<MockProduct>();
        private List<MockStockInReceipt> mockReceipts = new List<MockStockInReceipt>();
        private MockStockInReceipt? selectedReceipt = null;

        // Items currently in the grid during add/edit
        private List<MockStockInDetail> currentDetails = new List<MockStockInDetail>();
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucStockIn()
        {
            InitializeComponent();
        }

        private void ucStockIn_Load(object sender, EventArgs e)
        {
            InitializeProducts();
            InitializeMockReceipts();

            // Dynamic header customization to show receipts instead of products
            lblGridTitle.Text = "DANH SÁCH PHIẾU NHẬP";
            dgvDetails.Columns[0].HeaderText = "Mã Phiếu Nhập";
            dgvDetails.Columns[1].HeaderText = "Người Nhập";
            dgvDetails.Columns[2].HeaderText = "Ngày Nhập";
            dgvDetails.Columns[3].HeaderText = "Trạng Thái";
            dgvDetails.Columns[4].HeaderText = "Tổng Tiền";

            // Wire up cell click dynamically
            dgvDetails.CellClick += dgvDetails_CellClick;

            // Load all receipts in Tab 1 master grid
            LoadReceiptsGrid();

            // Select default receipt
            if (mockReceipts.Count > 0)
            {
                SelectReceiptRow(0);
            }

            SetEditState(false);
        }

        private void InitializeProducts()
        {
            mockProducts.Clear();
            mockProducts.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", DanhMuc = "Máy tính cầm tay", GiaNhap = 600000 });
            mockProducts.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", DanhMuc = "Sổ - Vở", GiaNhap = 6000 });
            mockProducts.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", DanhMuc = "Bút các loại", GiaNhap = 3500 });
            mockProducts.Add(new MockProduct { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", DanhMuc = "Giấy in - photo", GiaNhap = 70000 });
            mockProducts.Add(new MockProduct { MaSanPham = 5, TenSanPham = "Bút máy Hồng Hà Nét Hoa", DanhMuc = "Bút các loại", GiaNhap = 40000 });
            mockProducts.Add(new MockProduct { MaSanPham = 6, TenSanPham = "Sổ da cao cấp A5", DanhMuc = "Sổ - Vở", GiaNhap = 50000 });
            mockProducts.Add(new MockProduct { MaSanPham = 7, TenSanPham = "Bìa còng Thiên Long 7cm", DanhMuc = "Bìa - File hồ sơ", GiaNhap = 25000 });
            mockProducts.Add(new MockProduct { MaSanPham = 8, TenSanPham = "Thước kẻ học sinh Deli 20cm", DanhMuc = "Dụng cụ học sinh", GiaNhap = 4000 });
            mockProducts.Add(new MockProduct { MaSanPham = 9, TenSanPham = "Kéo văn phòng SDI", DanhMuc = "Đồ dùng văn phòng", GiaNhap = 15000 });
        }

        private void InitializeMockReceipts()
        {
            if (mockReceipts.Count > 0) return;

            var r1 = new MockStockInReceipt
            {
                MaPhieuNhap = 101,
                NguoiTao = "Nguyễn Văn Kho",
                NgayNhap = DateTime.Now.AddDays(-5),
                TrangThai = "Đã hoàn thành"
            };
            r1.Details.Add(new MockStockInDetail { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", SoLuong = 50, GiaNhap = 600000 });
            r1.Details.Add(new MockStockInDetail { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuong = 500, GiaNhap = 6000 });

            var r2 = new MockStockInReceipt
            {
                MaPhieuNhap = 102,
                NguoiTao = "Trần Quản Lý",
                NgayNhap = DateTime.Now.AddDays(-2),
                TrangThai = "Chờ xử lý"
            };
            r2.Details.Add(new MockStockInDetail { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuong = 1000, GiaNhap = 3500 });
            r2.Details.Add(new MockStockInDetail { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", SoLuong = 100, GiaNhap = 70000 });

            mockReceipts.Add(r1);
            mockReceipts.Add(r2);
        }

        private void LoadReceiptsGrid(List<MockStockInReceipt>? dataSource = null)
        {
            dgvDetails.Rows.Clear();
            var list = dataSource ?? mockReceipts;
            foreach (var r in list)
            {
                double total = r.Details.Sum(d => d.ThanhTien);
                dgvDetails.Rows.Add(
                    r.MaPhieuNhap,
                    r.NguoiTao,
                    r.NgayNhap.ToString("dd/MM/yyyy HH:mm"),
                    r.TrangThai,
                    total.ToString("N0") + " đ"
                );
            }
        }

        private void SelectReceiptRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDetails.Rows.Count) return;

            dgvDetails.ClearSelection();
            dgvDetails.Rows[rowIndex].Selected = true;

            int receiptId = Convert.ToInt32(dgvDetails.Rows[rowIndex].Cells[0].Value);
            selectedReceipt = mockReceipts.FirstOrDefault(r => r.MaPhieuNhap == receiptId);

            if (selectedReceipt != null)
            {
                txtMaPhieuNhap.Text = selectedReceipt.MaPhieuNhap.ToString();
                txtNguoiDung.Text = selectedReceipt.NguoiTao;
                dtNgayNhap.Value = selectedReceipt.NgayNhap;
                cboTrangThai.Text = selectedReceipt.TrangThai;

                currentDetails = selectedReceipt.Details.Select(d => new MockStockInDetail
                {
                    MaSanPham = d.MaSanPham,
                    TenSanPham = d.TenSanPham,
                    SoLuong = d.SoLuong,
                    GiaNhap = d.GiaNhap
                }).ToList();
            }
        }

        private void dgvDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectReceiptRow(e.RowIndex);
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Header fields
            txtMaPhieuNhap.ReadOnly = !isAddingNew;
            dtNgayNhap.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // Make all buttons visible at all times
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // Position them statically side-by-side
            btnAdd.Location = new Point(15, 470);
            btnEdit.Location = new Point(115, 470);
            btnDelete.Location = new Point(215, 470);

            btnSave.Location = new Point(15, 515);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 515);
            btnCancel.Size = new Size(140, 36);

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;

            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;

            if (editing)
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = selectedReceipt != null;
                btnDelete.Enabled = selectedReceipt != null;
            }
        }

        // ========================================================
        // TAB 1 EVENTS
        // ========================================================

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!isEditing)
            {
                // Enter Add Receipt mode
                isAddingNew = true;
                isEditing = true;

                txtMaPhieuNhap.Text = (mockReceipts.Count > 0 ? mockReceipts.Max(r => r.MaPhieuNhap) + 1 : 101).ToString();
                txtNguoiDung.Text = "Nguyễn Văn Kho";
                dtNgayNhap.Value = DateTime.Now;
                cboTrangThai.Text = "Chờ xử lý";

                currentDetails.Clear();

                SetEditState(true);

                // Auto switch to Tab 2 to pick products after preparing the receipt header
                btnChooseProducts_Click(this, EventArgs.Empty);
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedReceipt == null) return;
            if (selectedReceipt.TrangThai == "Đã hoàn thành" || selectedReceipt.TrangThai == "Đã hủy")
            {
                MessageBox.Show("Không thể chỉnh sửa phiếu nhập đã hoàn thành hoặc đã hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);

            // Auto switch to Tab 2 for editing products list
            btnChooseProducts_Click(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedReceipt == null) return;

            var confirmResult = MessageBox.Show($"Xác nhận xóa phiếu nhập #{selectedReceipt.MaPhieuNhap}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockReceipts.Remove(selectedReceipt);
                MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadReceiptsGrid();

                if (mockReceipts.Count > 0)
                {
                    SelectReceiptRow(0);
                }
                else
                {
                    selectedReceipt = null;
                    txtMaPhieuNhap.Text = "";
                    txtNguoiDung.Text = "";
                    cboTrangThai.SelectedIndex = -1;
                    currentDetails.Clear();
                }

                SetEditState(false);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (currentDetails.Count == 0)
            {
                MessageBox.Show("Phiếu nhập phải có ít nhất một sản phẩm! Hãy chọn sản phẩm ở Tab 2.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isAddingNew)
            {
                if (!int.TryParse(txtMaPhieuNhap.Text, out int id))
                {
                    MessageBox.Show("Mã phiếu nhập phải là số!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaPhieuNhap.Focus();
                    return;
                }

                if (mockReceipts.Any(r => r.MaPhieuNhap == id))
                {
                    MessageBox.Show("Mã phiếu nhập đã tồn tại!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaPhieuNhap.Focus();
                    return;
                }

                var newReceipt = new MockStockInReceipt
                {
                    MaPhieuNhap = id,
                    NguoiTao = txtNguoiDung.Text,
                    NgayNhap = dtNgayNhap.Value,
                    TrangThai = cboTrangThai.Text,
                    Details = currentDetails.Select(d => new MockStockInDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        GiaNhap = d.GiaNhap
                    }).ToList()
                };

                mockReceipts.Add(newReceipt);
                selectedReceipt = newReceipt;

                string invFeedback = "";
                if (newReceipt.TrangThai == "Đã hoàn thành")
                {
                    invFeedback = "\n[TỒN KHO] Số lượng tồn kho của các sản phẩm đã được cộng tăng tương ứng!";
                }

                MessageBox.Show("Thêm mới phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedReceipt != null)
                {
                    string oldStatus = selectedReceipt.TrangThai;
                    selectedReceipt.NgayNhap = dtNgayNhap.Value;
                    selectedReceipt.TrangThai = cboTrangThai.Text;
                    selectedReceipt.Details = currentDetails.Select(d => new MockStockInDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        GiaNhap = d.GiaNhap
                    }).ToList();

                    string invFeedback = "";
                    if (oldStatus != "Đã hoàn thành" && selectedReceipt.TrangThai == "Đã hoàn thành")
                    {
                        invFeedback = "\n[TỒN KHO] Phiếu nhập được chuyển sang 'Đã hoàn thành'. Tồn kho đã được cộng thêm!";
                    }
                    else if (oldStatus == "Đã hoàn thành" && selectedReceipt.TrangThai == "Đã hủy")
                    {
                        invFeedback = "\n[TỒN KHO] Phiếu nhập bị HỦY. Tồn kho đã được hoàn tác trừ lại!";
                    }

                    MessageBox.Show("Cập nhật phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            
            LoadReceiptsGrid();

            if (selectedReceipt != null)
            {
                int index = mockReceipts.IndexOf(selectedReceipt);
                if (index >= 0)
                {
                    SelectReceiptRow(index);
                }
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedReceipt != null)
            {
                int index = mockReceipts.IndexOf(selectedReceipt);
                if (index >= 0)
                {
                    SelectReceiptRow(index);
                }
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaPhieuNhap.Text, out int searchId))
            {
                MessageBox.Show("Vui lòng nhập Mã phiếu nhập (dạng số) cần tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var receipt = mockReceipts.FirstOrDefault(r => r.MaPhieuNhap == searchId);
            if (receipt != null)
            {
                int index = mockReceipts.IndexOf(receipt);
                if (index >= 0)
                {
                    SelectReceiptRow(index);
                }
                SetEditState(false);
            }
            else
            {
                MessageBox.Show("Không tìm thấy phiếu nhập có mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadReceiptsGrid();
            if (mockReceipts.Count > 0)
            {
                SelectReceiptRow(0);
            }
            SetEditState(false);
        }

        private void btnChooseProducts_Click(object? sender, EventArgs e)
        {
            // Switch to Tab 2
            tabMain.SelectedTab = tabChonSanPham;

            // Clear input TextBoxes first
            btnResetCartForm_Click(this, EventArgs.Empty);

            // Load selection lists
            LoadProductsSelectionGrid();
            LoadCurrentDetailsGrid();
        }

        // ========================================================
        // TAB 2 EVENTS (PRODUCT SELECTION)
        // ========================================================

        private void LoadProductsSelectionGrid(List<MockProduct>? dataSource = null)
        {
            dgvProductsSelection.Rows.Clear();
            var list = dataSource ?? mockProducts;
            foreach (var prod in list)
            {
                dgvProductsSelection.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.GiaNhap.ToString("N0") + " đ"
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
                        txtSelGiaNhap.Text = prod.GiaNhap.ToString();

                        // Check if already in cart to display its quantity
                        var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                        if (existing != null)
                        {
                            txtSelSoLuong.Text = existing.SoLuong.ToString();
                            txtSelGiaNhap.Text = existing.GiaNhap.ToString();
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
                        txtSelGiaNhap.Text = item.GiaNhap.ToString();

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
                    item.GiaNhap.ToString("N0") + " đ",
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
                MessageBox.Show("Giá nhập phải lớn hơn hoặc bằng 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong = qty;
                existing.GiaNhap = price;
            }
            else
            {
                currentDetails.Add(new MockStockInDetail
                {
                    MaSanPham = id,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    GiaNhap = price
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
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi phiếu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Sản phẩm này chưa được thêm vào phiếu nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
        }

        private void btnStockInSearch_Click(object? sender, EventArgs e)
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

        private void btnStockInRefresh_Click(object? sender, EventArgs e)
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
                // Optional loading of product detail visual picture or spec card
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
                    txtSelGiaNhap.Text = prod.GiaNhap.ToString();
                    txtSelSoLuong.Text = "1";
                    tabSelectionContainer.SelectedTab = tabListProducts; // shift back to list
                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
                }
            }
        }

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            // Switch back to Tab 1
            tabMain.SelectedTab = tabPhieuNhap;
        }
    }
}
