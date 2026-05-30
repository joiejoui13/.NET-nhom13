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
        private CustomerRepository repo = new CustomerRepository();
        private List<Customer> customers = new List<Customer>();
        private Customer? selectedCustomer = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucCustomer()
        {
            InitializeComponent();
            pnlGridCard.SizeChanged += (s, e) =>
            {
                dgvCustomers.Width = pnlGridCard.Width - 67;
                dgvCustomers.Height = pnlGridCard.Height - 158;
            };
        }

        private async void ucCustomer_Load(object sender, EventArgs e)
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn col in dgvCustomers.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            await LoadCustomersGridAsync();
            SetEditState(false);
            if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
        }

        private async Task LoadCustomersGridAsync(IEnumerable<Customer>? dataSource = null)
        {
            dgvCustomers.Rows.Clear();
            if (dataSource == null)
            {
                var data = await repo.GetAllAsync();
                customers = data.ToList();
                dataSource = customers;
            }

            foreach (var customer in dataSource)
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

            string customerId = dgvCustomers.Rows[rowIndex].Cells[0].Value?.ToString() ?? "";
            selectedCustomer = customers.FirstOrDefault(c => c.MaKhachHang == customerId);

            if (selectedCustomer != null)
            {
                PopulateCustomerDetails(selectedCustomer);
            }
        }

        private void PopulateCustomerDetails(Customer customer)
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

            // MÃ£ KH is always read-only because it is identity/auto-generated
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

            txtMaKhachHang.Text = "Tá»± Ä‘á»™ng táº¡o";

            SetEditState(true);
            txtTenKhachHang.Focus();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lÃ²ng chá»n má»™t khÃ¡ch hÃ ng Ä‘á»ƒ chá»‰nh sá»­a!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAddingNew = false;
            SetEditState(true);
            txtTenKhachHang.Focus();
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lÃ²ng chá»n má»™t khÃ¡ch hÃ ng Ä‘á»ƒ xÃ³a!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a khÃ¡ch hÃ ng '{selectedCustomer.TenKhachHang}' khÃ´ng?", "XÃ¡c nháº­n xÃ³a", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                await repo.DeleteAsync(selectedCustomer.MaKhachHang);
                MessageBox.Show("XÃ³a khÃ¡ch hÃ ng thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadCustomersGridAsync();
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

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            await LoadCustomersGridAsync();
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

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            // Search criteria can be entered in TenKhachHang or SoDienThoai textboxes
            string nameKeyword = txtTenKhachHang.Text.Trim().ToLower();
            string phoneKeyword = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(nameKeyword) && string.IsNullOrEmpty(phoneKeyword))
            {
                MessageBox.Show("Vui lÃ²ng nháº­p TÃªn hoáº·c Sá»‘ Ä‘iá»‡n thoáº¡i vÃ o Ã´ thÃ´ng tin Ä‘á»ƒ tÃ¬m kiáº¿m!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filtered = customers.Where(c =>
            {
                bool match = true;
                if (!string.IsNullOrEmpty(nameKeyword))
                {
                    match = match && c.TenKhachHang.ToLower().Contains(nameKeyword);
                }
                if (!string.IsNullOrEmpty(phoneKeyword))
                {
                    match = match && (!string.IsNullOrEmpty(c.SoDienThoai) && c.SoDienThoai.Contains(phoneKeyword));
                }
                return match;
            }).ToList();

            await LoadCustomersGridAsync(filtered);

            if (dgvCustomers.Rows.Count > 0)
            {
                SelectCustomerRow(0);
            }
            else
            {
                selectedCustomer = null;
                ClearInputs();
                MessageBox.Show("KhÃ´ng tÃ¬m tháº¥y khÃ¡ch hÃ ng phÃ¹ há»£p!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            string name = txtTenKhachHang.Text.Trim();
            string phone = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtDiaChi.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("TÃªn khÃ¡ch hÃ ng khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng!", "Lá»—i xÃ¡c thá»±c", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng!", "Lá»—i xÃ¡c thá»±c", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDienThoai.Focus();
                return;
            }

            if (isAddingNew)
            {
                string newId = "KH001";
                if (customers.Any())
                {
                    var maxNum = customers
                        .Where(c => c.MaKhachHang != null && c.MaKhachHang.StartsWith("KH"))
                        .Select(c =>
                        {
                            int.TryParse(c.MaKhachHang.Substring(2), out int n);
                            return n;
                        })
                        .DefaultIfEmpty(0)
                        .Max();
                    newId = "KH" + (maxNum + 1).ToString("D3");
                }

                var newCustomer = new Customer
                {
                    MaKhachHang = newId,
                    TenKhachHang = name,
                    SoDienThoai = phone,
                    Email = email,
                    DiaChi = address,
                    NgayTao = DateTime.Now
                };
                await repo.AddAsync(newCustomer);
                MessageBox.Show("ThÃªm má»›i khÃ¡ch hÃ ng thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                selectedCustomer = newCustomer;
            }
            else
            {
                if (selectedCustomer != null)
                {
                    selectedCustomer.TenKhachHang = name;
                    selectedCustomer.SoDienThoai = phone;
                    selectedCustomer.Email = email;
                    selectedCustomer.DiaChi = address;
                    await repo.UpdateAsync(selectedCustomer);
                    MessageBox.Show("Cáº­p nháº­t thÃ´ng tin khÃ¡ch hÃ ng thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);

            await LoadCustomersGridAsync();

            if (selectedCustomer != null)
            {
                var rowToSelect = customers.FirstOrDefault(c => c.TenKhachHang == name && c.SoDienThoai == phone);
                if (rowToSelect != null)
                {
                    int index = customers.IndexOf(rowToSelect);
                    if (index >= 0 && index < dgvCustomers.Rows.Count)
                    {
                        SelectCustomerRow(index);
                    }
                }
            }
        }

        private void lblSoDienThoai_Click(object sender, EventArgs e)
        {

        }

        private void lblInputTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
