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
    public partial class ucInventory : UserControl
    {
        private readonly InventoryRepository _inventoryRepo = new InventoryRepository();
        private readonly ProductRepository _productRepo = new ProductRepository();
        private List<InventoryLog> _allLogs = new List<InventoryLog>();
        private List<Product> _allProducts = new List<Product>();
        private bool _isAdding = false;

        public ucInventory()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvLichSu.AutoGenerateColumns = false;
            colMaLichSu.DataPropertyName = "MaLichSu";
            colMaSanPham.DataPropertyName = "MaSanPham";
            colTenSanPham.DataPropertyName = "TenSanPham";
            colSoLuongThayDoi.DataPropertyName = "SoLuongThayDoi";
            colLoai.DataPropertyName = "Loai";
            colNgay.DataPropertyName = "Ngay";
        }

        private async void ucInventory_Load(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            await LoadProductsAsync();
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

                    txtMaLichSu.Enabled = false;
                    cboSanPham.Enabled = false;
                    txtSoLuongThayDoi.ReadOnly = true;
                    cboLoaiThayDoi.Enabled = false;

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false; // Adjustment logs cannot be edited historically
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    dgvLichSu.Enabled = true;
                    break;

                case ViewState.Add:
                    _isAdding = true;

                    txtMaLichSu.Enabled = true;
                    txtMaLichSu.Text = string.Empty;
                    cboSanPham.Enabled = true;
                    if (cboSanPham.Items.Count > 0) cboSanPham.SelectedIndex = 0;
                    txtSoLuongThayDoi.ReadOnly = false;
                    txtSoLuongThayDoi.Text = "0";
                    cboLoaiThayDoi.Enabled = true;
                    cboLoaiThayDoi.SelectedIndex = 0;

                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;

                    dgvLichSu.Enabled = false;
                    txtMaLichSu.Focus();
                    break;
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var list = await _productRepo.GetAllAsync();
                _allProducts = list.ToList();
                cboSanPham.DataSource = _allProducts;
                cboSanPham.DisplayMember = "TenSanPham";
                cboSanPham.ValueMember = "MaSanPham";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _inventoryRepo.GetAllAsync();
                _allLogs = list.ToList();
                dgvLichSu.DataSource = null;
                dgvLichSu.DataSource = _allLogs;

                if (dgvLichSu.Rows.Count > 0)
                {
                    dgvLichSu.Rows[0].Selected = true;
                    PopulateFields(dgvLichSu.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lịch sử kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaLichSu.Text = row.Cells["colMaLichSu"].Value?.ToString() ?? string.Empty;
            
            string maSP = row.Cells["colMaSanPham"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(maSP) && _allProducts.Any(x => x.MaSanPham == maSP))
            {
                cboSanPham.SelectedValue = maSP;
            }

            int change = Convert.ToInt32(row.Cells["colSoLuongThayDoi"].Value ?? 0);
            txtSoLuongThayDoi.Text = Math.Abs(change).ToString();

            string type = row.Cells["colLoai"].Value?.ToString() ?? "Nhập kho";
            cboLoaiThayDoi.SelectedItem = type;
        }

        private void ClearFields()
        {
            txtMaLichSu.Text = string.Empty;
            if (cboSanPham.Items.Count > 0) cboSanPham.SelectedIndex = 0;
            txtSoLuongThayDoi.Text = "0";
            if (cboLoaiThayDoi.Items.Count > 0) cboLoaiThayDoi.SelectedIndex = 0;
        }

        private void dgvLichSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichSu.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvLichSu.Rows[e.RowIndex]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetState(ViewState.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Historically inventory logs are read-only
            MessageBox.Show("Không hỗ trợ sửa đổi trực tiếp lịch sử tồn kho đã ghi nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLichSu.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng lịch sử cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa dòng lịch sử '{txtMaLichSu.Text}'? (Hành động này chỉ xóa log và không đảo ngược số lượng tồn)", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int res = await _inventoryRepo.DeleteAsync(txtMaLichSu.Text);
                    if (res > 0)
                    {
                        MessageBox.Show("Xóa dòng lịch sử thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            await LoadProductsAsync();
            await LoadDataAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLichSu.Text))
            {
                MessageBox.Show("Mã lịch sử không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLichSu.Focus();
                return;
            }

            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuongThayDoi.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng thay đổi phải là số nguyên lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongThayDoi.Focus();
                return;
            }

            // Decide signing of quantity based on selected type
            string type = cboLoaiThayDoi.SelectedItem?.ToString() ?? "Nhập kho";
            int signedQty = qty;
            if (type == "Xuất kho bán" || type == "Xuất hủy")
            {
                signedQty = -qty;
            }

            // If subtracting, make sure we have enough stock (optional but let's warning)
            var prodId = cboSanPham.SelectedValue.ToString() ?? "";
            var prod = _allProducts.FirstOrDefault(x => x.MaSanPham == prodId);
            if (prod != null && signedQty < 0 && prod.SoLuongTon + signedQty < 0)
            {
                var confirm = MessageBox.Show($"Số lượng xuất ({qty}) vượt quá số lượng tồn hiện tại ({prod.SoLuongTon}). Bạn vẫn muốn tiếp tục?", 
                    "Cảnh báo xuất quá tồn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;
            }

            var log = new InventoryLog
            {
                MaLichSu = txtMaLichSu.Text.Trim(),
                MaSanPham = prodId,
                SoLuongThayDoi = signedQty,
                Loai = type,
                Ngay = DateTime.Now
            };

            try
            {
                if (_isAdding)
                {
                    var existing = await _inventoryRepo.GetByIdAsync(log.MaLichSu);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã lịch sử này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaLichSu.Focus();
                        return;
                    }

                    int res = await _inventoryRepo.AddAsync(log);
                    if (res > 0)
                    {
                        MessageBox.Show("Thêm lịch sử điều chỉnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetState(ViewState.View);
                        await LoadDataAsync();
                        // Reload products to update the local cached quantities
                        await LoadProductsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi nhận điều chỉnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetState(ViewState.View);
            if (dgvLichSu.SelectedRows.Count > 0)
            {
                PopulateFields(dgvLichSu.SelectedRows[0]);
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
                dgvLichSu.DataSource = _allLogs;
            }
            else
            {
                var filtered = _allLogs.Where(x =>
                    x.MaLichSu.ToLower().Contains(keyword) ||
                    x.MaSanPham.ToLower().Contains(keyword) ||
                    (x.TenSanPham != null && x.TenSanPham.ToLower().Contains(keyword)) ||
                    x.Loai.ToLower().Contains(keyword)
                ).ToList();
                dgvLichSu.DataSource = filtered;
            }
        }
    }
}