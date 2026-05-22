using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : UserControl
    {
        private readonly CustomerRepository _customerRepo = new CustomerRepository();
        private List<Customer> _allCustomers = new List<Customer>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucCustomer()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvCustomers.AutoGenerateColumns = false;
            colMaKhachHang.DataPropertyName = "MaKhachHang";
            colTenKhachHang.DataPropertyName = "TenKhachHang";
            colSoDienThoai.DataPropertyName = "SoDienThoai";
            colDiemTichLuy.DataPropertyName = "DiemTichLuy";
            colNgayTao.DataPropertyName = "NgayTao";
        }

        private async void ucCustomer_Load(object sender, EventArgs e)
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

                    txtMaKhachHang.ReadOnly = true;
                    txtTenKhachHang.ReadOnly = true;
                    txtSoDienThoai.ReadOnly = true;
                    txtDiemTichLuy.ReadOnly = true;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvCustomers.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaKhachHang.ReadOnly = true; // Auto-generated code
                    txtMaKhachHang.Text = "KH" + DateTime.Now.ToString("ddMMyyHHmmss");
                    txtTenKhachHang.ReadOnly = false;
                    txtTenKhachHang.Text = string.Empty;
                    txtSoDienThoai.ReadOnly = false;
                    txtSoDienThoai.Text = string.Empty;
                    txtDiemTichLuy.ReadOnly = false;
                    txtDiemTichLuy.Text = "0";

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvCustomers.Enabled = false;
                    txtTenKhachHang.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaKhachHang.ReadOnly = true;
                    txtTenKhachHang.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtDiemTichLuy.ReadOnly = false;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvCustomers.Enabled = false;
                    txtTenKhachHang.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _customerRepo.GetAllAsync();
                _allCustomers = list.ToList();
                dgvCustomers.DataSource = null;
                dgvCustomers.DataSource = _allCustomers;

                if (dgvCustomers.Rows.Count > 0)
                {
                    dgvCustomers.Rows[0].Selected = true;
                    PopulateFields(dgvCustomers.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaKhachHang.Text = row.Cells["colMaKhachHang"].Value?.ToString() ?? string.Empty;
            txtTenKhachHang.Text = row.Cells["colTenKhachHang"].Value?.ToString() ?? string.Empty;
            txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString() ?? string.Empty;
            txtDiemTichLuy.Text = row.Cells["colDiemTichLuy"].Value?.ToString() ?? "0";
        }

        private void ClearFields()
        {
            txtMaKhachHang.Text = string.Empty;
            txtTenKhachHang.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtDiemTichLuy.Text = "0";
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvCustomers.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhachHang.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhachHang.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{txtTenKhachHang.Text}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _customerRepo.DeleteAsync(txtMaKhachHang.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                PopulateFields(dgvCustomers.SelectedRows[0]);
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
                dgvCustomers.DataSource = _allCustomers;
            }
            else
            {
                var filtered = _allCustomers.Where(x =>
                    x.MaKhachHang.ToLower().Contains(keyword) ||
                    x.TenKhachHang.ToLower().Contains(keyword) ||
                    (x.SoDienThoai != null && x.SoDienThoai.ToLower().Contains(keyword))
                ).ToList();
                dgvCustomers.DataSource = filtered;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKhachHang.Text.Trim();
            string tenKH = txtTenKhachHang.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(maKH))
            {
                MessageBox.Show("Mã khách hàng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tenKH))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhachHang.Focus();
                return;
            }

            if (!int.TryParse(txtDiemTichLuy.Text, out int diem) || diem < 0)
            {
                MessageBox.Show("Điểm tích lũy phải là số nguyên không âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiemTichLuy.Focus();
                return;
            }

            var c = new Customer
            {
                MaKhachHang = maKH,
                TenKhachHang = tenKH,
                SoDienThoai = string.IsNullOrEmpty(sdt) ? null : sdt,
                DiemTichLuy = diem,
                NgayTao = DateTime.Now
            };

            try
            {
                if (_isAdding)
                {
                    var existing = await _customerRepo.GetByIdAsync(c.MaKhachHang);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã khách hàng này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int res = await _customerRepo.AddAsync(c);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    int res = await _customerRepo.UpdateAsync(c);
                    if (res > 0)
                    {
                        MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
