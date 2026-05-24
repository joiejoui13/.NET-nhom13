using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int MaDanhMuc { get; set; }
            public string TenDanhMuc { get; set; } = "";
            public double GiaNhap { get; set; }
            public double GiaBan { get; set; }
            public int SoLuongTon { get; set; }
            public string MoTa { get; set; } = "";
            public string TrangThai { get; set; } = "Đang bán";
            public DateTime NgayTao { get; set; }
        }

        public class MockCategory
        {
            public int MaDanhMuc { get; set; }
            public string TenDanhMuc { get; set; } = "";
        }

        private List<MockProduct> mockProducts = new List<MockProduct>();
        private List<MockCategory> mockCategories = new List<MockCategory>();
        private MockProduct? selectedProduct = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucProductList()
        {
            InitializeComponent();
        }

        private void ucProductList_Load(object sender, EventArgs e)
        {
            InitializeMockCategories();
            InitializeMockProducts();
            
            // Bind Categories ComboBox
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";
            cboDanhMuc.DataSource = mockCategories;

            // Initialize Status ComboBox
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang bán", "Ngưng bán" });
            cboTrangThai.SelectedIndex = 0;

            LoadProductsGrid();
            SetEditState(false);
            
            if (dgvSanPham.Rows.Count > 0)
            {
                SelectProductRow(0);
            }
        }

        private void InitializeMockCategories()
        {
            mockCategories.Clear();
            mockCategories.Add(new MockCategory { MaDanhMuc = 1, TenDanhMuc = "Bút các loại" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 2, TenDanhMuc = "Sổ - Vở" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 3, TenDanhMuc = "Giấy in - photo" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 4, TenDanhMuc = "Bìa - File hồ sơ" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 5, TenDanhMuc = "Dụng cụ học sinh" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 6, TenDanhMuc = "Đồ dùng văn phòng" });
            mockCategories.Add(new MockCategory { MaDanhMuc = 7, TenDanhMuc = "Máy tính cầm tay" });
        }

        private void InitializeMockProducts()
        {
            if (mockProducts.Count > 0) return;

            mockProducts.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", MaDanhMuc = 1, TenDanhMuc = "Bút các loại", GiaNhap = 3000, GiaBan = 5000, SoLuongTon = 1000, MoTa = "Bút quốc dân ngòi 0.5mm", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-2) });
            mockProducts.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Bút dạ quang Deli Macaron", MaDanhMuc = 1, TenDanhMuc = "Bút các loại", GiaNhap = 8000, GiaBan = 12000, SoLuongTon = 300, MoTa = "Bút highlight màu pastel", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-2) });
            mockProducts.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút máy Hồng Hà Nét Hoa", MaDanhMuc = 1, TenDanhMuc = "Bút các loại", GiaNhap = 35000, GiaBan = 45000, SoLuongTon = 150, MoTa = "Bút luyện chữ đẹp", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-1) });
            mockProducts.Add(new MockProduct { MaSanPham = 4, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", MaDanhMuc = 2, TenDanhMuc = "Sổ - Vở", GiaNhap = 6000, GiaBan = 9000, SoLuongTon = 800, MoTa = "Giấy chống lóa mắt", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-3) });
            mockProducts.Add(new MockProduct { MaSanPham = 5, TenSanPham = "Vở ô ly Campus 96 trang", MaDanhMuc = 2, TenDanhMuc = "Sổ - Vở", GiaNhap = 8500, GiaBan = 12000, SoLuongTon = 500, MoTa = "Gáy keo đa lớp siêu bền", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-1) });
            mockProducts.Add(new MockProduct { MaSanPham = 7, TenSanPham = "Giấy in Double A A4 70gsm", MaDanhMuc = 3, TenDanhMuc = "Giấy in - photo", GiaNhap = 65000, GiaBan = 80000, SoLuongTon = 200, MoTa = "Lốc 500 tờ giấy Thái", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-4) });
            mockProducts.Add(new MockProduct { MaSanPham = 12, TenSanPham = "Máy tính Casio FX-580VN X", MaDanhMuc = 7, TenDanhMuc = "Máy tính cầm tay", GiaNhap = 550000, GiaBan = 680000, SoLuongTon = 50, MoTa = "Máy tính khoa học chuẩn GD", TrangThai = "Đang bán", NgayTao = DateTime.Now.AddMonths(-5) });
        }

        private void LoadProductsGrid(List<MockProduct>? dataSource = null)
        {
            dgvSanPham.Rows.Clear();
            var list = dataSource ?? mockProducts;
            foreach (var prod in list)
            {
                dgvSanPham.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.TenDanhMuc,
                    prod.GiaNhap.ToString("N0") + " đ",
                    prod.GiaBan.ToString("N0") + " đ",
                    prod.SoLuongTon.ToString("N0"),
                    prod.TrangThai,
                    prod.NgayTao.ToString("dd/MM/yyyy")
                );
            }
        }

        private void SelectProductRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSanPham.Rows.Count) return;

            dgvSanPham.ClearSelection();
            dgvSanPham.Rows[rowIndex].Selected = true;

            int prodId = Convert.ToInt32(dgvSanPham.Rows[rowIndex].Cells[0].Value);
            selectedProduct = mockProducts.FirstOrDefault(p => p.MaSanPham == prodId);

            if (selectedProduct != null)
            {
                PopulateProductDetails(selectedProduct);
            }
        }

        private void PopulateProductDetails(MockProduct prod)
        {
            txtMaSanPham.Text = prod.MaSanPham.ToString();
            txtTenSanPham.Text = prod.TenSanPham;
            txtGiaNhap.Text = prod.GiaNhap.ToString();
            txtGiaBan.Text = prod.GiaBan.ToString();
            txtSoLuongTon.Text = prod.SoLuongTon.ToString();
            txtMoTa.Text = prod.MoTa;
            cboDanhMuc.SelectedValue = prod.MaDanhMuc;
            cboTrangThai.Text = prod.TrangThai;

            // Update details tab labels
            lblProductDetailName.Text = prod.TenSanPham.ToUpper();
            lblProductDetailPrice.Text = $"Giá bán: {prod.GiaBan.ToString("N0")} VNĐ";
            lblProductDetailStock.Text = $"Số lượng tồn: {prod.SoLuongTon.ToString("N0")}";
            
            lblProductDetailDesc.Text = $"Mã sản phẩm: {prod.MaSanPham}\n" +
                                        $"Danh mục: {prod.TenDanhMuc}\n" +
                                        $"Giá nhập: {prod.GiaNhap.ToString("N0")} VNĐ\n" +
                                        $"Trạng thái: {prod.TrangThai}";
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Product code is read-only
            txtMaSanPham.ReadOnly = true;

            // Input fields read-only state
            txtTenSanPham.ReadOnly = !editing;
            txtGiaNhap.ReadOnly = !editing;
            txtGiaBan.ReadOnly = !editing;
            txtSoLuongTon.ReadOnly = !editing;
            txtMoTa.ReadOnly = !editing;
            cboDanhMuc.Enabled = editing;
            cboTrangThai.Enabled = editing;
            btnChonAnh.Enabled = editing;

            // Buttons state
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
        }

        private void ClearInputs()
        {
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtGiaNhap.Text = "0";
            txtGiaBan.Text = "0";
            txtSoLuongTon.Text = "0";
            txtMoTa.Text = "";
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectProductRow(e.RowIndex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            ClearInputs();

            int nextId = mockProducts.Count > 0 ? mockProducts.Max(p => p.MaSanPham) + 1 : 1;
            txtMaSanPham.Text = nextId.ToString();

            SetEditState(true);
            txtTenSanPham.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAddingNew = false;
            SetEditState(true);
            txtTenSanPham.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{selectedProduct.TenSanPham}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockProducts.Remove(selectedProduct);
                MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProductsGrid();
                
                if (dgvSanPham.Rows.Count > 0)
                {
                    SelectProductRow(0);
                }
                else
                {
                    selectedProduct = null;
                    ClearInputs();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadProductsGrid();
            SetEditState(false);
            if (dgvSanPham.Rows.Count > 0)
            {
                SelectProductRow(0);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtTenSanPham.Text.Trim();
            string desc = txtMoTa.Text.Trim();
            string status = cboTrangThai.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenSanPham.Focus();
                return;
            }

            if (!double.TryParse(txtGiaNhap.Text, out double importPrice) || importPrice < 0)
            {
                MessageBox.Show("Giá nhập phải là số lớn hơn hoặc bằng 0!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNhap.Focus();
                return;
            }

            if (!double.TryParse(txtGiaBan.Text, out double salesPrice) || salesPrice <= 0)
            {
                MessageBox.Show("Giá bán phải là số lớn hơn 0!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaBan.Focus();
                return;
            }

            if (salesPrice < importPrice)
            {
                MessageBox.Show("Giá bán không được nhỏ hơn Giá nhập kho!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaBan.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuongTon.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên lớn hơn hoặc bằng 0!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongTon.Focus();
                return;
            }

            int catId = Convert.ToInt32(cboDanhMuc.SelectedValue);
            string catName = mockCategories.FirstOrDefault(c => c.MaDanhMuc == catId)?.TenDanhMuc ?? "Khác";

            if (isAddingNew)
            {
                int newId = mockProducts.Count > 0 ? mockProducts.Max(p => p.MaSanPham) + 1 : 1;
                var newProduct = new MockProduct
                {
                    MaSanPham = newId,
                    TenSanPham = name,
                    MaDanhMuc = catId,
                    TenDanhMuc = catName,
                    GiaNhap = importPrice,
                    GiaBan = salesPrice,
                    SoLuongTon = stock,
                    MoTa = desc,
                    TrangThai = status,
                    NgayTao = DateTime.Now
                };
                mockProducts.Add(newProduct);
                selectedProduct = newProduct;
                MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedProduct != null)
                {
                    selectedProduct.TenSanPham = name;
                    selectedProduct.MaDanhMuc = catId;
                    selectedProduct.TenDanhMuc = catName;
                    selectedProduct.GiaNhap = importPrice;
                    selectedProduct.GiaBan = salesPrice;
                    selectedProduct.SoLuongTon = stock;
                    selectedProduct.MoTa = desc;
                    selectedProduct.TrangThai = status;
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadProductsGrid();

            // Re-select row
            if (selectedProduct != null)
            {
                int index = mockProducts.IndexOf(selectedProduct);
                if (index >= 0 && index < dgvSanPham.Rows.Count)
                {
                    SelectProductRow(index);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedProduct != null)
            {
                PopulateProductDetails(selectedProduct);
            }
            else if (dgvSanPham.Rows.Count > 0)
            {
                SelectProductRow(0);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Tận dụng các trường thông tin ở Panel nhập liệu bên trái làm điều kiện tìm kiếm/lọc
            string nameTerm = txtTenSanPham.Text.Trim().ToLower();
            int selectedCatId = cboDanhMuc.SelectedValue is int id ? id : -1;
            string statusTerm = cboTrangThai.Text;
            
            // Lọc nâng cao theo Khoảng Giá bán và Tồn kho từ ô nhập tương ứng (nếu có giá trị khác 0)
            double.TryParse(txtGiaBan.Text, out double priceLimit);
            int.TryParse(txtSoLuongTon.Text, out int stockLimit);

            var filtered = mockProducts.Where(p =>
            {
                // Tìm theo từ khóa (tên hoặc mô tả)
                bool matchesName = string.IsNullOrEmpty(nameTerm) || 
                                   p.TenSanPham.ToLower().Contains(nameTerm) || 
                                   p.MoTa.ToLower().Contains(nameTerm);

                // Lọc theo Danh mục
                bool matchesCat = selectedCatId <= 0 || p.MaDanhMuc == selectedCatId;

                // Lọc theo Trạng thái
                bool matchesStatus = string.IsNullOrEmpty(statusTerm) || p.TrangThai == statusTerm;

                // Lọc nâng cao: Giá bán (nếu nhập giá trị lọc > 0, lọc các sản phẩm có giá <= mức này)
                bool matchesPrice = priceLimit <= 0 || p.GiaBan <= priceLimit;

                // Lọc nâng cao: Tồn kho (nếu nhập tồn kho > 0, lọc các sản phẩm có tồn <= mức này)
                bool matchesStock = stockLimit <= 0 || p.SoLuongTon <= stockLimit;

                return matchesName && matchesCat && matchesStatus && matchesPrice && matchesStock;
            }).ToList();

            LoadProductsGrid(filtered);

            if (dgvSanPham.Rows.Count > 0)
            {
                SelectProductRow(0);
                MessageBox.Show($"Tìm thấy {dgvSanPham.Rows.Count} sản phẩm phù hợp!", "Tìm kiếm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                selectedProduct = null;
                ClearInputs();
                MessageBox.Show("Không tìm thấy sản phẩm nào khớp với các tiêu chí tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(ofd.FileName);
                        picProductImage.Image = img;
                        picProductDetailImage.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

