using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DTO;
using AssignmentApp.DAL.Repositories.Warehouse;
using System.Drawing;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        private ProductRepository _productRepo;
        private CategoryRepository _categoryRepo;
        private List<Product> _products = new List<Product>();
        private List<Category> _categories = new List<Category>();
        private Product? selectedProduct = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucProductList()
        {
            InitializeComponent();
            _productRepo = new ProductRepository();
            _categoryRepo = new CategoryRepository();
        }

        private async void ucProductList_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu sản phẩm: " + ex.Message);
            }
        }

        private async Task LoadDataAsync()
        {
            var catTask = _categoryRepo.GetAllAsync();
            var prodTask = _productRepo.GetAllAsync();

            await Task.WhenAll(catTask, prodTask);
            _categories = catTask.Result.ToList();
            _products = prodTask.Result.ToList();

            // Bind Categories ComboBox
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";
            cboDanhMuc.DataSource = _categories;

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

        private void LoadProductsGrid(List<Product>? dataSource = null)
        {
            dgvSanPham.Rows.Clear();
            var list = dataSource ?? _products;
            foreach (var prod in list)
            {
                string catName = _categories.FirstOrDefault(c => c.MaDanhMuc == prod.MaDanhMuc)?.TenDanhMuc ?? "Khác";
                dgvSanPham.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    catName,
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

            string prodId = dgvSanPham.Rows[rowIndex].Cells[0].Value.ToString() ?? "";
            selectedProduct = _products.FirstOrDefault(p => p.MaSanPham == prodId);

            if (selectedProduct != null)
            {
                PopulateProductDetails(selectedProduct);
            }
        }

        private void PopulateProductDetails(Product prod)
        {
            txtMaSanPham.Text = prod.MaSanPham;
            txtTenSanPham.Text = prod.TenSanPham;
            txtGiaNhap.Text = prod.GiaNhap.ToString("0");
            txtGiaBan.Text = prod.GiaBan.ToString("0");
            txtSoLuongTon.Text = prod.SoLuongTon.ToString();
            txtMoTa.Text = prod.MoTa;
            cboDanhMuc.SelectedValue = prod.MaDanhMuc;
            cboTrangThai.Text = prod.TrangThai;

            string catName = _categories.FirstOrDefault(c => c.MaDanhMuc == prod.MaDanhMuc)?.TenDanhMuc ?? "Khác";

            // Update details tab labels
            lblProductDetailName.Text = prod.TenSanPham.ToUpper();
            lblProductDetailPrice.Text = $"Giá bán: {prod.GiaBan.ToString("N0")} VNĐ";
            lblProductDetailStock.Text = $"Số lượng tồn: {prod.SoLuongTon.ToString("N0")}";
            
            lblProductDetailDesc.Text = $"Mã sản phẩm: {prod.MaSanPham}\n" +
                                        $"Danh mục: {catName}\n" +
                                        $"Giá nhập: {prod.GiaNhap.ToString("N0")} VNĐ\n" +
                                        $"Trạng thái: {prod.TrangThai}";
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Product code is read-only unless adding new
            txtMaSanPham.ReadOnly = !isAddingNew;

            // Input fields read-only state
            txtTenSanPham.ReadOnly = !editing;
            txtGiaNhap.ReadOnly = !editing;
            txtGiaBan.ReadOnly = !editing;
            txtSoLuongTon.ReadOnly = !editing;
            txtMoTa.ReadOnly = !editing;
            cboDanhMuc.Enabled = editing;
            cboTrangThai.Enabled = editing;
            btnChonAnh.Enabled = editing;

            // Make all buttons visible at all times
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnAdd.Location = new Point(15, 510);
            btnEdit.Location = new Point(115, 510);
            btnDelete.Location = new Point(215, 510);

            btnSave.Location = new Point(15, 555);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 555);
            btnCancel.Size = new Size(140, 36);

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;

            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
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
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
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
            SetEditState(true);
            txtMaSanPham.Focus();
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

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{selectedProduct.TenSanPham}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try {
                    await _productRepo.DeleteAsync(selectedProduct.MaSanPham);
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            await LoadDataAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maSp = txtMaSanPham.Text.Trim();
            string name = txtTenSanPham.Text.Trim();
            string desc = txtMoTa.Text.Trim();
            string status = cboTrangThai.Text;

            if (string.IsNullOrEmpty(maSp))
            {
                MessageBox.Show("Mã sản phẩm không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSanPham.Focus();
                return;
            }
            
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenSanPham.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal importPrice) || importPrice < 0)
            {
                MessageBox.Show("Giá nhập phải là số lớn hơn hoặc bằng 0!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNhap.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out decimal salesPrice) || salesPrice <= 0)
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

            string catId = cboDanhMuc.SelectedValue?.ToString() ?? "";

            var prod = new Product
            {
                MaSanPham = maSp,
                TenSanPham = name,
                MaDanhMuc = catId,
                GiaNhap = importPrice,
                GiaBan = salesPrice,
                SoLuongTon = stock,
                MoTa = desc,
                TrangThai = status,
                NgayTao = DateTime.Now
            };

            try {
                if (isAddingNew)
                {
                    await _productRepo.AddAsync(prod);
                    MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _productRepo.UpdateAsync(prod);
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                isAddingNew = false;
                SetEditState(false);
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string nameTerm = txtTenSanPham.Text.Trim().ToLower();
            string selectedCatId = cboDanhMuc.SelectedValue?.ToString() ?? "";
            string statusTerm = cboTrangThai.Text;
            
            decimal.TryParse(txtGiaBan.Text, out decimal priceLimit);
            int.TryParse(txtSoLuongTon.Text, out int stockLimit);

            var filtered = _products.Where(p =>
            {
                bool matchesName = string.IsNullOrEmpty(nameTerm) || 
                                   p.TenSanPham.ToLower().Contains(nameTerm) || 
                                   (p.MoTa != null && p.MoTa.ToLower().Contains(nameTerm));

                bool matchesCat = string.IsNullOrEmpty(selectedCatId) || p.MaDanhMuc == selectedCatId;
                bool matchesStatus = string.IsNullOrEmpty(statusTerm) || p.TrangThai == statusTerm;
                bool matchesPrice = priceLimit <= 0 || p.GiaBan <= priceLimit;
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
