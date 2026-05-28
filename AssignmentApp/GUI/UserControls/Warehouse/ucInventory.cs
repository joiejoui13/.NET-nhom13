using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucInventory : UserControl
    {
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuongTon { get; set; }
        }

        public class MockInventoryLog
        {
            public int MaLichSu { get; set; }
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public DateTime Thoigian { get; set; }
            public int ThayDoi { get; set; }
            public int SoLuongTruoc { get; set; }
            public int SoLuongSau { get; set; }
            public string LoaiGiaoDich { get; set; } = ""; // "Nhập kho", "Xuất kho bán", "Xuất hủy"
            public int MaThamChieu { get; set; }
            public string TrangThai { get; set; } = "Đang hoạt động"; // "Đang hoạt động", "Đã khóa"
        }

        private List<MockProduct> mockProducts = new List<MockProduct>();
        private List<MockInventoryLog> mockLogs = new List<MockInventoryLog>();
        private MockInventoryLog? selectedLog = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucInventory()
        {
            InitializeComponent();
        }

        private void ucInventory_Load(object sender, EventArgs e)
        {
            InitializeProducts();
            InitializeMockLogs();

            // Set up ComboBox bindings
            cboSanPham.DisplayMember = "TenSanPham";
            cboSanPham.ValueMember = "MaSanPham";
            cboSanPham.DataSource = mockProducts;

            LoadLogsGrid();
            SetEditState(false);

            if (dgvLichSu.Rows.Count > 0)
            {
                SelectLogRow(0);
            }
        }

        private void InitializeProducts()
        {
            mockProducts.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", SoLuongTon = 120 });
            mockProducts.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuongTon = 850 });
            mockProducts.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuongTon = 1500 });
        }

        private void InitializeMockLogs()
        {
            if (mockLogs.Count > 0) return;

            mockLogs.Add(new MockInventoryLog
            {
                MaLichSu = 1,
                MaSanPham = 1,
                TenSanPham = "Máy tính Casio FX-580VN X",
                Thoigian = DateTime.Now.AddDays(-5),
                ThayDoi = 50,
                SoLuongTruoc = 70,
                SoLuongSau = 120,
                LoaiGiaoDich = "Nhập kho",
                MaThamChieu = 101,
                TrangThai = "Đang hoạt động"
            });

            mockLogs.Add(new MockInventoryLog
            {
                MaLichSu = 2,
                MaSanPham = 2,
                TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang",
                Thoigian = DateTime.Now.AddDays(-5),
                ThayDoi = 500,
                SoLuongTruoc = 350,
                SoLuongSau = 850,
                LoaiGiaoDich = "Nhập kho",
                MaThamChieu = 101,
                TrangThai = "Đang hoạt động"
            });

            mockLogs.Add(new MockInventoryLog
            {
                MaLichSu = 3,
                MaSanPham = 3,
                TenSanPham = "Bút bi Thiên Long TL-027 Xanh",
                Thoigian = DateTime.Now.AddDays(-2),
                ThayDoi = -150,
                SoLuongTruoc = 1650,
                SoLuongSau = 1500,
                LoaiGiaoDich = "Xuất kho bán",
                MaThamChieu = 5002,
                TrangThai = "Đang hoạt động"
            });
        }

        private void LoadLogsGrid(List<MockInventoryLog>? dataSource = null)
        {
            dgvLichSu.Rows.Clear();
            var list = dataSource ?? mockLogs;
            foreach (var log in list)
            {
                dgvLichSu.Rows.Add(
                    log.MaLichSu,
                    log.MaSanPham,
                    log.TenSanPham,
                    (log.ThayDoi > 0 ? "+" : "") + log.ThayDoi,
                    log.SoLuongTruoc,
                    log.SoLuongSau,
                    log.LoaiGiaoDich,
                    log.MaThamChieu,
                    log.TrangThai,
                    log.Thoigian.ToString("dd/MM/yyyy HH:mm")
                );
            }
        }

        private void SelectLogRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvLichSu.Rows.Count) return;

            dgvLichSu.ClearSelection();
            dgvLichSu.Rows[rowIndex].Selected = true;

            int logId = Convert.ToInt32(dgvLichSu.Rows[rowIndex].Cells[0].Value);
            selectedLog = mockLogs.FirstOrDefault(l => l.MaLichSu == logId);

            if (selectedLog != null)
            {
                PopulateLogDetails(selectedLog);
            }
        }

        private void PopulateLogDetails(MockInventoryLog log)
        {
            txtMaLichSu.Text = log.MaLichSu.ToString();
            txtMaThamChieu.Text = log.MaThamChieu.ToString();
            cboSanPham.SelectedValue = log.MaSanPham;
            txtSoLuongThayDoi.Text = log.ThayDoi.ToString();
            cboLoaiThayDoi.Text = log.LoaiGiaoDich;
            txtSoLuongTruoc.Text = log.SoLuongTruoc.ToString();
            txtSoLuongSau.Text = log.SoLuongSau.ToString();
            cboTrangThai.Text = log.TrangThai;
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Read-only logic
            txtMaLichSu.ReadOnly = true; // Auto-generated
            txtSoLuongTruoc.ReadOnly = true; // Computed
            txtSoLuongSau.ReadOnly = true; // Computed

            // Editable only during adding/editing
            txtMaThamChieu.ReadOnly = !editing;
            cboSanPham.Enabled = editing;
            txtSoLuongThayDoi.ReadOnly = !editing;
            cboLoaiThayDoi.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // Buttons
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
        }

        private void ClearInputs()
        {
            txtMaLichSu.Text = "";
            txtMaThamChieu.Text = "";
            cboSanPham.SelectedIndex = mockProducts.Count > 0 ? 0 : -1;
            txtSoLuongThayDoi.Text = "0";
            cboLoaiThayDoi.SelectedIndex = 0;
            txtSoLuongTruoc.Text = "0";
            txtSoLuongSau.Text = "0";
            cboTrangThai.SelectedIndex = 0;
        }

        private void dgvLichSu_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectLogRow(e.RowIndex);
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            isAddingNew = true;
            ClearInputs();

            int nextId = mockLogs.Count > 0 ? mockLogs.Max(l => l.MaLichSu) + 1 : 1;
            txtMaLichSu.Text = nextId.ToString();
            txtMaThamChieu.Text = "0"; // manual adjustment code

            // Auto-compute stock levels based on selected product
            UpdateComputedStock();

            SetEditState(true);
            txtSoLuongThayDoi.Focus();
        }

        private void UpdateComputedStock()
        {
            if (cboSanPham.SelectedValue is int prodId)
            {
                var prod = mockProducts.FirstOrDefault(p => p.MaSanPham == prodId);
                if (prod != null)
                {
                    txtSoLuongTruoc.Text = prod.SoLuongTon.ToString();
                    if (int.TryParse(txtSoLuongThayDoi.Text, out int change))
                    {
                        txtSoLuongSau.Text = (prod.SoLuongTon + change).ToString();
                    }
                    else
                    {
                        txtSoLuongSau.Text = prod.SoLuongTon.ToString();
                    }
                }
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedLog == null)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi lịch sử kho để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (selectedLog.TrangThai == "Đã khóa")
            {
                MessageBox.Show("Bản ghi lịch sử này đã bị khóa hệ thống, không thể chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedLog == null)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi lịch sử để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedLog.TrangThai == "Đã khóa")
            {
                MessageBox.Show("Bản ghi lịch sử kho này đã khóa, không thể xóa bỏ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Xác nhận xóa bản ghi lịch sử #{selectedLog.MaLichSu}?\nThao tác này có thể ảnh hưởng đến kiểm kê tồn kho.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                mockLogs.Remove(selectedLog);
                MessageBox.Show("Xóa bản ghi lịch sử thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLogsGrid();

                if (dgvLichSu.Rows.Count > 0)
                {
                    SelectLogRow(0);
                }
                else
                {
                    selectedLog = null;
                    ClearInputs();
                }
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadLogsGrid();
            SetEditState(false);
            if (dgvLichSu.Rows.Count > 0)
            {
                SelectLogRow(0);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedLog != null)
            {
                PopulateLogDetails(selectedLog);
            }
            else if (dgvLichSu.Rows.Count > 0)
            {
                SelectLogRow(0);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            // Search by MaLichSu or MaThamChieu matching values
            string idTerm = txtMaLichSu.Text.Trim();
            string refTerm = txtMaThamChieu.Text.Trim();

            if (string.IsNullOrEmpty(idTerm) && string.IsNullOrEmpty(refTerm))
            {
                MessageBox.Show("Vui lòng nhập Mã lịch sử hoặc Mã tham chiếu cần tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filtered = mockLogs.Where(l =>
            {
                bool match = false;
                if (!string.IsNullOrEmpty(idTerm) && int.TryParse(idTerm, out int logId))
                {
                    match = match || l.MaLichSu == logId;
                }
                if (!string.IsNullOrEmpty(refTerm) && int.TryParse(refTerm, out int refId))
                {
                    match = match || l.MaThamChieu == refId;
                }
                return match;
            }).ToList();

            LoadLogsGrid(filtered);

            if (dgvLichSu.Rows.Count > 0)
            {
                SelectLogRow(0);
            }
            else
            {
                selectedLog = null;
                ClearInputs();
                MessageBox.Show("Không tìm thấy bản ghi lịch sử phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtSoLuongThayDoi.Text, out int change) || change == 0)
            {
                MessageBox.Show("Số lượng thay đổi phải khác 0!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongThayDoi.Focus();
                return;
            }

            if (!int.TryParse(txtMaThamChieu.Text, out int refId))
            {
                MessageBox.Show("Mã tham chiếu phải là số!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaThamChieu.Focus();
                return;
            }

            if (cboSanPham.SelectedValue is int prodId && cboSanPham.SelectedItem is MockProduct prod)
            {
                int before = prod.SoLuongTon;
                int after = before + change;

                if (after < 0)
                {
                    MessageBox.Show("Tồn kho sau khi điều chỉnh không thể nhỏ hơn 0!", "Lỗi tồn kho âm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSoLuongThayDoi.Focus();
                    return;
                }

                if (isAddingNew)
                {
                    int newId = mockLogs.Count > 0 ? mockLogs.Max(l => l.MaLichSu) + 1 : 1;
                    var newLog = new MockInventoryLog
                    {
                        MaLichSu = newId,
                        MaSanPham = prodId,
                        TenSanPham = prod.TenSanPham,
                        Thoigian = DateTime.Now,
                        ThayDoi = change,
                        SoLuongTruoc = before,
                        SoLuongSau = after,
                        LoaiGiaoDich = cboLoaiThayDoi.Text,
                        MaThamChieu = refId,
                        TrangThai = cboTrangThai.Text
                    };

                    mockLogs.Add(newLog);
                    selectedLog = newLog;

                    // Update product mock stock level
                    prod.SoLuongTon = after;

                    MessageBox.Show($"Thêm bản ghi lịch sử kho thành công!\nSố lượng tồn kho mới của '{prod.TenSanPham}' là: {after}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (selectedLog != null)
                    {
                        // Roll back previous change
                        var originalProduct = mockProducts.FirstOrDefault(p => p.MaSanPham == selectedLog.MaSanPham);
                        if (originalProduct != null)
                        {
                            originalProduct.SoLuongTon -= selectedLog.ThayDoi;
                        }

                        // Apply new change
                        var newProduct = mockProducts.FirstOrDefault(p => p.MaSanPham == prodId);
                        if (newProduct != null)
                        {
                            selectedLog.MaSanPham = prodId;
                            selectedLog.TenSanPham = newProduct.TenSanPham;
                            selectedLog.SoLuongTruoc = newProduct.SoLuongTon;
                            selectedLog.ThayDoi = change;
                            selectedLog.SoLuongSau = newProduct.SoLuongTon + change;

                            newProduct.SoLuongTon = selectedLog.SoLuongSau;
                        }

                        selectedLog.LoaiGiaoDich = cboLoaiThayDoi.Text;
                        selectedLog.MaThamChieu = refId;
                        selectedLog.TrangThai = cboTrangThai.Text;

                        MessageBox.Show("Cập nhật bản ghi điều chỉnh kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadLogsGrid();

            // Re-select row
            if (selectedLog != null)
            {
                int index = mockLogs.IndexOf(selectedLog);
                if (index >= 0 && index < dgvLichSu.Rows.Count)
                {
                    SelectLogRow(index);
                }
            }
        }
    }
}