using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();
        private List<Product> _allProducts = new List<Product>();
        private List<Category> _allCategories = new List<Category>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucProductList()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvSanPham.AutoGenerateColumns = false;
            colMaSanPham.DataPropertyName = "MaSanPham";
            colTenSanPham.DataPropertyName = "TenSanPham";
            colMaDanhMuc.DataPropertyName = "MaDanhMuc";
            colGiaNhap.DataPropertyName = "GiaNhap";
            colGiaBan.DataPropertyName = "GiaBan";
            colSoLuongTon.DataPropertyName = "SoLuongTon";
            colTrangThai.DataPropertyName = "TrangThai";
            colNgayTao.DataPropertyName = "NgayTao";
        }

        private async void ucProductList_Load(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            await LoadCategoriesAsync();
            await LoadDataAsync();
        }

        private enum ViewState
        {
            View,
            Add,
            Edit
        }

        private void SetState(ViewState state)
        {
            switch (state)
            {
                case ViewState.View:
                    _isAdding = false;
                    _isEditing = false;

                    txtMaSanPham.Enabled = false;
                    txtTenSanPham.ReadOnly = true;
                    cboDanhMuc.Enabled = false;
                    txtGiaNhap.ReadOnly = true;
                    txtGiaBan.ReadOnly = true;
                    txtSoLuongTon.ReadOnly = true;
                    txtMoTa.ReadOnly = true;
                    cboTrangThai.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvSanPham.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaSanPham.Enabled = true;
                    txtMaSanPham.Text = string.Empty;
                    txtTenSanPham.ReadOnly = false;
                    txtTenSanPham.Text = string.Empty;
                    cboDanhMuc.Enabled = true;
                    if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
                    txtGiaNhap.ReadOnly = false;
                    txtGiaNhap.Text = "0";
                    txtGiaBan.ReadOnly = false;
                    txtGiaBan.Text = "0";
                    txtSoLuongTon.ReadOnly = false;
                    txtSoLuongTon.Text = "0";
                    txtMoTa.ReadOnly = false;
                    txtMoTa.Text = string.Empty;
                    cboTrangThai.Enabled = true;
                    cboTrangThai.SelectedIndex = 0;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvSanPham.Enabled = false;
                    txtMaSanPham.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaSanPham.Enabled = false;
                    txtTenSanPham.ReadOnly = false;
                    cboDanhMuc.Enabled = true;
                    txtGiaNhap.ReadOnly = false;
                    txtGiaBan.ReadOnly = false;
                    txtSoLuongTon.ReadOnly = false;
                    txtMoTa.ReadOnly = false;
                    cboTrangThai.Enabled = true;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvSanPham.Enabled = false;
                    txtTenSanPham.Focus();
                    break;
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var list = await _categoryRepo.GetAllAsync();
                _allCategories = list.ToList();
                cboDanhMuc.DataSource = _allCategories;
                cboDanhMuc.DisplayMember = "TenDanhMuc";
                cboDanhMuc.ValueMember = "MaDanhMuc";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _productRepo.GetAllAsync();
                _allProducts = list.ToList();
                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = _allProducts;

                if (dgvSanPham.Rows.Count > 0)
                {
                    dgvSanPham.Rows[0].Selected = true;
                    PopulateFields(dgvSanPham.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaSanPham.Text = row.Cells["colMaSanPham"].Value?.ToString() ?? string.Empty;
            txtTenSanPham.Text = row.Cells["colTenSanPham"].Value?.ToString() ?? string.Empty;
            
            string maDM = row.Cells["colMaDanhMuc"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(maDM) && _allCategories.Any(x => x.MaDanhMuc == maDM))
            {
                cboDanhMuc.SelectedValue = maDM;
            }

            txtGiaNhap.Text = row.Cells["colGiaNhap"].Value?.ToString() ?? "0";
            txtGiaBan.Text = row.Cells["colGiaBan"].Value?.ToString() ?? "0";
            txtSoLuongTon.Text = row.Cells["colSoLuongTon"].Value?.ToString() ?? "0";
            
            // Find corresponding model to show description
            var code = txtMaSanPham.Text;
            var prod = _allProducts.FirstOrDefault(x => x.MaSanPham == code);
            txtMoTa.Text = prod?.MoTa ?? string.Empty;

            string status = row.Cells["colTrangThai"].Value?.ToString() ?? "Hoạt động";
            cboTrangThai.SelectedItem = status;
        }

        private void ClearFields()
        {
            txtMaSanPham.Text = string.Empty;
            txtTenSanPham.Text = string.Empty;
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
            txtGiaNhap.Text = "0";
            txtGiaBan.Text = "0";
            txtSoLuongTon.Text = "0";
            txtMoTa.Text = string.Empty;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSanPham.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvSanPham.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSanPham.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSanPham.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{txtTenSanPham.Text}' không?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _productRepo.DeleteAsync(txtMaSanPham.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thất bại. Không tìm thấy mã sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            await LoadCategoriesAsync();
            await LoadDataAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSanPham.Text))
            {
                MessageBox.Show("Mã sản phẩm không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSanPham.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return;
            }

            if (cboDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập phải là số lớn hơn hoặc bằng 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out decimal giaBan) || giaBan < 0)
            {
                MessageBox.Show("Giá bán phải là số lớn hơn hoặc bằng 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuongTon.Text, out int soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên lớn hơn hoặc bằng 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return;
            }

            var p = new Product
            {
                MaSanPham = txtMaSanPham.Text.Trim(),
                TenSanPham = txtTenSanPham.Text.Trim(),
                MaDanhMuc = cboDanhMuc.SelectedValue.ToString() ?? string.Empty,
                GiaNhap = giaNhap,
                GiaBan = giaBan,
                SoLuongTon = soLuongTon,
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động",
                NgayTao = DateTime.Now
            };

            try
            {
                if (_isAdding)
                {
                    var existing = await _productRepo.GetByIdAsync(p.MaSanPham);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã sản phẩm này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaSanPham.Focus();
                        return;
                    }

                    int res = await _productRepo.AddAsync(p);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    int res = await _productRepo.UpdateAsync(p);
                    if (res > 0)
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                PopulateFields(dgvSanPham.SelectedRows[0]);
            }
            else
            {
                ClearFields();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvSanPham.DataSource = _allProducts;
            }
            else
            {
                var filtered = _allProducts.Where(x =>
                    x.MaSanPham.ToLower().Contains(keyword) ||
                    x.TenSanPham.ToLower().Contains(keyword) ||
                    (x.MoTa != null && x.MoTa.ToLower().Contains(keyword))
                ).ToList();
                dgvSanPham.DataSource = filtered;
            }
        }
    }
}
