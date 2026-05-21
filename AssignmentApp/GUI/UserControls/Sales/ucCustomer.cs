using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : Base.ucBase
    {
        private CustomerService _customerService;

        public ucCustomer()
        {
            InitializeComponent();
            _customerService = new CustomerService(new CustomerRepository());
        }

        private async void ucCustomer_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                dgvCustomers.DataSource = customers;

                // Format DataGridView
                if (dgvCustomers.Columns["MaKhachHang"] != null)
                    dgvCustomers.Columns["MaKhachHang"].HeaderText = "Mã KH";
                if (dgvCustomers.Columns["TenKhachHang"] != null)
                    dgvCustomers.Columns["TenKhachHang"].HeaderText = "Tên Khách Hàng";
                if (dgvCustomers.Columns["SoDienThoai"] != null)
                    dgvCustomers.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                if (dgvCustomers.Columns["DiemTichLuy"] != null)
                    dgvCustomers.Columns["DiemTichLuy"].HeaderText = "Điểm Tích Lũy";
                if (dgvCustomers.Columns["NgayTao"] != null)
                    dgvCustomers.Columns["NgayTao"].HeaderText = "Ngày Tạo";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKhachHang"].Value?.ToString();
                txtTenKH.Text = row.Cells["TenKhachHang"].Value?.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtDiem.Text = row.Cells["DiemTichLuy"].Value?.ToString();
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            int diem = 0;
            int.TryParse(txtDiem.Text, out diem);

            Customer newCustomer = new Customer
            {
                TenKhachHang = txtTenKH.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                DiemTichLuy = diem
            };

            bool result = await _customerService.AddCustomerAsync(newCustomer);
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
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật.");
                return;
            }

            int diem = 0;
            int.TryParse(txtDiem.Text, out diem);

            Customer updateCustomer = new Customer
            {
                MaKhachHang = txtMaKH.Text,
                TenKhachHang = txtTenKH.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                DiemTichLuy = diem
            };

            bool result = await _customerService.UpdateCustomerAsync(updateCustomer);
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
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtDiem.Clear();
            _ = LoadData();
        }

        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
