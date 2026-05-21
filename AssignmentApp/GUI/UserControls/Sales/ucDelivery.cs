using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : Base.ucBase
    {
        private DeliveryService _deliveryService;

        public ucDelivery()
        {
            InitializeComponent();
            _deliveryService = new DeliveryService(new DeliveryRepository());
        }

        private async void ucDelivery_Load(object sender, EventArgs e)
        {
            cbTrangThai.SelectedIndex = 0;
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveriesAsync();
                dgvDeliveries.DataSource = deliveries;

                if (dgvDeliveries.Columns["MaGiaoHang"] != null)
                    dgvDeliveries.Columns["MaGiaoHang"].HeaderText = "Mã Giao Hàng";
                if (dgvDeliveries.Columns["MaHoaDon"] != null)
                    dgvDeliveries.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
                if (dgvDeliveries.Columns["DiaChiGiao"] != null)
                    dgvDeliveries.Columns["DiaChiGiao"].HeaderText = "Địa Chỉ Giao";
                if (dgvDeliveries.Columns["TrangThaiGiao"] != null)
                    dgvDeliveries.Columns["TrangThaiGiao"].HeaderText = "Trạng Thái";
                if (dgvDeliveries.Columns["NgayGiao"] != null)
                    dgvDeliveries.Columns["NgayGiao"].HeaderText = "Ngày Giao";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDeliveries.Rows[e.RowIndex];
                txtMaGH.Text = row.Cells["MaGiaoHang"].Value?.ToString();
                txtMaHD.Text = row.Cells["MaHoaDon"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChiGiao"].Value?.ToString();

                string trangThai = row.Cells["TrangThaiGiao"].Value?.ToString();
                if (!string.IsNullOrEmpty(trangThai))
                {
                    cbTrangThai.SelectedItem = trangThai;
                }

                var ngayGiao = row.Cells["NgayGiao"].Value;
                if (ngayGiao != null && ngayGiao != DBNull.Value)
                {
                    dtpNgayGiao.Value = Convert.ToDateTime(ngayGiao);
                }
                else
                {
                    dtpNgayGiao.Value = DateTime.Now;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn.");
                return;
            }

            Delivery newDelivery = new Delivery
            {
                MaHoaDon = txtMaHD.Text.Trim(),
                DiaChiGiao = txtDiaChi.Text.Trim(),
                TrangThaiGiao = cbTrangThai.SelectedItem?.ToString() ?? "Chưa giao",
                NgayGiao = null // Add new delivery usually doesn't have delivery date yet unless it's immediately delivered
            };

            // If user explicitly sets it to "Đã giao" on creation
            if (newDelivery.TrangThaiGiao == "Đã giao")
            {
                newDelivery.NgayGiao = dtpNgayGiao.Value;
            }

            bool result = await _deliveryService.AddDeliveryAsync(newDelivery);
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
            if (string.IsNullOrWhiteSpace(txtMaGH.Text))
            {
                MessageBox.Show("Vui lòng chọn phiếu giao hàng cần cập nhật.");
                return;
            }

            Delivery updateDelivery = new Delivery
            {
                MaGiaoHang = txtMaGH.Text,
                MaHoaDon = txtMaHD.Text.Trim(),
                DiaChiGiao = txtDiaChi.Text.Trim(),
                TrangThaiGiao = cbTrangThai.SelectedItem?.ToString()
            };

            if (updateDelivery.TrangThaiGiao == "Đã giao")
            {
                updateDelivery.NgayGiao = dtpNgayGiao.Value;
            }
            else
            {
                updateDelivery.NgayGiao = null;
            }

            bool result = await _deliveryService.UpdateDeliveryAsync(updateDelivery);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMaGH.Clear();
            txtMaHD.Clear();
            txtDiaChi.Clear();
            cbTrangThai.SelectedIndex = 0;
            dtpNgayGiao.Value = DateTime.Now;
            _ = LoadData();
        }

        private void dgvDeliveries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void cbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
