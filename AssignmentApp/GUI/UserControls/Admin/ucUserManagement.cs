using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Admin;
using AssignmentApp.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Admin
{
    /// <summary>
    /// Giao diện người dùng (Tầng GUI - Presentation Layer).
    /// Chịu trách nhiệm hiển thị dữ liệu và tiếp nhận thao tác từ người dùng.
    /// Thiết kế chuẩn 3-Tier: Hoàn toàn không chứa câu lệnh SQL. Mọi thao tác xử lý nghiệp vụ đều gọi thông qua các Service (BLL) bằng Dependency Injection.
    /// Ứng dụng triệt để cơ chế xử lý bất đồng bộ (async/await) để tránh làm đơ (freeze) giao diện khi tải dữ liệu.
    /// </summary>
    public partial class ucUserManagement : UserControl
    {
        private readonly IUserService _userService;

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        public ucUserManagement()
        {
            InitializeComponent();

            _userService = Program.ServiceProvider.GetRequiredService<IUserService>();

            cboVaiTro.Items.AddRange(new object[] { "ADMIN", "SALES", "WAREHOUSE" });
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Tạm khóa", "Nghỉ việc" });
            
            this.Load += ucUserManagement_Load;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Kích hoạt khi giao diện vừa được tải lên. Khởi tạo cấu hình và gọi BLL để lấy dữ liệu đổ vào Grid.
        /// </summary>
        private async void ucUserManagement_Load(object sender, EventArgs e)
        {
            await Load_DataGridViewAsync();
            
            ResetValues();
            txtMaNguoiDung.Enabled = false; 
            ToggleInputs(false);            
            
            btnAdd.Enabled = true;          
            btnEdit.Enabled = false;        
            btnDelete.Enabled = false;      
            btnSave.Enabled = false;        
            btnCancel.Enabled = false;      
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        private async Task Load_DataGridViewAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                
                dgvUsers.AutoGenerateColumns = false;
                dgvUsers.DataSource = users.ToList();

                dgvUsers.Columns[0].DataPropertyName = "MaNguoiDung";
                dgvUsers.Columns[1].DataPropertyName = "TenNguoiDung";
                dgvUsers.Columns[2].DataPropertyName = "SoDienThoai";
                dgvUsers.Columns[3].DataPropertyName = "Email";
                dgvUsers.Columns[4].DataPropertyName = "VaiTro";
                dgvUsers.Columns[5].DataPropertyName = "TrangThai";
                dgvUsers.Columns[6].DataPropertyName = "NgayTao";

                dgvUsers.Columns[0].HeaderText = "Mã ND";
                dgvUsers.Columns[1].HeaderText = "Tên Người Dùng";
                dgvUsers.Columns[2].HeaderText = "Số Điện Thoại";
                dgvUsers.Columns[3].HeaderText = "Email";
                dgvUsers.Columns[4].HeaderText = "Vai Trò";
                dgvUsers.Columns[5].HeaderText = "Trạng Thái";
                dgvUsers.Columns[6].HeaderText = "Ngày Tạo";

                dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                dgvUsers.Columns[0].Width = 80;
                dgvUsers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgvUsers.Columns[1].Width = 220;
                
                dgvUsers.Columns[2].Width = 140;
                
                dgvUsers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                
                dgvUsers.Columns[4].Width = 130;
                
                dgvUsers.Columns[5].Width = 150; 
                
                dgvUsers.Columns[6].Width = 180; 
          

                dgvUsers.RowTemplate.Height = 40; 
                dgvUsers.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
                dgvUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                dgvUsers.ColumnHeadersHeight = 40;
                dgvUsers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dgvUsers.AllowUserToAddRows = false; 
                dgvUsers.EditMode = DataGridViewEditMode.EditProgrammatically; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetValues()
        {
            txtMaNguoiDung.Text = "";
            txtTenNguoiDung.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtMatKhau.Text = "";
            cboVaiTro.SelectedIndex = -1; 
            cboTrangThai.SelectedIndex = -1;
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenNguoiDung.Enabled = isEnabled;
            txtSoDienThoai.Enabled = isEnabled;
            txtEmail.Enabled = isEnabled;
            txtMatKhau.Enabled = isEnabled;
            cboVaiTro.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        private User GetUserFromInputs()
        {
            var user = new User
            {
                TenNguoiDung = txtTenNguoiDung.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                VaiTro = cboVaiTro.Text,
                TrangThai = cboTrangThai.Text
            };

            if (int.TryParse(txtMaNguoiDung.Text, out int id))
            {
                user.MaNguoiDung = id;
            }

            return user;
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtMaNguoiDung.Enabled == true)
                {
                    txtMaNguoiDung.Enabled = false;
                    btnAdd.Enabled = true;
                }

                txtMaNguoiDung.Text = dgvUsers.Rows[e.RowIndex].Cells[0].Value?.ToString();
                txtTenNguoiDung.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value?.ToString();
                txtSoDienThoai.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value?.ToString();
                txtEmail.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value?.ToString();
                cboVaiTro.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value?.ToString();
                cboTrangThai.Text = dgvUsers.Rows[e.RowIndex].Cells[5].Value?.ToString();
                
                txtMatKhau.Text = ""; 
                
                ToggleInputs(true);             
                
                btnEdit.Enabled = true;         
                btnDelete.Enabled = true;       
                btnCancel.Enabled = true;       
                
                btnAdd.Enabled = false;         
                btnSave.Enabled = false;        
            }
        }

        #endregion

        #region 4. CÁC HÀM XỬ LÝ NÚT BẤM (BUTTON CLICK HANDLERS)
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues(); 
            
            txtMaNguoiDung.Enabled = false; 
            txtMaNguoiDung.Text = "Tự động sinh"; 
            
            ToggleInputs(true); 
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            txtTenNguoiDung.Focus(); 
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var user = GetUserFromInputs();
                
                bool success = await _userService.AddUserAsync(user);

                if (success)
                {
                    MessageBox.Show("Thêm người dùng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);
                    
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaNguoiDung.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count == 0 || string.IsNullOrEmpty(txtMaNguoiDung.Text) || txtMaNguoiDung.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var user = GetUserFromInputs();
                bool updatePassword = !string.IsNullOrWhiteSpace(txtMatKhau.Text);

                bool success = await _userService.UpdateUserAsync(user, updatePassword);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);
                    
                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAdd.Enabled = true;
                    txtMaNguoiDung.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count == 0 || string.IsNullOrEmpty(txtMaNguoiDung.Text) || txtMaNguoiDung.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn khóa người dùng này?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    int id = int.Parse(txtMaNguoiDung.Text);
                    bool success = await _userService.LockUserAsync(id);
                    
                    if (success)
                    {
                        await Load_DataGridViewAsync();
                        ResetValues();
                        ToggleInputs(false);
                        
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnCancel.Enabled = false;
                        btnAdd.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetValues();
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaNguoiDung.Enabled = false;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaNguoiDung.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaNguoiDung.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật!\nVui lòng nhập thông tin cần tìm kiếm vào các ô dữ liệu rồi ấn Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNguoiDung.Focus();
                return;
            }

            try
            {
                string idTerm = txtMaNguoiDung.Text.Trim();
                string nameTerm = txtTenNguoiDung.Text.Trim();
                string phoneTerm = txtSoDienThoai.Text.Trim();
                string emailTerm = txtEmail.Text.Trim();
                string roleTerm = cboVaiTro.Text;
                string statusTerm = cboTrangThai.Text;

                if (string.IsNullOrEmpty(idTerm) && string.IsNullOrEmpty(nameTerm) && string.IsNullOrEmpty(phoneTerm) &&
                    string.IsNullOrEmpty(emailTerm) && string.IsNullOrEmpty(roleTerm) && string.IsNullOrEmpty(statusTerm))
                {
                    MessageBox.Show("Vui lòng điền ít nhất một tiêu chí tìm kiếm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var users = await _userService.SearchUsersAsync(idTerm, nameTerm, phoneTerm, emailTerm, roleTerm, statusTerm);
                var userList = users.ToList();
                
                dgvUsers.DataSource = userList;

                if (userList.Count > 0)
                {
                    ResetValues();
                    MessageBox.Show($"Tìm thấy {userList.Count} người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ResetValues();
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                btnCancel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await Load_DataGridViewAsync();
            ResetValues();
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaNguoiDung.Enabled = false;
        }

        #endregion

        #region 5. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        #endregion
    }
}
