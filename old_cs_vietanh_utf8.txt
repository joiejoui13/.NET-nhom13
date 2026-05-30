using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;
using AssignmentApp.GUI.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private OrderRepository _orderRepo;
        private List<Order> _currentOrders = new List<Order>();
        private Order _selectedOrder = new Order();
        private bool _isEditing = false;

        public ucOrderManagement(bool defaultToPOS = false)
        {
            InitializeComponent();
            _orderRepo = new OrderRepository();
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
        }

        private async void ucOrderManagement_Load(object sender, EventArgs e)
        {
            SetEditState(false);
            await LoadOrdersAsync();
            
            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
        }

        private async System.Threading.Tasks.Task LoadOrdersAsync(string keyword = "")
        {
            if (string.IsNullOrEmpty(keyword))
                _currentOrders = (await _orderRepo.GetAllAsync()).ToList();
            else
                _currentOrders = (await _orderRepo.SearchAsync(keyword)).ToList();

            dgvOrders.Rows.Clear();
            foreach (var o in _currentOrders)
            {
                dgvOrders.Rows.Add(
                    o.MaHoaDon,
                    o.TenKhachHang ?? "Khách lẻ",
                    o.TenNguoiDung,
                    o.TongTien.ToString("N0"),
                    o.GiamGia.ToString("N0"),
                    o.HinhThucThanhToan,
                    o.TrangThai,
                    o.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    "Đơn bán hàng",
                    o.TrangThai == "Đã huỷ" ? "" : ""
                );
            }
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !_isEditing)
            {
                SelectOrderRow(e.RowIndex);
            }
        }

        private async void SelectOrderRow(int index)
        {
            if (index < 0 || index >= _currentOrders.Count) return;
            _selectedOrder = _currentOrders[index];

            txtMaHoaDon.Text = _selectedOrder.MaHoaDon.ToString();
            txtMaKhachHang.Text = _selectedOrder.TenKhachHang ?? "Khách lẻ";
            txtTenNguoiDung.Text = _selectedOrder.TenNguoiDung;
            txtTongTien.Text = _selectedOrder.TongTien.ToString("N0");
            txtGiamGia.Text = _selectedOrder.GiamGia.ToString("N0");
            txtHinhThucThanhToan.Text = _selectedOrder.HinhThucThanhToan;
            cboTrangThai.Text = _selectedOrder.TrangThai;
            txtNgayTao.Text = _selectedOrder.NgayTao.ToString("dd/MM/yyyy HH:mm");
            cboLoaiHoaDon.Text = "Đơn bán hàng";

            var details = await _orderRepo.GetDetailsAsync(_selectedOrder.MaHoaDon.ToString());
            dgvOrderDetails.Rows.Clear();
            foreach (var d in details)
            {
                dgvOrderDetails.Rows.Add(
                    d.MaSanPham,
                    d.TenSanPham,
                    d.SoLuong,
                    d.DonGia.ToString("N0"),
                    d.ThanhTien.ToString("N0")
                );
            }
        }

        private void SetEditState(bool editing)
        {
            _isEditing = editing;
            cboTrangThai.Enabled = editing;
            txtLyDoHuy.ReadOnly = !editing || cboTrangThai.Text != "Đã huỷ";

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isEditing)
            {
                bool isCanceled = cboTrangThai.Text == "Đã huỷ";
                txtLyDoHuy.ReadOnly = !isCanceled;
                if (!isCanceled) txtLyDoHuy.Text = "";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmPOS pos = new frmPOS();
            pos.ShowDialog();
            _ = LoadOrdersAsync(); // Reload after POS is closed
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_selectedOrder.TrangThai == "Đã hoàn thành" || _selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Không thể cập nhật hóa đơn đã hoàn thành hoặc đã hủy.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetEditState(true);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null) return;
            if (_selectedOrder.TrangThai == "Đã hoàn thành")
            {
                MessageBox.Show("Không thể hủy hóa đơn đã hoàn thành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var result = MessageBox.Show($"Bạn có chắc chắn muốn hủy đơn hàng {_selectedOrder.MaHoaDon} và hoàn lại kho không?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = await _orderRepo.DeleteOrderTransactionAsync(_selectedOrder.MaHoaDon.ToString());
                    if (success)
                    {
                        MessageBox.Show("Hủy hóa đơn thành công. Hàng hóa đã được hoàn lại kho.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadOrdersAsync();
                        if (_currentOrders.Count > 0) SelectOrderRow(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null) return;
            
            string newStatus = cboTrangThai.Text;
            if (newStatus == "Đã huỷ" && string.IsNullOrWhiteSpace(txtLyDoHuy.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do hủy.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (newStatus == "Đã huỷ")
                {
                    await _orderRepo.DeleteOrderTransactionAsync(_selectedOrder.MaHoaDon.ToString());
                    MessageBox.Show("Đã hủy đơn hàng và hoàn kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AssignmentApp.DAL.Core.DbContext.Ketnoi();
                    var conn = AssignmentApp.DAL.Core.DbContext.Conn;
                    string sql = "UPDATE HoaDon SET TrangThai = @Status WHERE MaHoaDon = @MaHoaDon";
                    await Dapper.SqlMapper.ExecuteAsync(conn, sql, new { Status = newStatus, MaHoaDon = _selectedOrder.MaHoaDon });
                    MessageBox.Show("Cập nhật trạng thái thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                SetEditState(false);
                await LoadOrdersAsync();
                
                int idx = _currentOrders.FindIndex(o => o.MaHoaDon == _selectedOrder.MaHoaDon);
                if (idx >= 0) SelectOrderRow(idx);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEditState(false);
            if (_selectedOrder != null) SelectOrderRow(_currentOrders.IndexOf(_selectedOrder));
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadOrdersAsync(); // placeholder
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadOrdersAsync();
        }
    }
}
