using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Core;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : UserControl
    {
        private readonly DeliveryRepository _deliveryRepo = new DeliveryRepository();
        private List<Delivery> _allDeliveries = new List<Delivery>();
        private bool _isAdding = false;
        private bool _isEditing = false;

        public ucDelivery()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvDeliveries.AutoGenerateColumns = false;
            colMaGiaoHang.DataPropertyName = "MaGiaoHang";
            colMaHoaDon.DataPropertyName = "MaHoaDon";
            colDiaChiGiao.DataPropertyName = "DiaChiGiao";
            colTrangThaiGiao.DataPropertyName = "TrangThaiGiao";
            colNgayGiao.DataPropertyName = "NgayGiao";
        }

        private async void ucDelivery_Load(object sender, EventArgs e)
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

                    txtMaGiaoHang.ReadOnly = true;
                    txtMaHoaDon.ReadOnly = true;
                    txtDiaChiGiao.ReadOnly = true;
                    cboTrangThaiGiao.Enabled = false;
                    dtpNgayGiao.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvDeliveries.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;
                    _isEditing = false;

                    txtMaGiaoHang.ReadOnly = true; // Auto-generated code
                    txtMaGiaoHang.Text = "GH" + DateTime.Now.ToString("ddMMyyHHmmss");
                    txtMaHoaDon.ReadOnly = false;
                    txtMaHoaDon.Text = string.Empty;
                    txtDiaChiGiao.ReadOnly = false;
                    txtDiaChiGiao.Text = string.Empty;
                    cboTrangThaiGiao.Enabled = true;
                    cboTrangThaiGiao.SelectedIndex = 0;
                    dtpNgayGiao.Enabled = true;
                    dtpNgayGiao.Value = DateTime.Today;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvDeliveries.Enabled = false;
                    txtMaHoaDon.Focus();
                    break;

                case ViewState.Edit:
                    _isAdding = false;
                    _isEditing = true;

                    txtMaGiaoHang.ReadOnly = true;
                    txtMaHoaDon.ReadOnly = false;
                    txtDiaChiGiao.ReadOnly = false;
                    cboTrangThaiGiao.Enabled = true;
                    dtpNgayGiao.Enabled = true;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvDeliveries.Enabled = false;
                    txtMaHoaDon.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _deliveryRepo.GetAllAsync();
                _allDeliveries = list.ToList();
                dgvDeliveries.DataSource = null;
                dgvDeliveries.DataSource = _allDeliveries;

                if (dgvDeliveries.Rows.Count > 0)
                {
                    dgvDeliveries.Rows[0].Selected = true;
                    PopulateFields(dgvDeliveries.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu giao hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaGiaoHang.Text = row.Cells["colMaGiaoHang"].Value?.ToString() ?? string.Empty;
            txtMaHoaDon.Text = row.Cells["colMaHoaDon"].Value?.ToString() ?? string.Empty;
            txtDiaChiGiao.Text = row.Cells["colDiaChiGiao"].Value?.ToString() ?? string.Empty;

            string status = row.Cells["colTrangThaiGiao"].Value?.ToString() ?? "Chưa giao";
            int idx = cboTrangThaiGiao.Items.IndexOf(status);
            cboTrangThaiGiao.SelectedIndex = idx >= 0 ? idx : 0;

            if (row.Cells["colNgayGiao"].Value is DateTime dt)
            {
                dtpNgayGiao.Value = dt;
            }
            else
            {
                dtpNgayGiao.Value = DateTime.Today;
            }
        }

        private void ClearFields()
        {
            txtMaGiaoHang.Text = string.Empty;
            txtMaHoaDon.Text = string.Empty;
            txtDiaChiGiao.Text = string.Empty;
            if (cboTrangThaiGiao.Items.Count > 0) cboTrangThaiGiao.SelectedIndex = 0;
            dtpNgayGiao.Value = DateTime.Today;
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDeliveries.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvDeliveries.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaGiaoHang.Text))
            {
                MessageBox.Show("Vui lòng chọn vận đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetState(ViewState.Edit);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaGiaoHang.Text))
            {
                MessageBox.Show("Vui lòng chọn vận đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa vận đơn '{txtMaGiaoHang.Text}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _deliveryRepo.DeleteAsync(txtMaGiaoHang.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa vận đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (dgvDeliveries.SelectedRows.Count > 0)
            {
                PopulateFields(dgvDeliveries.SelectedRows[0]);
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
                dgvDeliveries.DataSource = _allDeliveries;
            }
            else
            {
                var filtered = _allDeliveries.Where(x =>
                    x.MaGiaoHang.ToLower().Contains(keyword) ||
                    x.MaHoaDon.ToLower().Contains(keyword) ||
                    (x.DiaChiGiao != null && x.DiaChiGiao.ToLower().Contains(keyword)) ||
                    (x.TrangThaiGiao != null && x.TrangThaiGiao.ToLower().Contains(keyword))
                ).ToList();
                dgvDeliveries.DataSource = filtered;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maGH = txtMaGiaoHang.Text.Trim();
            string maHD = txtMaHoaDon.Text.Trim();
            string diaChi = txtDiaChiGiao.Text.Trim();
            string status = cboTrangThaiGiao.SelectedItem?.ToString() ?? "Chưa giao";

            if (string.IsNullOrWhiteSpace(maGH))
            {
                MessageBox.Show("Mã giao hàng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(maHD))
            {
                MessageBox.Show("Mã hóa đơn không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            // Verify Invoice (MaHoaDon) exists in database
            try
            {
                if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
                int invoiceCount = await DbContext.Conn.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM HoaDon WHERE MaHoaDon = @MaHoaDon", new { MaHoaDon = maHD });
                if (invoiceCount == 0)
                {
                    MessageBox.Show($"Mã hóa đơn '{maHD}' không tồn tại trong hệ thống! Vui lòng kiểm tra lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHoaDon.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var d = new Delivery
            {
                MaGiaoHang = maGH,
                MaHoaDon = maHD,
                DiaChiGiao = string.IsNullOrEmpty(diaChi) ? null : diaChi,
                TrangThaiGiao = status,
                NgayGiao = dtpNgayGiao.Value
            };

            try
            {
                if (_isAdding)
                {
                    var existing = await _deliveryRepo.GetByIdAsync(d.MaGiaoHang);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã vận đơn này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int res = await _deliveryRepo.AddAsync(d);
                    if (res > 0)
                    {
                        // Also update MaGiaoHang field in HoaDon table
                        await DbContext.Conn.ExecuteAsync("UPDATE HoaDon SET MaGiaoHang = @MaGiaoHang WHERE MaHoaDon = @MaHoaDon", new { MaGiaoHang = maGH, MaHoaDon = maHD });

                        MessageBox.Show("Thêm vận đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                    }
                }
                else if (_isEditing)
                {
                    int res = await _deliveryRepo.UpdateAsync(d);
                    if (res > 0)
                    {
                        // Update MaGiaoHang field in HoaDon table
                        await DbContext.Conn.ExecuteAsync("UPDATE HoaDon SET MaGiaoHang = @MaGiaoHang WHERE MaHoaDon = @MaHoaDon", new { MaGiaoHang = maGH, MaHoaDon = maHD });

                        MessageBox.Show("Cập nhật vận đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
