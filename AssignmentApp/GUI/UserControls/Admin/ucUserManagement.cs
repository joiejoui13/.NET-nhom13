using System;
using System.Data;
using System.Windows.Forms;
using AssignmentApp.DAL.Core;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucUserManagement : UserControl
    {
        public ucUserManagement()
        {
            InitializeComponent();
        }

        // 5.2.2. Viết thủ tục Form_Load của ucUserManagement
        private void ucUserManagement_Load(object sender, EventArgs e)
        {
            DbContext.Ketnoi();
            Load_DataGridView();
            
            ResetValues();
            txtMaNguoiDung.Enabled = false; // Mã tự sinh nên khóa lại
            
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        // 5.2.3. Viết thủ tục Load_DataGridView
        private void Load_DataGridView()
        {
            // Lấy dữ liệu (Không lấy cột MatKhau để bảo mật trên lưới)
            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, VaiTro, TrangThai, NgayTao FROM NguoiDung";
            DataTable tblND = DbContext.GetDataToTable(sql);
            
            // Tắt tính năng tự sinh cột
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = tblND;

            // Map dữ liệu vào các cột có sẵn trên Designer
            dgvUsers.Columns[0].DataPropertyName = "MaNguoiDung";
            dgvUsers.Columns[1].DataPropertyName = "TenNguoiDung";
            dgvUsers.Columns[2].DataPropertyName = "SoDienThoai";
            dgvUsers.Columns[3].DataPropertyName = "Email";
            dgvUsers.Columns[4].DataPropertyName = "VaiTro";
            dgvUsers.Columns[5].DataPropertyName = "TrangThai";
            dgvUsers.Columns[6].DataPropertyName = "NgayTao";

            // Đặt tên tiêu đề cột cho rõ ràng, dễ nhìn
            dgvUsers.Columns[0].HeaderText = "Mã ND";
            dgvUsers.Columns[1].HeaderText = "Tên Người Dùng";
            dgvUsers.Columns[2].HeaderText = "Số Điện Thoại";
            dgvUsers.Columns[3].HeaderText = "Email";
            dgvUsers.Columns[4].HeaderText = "Vai Trò";
            dgvUsers.Columns[5].HeaderText = "Trạng Thái";
            dgvUsers.Columns[6].HeaderText = "Ngày Tạo";

            // Định dạng lề và kích thước để không bị mất chữ
            // Bước quan trọng: Tắt chế độ AutoSize toàn cục để tránh các cột ép nhau
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvUsers.Columns[0].Width = 80;
            dgvUsers.Columns[0].MinimumWidth = 80;
            dgvUsers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[1].Width = 220; // Tên người dùng cần không gian rộng
            dgvUsers.Columns[1].MinimumWidth = 220;
            
            dgvUsers.Columns[2].Width = 140;
            dgvUsers.Columns[2].MinimumWidth = 140;
            dgvUsers.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[3].MinimumWidth = 150;
            dgvUsers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Email giãn tự động
            
            dgvUsers.Columns[4].Width = 130;
            dgvUsers.Columns[4].MinimumWidth = 130;
            dgvUsers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[5].Width = 150; // Trạng thái
            dgvUsers.Columns[5].MinimumWidth = 150;
            dgvUsers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[6].Width = 180; // Ngày tháng
            dgvUsers.Columns[6].MinimumWidth = 180;
            dgvUsers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Định dạng Header và Dòng
            dgvUsers.RowTemplate.Height = 40;
            dgvUsers.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 40;
            dgvUsers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // 5.2.4. Viết thủ tục ResetValues
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

        // 5.2.5. Viết thủ tục DataGridView_Click
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNguoiDung.Focus();
                return;
            }
            if (dgvUsers.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Đổ dữ liệu lên TextBox dựa vào số thứ tự cột (0 đến 6)
            txtMaNguoiDung.Text = dgvUsers.CurrentRow.Cells[0].Value.ToString();
            txtTenNguoiDung.Text = dgvUsers.CurrentRow.Cells[1].Value.ToString();
            txtSoDienThoai.Text = dgvUsers.CurrentRow.Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.CurrentRow.Cells[3].Value.ToString();
            cboVaiTro.Text = dgvUsers.CurrentRow.Cells[4].Value.ToString();
            cboTrangThai.Text = dgvUsers.CurrentRow.Cells[5].Value.ToString();
            // Lưu ý: Không hiển thị mật khẩu ngược lên ô txtMatKhau để bảo mật
            txtMatKhau.Text = ""; 
            
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        // 5.2.6. Viết thủ tục btnThem_Click (Nút Thêm mới)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Bước 1: Xóa trắng form
            ResetValues();
            
            // Bước 2: Thiết lập trạng thái nút bấm
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            // Bước 3: Đưa trỏ chuột vào vị trí sẵn sàng nhập
            txtMaNguoiDung.Enabled = false; 
            txtTenNguoiDung.Focus();
        }

        // 5.2.7. Viết thủ tục btnLuu_Click (Nút Lưu)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra dữ liệu
            if (txtTenNguoiDung.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguoiDung.Focus();
                return;
            }
            if (txtMatKhau.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng thiết lập mật khẩu ban đầu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            // (Ghi chú: Đáng lẽ mật khẩu phải được băm (hash) qua PasswordHasher, 
            // nhưng ở đây tạm lưu thẳng giống cách làm phổ biến trong các bài hướng dẫn cơ bản)
            
            // Bước 2: Tạo SQL
            string sql = $@"INSERT INTO NguoiDung(TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                            VALUES(N'{txtTenNguoiDung.Text}', '{txtSoDienThoai.Text}', '{txtEmail.Text}', 
                                   '{txtMatKhau.Text}', N'{cboVaiTro.Text}', N'{cboTrangThai.Text}', GETDATE())";
                  
            // Bước 3: Thực thi
            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            
            // Bước 4: Reset nút
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        // 5.2.8. Viết thủ tục btnSua_Click (Nút Sửa)
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra chọn dữ liệu
            if (dgvUsers.Rows.Count == 0 || txtMaNguoiDung.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNguoiDung.Text.Trim().Length == 0)
            {
                MessageBox.Show("Không được để trống tên người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguoiDung.Focus();
                return;
            }

            // Bước 2: Tạo SQL UPDATE. Nếu ô Mật khẩu có gõ chữ mới thì mới Update mật khẩu
            string sql = $@"UPDATE NguoiDung SET 
                            TenNguoiDung = N'{txtTenNguoiDung.Text}', 
                            SoDienThoai = '{txtSoDienThoai.Text}', 
                            Email = '{txtEmail.Text}', 
                            VaiTro = N'{cboVaiTro.Text}', 
                            TrangThai = N'{cboTrangThai.Text}'";
                            
            if (txtMatKhau.Text.Trim() != "")
            {
                sql += $", MatKhau = '{txtMatKhau.Text}'";
            }
            
            sql += $" WHERE MaNguoiDung = {txtMaNguoiDung.Text}";

            // Bước 3: Thực thi
            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            
            // Bước 4: Khóa nút
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        // 5.2.9. Viết thủ tục btnXoa_Click (Nút Xóa)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count == 0 || txtMaNguoiDung.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"DELETE NguoiDung WHERE MaNguoiDung = {txtMaNguoiDung.Text}";
                
                DbContext.RunSqlDel(sql);
                Load_DataGridView();
                ResetValues();
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        // 5.2.10. Viết thủ tục btnBoqua_Click (Nút Bỏ qua)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaNguoiDung.Enabled = false;
        }

        // 5.2.11. Viết thủ tục btnTimkiem_Click (Nút Tìm kiếm)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtMaNguoiDung.Enabled = true; 

            if (txtMaNguoiDung.Text == "" && txtTenNguoiDung.Text == "" && txtSoDienThoai.Text == "")
            {
                MessageBox.Show("Hãy nhập mã, tên hoặc số điện thoại để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, VaiTro, TrangThai, NgayTao FROM NguoiDung WHERE 1=1";
            
            if (txtMaNguoiDung.Text != "")
                sql += $" AND MaNguoiDung = {txtMaNguoiDung.Text}";
                
            if (txtTenNguoiDung.Text != "")
                sql += $" AND TenNguoiDung LIKE N'%{txtTenNguoiDung.Text}%'";
                
            if (txtSoDienThoai.Text != "")
                sql += $" AND SoDienThoai LIKE '%{txtSoDienThoai.Text}%'";

            DataTable tblND = DbContext.GetDataToTable(sql);
            if (tblND.Rows.Count == 0)
                MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Tìm thấy {tblND.Rows.Count} người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            dgvUsers.DataSource = tblND;
            ResetValues();
            
            btnCancel.Enabled = true;
        }

        // 5.2.12. Viết thủ tục btnHienthi_Click (Nút Làm mới)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load_DataGridView();
            ResetValues();
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
