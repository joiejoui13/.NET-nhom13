using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class ucReturns : UserControl
    {
        private readonly ReturnRepository _returnRepo = new ReturnRepository();
        private List<Return> _allReturns = new List<Return>();
        private bool _isAdding = false;

        public ucReturns()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvReturns.AutoGenerateColumns = false;
            colMaTraHang.DataPropertyName = "MaTraHang";
            colMaHoaDon.DataPropertyName = "MaHoaDon";
            colNgayTra.DataPropertyName = "NgayTra";
            colLyDo.DataPropertyName = "LyDo";
            colTongTienHoan.DataPropertyName = "TongTienHoan";
        }

        private async void ucReturns_Load(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            await LoadDataAsync();
        }

        private enum ViewState
        {
            View,
            Add
        }

        private void SetState(ViewState state)
        {
            switch (state)
            {
                case ViewState.View:
                    _isAdding = false;

                    txtMaTraHang.ReadOnly = true;
                    txtMaHoaDon.ReadOnly = true;
                    txtLyDo.ReadOnly = true;
                    txtTongTienHoan.ReadOnly = true;
                    dtpNgayTra.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvReturns.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;

                    txtMaTraHang.ReadOnly = true; // Auto-generated code
                    txtMaTraHang.Text = "TH" + DateTime.Now.ToString("ddMMyyHHmmss");
                    txtMaHoaDon.ReadOnly = false;
                    txtMaHoaDon.Text = string.Empty;
                    txtLyDo.ReadOnly = false;
                    txtLyDo.Text = string.Empty;
                    txtTongTienHoan.ReadOnly = false;
                    txtTongTienHoan.Text = "0";
                    dtpNgayTra.Enabled = true;
                    dtpNgayTra.Value = DateTime.Today;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvReturns.Enabled = false;
                    txtMaHoaDon.Focus();
                    break;
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _returnRepo.GetAllAsync();
                _allReturns = list.ToList();
                dgvReturns.DataSource = null;
                dgvReturns.DataSource = _allReturns;

                if (dgvReturns.Rows.Count > 0)
                {
                    dgvReturns.Rows[0].Selected = true;
                    PopulateFields(dgvReturns.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu trả hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaTraHang.Text = row.Cells["colMaTraHang"].Value?.ToString() ?? string.Empty;
            txtMaHoaDon.Text = row.Cells["colMaHoaDon"].Value?.ToString() ?? string.Empty;
            txtLyDo.Text = row.Cells["colLyDo"].Value?.ToString() ?? string.Empty;
            txtTongTienHoan.Text = row.Cells["colTongTienHoan"].Value?.ToString() ?? "0";

            if (row.Cells["colNgayTra"].Value is DateTime dt)
            {
                dtpNgayTra.Value = dt;
            }
            else
            {
                dtpNgayTra.Value = DateTime.Today;
            }
        }

        private void ClearFields()
        {
            txtMaTraHang.Text = string.Empty;
            txtMaHoaDon.Text = string.Empty;
            txtLyDo.Text = string.Empty;
            txtTongTienHoan.Text = "0";
            dtpNgayTra.Value = DateTime.Today;
        }

        private void dgvReturns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvReturns.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvReturns.Rows[e.RowIndex]);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đơn trả hàng là chứng từ kế toán vĩnh viễn, không thể chỉnh sửa trực tiếp sau khi đã lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTraHang.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn trả hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa đơn trả hàng '{txtMaTraHang.Text}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

                    using (var trans = DbContext.Conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Delete details
                            await DbContext.Conn.ExecuteAsync("DELETE FROM ChiTietTraHang WHERE MaTraHang = @MaTraHang", new { MaTraHang = txtMaTraHang.Text }, trans);
                            // 2. Delete master
                            await DbContext.Conn.ExecuteAsync("DELETE FROM TraHang WHERE MaTraHang = @MaTraHang", new { MaTraHang = txtMaTraHang.Text }, trans);

                            trans.Commit();
                            MessageBox.Show("Xóa đơn trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                    await LoadDataAsync();
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
            if (dgvReturns.SelectedRows.Count > 0)
            {
                PopulateFields(dgvReturns.SelectedRows[0]);
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
                dgvReturns.DataSource = _allReturns;
            }
            else
            {
                var filtered = _allReturns.Where(x =>
                    x.MaTraHang.ToLower().Contains(keyword) ||
                    x.MaHoaDon.ToLower().Contains(keyword) ||
                    (x.LyDo != null && x.LyDo.ToLower().Contains(keyword))
                ).ToList();
                dgvReturns.DataSource = filtered;
            }
        }

        private async void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            string maHD = txtMaHoaDon.Text.Trim();
            if (string.IsNullOrEmpty(maHD) || !_isAdding) return;

            try
            {
                if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
                var total = await DbContext.Conn.QueryFirstOrDefaultAsync<decimal?>("SELECT TongTien FROM HoaDon WHERE MaHoaDon = @MaHoaDon", new { MaHoaDon = maHD });
                if (total.HasValue)
                {
                    txtTongTienHoan.Text = total.Value.ToString("N0");
                }
                else
                {
                    MessageBox.Show("Mã hóa đơn này không tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHoaDon.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maTH = txtMaTraHang.Text.Trim();
            string maHD = txtMaHoaDon.Text.Trim();
            string lyDo = txtLyDo.Text.Trim();

            if (string.IsNullOrWhiteSpace(maTH))
            {
                MessageBox.Show("Mã trả hàng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(maHD))
            {
                MessageBox.Show("Mã hóa đơn không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            if (!decimal.TryParse(txtTongTienHoan.Text, out decimal totalRefund) || totalRefund < 0)
            {
                MessageBox.Show("Tổng tiền hoàn phải là số không âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTongTienHoan.Focus();
                return;
            }

            try
            {
                if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
                
                // Fetch details from invoice to create return details automatically
                string getItemsSql = "SELECT MaSanPham, SoLuong, ThanhTien AS TienHoan FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
                var invoiceItems = (await DbContext.Conn.QueryAsync<ReturnDetail>(getItemsSql, new { MaHoaDon = maHD })).ToList();

                if (invoiceItems.Count == 0)
                {
                    MessageBox.Show("Hóa đơn này không có sản phẩm nào hoặc không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaHoaDon.Focus();
                    return;
                }

                // Prepare return details list
                int count = 1;
                foreach (var item in invoiceItems)
                {
                    item.MaChiTietTra = "CTT" + maTH.Substring(2) + count.ToString("D2");
                    item.MaTraHang = maTH;
                    count++;
                }

                var r = new Return
                {
                    MaTraHang = maTH,
                    MaHoaDon = maHD,
                    NgayTra = dtpNgayTra.Value,
                    LyDo = string.IsNullOrEmpty(lyDo) ? null : lyDo,
                    TongTienHoan = totalRefund,
                    MaNguoiDung = null // Optional
                };

                bool saved = await _returnRepo.SaveReturnTransactionAsync(r, invoiceItems);
                if (saved)
                {
                    MessageBox.Show("Tạo đơn trả hàng và hoàn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetState(ViewState.View);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu trả hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
