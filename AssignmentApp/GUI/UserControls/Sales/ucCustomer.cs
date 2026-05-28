using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : UserControl
    {
        public class MockCustomer
        {
            public int MaKhachHang { get; set; }
            public string TenKhachHang { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
            public string Email { get; set; } = "";
            public string DiaChi { get; set; } = "";
            public DateTime NgayTao { get; set; }
        }

        private List<MockCustomer> mockCustomers = new List<MockCustomer>();
        private MockCustomer? selectedCustomer = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

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

        private void ucCustomer_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            LoadCustomersGrid();
            SetEditState(false);
            if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
        }

        private void InitializeMockData()
        {
            if (mockCustomers.Count > 0) return;

            mockCustomers.Add(new MockCustomer
            {
                MaKhachHang = 1,
                TenKhachHang = "Nguyễn Văn A",
                SoDienThoai = "0987654321",
                Email = "nguyenvana@gmail.com",
                DiaChi = "123 Đường Lê Lợi, Quận 1, TP. HCM",
                NgayTao = DateTime.Now.AddMonths(-3)
            });

            mockCustomers.Add(new MockCustomer
            {
                MaKhachHang = 2,
                TenKhachHang = "Trần Thị B",
                SoDienThoai = "0912345678",
                Email = "tranthib@yahoo.com",
                DiaChi = "456 Đường Nguyễn Huệ, Quận 3, TP. HCM",
                NgayTao = DateTime.Now.AddMonths(-2)
            });

            mockCustomers.Add(new MockCustomer
            {
                MaKhachHang = 3,
                TenKhachHang = "Lê Văn C",
                SoDienThoai = "0909090909",
                Email = "levanc@outlook.com",
                DiaChi = "789 Đường Điện Biên Phủ, Bình Thạnh, TP. HCM",
                NgayTao = DateTime.Now.AddMonths(-1)
            });
        }

        private void LoadCustomersGrid(List<MockCustomer>? dataSource = null)
        {
            dgvCustomers.Rows.Clear();
            var list = dataSource ?? mockCustomers;
            foreach (var customer in list)
            {
                dgvCustomers.Rows.Add(
                    customer.MaKhachHang,
                    customer.TenKhachHang,
                    customer.SoDienThoai,
                    customer.Email,
                    customer.DiaChi,
                    customer.NgayTao.ToString("dd/MM/yyyy")
                );
            }
        }

        private void SelectCustomerRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvCustomers.Rows.Count) return;

            dgvCustomers.ClearSelection();
            dgvCustomers.Rows[rowIndex].Selected = true;

            int customerId = Convert.ToInt32(dgvCustomers.Rows[rowIndex].Cells[0].Value);
            selectedCustomer = mockCustomers.FirstOrDefault(c => c.MaKhachHang == customerId);

            if (selectedCustomer != null)
            {
                PopulateCustomerDetails(selectedCustomer);
            }
        }

        private void PopulateCustomerDetails(MockCustomer customer)
        {
            txtMaKhachHang.Text = customer.MaKhachHang.ToString();
            txtTenKhachHang.Text = customer.TenKhachHang;
            txtSoDienThoai.Text = customer.SoDienThoai;
            txtEmail.Text = customer.Email;
            txtDiaChi.Text = customer.DiaChi;
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Mã KH is always read-only because it is identity/auto-generated
            txtMaKhachHang.ReadOnly = true;

            // Other fields are editable only when editing
            txtTenKhachHang.ReadOnly = !editing;
            txtSoDienThoai.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
            txtDiaChi.ReadOnly = !editing;

            // Make all buttons visible at all times
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // Position them statically side-by-side
            btnAdd.Location = new Point(15, 510);
            btnEdit.Location = new Point(115, 510);
            btnDelete.Location = new Point(215, 510);

            btnSave.Location = new Point(15, 555);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 555);
            btnCancel.Size = new Size(140, 36);

            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;

            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }

        private void ClearInputs()
        {
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
        }

        private void dgvCustomers_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectCustomerRow(e.RowIndex);
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            isAddingNew = true;
            ClearInputs();
            
            // Generate temporary new ID for visualization
            int nextId = mockCustomers.Count > 0 ? mockCustomers.Max(c => c.MaKhachHang) + 1 : 1;
            txtMaKhachHang.Text = nextId.ToString();
            
            SetEditState(true);
            txtTenKhachHang.Focus();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAddingNew = false;
            SetEditState(true);
            txtTenKhachHang.Focus();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{selectedCustomer.TenKhachHang}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockCustomers.Remove(selectedCustomer);
                MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomersGrid();
                if (dgvCustomers.Rows.Count > 0)
                {
                    SelectCustomerRow(0);
                }
                else
                {
                    selectedCustomer = null;
                    ClearInputs();
                }
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadCustomersGrid();
            SetEditState(false);
            if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedCustomer != null)
            {
                PopulateCustomerDetails(selectedCustomer);
            }
            else if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            // Search criteria can be entered in TenKhachHang or SoDienThoai textboxes
            string nameKeyword = txtTenKhachHang.Text.Trim().ToLower();
            string phoneKeyword = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(nameKeyword) && string.IsNullOrEmpty(phoneKeyword))
            {
                MessageBox.Show("Vui lòng nhập Tên hoặc Số điện thoại vào ô thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filtered = mockCustomers.Where(c =>
            {
                bool match = true;
                if (!string.IsNullOrEmpty(nameKeyword))
                {
                    match = match && c.TenKhachHang.ToLower().Contains(nameKeyword);
                }
                if (!string.IsNullOrEmpty(phoneKeyword))
                {
                    match = match && c.SoDienThoai.Contains(phoneKeyword);
                }
                return match;
            }).ToList();

            LoadCustomersGrid(filtered);

            if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
            else
            {
                selectedCustomer = null;
                ClearInputs();
                MessageBox.Show("Không tìm thấy khách hàng phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            string name = txtTenKhachHang.Text.Trim();
            string phone = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtDiaChi.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDienThoai.Focus();
                return;
            }

            if (isAddingNew)
            {
                int newId = mockCustomers.Count > 0 ? mockCustomers.Max(c => c.MaKhachHang) + 1 : 1;
                var newCustomer = new MockCustomer
                {
                    MaKhachHang = newId,
                    TenKhachHang = name,
                    SoDienThoai = phone,
                    Email = email,
                    DiaChi = address,
                    NgayTao = DateTime.Now
                };
                mockCustomers.Add(newCustomer);
                selectedCustomer = newCustomer;
                MessageBox.Show("Thêm mới khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedCustomer != null)
                {
                    selectedCustomer.TenKhachHang = name;
                    selectedCustomer.SoDienThoai = phone;
                    selectedCustomer.Email = email;
                    selectedCustomer.DiaChi = address;
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadCustomersGrid();

            // Re-select row
            if (selectedCustomer != null)
            {
                int index = mockCustomers.IndexOf(selectedCustomer);
                if (index >= 0 && index < dgvCustomers.Rows.Count)
                {
                    SelectCustomerRow(index);
                }
            }
        }
    }
}
