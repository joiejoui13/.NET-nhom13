using System;
using System.Data; // Thêm thư viện này để thao tác với đối tượng DataTable
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Để gọi class DbContext xử lý dữ liệu với CSDL

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucUserManagement : UserControl
    {
        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo mặc định của UserControl.
        /// Chạy đầu tiên khi khởi tạo đối tượng, dùng để vẽ giao diện và thiết lập các cấu hình tĩnh.
        /// </summary>
        public ucUserManagement()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Danh sách tùy chọn Vai trò và Trạng thái.
            // (Đã được bóc tách từ file Designer để quản lý logic tập trung tại file .cs)
            cboVaiTro.Items.AddRange(new object[] { "ADMIN", "SALES", "WAREHOUSE" });
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Tạm khóa", "Nghỉ việc" });
        }

        /// <summary>
        /// Sự kiện Load: Kích hoạt khi UserControl lần đầu được nạp lên giao diện phần mềm.
        /// Chứa các logic kết nối CSDL, tải dữ liệu lên lưới và thiết lập trạng thái khởi điểm của các nút bấm.
        /// </summary>
        private void ucUserManagement_Load(object sender, EventArgs e)
        {
            // 1. Kết nối CSDL trước khi thao tác
            DbContext.Ketnoi();

            // 2. Tải toàn bộ dữ liệu danh sách người dùng lên DataGridView
            Load_DataGridView();
            
            // 3. Thiết lập giao diện ban đầu (Trạng thái nghỉ)
            ResetValues();
            txtMaNguoiDung.Enabled = false; // Mã tự sinh từ DB nên không cho người dùng tự gõ
            ToggleInputs(false);            // Khóa toàn bộ các ô nhập liệu vì chưa vào chế độ Thêm/Sửa
            
            // 4. Thiết lập trạng thái các nút chức năng ban đầu
            btnAdd.Enabled = true;          // Bật nút Thêm mới
            btnEdit.Enabled = false;        // Tắt Sửa (Vì chưa chọn dòng nào)
            btnDelete.Enabled = false;      // Tắt Xóa
            btnSave.Enabled = false;        // Tắt Lưu
            btnCancel.Enabled = false;      // Tắt Hủy
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Truy vấn dữ liệu từ bảng NguoiDung và đổ lên giao diện DataGridView.
        /// Tại đây chứa toàn bộ cấu hình Binding (ánh xạ cột) để tách biệt với Designer.
        /// </summary>
        private void Load_DataGridView()
        {
            // Truy vấn lấy dữ liệu cần thiết (Cố tình không lấy cột MatKhau để bảo mật dữ liệu hiển thị trên lưới)
            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, VaiTro, TrangThai, NgayTao FROM NguoiDung";
            DataTable tblND = DbContext.GetDataToTable(sql);
            
            // QUAN TRỌNG: Tắt tính năng tự động đẻ thêm cột từ DataTable (Để không phá vỡ thiết kế cột trên Designer)
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = tblND;

            // ÁNH XẠ DỮ LIỆU (Data Binding): Nối các trường từ DB vào đúng vị trí cột trên lưới
            dgvUsers.Columns[0].DataPropertyName = "MaNguoiDung";
            dgvUsers.Columns[1].DataPropertyName = "TenNguoiDung";
            dgvUsers.Columns[2].DataPropertyName = "SoDienThoai";
            dgvUsers.Columns[3].DataPropertyName = "Email";
            dgvUsers.Columns[4].DataPropertyName = "VaiTro";
            dgvUsers.Columns[5].DataPropertyName = "TrangThai";
            dgvUsers.Columns[6].DataPropertyName = "NgayTao";

            // ĐẶT TIÊU ĐỀ CỘT: Cập nhật nhãn hiển thị cho rõ nghĩa
            dgvUsers.Columns[0].HeaderText = "Mã ND";
            dgvUsers.Columns[1].HeaderText = "Tên Người Dùng";
            dgvUsers.Columns[2].HeaderText = "Số Điện Thoại";
            dgvUsers.Columns[3].HeaderText = "Email";
            dgvUsers.Columns[4].HeaderText = "Vai Trò";
            dgvUsers.Columns[5].HeaderText = "Trạng Thái";
            dgvUsers.Columns[6].HeaderText = "Ngày Tạo";

            // CĂN CHỈNH CHIỀU RỘNG VÀ HIỂN THỊ CỘT
            // Bước quan trọng: Tắt chế độ AutoSize toàn cục để tránh các cột tự ép nhau quá nhỏ
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
            dgvUsers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Email giãn tự động lấp khoảng trống
            
            dgvUsers.Columns[4].Width = 130;
            dgvUsers.Columns[4].MinimumWidth = 130;
            dgvUsers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[5].Width = 150; // Trạng thái
            dgvUsers.Columns[5].MinimumWidth = 150;
            dgvUsers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvUsers.Columns[6].Width = 180; // Ngày tháng
            dgvUsers.Columns[6].MinimumWidth = 180;
            dgvUsers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ĐỊNH DẠNG CHUNG CỦA LƯỚI
            dgvUsers.RowTemplate.Height = 40; // Chiều cao mỗi dòng dữ liệu
            dgvUsers.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 40;
            dgvUsers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // CHẾ ĐỘ BẢO VỆ LƯỚI
            dgvUsers.AllowUserToAddRows = false; // Không cho thêm trực tiếp ở dòng cuối cùng của lưới
            dgvUsers.EditMode = DataGridViewEditMode.EditProgrammatically; // Chỉ cho phép cập nhật thông qua code/Form, cấm gõ trực tiếp
        }

        /// <summary>
        /// Đưa toàn bộ các ô nhập liệu về trạng thái trống hoặc giá trị mặc định.
        /// Dùng khi Thêm mới hoặc khi Hủy thao tác.
        /// </summary>
        private void ResetValues()
        {
            txtMaNguoiDung.Text = "";
            txtTenNguoiDung.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtMatKhau.Text = "";
            cboVaiTro.SelectedIndex = -1; // Reset ComboBox về trạng thái chưa chọn
            cboTrangThai.SelectedIndex = -1;
        }

        /// <summary>
        /// Bật/Tắt khả năng chỉnh sửa của các ô nhập liệu trên form.
        /// </summary>
        /// <param name="isEnabled">true: Cho phép nhập/chỉnh sửa, false: Khóa lại (Read-only mờ)</param>
        private void ToggleInputs(bool isEnabled)
        {
            txtTenNguoiDung.Enabled = isEnabled;
            txtSoDienThoai.Enabled = isEnabled;
            txtEmail.Enabled = isEnabled;
            txtMatKhau.Enabled = isEnabled;
            cboVaiTro.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của dữ liệu đầu vào (Validation).
        /// Hàm này giúp tập trung logic kiểm duyệt vào một nơi, tránh lặp lại code ở cả nút Thêm và Sửa.
        /// </summary>
        private bool ValidateUserInputs(bool isAddingNew)
        {
            // Kiểm tra Tên người dùng (Bắt buộc)
            if (txtTenNguoiDung.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguoiDung.Focus();
                return false;
            }

            // Kiểm tra Mật khẩu (Bắt buộc khi Thêm mới. Khi Sửa thì có thể để trống nếu không muốn đổi pass)
            if (isAddingNew && txtMatKhau.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng thiết lập mật khẩu ban đầu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            return true; // Dữ liệu hợp lệ
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Sự kiện Click vào một ô bất kỳ trong lưới danh sách.
        /// Dùng để đồng bộ dữ liệu từ dòng được chọn lên các TextBox phía trên (Binding ngược).
        /// </summary>
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click hợp lệ (không phải click vào tiêu đề cột rowIndex = -1)
            if (e.RowIndex >= 0)
            {
                // THOÁT CHẾ ĐỘ TÌM KIẾM (Nếu Form đang ở trạng thái Tìm kiếm)
                if (txtMaNguoiDung.Enabled == true)
                {
                    txtMaNguoiDung.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // ĐẨY DỮ LIỆU TỪ DÒNG ĐƯỢC CHỌN LÊN TEXTBOX DỰA VÀO VỊ TRÍ CỘT
                txtMaNguoiDung.Text = dgvUsers.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtTenNguoiDung.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtSoDienThoai.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtEmail.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value.ToString();
                cboVaiTro.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value.ToString();
                cboTrangThai.Text = dgvUsers.Rows[e.RowIndex].Cells[5].Value.ToString();
                
                // Lưu ý cực kỳ quan trọng: Luôn để trống mật khẩu khi hiển thị lại để bảo mật!
                txtMatKhau.Text = ""; 
                
                // CHUYỂN ĐỔI TRẠNG THÁI GIAO DIỆN
                ToggleInputs(true);             // Mở khóa các ô để người dùng có thể chỉnh sửa
                
                btnEdit.Enabled = true;         // Có dòng được chọn -> Được quyền Sửa
                btnDelete.Enabled = true;       // Có dòng được chọn -> Được quyền Xóa
                btnCancel.Enabled = true;       // Bật Hủy để bỏ qua lựa chọn
                
                btnAdd.Enabled = false;         // Đang chọn dòng cũ thì cấm ấn Thêm mới
                btnSave.Enabled = false;        // Nút Lưu chỉ dành cho tính năng Thêm mới
            }
        }

        #endregion

        #region 4. CÁC HÀM XỬ LÝ NÚT BẤM (BUTTON CLICK HANDLERS)

        /// <summary>
        /// Nút Thêm: Đưa Form vào chế độ sẵn sàng nhập liệu người dùng mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues(); // Xóa sạch rác còn sót trên form
            
            txtMaNguoiDung.Enabled = false; 
            txtMaNguoiDung.Text = "Tự động sinh"; // Thông báo mã sẽ do DB tự cấp
            
            ToggleInputs(true); // Mở khóa nhập liệu
            
            // Chuyển nút bấm sang trạng thái "Đang chờ Lưu"
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            txtTenNguoiDung.Focus(); // Đưa con trỏ chuột vào ô Tên đầu tiên
        }

        /// <summary>
        /// Nút Lưu: Thực thi quá trình đẩy dữ liệu mới (Insert) vào Database.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra tính hợp lệ của dữ liệu (truyền true vì đang ở chế độ Thêm mới)
            if (ValidateUserInputs(true) == false)
            {
                return; // Nếu dữ liệu sai, ngắt hàm luôn
            }

            // 2. Tạo câu lệnh SQL Insert
            string sql = $@"INSERT INTO NguoiDung(TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                            VALUES(N'{txtTenNguoiDung.Text}', '{txtSoDienThoai.Text}', '{txtEmail.Text}', 
                                   '{txtMatKhau.Text}', N'{cboVaiTro.Text}', N'{cboTrangThai.Text}', GETDATE())";
                  
            // 3. Thực thi lưu vào CSDL
            DbContext.RunSql(sql);
            MessageBox.Show("Thêm người dùng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 4. Cập nhật lại giao diện và khóa form
            Load_DataGridView();
            ResetValues();
            ToggleInputs(false);
            
            // 5. Trả nút bấm về mặc định
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaNguoiDung.Enabled = false;
        }

        /// <summary>
        /// Nút Sửa: Thực thi quá trình cập nhật (Update) dữ liệu của người dùng đang chọn.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn người nào trên lưới chưa
            if (dgvUsers.Rows.Count == 0 || txtMaNguoiDung.Text == "" || txtMaNguoiDung.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Kiểm tra tính hợp lệ (truyền false vì đang ở chế độ Sửa, không bắt buộc nhập mật khẩu mới)
            if (ValidateUserInputs(false) == false)
            {
                return; 
            }

            // 3. Khởi tạo câu truy vấn Update cơ bản
            string sql = $@"UPDATE NguoiDung SET 
                            TenNguoiDung = N'{txtTenNguoiDung.Text}', 
                            SoDienThoai = '{txtSoDienThoai.Text}', 
                            Email = '{txtEmail.Text}', 
                            VaiTro = N'{cboVaiTro.Text}', 
                            TrangThai = N'{cboTrangThai.Text}'";
                            
            // 4. Kiểm tra riêng Mật khẩu: Nếu người dùng có gõ pass mới thì mới Update, không thì giữ nguyên pass cũ trong DB
            if (txtMatKhau.Text.Trim() != "")
            {
                sql += $", MatKhau = '{txtMatKhau.Text}'";
            }
            
            // 5. Thêm điều kiện khóa chính (Cực kỳ quan trọng để không update nhầm cả bảng)
            sql += $" WHERE MaNguoiDung = {txtMaNguoiDung.Text}";

            // 6. Thực thi
            DbContext.RunSql(sql);
            MessageBox.Show("Cập nhật thông tin người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 7. Cập nhật giao diện
            Load_DataGridView();
            ResetValues();
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtMaNguoiDung.Enabled = false;
        }

        /// <summary>
        /// Nút Xóa: Thực thi quy trình Xóa Mềm (Soft Delete) bằng cách khóa tài khoản thay vì xóa mất khỏi DB.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. Đảm bảo đã chọn đúng bản ghi
            if (dgvUsers.Rows.Count == 0 || txtMaNguoiDung.Text == "" || txtMaNguoiDung.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Hiện hộp thoại cảnh báo chống bấm nhầm
            if (MessageBox.Show("Bạn có chắc chắn muốn khóa người dùng này?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Update trạng thái thành 'Khóa'
                string sql = $"UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE MaNguoiDung = {txtMaNguoiDung.Text}";
                
                DbContext.RunSql(sql);
                Load_DataGridView();
                ResetValues();
                ToggleInputs(false);
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        /// <summary>
        /// Nút Hủy / Bỏ qua: Thoát khỏi trạng thái Thêm/Sửa đang dở dang và đưa Form về trạng thái nghỉ.
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
        /// Nút Tìm Kiếm: Kích hoạt chế độ tìm kiếm hoặc thực thi tìm kiếm theo các ô đã nhập.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lần 1 (PHASE 1): Kích hoạt chế độ điền thông tin tìm kiếm
            if (txtMaNguoiDung.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaNguoiDung.Enabled = true; // Mở khóa ô mã để có thể tìm bằng ID

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật!\nVui lòng nhập thông tin cần tìm kiếm vào các ô dữ liệu rồi ấn Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNguoiDung.Focus();
                return;
            }

            // Lần 2 (PHASE 2): Thực hiện tìm kiếm và đổ dữ liệu
            string idTerm = txtMaNguoiDung.Text.Trim();
            string nameTerm = txtTenNguoiDung.Text.Trim();
            string phoneTerm = txtSoDienThoai.Text.Trim();
            string emailTerm = txtEmail.Text.Trim();
            string roleTerm = cboVaiTro.Text;
            string statusTerm = cboTrangThai.Text;

            // Xây dựng câu SQL gốc
            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, VaiTro, TrangThai, NgayTao FROM NguoiDung WHERE 1=1";
            
            // Nối thêm điều kiện (Sử dụng cấu trúc cơ bản nối chuỗi)
            if (string.IsNullOrEmpty(idTerm) == false)
                sql += $" AND MaNguoiDung = {idTerm}";
                
            if (string.IsNullOrEmpty(nameTerm) == false)
                sql += $" AND TenNguoiDung LIKE N'%{nameTerm}%'";
                
            if (string.IsNullOrEmpty(phoneTerm) == false)
                sql += $" AND SoDienThoai LIKE '%{phoneTerm}%'";
                
            if (string.IsNullOrEmpty(emailTerm) == false)
                sql += $" AND Email LIKE '%{emailTerm}%'";
                
            if (string.IsNullOrEmpty(roleTerm) == false)
                sql += $" AND VaiTro = N'{roleTerm}'";
                
            if (string.IsNullOrEmpty(statusTerm) == false)
                sql += $" AND TrangThai = N'{statusTerm}'";

            DataTable tblND = DbContext.GetDataToTable(sql);
            dgvUsers.DataSource = tblND;

            // Thông báo kết quả
            if (tblND.Rows.Count > 0)
            {
                ResetValues();
                MessageBox.Show($"Tìm thấy {tblND.Rows.Count} người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResetValues();
                MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            btnCancel.Enabled = false;
        }

        /// <summary>
        /// Nút Làm mới (Refresh): Tải lại toàn bộ dữ liệu gốc và xóa chế độ tìm kiếm.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load_DataGridView();
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

        // Hàm sự kiện trống lỡ tạo trong Designer
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        #endregion
    }
}
