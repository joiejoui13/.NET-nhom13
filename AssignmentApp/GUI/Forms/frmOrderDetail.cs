using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.Forms
{
    public partial class frmOrderDetail : Form
    {
        private OrderDetailService _orderDetailService;
        private string _maHoaDon;

        public frmOrderDetail(string maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
            _orderDetailService = new OrderDetailService(new OrderDetailRepository(), new OrderRepository());
            lblTitle.Text = $"CHI TIẾT HÓA ĐƠN: {_maHoaDon}";
        }

        private async void frmOrderDetail_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var details = await _orderDetailService.GetOrderDetailsByOrderIdAsync(_maHoaDon);
                dgvOrderDetails.DataSource = details;

                if (dgvOrderDetails.Columns["MaChiTiet"] != null)
                    dgvOrderDetails.Columns["MaChiTiet"].HeaderText = "Mã Chi Tiết";
                if (dgvOrderDetails.Columns["MaHoaDon"] != null)
                    dgvOrderDetails.Columns["MaHoaDon"].Visible = false;
                if (dgvOrderDetails.Columns["MaSanPham"] != null)
                    dgvOrderDetails.Columns["MaSanPham"].HeaderText = "Sản Phẩm";
                if (dgvOrderDetails.Columns["SoLuong"] != null)
                    dgvOrderDetails.Columns["SoLuong"].HeaderText = "Số Lượng";
                if (dgvOrderDetails.Columns["DonGia"] != null)
                    dgvOrderDetails.Columns["DonGia"].HeaderText = "Đơn Giá";
                if (dgvOrderDetails.Columns["ThanhTien"] != null)
                    dgvOrderDetails.Columns["ThanhTien"].HeaderText = "Thành Tiền";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu chi tiết: " + ex.Message);
            }
        }

        private void dgvOrderDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrderDetails.Rows[e.RowIndex];
                txtMaChiTiet.Text = row.Cells["MaChiTiet"].Value?.ToString();
                txtMaSanPham.Text = row.Cells["MaSanPham"].Value?.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
                txtThanhTien.Text = row.Cells["ThanhTien"].Value?.ToString();
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoLuong.Text, out int sl) || !decimal.TryParse(txtDonGia.Text, out decimal dg))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số lượng và đơn giá.");
                return;
            }

            OrderDetail newDetail = new OrderDetail
            {
                MaHoaDon = _maHoaDon,
                MaSanPham = txtMaSanPham.Text.Trim(),
                SoLuong = sl,
                DonGia = dg
            };

            bool result = await _orderDetailService.AddOrderDetailAsync(newDetail);
            if (result)
            {
                MessageBox.Show("Thêm sản phẩm thành công!");
                ClearInputs();
                _ = LoadData();
            }
            else
            {
                MessageBox.Show("Thêm sản phẩm thất bại.");
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaChiTiet.Text))
            {
                MessageBox.Show("Vui lòng chọn 1 chi tiết để cập nhật.");
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int sl) || !decimal.TryParse(txtDonGia.Text, out decimal dg))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số lượng và đơn giá.");
                return;
            }

            OrderDetail detail = new OrderDetail
            {
                MaChiTiet = txtMaChiTiet.Text,
                MaHoaDon = _maHoaDon,
                MaSanPham = txtMaSanPham.Text.Trim(),
                SoLuong = sl,
                DonGia = dg
            };

            bool result = await _orderDetailService.UpdateOrderDetailAsync(detail);
            if (result)
            {
                MessageBox.Show("Cập nhật thành công!");
                ClearInputs();
                _ = LoadData();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaChiTiet.Text))
            {
                MessageBox.Show("Vui lòng chọn 1 chi tiết để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi Hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool result = await _orderDetailService.DeleteOrderDetailAsync(txtMaChiTiet.Text, _maHoaDon);
                if (result)
                {
                    MessageBox.Show("Xóa thành công!");
                    ClearInputs();
                    _ = LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }
        }

        private void ClearInputs()
        {
            txtMaChiTiet.Clear();
            txtMaSanPham.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
        }
    }
}
