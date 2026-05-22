using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucPromotion : UserControl
    {
        private readonly PromotionRepository _promotionRepo = new PromotionRepository();
        private List<Promotion> _allPromotions = new List<Promotion>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucPromotion()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvPromotion.AutoGenerateColumns = false;
            colMaKhuyenMai.DataPropertyName = "MaKhuyenMai";
            colTenKhuyenMai.DataPropertyName = "TenKhuyenMai";
            colPhanTramGiamGia.DataPropertyName = "PhanTramGiamGia";
            colNgayBatDau.DataPropertyName = "NgayBatDau";
            colNgayHetHan.DataPropertyName = "NgayHetHan";
            colTrangThai.DataPropertyName = "TrangThai";
        }

        private async void ucPromotion_Load(object sender, EventArgs e)
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

                    txtMaKhuyenMai.Enabled = false;
                    txtTenKhuyenMai.ReadOnly = true;
                    txtPhanTramGiamGia.ReadOnly = true;
                    dtNgayBatDau.Enabled = false;
                    dtNgayHetHan.Enabled = false;
                    txtMoTaKhuyenMai.ReadOnly = true;
                    cboTrangThai.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvPromotion.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaKhuyenMai.Enabled = true;
                    txtMaKhuyenMai.Text = string.Empty;
                    txtTenKhuyenMai.ReadOnly = false;
                    txtTenKhuyenMai.Text = string.Empty;
                    txtPhanTramGiamGia.ReadOnly = false;
                    txtPhanTramGiamGia.Text = "0";
                    dtNgayBatDau.Enabled = true;
                    dtNgayBatDau.Value = DateTime.Today;
                    dtNgayHetHan.Enabled = true;
                    dtNgayHetHan.Value = DateTime.Today.AddDays(7);
                    txtMoTaKhuyenMai.ReadOnly = false;
                    txtMoTaKhuyenMai.Text = string.Empty;
                    cboTrangThai.Enabled = true;
                    cboTrangThai.SelectedIndex = 0;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvPromotion.Enabled = false;
                    txtMaKhuyenMai.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaKhuyenMai.Enabled = false;
                    txtTenKhuyenMai.ReadOnly = false;
                    txtPhanTramGiamGia.ReadOnly = false;
                    dtNgayBatDau.Enabled = true;
                    dtNgayHetHan.Enabled = true;
                    txtMoTaKhuyenMai.ReadOnly = false;
                    cboTrangThai.Enabled = true;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvPromotion.Enabled = false;
                    txtTenKhuyenMai.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _promotionRepo.GetAllAsync();
                _allPromotions = list.ToList();
                dgvPromotion.DataSource = null;
                dgvPromotion.DataSource = _allPromotions;

                if (dgvPromotion.Rows.Count > 0)
                {
                    dgvPromotion.Rows[0].Selected = true;
                    PopulateFields(dgvPromotion.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaKhuyenMai.Text = row.Cells["colMaKhuyenMai"].Value?.ToString() ?? string.Empty;
            txtTenKhuyenMai.Text = row.Cells["colTenKhuyenMai"].Value?.ToString() ?? string.Empty;
            txtPhanTramGiamGia.Text = row.Cells["colPhanTramGiamGia"].Value?.ToString() ?? "0";

            if (row.Cells["colNgayBatDau"].Value is DateTime start)
            {
                dtNgayBatDau.Value = start;
            }
            if (row.Cells["colNgayHetHan"].Value is DateTime end)
            {
                dtNgayHetHan.Value = end;
            }

            var promoCode = txtMaKhuyenMai.Text;
            var promo = _allPromotions.FirstOrDefault(x => x.MaKhuyenMai == promoCode);
            txtMoTaKhuyenMai.Text = promo?.MoTaKhuyenMai ?? string.Empty;

            string status = row.Cells["colTrangThai"].Value?.ToString() ?? "Hoạt động";
            cboTrangThai.SelectedItem = status;
        }

        private void ClearFields()
        {
            txtMaKhuyenMai.Text = string.Empty;
            txtTenKhuyenMai.Text = string.Empty;
            txtPhanTramGiamGia.Text = "0";
            dtNgayBatDau.Value = DateTime.Today;
            dtNgayHetHan.Value = DateTime.Today.AddDays(7);
            txtMoTaKhuyenMai.Text = string.Empty;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
        }

        private void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPromotion.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvPromotion.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhuyenMai.Text))
            {
                MessageBox.Show("Vui lòng chọn chương trình khuyến mãi cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhuyenMai.Text))
            {
                MessageBox.Show("Vui lòng chọn chương trình khuyến mãi cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khuyến mãi '{txtTenKhuyenMai.Text}' không?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _promotionRepo.DeleteAsync(txtMaKhuyenMai.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (dgvPromotion.SelectedRows.Count > 0)
            {
                PopulateFields(dgvPromotion.SelectedRows[0]);
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
                dgvPromotion.DataSource = _allPromotions;
            }
            else
            {
                var filtered = _allPromotions.Where(x =>
                    x.MaKhuyenMai.ToLower().Contains(keyword) ||
                    x.TenKhuyenMai.ToLower().Contains(keyword) ||
                    (x.MoTaKhuyenMai != null && x.MoTaKhuyenMai.ToLower().Contains(keyword))
                ).ToList();
                dgvPromotion.DataSource = filtered;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maKM = txtMaKhuyenMai.Text.Trim();
            string tenKM = txtTenKhuyenMai.Text.Trim();

            if (string.IsNullOrWhiteSpace(maKM))
            {
                MessageBox.Show("Mã khuyến mãi không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhuyenMai.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tenKM))
            {
                MessageBox.Show("Tên khuyến mãi không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhuyenMai.Focus();
                return;
            }

            if (!int.TryParse(txtPhanTramGiamGia.Text, out int percent) || percent < 0 || percent > 100)
            {
                MessageBox.Show("Phần trăm giảm giá phải là số nguyên từ 0 đến 100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhanTramGiamGia.Focus();
                return;
            }

            if (dtNgayBatDau.Value.Date > dtNgayHetHan.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày hết hạn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtNgayBatDau.Focus();
                return;
            }

            var p = new Promotion
            {
                MaKhuyenMai = maKM,
                TenKhuyenMai = tenKM,
                PhanTramGiamGia = percent,
                NgayBatDau = dtNgayBatDau.Value,
                NgayHetHan = dtNgayHetHan.Value,
                MoTaKhuyenMai = txtMoTaKhuyenMai.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động"
            };

            try
            {
                if (_isAdding)
                {
                    var existing = await _promotionRepo.GetByIdAsync(p.MaKhuyenMai);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã khuyến mãi này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaKhuyenMai.Focus();
                        return;
                    }

                    int res = await _promotionRepo.AddAsync(p);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    int res = await _promotionRepo.UpdateAsync(p);
                    if (res > 0)
                    {
                        MessageBox.Show("Cập nhật khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
