using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : UserControl
    {
        public class MockDelivery
        {
            public int MaGiaoHang { get; set; }
            public int MaHoaDon { get; set; }
            public string DiaChiGiao { get; set; } = "";
            public string TrangThaiGiao { get; set; } = "Chờ giao";
            public DateTime? NgayGiao { get; set; }
        }

        private List<MockDelivery> mockDeliveries = new List<MockDelivery>();
        private MockDelivery? selectedDelivery = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucDelivery()
        {
            InitializeComponent();
        }

        private void ucDelivery_Load(object sender, EventArgs e)
        {
            InitializeMockDeliveries();

            // Set up Status Combobox
            cboTrangThaiGiao.Items.Clear();
            cboTrangThaiGiao.Items.AddRange(new object[] { "Chờ giao", "Đang giao", "Đã giao", "Đã hủy" });
            cboTrangThaiGiao.SelectedIndex = 0;

            LoadDeliveriesGrid();
            SetEditState(false);

            if (dgvDeliveries.Rows.Count > 0)
            {
                SelectDeliveryRow(0);
            }
        }

        private void InitializeMockDeliveries()
        {
            if (mockDeliveries.Count > 0) return;

            mockDeliveries.Add(new MockDelivery
            {
                MaGiaoHang = 1,
                MaHoaDon = 2,
                DiaChiGiao = "Tòa nhà văn phòng Cầu Giấy, Hà Nội",
                TrangThaiGiao = "Đang giao",
                NgayGiao = null
            });

            mockDeliveries.Add(new MockDelivery
            {
                MaGiaoHang = 2,
                MaHoaDon = 1,
                DiaChiGiao = "Thanh Xuân, Hà Nội",
                TrangThaiGiao = "Đã giao",
                NgayGiao = DateTime.Now.AddDays(-2)
            });
        }

        private void LoadDeliveriesGrid(List<MockDelivery>? dataSource = null)
        {
            dgvDeliveries.Rows.Clear();
            var list = dataSource ?? mockDeliveries;
            foreach (var del in list)
            {
                dgvDeliveries.Rows.Add(
                    del.MaGiaoHang,
                    del.MaHoaDon,
                    del.DiaChiGiao,
                    del.TrangThaiGiao,
                    del.NgayGiao?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa giao"
                );
            }
        }

        private void SelectDeliveryRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDeliveries.Rows.Count) return;

            dgvDeliveries.ClearSelection();
            dgvDeliveries.Rows[rowIndex].Selected = true;

            int deliveryId = Convert.ToInt32(dgvDeliveries.Rows[rowIndex].Cells[0].Value);
            selectedDelivery = mockDeliveries.FirstOrDefault(d => d.MaGiaoHang == deliveryId);

            if (selectedDelivery != null)
            {
                PopulateDeliveryDetails(selectedDelivery);
            }
        }

        private void PopulateDeliveryDetails(MockDelivery del)
        {
            txtMaGiaoHang.Text = del.MaGiaoHang.ToString();
            txtMaHoaDon.Text = del.MaHoaDon.ToString();
            txtDiaChiGiao.Text = del.DiaChiGiao;
            cboTrangThaiGiao.Text = del.TrangThaiGiao;
            if (del.NgayGiao.HasValue)
            {
                dtpNgayGiao.Value = del.NgayGiao.Value;
            }
            else
            {
                dtpNgayGiao.Value = DateTime.Now;
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Identity column is read-only
            txtMaGiaoHang.ReadOnly = true;

            // Toggle input controls
            txtMaHoaDon.ReadOnly = !editing;
            txtDiaChiGiao.ReadOnly = !editing;
            cboTrangThaiGiao.Enabled = editing;
            dtpNgayGiao.Enabled = editing;

            // Toggle buttons
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
        }

        private void ClearInputs()
        {
            txtMaGiaoHang.Text = "";
            txtMaHoaDon.Text = "";
            txtDiaChiGiao.Text = "";
            cboTrangThaiGiao.SelectedIndex = 0;
            dtpNgayGiao.Value = DateTime.Now;
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectDeliveryRow(e.RowIndex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            ClearInputs();

            int nextId = mockDeliveries.Count > 0 ? mockDeliveries.Max(d => d.MaGiaoHang) + 1 : 1;
            txtMaGiaoHang.Text = nextId.ToString();

            SetEditState(true);
            txtMaHoaDon.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedDelivery == null)
            {
                MessageBox.Show("Vui lòng chọn một phiếu giao hàng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAddingNew = false;
            SetEditState(true);
            txtDiaChiGiao.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedDelivery == null)
            {
                MessageBox.Show("Vui lòng chọn một phiếu giao hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Xác nhận xóa phiếu giao hàng #{selectedDelivery.MaGiaoHang}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockDeliveries.Remove(selectedDelivery);
                MessageBox.Show("Xóa phiếu giao hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDeliveriesGrid();

                if (dgvDeliveries.Rows.Count > 0)
                {
                    SelectDeliveryRow(0);
                }
                else
                {
                    selectedDelivery = null;
                    ClearInputs();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ClearInputs();
            LoadDeliveriesGrid();
            SetEditState(false);
            if (dgvDeliveries.Rows.Count > 0)
            {
                SelectDeliveryRow(0);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedDelivery != null)
            {
                PopulateDeliveryDetails(selectedDelivery);
            }
            else if (dgvDeliveries.Rows.Count > 0)
            {
                SelectDeliveryRow(0);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            // Nếu ô tìm kiếm chung trống, ta kiểm tra và lấy tiêu chí từ ô nhập liệu Panel trái
            if (string.IsNullOrEmpty(keyword))
            {
                string orderIdTerm = txtMaHoaDon.Text.Trim();
                string addressTerm = txtDiaChiGiao.Text.Trim().ToLower();

                var filteredInputs = mockDeliveries.Where(d =>
                {
                    bool matchOrder = string.IsNullOrEmpty(orderIdTerm) || d.MaHoaDon.ToString() == orderIdTerm;
                    bool matchAddress = string.IsNullOrEmpty(addressTerm) || d.DiaChiGiao.ToLower().Contains(addressTerm);
                    return matchOrder && matchAddress;
                }).ToList();

                LoadDeliveriesGrid(filteredInputs);
            }
            else
            {
                var filtered = mockDeliveries.Where(d =>
                    d.MaGiaoHang.ToString() == keyword ||
                    d.MaHoaDon.ToString() == keyword ||
                    d.DiaChiGiao.ToLower().Contains(keyword) ||
                    d.TrangThaiGiao.ToLower().Contains(keyword)
                ).ToList();

                LoadDeliveriesGrid(filtered);
            }

            if (dgvDeliveries.Rows.Count > 0)
            {
                SelectDeliveryRow(0);
            }
            else
            {
                selectedDelivery = null;
                ClearInputs();
                MessageBox.Show("Không tìm thấy phiếu giao hàng phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string address = txtDiaChiGiao.Text.Trim();
            string status = cboTrangThaiGiao.Text;

            if (!int.TryParse(txtMaHoaDon.Text, out int orderId) || orderId <= 0)
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên dương hợp lệ!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaHoaDon.Focus();
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Địa chỉ giao hàng không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChiGiao.Focus();
                return;
            }

            DateTime? deliveryDate = null;
            if (status == "Đã giao")
            {
                deliveryDate = dtpNgayGiao.Value;
            }

            if (isAddingNew)
            {
                int newId = mockDeliveries.Count > 0 ? mockDeliveries.Max(d => d.MaGiaoHang) + 1 : 1;
                var newDelivery = new MockDelivery
                {
                    MaGiaoHang = newId,
                    MaHoaDon = orderId,
                    DiaChiGiao = address,
                    TrangThaiGiao = status,
                    NgayGiao = deliveryDate
                };

                mockDeliveries.Add(newDelivery);
                selectedDelivery = newDelivery;
                MessageBox.Show("Thêm mới phiếu giao hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedDelivery != null)
                {
                    selectedDelivery.MaHoaDon = orderId;
                    selectedDelivery.DiaChiGiao = address;
                    selectedDelivery.TrangThaiGiao = status;
                    selectedDelivery.NgayGiao = deliveryDate;
                    MessageBox.Show("Cập nhật phiếu giao hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadDeliveriesGrid();

            // Re-select row
            if (selectedDelivery != null)
            {
                int index = mockDeliveries.IndexOf(selectedDelivery);
                if (index >= 0 && index < dgvDeliveries.Rows.Count)
                {
                    SelectDeliveryRow(index);
                }
            }
        }
    }
}

