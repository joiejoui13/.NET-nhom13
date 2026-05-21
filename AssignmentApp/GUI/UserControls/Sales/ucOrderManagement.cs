using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;
using AssignmentApp.GUI.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : Base.ucBase
    {
        private OrderService _orderService;

        public ucOrderManagement()
        {
            InitializeComponent();
            _orderService = new OrderService(new OrderRepository());
        }

        private async void ucOrderManagement_Load(object sender, EventArgs e)
        {
            cbHinhThucThanhToan.SelectedIndex = 0;
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                dgvOrders.DataSource = orders;

                if (dgvOrders.Columns["MaHoaDon"] != null)
                    dgvOrders.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
                if (dgvOrders.Columns["MaKhachHang"] != null)
                    dgvOrders.Columns["MaKhachHang"].HeaderText = "Khách Hàng";
                if (dgvOrders.Columns["MaNguoiDung"] != null)
                    dgvOrders.Columns["MaNguoiDung"].HeaderText = "Nhân Viên";
                if (dgvOrders.Columns["MaKhuyenMai"] != null)
                    dgvOrders.Columns["MaKhuyenMai"].HeaderText = "Khuyến Mãi";
                if (dgvOrders.Columns["GiamGia"] != null)
                    dgvOrders.Columns["GiamGia"].HeaderText = "Giảm Giá";
                if (dgvOrders.Columns["MaGiaoHang"] != null)
                    dgvOrders.Columns["MaGiaoHang"].HeaderText = "Giao Hàng";
                if (dgvOrders.Columns["TongTien"] != null)
                    dgvOrders.Columns["TongTien"].HeaderText = "Tổng Tiền";
                if (dgvOrders.Columns["HinhThucThanhToan"] != null)
                    dgvOrders.Columns["HinhThucThanhToan"].HeaderText = "Hình Thức TT";
                if (dgvOrders.Columns["NgayTao"] != null)
                    dgvOrders.Columns["NgayTao"].HeaderText = "Ngày Tạo";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];
                txtMaHD.Text = row.Cells["MaHoaDon"].Value?.ToString();
                txtMaKH.Text = row.Cells["MaKhachHang"].Value?.ToString();
                txtMaND.Text = row.Cells["MaNguoiDung"].Value?.ToString();
                txtMaKM.Text = row.Cells["MaKhuyenMai"].Value?.ToString();
                txtGiamGia.Text = row.Cells["GiamGia"].Value?.ToString();
                txtMaGiaoHang.Text = row.Cells["MaGiaoHang"].Value?.ToString();
                txtTongTien.Text = row.Cells["TongTien"].Value?.ToString();

                string hinhThuc = row.Cells["HinhThucThanhToan"].Value?.ToString();
                if (!string.IsNullOrEmpty(hinhThuc))
                {
                    cbHinhThucThanhToan.SelectedItem = hinhThuc;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order
            {
                MaKhachHang = txtMaKH.Text.Trim(),
                MaNguoiDung = txtMaND.Text.Trim(),
                MaKhuyenMai = string.IsNullOrEmpty(txtMaKM.Text) ? null : txtMaKM.Text.Trim(),
                GiamGia = decimal.TryParse(txtGiamGia.Text, out decimal gg) ? gg : 0,
                MaGiaoHang = string.IsNullOrEmpty(txtMaGiaoHang.Text) ? null : txtMaGiaoHang.Text.Trim(),
                TongTien = 0, // Initial total is 0, will be updated via OrderDetail
                HinhThucThanhToan = cbHinhThucThanhToan.SelectedItem?.ToString()
            };

            bool result = await _orderService.AddOrderAsync(newOrder);
            if (result)
            {
                MessageBox.Show("Thêm mới thành công!");
                btnRefresh_Click(null, null);
            }
            else
            {
                MessageBox.Show("Thêm mới thất bại.");
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn Hóa đơn cần cập nhật.");
                return;
            }

            Order updateOrder = new Order
            {
                MaHoaDon = txtMaHD.Text,
                MaKhachHang = txtMaKH.Text.Trim(),
                MaNguoiDung = txtMaND.Text.Trim(),
                MaKhuyenMai = string.IsNullOrEmpty(txtMaKM.Text) ? null : txtMaKM.Text.Trim(),
                GiamGia = decimal.TryParse(txtGiamGia.Text, out decimal gg) ? gg : 0,
                MaGiaoHang = string.IsNullOrEmpty(txtMaGiaoHang.Text) ? null : txtMaGiaoHang.Text.Trim(),
                HinhThucThanhToan = cbHinhThucThanhToan.SelectedItem?.ToString()
            };

            bool result = await _orderService.UpdateOrderAsync(updateOrder);
            if (result)
            {
                MessageBox.Show("Cập nhật thành công!");
                btnRefresh_Click(null, null);
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn Hóa đơn cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Hóa đơn này cùng toàn bộ Chi tiết bên trong?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool result = await _orderService.DeleteOrderAsync(txtMaHD.Text);
                if (result)
                {
                    MessageBox.Show("Xóa thành công!");
                    btnRefresh_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn một Hóa đơn để xem chi tiết.");
                return;
            }

            // Open Order Detail form and pass the MaHoaDon
            frmOrderDetail detailForm = new frmOrderDetail(txtMaHD.Text);
            detailForm.ShowDialog();

            // Refresh grid in case total amount was updated
            _ = LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMaHD.Clear();
            txtMaKH.Clear();
            txtMaND.Clear();
            txtMaKM.Clear();
            txtGiamGia.Text = "0";
            txtMaGiaoHang.Clear();
            txtTongTien.Clear();
            cbHinhThucThanhToan.SelectedIndex = 0;
            _ = LoadData();
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void txtMaGiaoHang_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
