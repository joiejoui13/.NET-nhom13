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
    public partial class ucCategory : UserControl
    {
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();
        private List<Category> _allCategories = new List<Category>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucCategory()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvDanhMuc.AutoGenerateColumns = false;
            colMaDanhMuc.DataPropertyName = "MaDanhMuc";
            colTenDanhMuc.DataPropertyName = "TenDanhMuc";
            colMoTa.DataPropertyName = "MoTa";
            colNgayTao.DataPropertyName = "NgayTao";
        }

        private async void ucCategory_Load(object sender, EventArgs e)
        {
            SetState(ViewState.View);
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

                    txtMaDanhMuc.Enabled = false;
                    txtTenDanhMuc.ReadOnly = true;
                    txtMoTa.ReadOnly = true;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvDanhMuc.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaDanhMuc.Enabled = true;
                    txtMaDanhMuc.Text = string.Empty;
                    txtTenDanhMuc.ReadOnly = false;
                    txtTenDanhMuc.Text = string.Empty;
                    txtMoTa.ReadOnly = false;
                    txtMoTa.Text = string.Empty;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvDanhMuc.Enabled = false;
                    txtMaDanhMuc.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaDanhMuc.Enabled = false; // Primary key read-only
                    txtTenDanhMuc.ReadOnly = false;
                    txtMoTa.ReadOnly = false;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvDanhMuc.Enabled = false;
                    txtTenDanhMuc.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _categoryRepo.GetAllAsync();
                _allCategories = list.ToList();
                dgvDanhMuc.DataSource = null;
                dgvDanhMuc.DataSource = _allCategories;

                if (dgvDanhMuc.Rows.Count > 0)
                {
                    dgvDanhMuc.Rows[0].Selected = true;
                    PopulateFields(dgvDanhMuc.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaDanhMuc.Text = row.Cells["colMaDanhMuc"].Value?.ToString() ?? string.Empty;
            txtTenDanhMuc.Text = row.Cells["colTenDanhMuc"].Value?.ToString() ?? string.Empty;
            txtMoTa.Text = row.Cells["colMoTa"].Value?.ToString() ?? string.Empty;
        }

        private void ClearFields()
        {
            txtMaDanhMuc.Text = string.Empty;
            txtTenDanhMuc.Text = string.Empty;
            txtMoTa.Text = string.Empty;
        }

        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDanhMuc.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvDanhMuc.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng chọn danh mục cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{txtTenDanhMuc.Text}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _categoryRepo.DeleteAsync(txtMaDanhMuc.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Xóa danh mục thất bại. Không tìm thấy mã danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            await LoadDataAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDanhMuc.Text))
            {
                MessageBox.Show("Mã danh mục không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaDanhMuc.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenDanhMuc.Text))
            {
                MessageBox.Show("Tên danh mục không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return;
            }

            var c = new Category
            {
                MaDanhMuc = txtMaDanhMuc.Text.Trim(),
                TenDanhMuc = txtTenDanhMuc.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                NgayTao = DateTime.Now
            };

            try
            {
                if (_isAdding)
                {
                    // Check if already exists
                    var existing = await _categoryRepo.GetByIdAsync(c.MaDanhMuc);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã danh mục này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaDanhMuc.Focus();
                        return;
                    }

                    int res = await _categoryRepo.AddAsync(c);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    int res = await _categoryRepo.UpdateAsync(c);
                    if (res > 0)
                    {
                        MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Re-populate from grid selection if any
            if (dgvDanhMuc.SelectedRows.Count > 0)
            {
                PopulateFields(dgvDanhMuc.SelectedRows[0]);
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
                dgvDanhMuc.DataSource = _allCategories;
            }
            else
            {
                var filtered = _allCategories.Where(x =>
                    x.MaDanhMuc.ToLower().Contains(keyword) ||
                    x.TenDanhMuc.ToLower().Contains(keyword) ||
                    (x.MoTa != null && x.MoTa.ToLower().Contains(keyword))
                ).ToList();
                dgvDanhMuc.DataSource = filtered;
            }
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
