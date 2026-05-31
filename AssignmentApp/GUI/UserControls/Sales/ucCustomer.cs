using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : UserControl
    {
        private readonly BLL.Services.Sales.ICustomerService _customerService;
        private bool isAddingNew = false;

        public ucCustomer()
        {
            InitializeComponent();
            _customerService = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<BLL.Services.Sales.ICustomerService>(Program.ServiceProvider);
        }

        private async void ucCustomer_Load(object sender, EventArgs e)
        {
            dtpNgayTao.ValueChanged -= dtpNgayTao_ValueChanged;
            dtpNgayTao.ValueChanged += dtpNgayTao_ValueChanged;
            await LoadDataAsync();
            ResetState();
        }

        private void dtpNgayTao_ValueChanged(object sender, EventArgs e)
        {
            if (dtpNgayTao.CustomFormat == " ")
            {
                dtpNgayTao.Format = DateTimePickerFormat.Short;
            }
        }

        private async Task LoadDataAsync()
        {
            dgvCustomers.Rows.Clear();
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                foreach (var c in customers)
                {
                    string ngayTao = c.NgayTao != DateTime.MinValue ? c.NgayTao.ToString("dd/MM/yyyy") : "";
                    dgvCustomers.Rows.Add(c.MaKhachHang.ToString(), c.TenKhachHang, c.SoDienThoai ?? "", c.Email ?? "", c.DiaChi ?? "", ngayTao, c.TrangThai ?? "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void ResetValues()
        {
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            dtpNgayTao.Format = DateTimePickerFormat.Short;
            dtpNgayTao.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenKhachHang.Enabled = isEnabled;
            txtSoDienThoai.Enabled = isEnabled;
            txtEmail.Enabled = isEnabled;
            txtDiaChi.Enabled = isEnabled;
            dtpNgayTao.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        private void ResetState()
        {
            isAddingNew = false;
            ResetValues();

            txtMaKhachHang.Enabled = false;
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["colMaKhachHang"].Value?.ToString();
                txtTenKhachHang.Text = row.Cells["colTenKhachHang"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["colEmail"].Value?.ToString();
                txtDiaChi.Text = row.Cells["colDiaChi"].Value?.ToString();
                cboTrangThai.Text = row.Cells["colTrangThai"].Value?.ToString();
                
                try
                {
                    string ngayTaoStr = row.Cells["colNgayTao"].Value?.ToString();
                    if (!string.IsNullOrEmpty(ngayTaoStr))
                    {
                        dtpNgayTao.Value = DateTime.ParseExact(ngayTaoStr, "dd/MM/yyyy", null);
                    }
                } catch { }

                ToggleInputs(true);
                txtMaKhachHang.Enabled = false;

                btnAdd.Enabled = false;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                btnSave.Enabled = false;
                btnCancel.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            ResetValues();
            ToggleInputs(true);

            txtMaKhachHang.Text = "Tự động sinh";
            txtMaKhachHang.Enabled = false; 
            dtpNgayTao.Enabled = false; 
            cboTrangThai.Text = "Hoạt động"; 

            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            
            txtTenKhachHang.Focus();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text) || isAddingNew || txtMaKhachHang.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa!");
                return;
            }

            var customer = new DTO.Customer
            {
                MaKhachHang = int.Parse(txtMaKhachHang.Text),
                TenKhachHang = txtTenKhachHang.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            try
            {
                await _customerService.UpdateCustomerAsync(customer);
                MessageBox.Show("Lưu thay đổi thành công!");
                await LoadDataAsync();
                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!isAddingNew) return;

            var customer = new DTO.Customer
            {
                TenKhachHang = txtTenKhachHang.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                TrangThai = cboTrangThai.Text
            };

            try
            {
                await _customerService.AddCustomerAsync(customer);
                MessageBox.Show("Thêm khách hàng thành công!");
                await LoadDataAsync();
                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text) || txtMaKhachHang.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Ngừng hoạt động) khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _customerService.SoftDeleteCustomerAsync(int.Parse(txtMaKhachHang.Text));
                    MessageBox.Show("Đã xóa bản ghi (chuyển trạng thái) thành công!");
                    await LoadDataAsync();
                    ResetState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
            ResetState();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaKhachHang.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaKhachHang.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                
                dtpNgayTao.Format = DateTimePickerFormat.Custom;
                dtpNgayTao.CustomFormat = " "; // Đặt ngày tạo về null

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập Tên hoặc SĐT rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhachHang.Focus();
                return;
            }

            string ten = txtTenKhachHang.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(ten) && string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập Tên Khách Hàng hoặc Số Điện Thoại để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvCustomers.Rows.Clear();
            try
            {
                var customers = await _customerService.SearchCustomersAsync(ten, sdt);
                foreach (var c in customers)
                {
                    string ngayTao = c.NgayTao != DateTime.MinValue ? c.NgayTao.ToString("dd/MM/yyyy") : "";
                    dgvCustomers.Rows.Add(c.MaKhachHang.ToString(), c.TenKhachHang, c.SoDienThoai ?? "", c.Email ?? "", c.DiaChi ?? "", ngayTao, c.TrangThai ?? "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetState();
        }
    }
}
