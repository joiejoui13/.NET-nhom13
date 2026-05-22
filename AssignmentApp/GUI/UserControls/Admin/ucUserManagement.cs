using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucUserManagement : UserControl
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private List<User> _allUsers = new List<User>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucUserManagement()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvUsers.AutoGenerateColumns = false;
            colMaNguoiDung.DataPropertyName = "MaNguoiDung";
            colTenNguoiDung.DataPropertyName = "TenNguoiDung";
            colSoDienThoai.DataPropertyName = "SoDienThoai";
            colEmail.DataPropertyName = "Email";
            colVaiTro.DataPropertyName = "VaiTro";
            colTrangThai.DataPropertyName = "TrangThai";
            colNgayTao.DataPropertyName = "NgayTao";
        }

        private async void ucUserManagement_Load(object sender, EventArgs e)
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

                    txtMaNguoiDung.Enabled = false;
                    txtTenNguoiDung.ReadOnly = true;
                    txtSoDienThoai.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    txtMatKhau.ReadOnly = true;
                    cboVaiTro.Enabled = false;
                    cboTrangThai.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvUsers.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaNguoiDung.Enabled = true;
                    txtMaNguoiDung.Text = string.Empty;
                    txtTenNguoiDung.ReadOnly = false;
                    txtTenNguoiDung.Text = string.Empty;
                    txtSoDienThoai.ReadOnly = false;
                    txtSoDienThoai.Text = string.Empty;
                    txtEmail.ReadOnly = false;
                    txtEmail.Text = string.Empty;
                    txtMatKhau.ReadOnly = false;
                    txtMatKhau.Text = string.Empty;
                    txtMatKhau.PlaceholderText = "Nhập mật khẩu bắt buộc...";
                    cboVaiTro.Enabled = true;
                    cboVaiTro.SelectedIndex = 1; // Default to SALES or ADMIN
                    cboTrangThai.Enabled = true;
                    cboTrangThai.SelectedIndex = 0; // Default to Hoạt động

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvUsers.Enabled = false;
                    txtMaNguoiDung.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaNguoiDung.Enabled = false;
                    txtTenNguoiDung.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    txtMatKhau.ReadOnly = false;
                    txtMatKhau.Text = string.Empty;
                    txtMatKhau.PlaceholderText = "Để trống nếu không muốn đổi mật khẩu...";
                    cboVaiTro.Enabled = true;
                    cboTrangThai.Enabled = true;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvUsers.Enabled = false;
                    txtTenNguoiDung.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _userRepo.GetAllAsync();
                _allUsers = list.ToList();
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = _allUsers;

                if (dgvUsers.Rows.Count > 0)
                {
                    dgvUsers.Rows[0].Selected = true;
                    PopulateFields(dgvUsers.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaNguoiDung.Text = row.Cells["colMaNguoiDung"].Value?.ToString() ?? string.Empty;
            txtTenNguoiDung.Text = row.Cells["colTenNguoiDung"].Value?.ToString() ?? string.Empty;
            txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells["colEmail"].Value?.ToString() ?? string.Empty;
            txtMatKhau.Text = string.Empty; // Don't show hashed password in plain text

            string role = row.Cells["colVaiTro"].Value?.ToString() ?? "SALES";
            cboVaiTro.SelectedItem = role;

            string status = row.Cells["colTrangThai"].Value?.ToString() ?? "Hoạt động";
            cboTrangThai.SelectedItem = status;
        }

        private void ClearFields()
        {
            txtMaNguoiDung.Text = string.Empty;
            txtTenNguoiDung.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMatKhau.Text = string.Empty;
            if (cboVaiTro.Items.Count > 0) cboVaiTro.SelectedIndex = 1;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvUsers.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNguoiDung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNguoiDung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa người dùng '{txtTenNguoiDung.Text}' không?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _userRepo.DeleteAsync(txtMaNguoiDung.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            if (dgvUsers.SelectedRows.Count > 0)
            {
                PopulateFields(dgvUsers.SelectedRows[0]);
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
                dgvUsers.DataSource = _allUsers;
            }
            else
            {
                var filtered = _allUsers.Where(x =>
                    x.MaNguoiDung.ToLower().Contains(keyword) ||
                    x.TenNguoiDung.ToLower().Contains(keyword) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword)) ||
                    (x.Email != null && x.Email.ToLower().Contains(keyword)) ||
                    x.VaiTro.ToLower().Contains(keyword)
                ).ToList();
                dgvUsers.DataSource = filtered;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtMaNguoiDung.Text.Trim();
            string fullName = txtTenNguoiDung.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Mã người dùng (tên tài khoản) không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNguoiDung.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Tên người dùng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguoiDung.Focus();
                return;
            }

            string password = txtMatKhau.Text;
            if (_isAdding && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Mật khẩu là bắt buộc khi thêm mới người dùng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            try
            {
                User existing = await _userRepo.GetByIdAsync(username);

                if (_isAdding)
                {
                    if (existing != null)
                    {
                        MessageBox.Show("Mã người dùng (tên tài khoản) này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaNguoiDung.Focus();
                        return;
                    }

                    var newUser = new User
                    {
                        MaNguoiDung = username,
                        TenNguoiDung = fullName,
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        MatKhau = PasswordHasher.HashPassword(password),
                        VaiTro = cboVaiTro.SelectedItem?.ToString() ?? "SALES",
                        TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động",
                        NgayTao = DateTime.Now
                    };

                    int res = await _userRepo.AddAsync(newUser);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm người dùng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    if (existing == null)
                    {
                        MessageBox.Show("Người dùng không còn tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Prepare updated properties
                    existing.TenNguoiDung = fullName;
                    existing.SoDienThoai = txtSoDienThoai.Text.Trim();
                    existing.Email = txtEmail.Text.Trim();
                    existing.VaiTro = cboVaiTro.SelectedItem?.ToString() ?? "SALES";
                    existing.TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động";

                    // Update password only if a new password is typed
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        existing.MatKhau = PasswordHasher.HashPassword(password);
                    }

                    int res = await _userRepo.UpdateAsync(existing);
                    if (res > 0)
                    {
                        MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
