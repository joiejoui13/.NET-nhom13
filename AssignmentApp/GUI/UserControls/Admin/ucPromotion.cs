using System;
using System.Data; // Thêm thư viện này để thao tác với đối tượng DataTable
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Để gọi class DbContext xử lý dữ liệu với CSDL

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucPromotion : UserControl
    {
        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo mặc định của UserControl.
        /// Chạy đầu tiên khi khởi tạo đối tượng, dùng để vẽ giao diện và thiết lập các cấu hình tĩnh.
        /// </summary>
        public ucPromotion()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Danh sách tùy chọn trạng thái. 
            // (Đã được bóc tách từ file Designer để quản lý logic tập trung tại file .cs)
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Không hoạt động" });
        }

        /// <summary>
        /// Sự kiện Load: Kích hoạt khi UserControl lần đầu được nạp lên giao diện phần mềm.
        /// Chứa các logic kết nối CSDL, tải dữ liệu lên lưới và thiết lập trạng thái khởi điểm của các nút bấm.
        /// </summary>
        private void ucPromotion_Load(object sender, EventArgs e)
        {
            // 1. Kết nối CSDL trước khi thao tác
            DbContext.Ketnoi(); 

            // 2. Tải toàn bộ dữ liệu danh sách khuyến mãi lên DataGridView
            Load_DataGridView();
            
            // 3. Thiết lập giao diện ban đầu (Trạng thái nghỉ)
            ResetValues(); 
            txtMaKhuyenMai.Enabled = false; // Mã tự sinh từ DB nên không cho người dùng tự gõ
            ToggleInputs(false);            // Khóa toàn bộ các ô nhập liệu vì chưa vào chế độ Thêm/Sửa
            
            // 4. Thiết lập trạng thái các nút chức năng
            btnAdd.Enabled = true;          // Bật nút Thêm mới
            btnEdit.Enabled = false;        // Tắt Sửa (Vì chưa chọn dòng nào)
            btnDelete.Enabled = false;      // Tắt Xóa
            btnSave.Enabled = false;        // Tắt Lưu
            btnCancel.Enabled = false;      // Tắt Hủy
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Truy vấn dữ liệu từ bảng KhuyenMai và đổ lên giao diện DataGridView.
        /// Tại đây chứa toàn bộ cấu hình Binding (ánh xạ cột) để tách biệt với Designer.
        /// </summary>
        private void Load_DataGridView()
        {
            // Truy vấn lấy dữ liệu cần thiết
            string sql = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai";
            DataTable tblKM = DbContext.GetDataToTable(sql);
            
            // QUAN TRỌNG: Tắt tính năng tự động đẻ thêm cột từ DataTable (Để không phá vỡ thiết kế cột trên Designer)
            dgvPromotion.AutoGenerateColumns = false;
            dgvPromotion.DataSource = tblKM;

            // ÁNH XẠ DỮ LIỆU (Data Binding): Nối các trường từ DB vào đúng vị trí cột trên lưới
            dgvPromotion.Columns[0].DataPropertyName = "MaKhuyenMai";
            dgvPromotion.Columns[1].DataPropertyName = "TenKhuyenMai";
            dgvPromotion.Columns[2].DataPropertyName = "PhanTramGiamGia";
            dgvPromotion.Columns[3].DataPropertyName = "NgayBatDau";
            dgvPromotion.Columns[4].DataPropertyName = "NgayKetThuc";
            dgvPromotion.Columns[5].DataPropertyName = "TrangThai";

            // ĐẶT TIÊU ĐỀ CỘT: Cập nhật nhãn hiển thị cho rõ nghĩa
            dgvPromotion.Columns[0].HeaderText = "Mã KM";
            dgvPromotion.Columns[1].HeaderText = "Tên Khuyến Mãi";
            dgvPromotion.Columns[2].HeaderText = "% Giảm";
            dgvPromotion.Columns[3].HeaderText = "Ngày Bắt Đầu";
            dgvPromotion.Columns[4].HeaderText = "Ngày Kết Thúc";
            dgvPromotion.Columns[5].HeaderText = "Trạng Thái";

            // CĂN CHỈNH CHIỀU RỘNG VÀ HIỂN THỊ CỘT
            dgvPromotion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvPromotion.Columns[0].Width = 80;
            dgvPromotion.Columns[0].MinimumWidth = 80;
            dgvPromotion.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvPromotion.Columns[1].MinimumWidth = 200;
            dgvPromotion.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Cột tên cho giãn tự động lấp đầy khoảng trống
            
            dgvPromotion.Columns[2].Width = 90;
            dgvPromotion.Columns[2].MinimumWidth = 90;
            dgvPromotion.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvPromotion.Columns[3].Width = 150;
            dgvPromotion.Columns[3].MinimumWidth = 150;
            dgvPromotion.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvPromotion.Columns[4].Width = 150;
            dgvPromotion.Columns[4].MinimumWidth = 150;
            dgvPromotion.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvPromotion.Columns[5].Width = 120;
            dgvPromotion.Columns[5].MinimumWidth = 120;
            dgvPromotion.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ĐỊNH DẠNG CHUNG CỦA LƯỚI
            dgvPromotion.RowTemplate.Height = 40; // Chiều cao mỗi dòng dữ liệu
            dgvPromotion.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvPromotion.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvPromotion.ColumnHeadersHeight = 40; 
            dgvPromotion.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // CHẾ ĐỘ BẢO VỆ LƯỚI
            dgvPromotion.AllowUserToAddRows = false; // Không cho thêm trực tiếp ở dòng cuối cùng của lưới
            dgvPromotion.EditMode = DataGridViewEditMode.EditProgrammatically; // Chỉ cho phép cập nhật thông qua code/Form, cấm gõ trực tiếp
        }

        /// <summary>
        /// Đưa toàn bộ các ô nhập liệu về trạng thái trống hoặc giá trị mặc định.
        /// Dùng khi Thêm mới hoặc khi Hủy thao tác.
        /// </summary>
        private void ResetValues()
        {
            txtMaKhuyenMai.Text = "";
            txtTenKhuyenMai.Text = "";
            txtPhanTramGiamGia.Text = "";
            txtMoTaKhuyenMai.Text = "";
            dtNgayBatDau.Value = DateTime.Now;
            dtNgayBatDau.Checked = true;
            dtNgayHetHan.Value = DateTime.Now;
            dtNgayHetHan.Checked = true;
            cboTrangThai.SelectedIndex = -1; // Reset ComboBox về trạng thái chưa chọn
        }

        /// <summary>
        /// Bật/Tắt khả năng chỉnh sửa của các ô nhập liệu trên form.
        /// </summary>
        /// <param name="isEnabled">true: Cho phép nhập/chỉnh sửa, false: Khóa lại (Read-only mờ)</param>
        private void ToggleInputs(bool isEnabled)
        {
            txtTenKhuyenMai.Enabled = isEnabled;
            txtPhanTramGiamGia.Enabled = isEnabled;
            txtMoTaKhuyenMai.Enabled = isEnabled;
            dtNgayBatDau.Enabled = isEnabled;
            dtNgayHetHan.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của dữ liệu đầu vào (Validation).
        /// Trả về true nếu dữ liệu hợp lệ và gán giá trị ra các biến out. 
        /// Trả về false và báo lỗi nếu dữ liệu sai quy tắc.
        /// </summary>
        private bool ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status)
        {
            // Lấy dữ liệu và loại bỏ khoảng trắng dư thừa
            name = txtTenKhuyenMai.Text.Trim();
            desc = txtMoTaKhuyenMai.Text.Trim();
            status = cboTrangThai.Text;
            startDate = dtNgayBatDau.Value.ToString("yyyy-MM-dd");
            endDate = dtNgayHetHan.Value.ToString("yyyy-MM-dd");
            percent = 0;

            // Kiểm tra Tên khuyến mãi (Bắt buộc)
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Bạn phải nhập tên khuyến mãi!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhuyenMai.Focus();
                return false;
            }

            // Kiểm tra Phần trăm giảm giá (Bắt buộc, kiểu số từ 0 - 100)
            string phanTram = txtPhanTramGiamGia.Text.Trim();
            if (string.IsNullOrEmpty(phanTram)) phanTram = "0"; // Mặc định là 0 nếu để trống
            if (!float.TryParse(phanTram, out percent) || percent < 0 || percent > 100)
            {
                MessageBox.Show("Phần trăm giảm giá phải là số từ 0 đến 100!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhanTramGiamGia.Focus();
                return false;
            }

            // Kiểm tra Logic ngày tháng (Bắt đầu phải <= Kết thúc)
            if (dtNgayBatDau.Value > dtNgayHetHan.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtNgayBatDau.Focus();
                return false;
            }

            // Kiểm tra Trạng thái (Bắt buộc chọn)
            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }
            
            return true; // Qua hết các trạm kiểm duyệt -> Hợp lệ
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Sự kiện Click vào một ô bất kỳ trong lưới danh sách Khuyến Mãi.
        /// Dùng để đồng bộ dữ liệu từ dòng được chọn lên các TextBox phía trên (Binding ngược).
        /// </summary>
        private void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click hợp lệ (không phải click vào tiêu đề cột rowIndex = -1)
            if (e.RowIndex >= 0)
            {
                // THOÁT CHẾ ĐỘ TÌM KIẾM (Nếu Form đang ở trạng thái Tìm kiếm)
                if (txtMaKhuyenMai.Enabled == true)
                {
                    txtMaKhuyenMai.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // 1. ĐẨY DỮ LIỆU TỪ DÒNG LÊN TEXTBOX
                string id = dgvPromotion.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtMaKhuyenMai.Text = id;
                txtTenKhuyenMai.Text = dgvPromotion.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPhanTramGiamGia.Text = dgvPromotion.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtNgayBatDau.Value = Convert.ToDateTime(dgvPromotion.Rows[e.RowIndex].Cells[3].Value);
                dtNgayHetHan.Value = Convert.ToDateTime(dgvPromotion.Rows[e.RowIndex].Cells[4].Value);
                cboTrangThai.Text = dgvPromotion.Rows[e.RowIndex].Cells[5].Value.ToString();
                
                // Lấy thêm trường 'Mô tả' từ DB do trường này không được hiển thị trực tiếp trên lưới
                string sqlDesc = $"SELECT MoTaKhuyenMai FROM KhuyenMai WHERE MaKhuyenMai = {id}";
                txtMoTaKhuyenMai.Text = DbContext.GetFieldValues(sqlDesc);
                
                // 2. CHUYỂN ĐỔI TRẠNG THÁI GIAO DIỆN
                ToggleInputs(true);             // Mở khóa các ô để người dùng có thể chỉnh sửa
                
                btnEdit.Enabled = true;         // Có dòng được chọn -> Được quyền Sửa
                btnDelete.Enabled = true;       // Có dòng được chọn -> Được quyền Xóa
                btnCancel.Enabled = true;       // Bật Hủy để bỏ qua lựa chọn
                
                btnAdd.Enabled = false;         // Đang chọn dòng cũ thì cấm ấn Thêm mới (Tránh nhầm lẫn)
                btnSave.Enabled = false;        // Chỉ Lưu khi ấn Thêm mới (Sửa thì ấn nút Sửa để lưu)
            }
        }

        #endregion

        #region 4. CÁC HÀM XỬ LÝ NÚT BẤM (BUTTON CLICK HANDLERS)

        /// <summary>
        /// Nút Thêm: Đưa Form vào chế độ sẵn sàng nhập liệu mới.
        /// Lúc này Form chờ người dùng điền đủ thông tin và ấn nút Lưu.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues(); // Xóa sạch rác còn sót trên form
            
            txtMaKhuyenMai.Enabled = false;
            txtMaKhuyenMai.Text = "Tự động sinh"; // Gợi ý người dùng không cần bận tâm mã
            cboTrangThai.Text = "Hoạt động"; // Giá trị Default
            
            ToggleInputs(true); // Mở khóa nhập liệu
            
            // Chuyển nút bấm sang trạng thái "Đang chờ Lưu"
            btnSave.Enabled = true;     
            btnCancel.Enabled = true;   
            
            btnAdd.Enabled = false;     
            btnEdit.Enabled = false;    
            btnDelete.Enabled = false;  

            txtTenKhuyenMai.Focus(); // Di chuyển con trỏ chuột vào ô Tên để gõ luôn cho tiện
        }

        /// <summary>
        /// Nút Lưu: Thực thi quá trình đẩy dữ liệu mới (Insert) vào Database.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Validate kiểm duyệt
            if (!ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status))
                return; // Có lỗi thì dừng tại đây, Validation đã tự hiện MessageBox
            
            // 2. Tạo truy vấn SQL Insert
            string sql = $@"INSERT INTO KhuyenMai(TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) 
                            VALUES(N'{name}', {percent}, '{startDate}', '{endDate}', N'{desc}', N'{status}')";
                  
            // 3. Chạy lệnh
            DbContext.RunSql(sql);
            MessageBox.Show("Thêm mới khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 4. Cập nhật lại giao diện
            Load_DataGridView();    // Tải lại lưới để hiện bản ghi mới
            ResetValues();          // Xóa trắng ô nhập liệu
            ToggleInputs(false);    // Khóa form lại
            
            // 5. Đưa nút bấm về trạng thái nghỉ
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaKhuyenMai.Enabled = false;
        }

        /// <summary>
        /// Nút Sửa: Thực thi quá trình cập nhật (Update) dữ liệu của bản ghi đang chọn lên Database.
        /// Yêu cầu: Phải chọn 1 dòng trên lưới trước.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn gì chưa
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text) || txtMaKhuyenMai.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một khuyến mãi trong danh sách để chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validate dữ liệu mới
            if (!ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status))
                return;

            // 3. Khởi tạo truy vấn Update
            string sql = $@"UPDATE KhuyenMai SET 
                            TenKhuyenMai = N'{name}', 
                            PhanTramGiamGia = {percent}, 
                            NgayBatDau = '{startDate}', 
                            NgayKetThuc = '{endDate}', 
                            MoTaKhuyenMai = N'{desc}',
                            TrangThai = N'{status}' 
                            WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";

            // 4. Thực thi Update
            DbContext.RunSql(sql);
            MessageBox.Show("Cập nhật thông tin khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 5. Cập nhật lại giao diện
            Load_DataGridView();
            ResetValues();
            ToggleInputs(false);
            
            // 6. Reset nút bấm
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtMaKhuyenMai.Enabled = false;
        }

        /// <summary>
        /// Nút Xóa: Thực thi quy trình Xóa Mềm (Soft Delete) bản ghi bằng cách đổi trạng thái sang "Không hoạt động".
        /// Yêu cầu: Phải chọn 1 dòng trên lưới.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. Đảm bảo đã chọn 1 dòng hợp lệ
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text) || txtMaKhuyenMai.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Yêu cầu xác nhận (Chống xóa nhầm)
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Không hoạt động) khuyến mãi này không?", "Xác nhận hành động", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Update trạng thái thay vì chạy DELETE FROM KhuyenMai (Bảo toàn dữ liệu lịch sử)
                string sql = $"UPDATE KhuyenMai SET TrangThai = N'Không hoạt động' WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";
                DbContext.RunSql(sql);
                
                // Cập nhật lại UI
                Load_DataGridView();
                ResetValues();
                ToggleInputs(false);
                
                // Trả các nút về trạng thái nghỉ
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
            txtMaKhuyenMai.Enabled = false;
        }

        /// <summary>
        /// Nút Tìm Kiếm: Hoạt động theo 2 Phase:
        /// Phase 1: Mở khóa các ô để người dùng nhập tiêu chí lọc.
        /// Phase 2: Thực hiện truy vấn dựa trên các tiêu chí vừa nhập và cập nhật lưới.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // PHASE 1: Kích hoạt chế độ điền thông tin tìm kiếm
            if (txtMaKhuyenMai.Enabled == false && btnAdd.Enabled == true)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaKhuyenMai.Enabled = true; // Mở khóa ô mã để có thể tìm bằng ID
                
                // Disable DateTimePicker (Bỏ check) để mặc định không bắt buộc tìm bằng ngày
                dtNgayBatDau.Checked = false;
                dtNgayHetHan.Checked = false;

                // Khóa các tính năng thao tác dữ liệu
                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập các tiêu chí cần lọc vào ô nhập liệu rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhuyenMai.Focus();
                return;
            }

            // PHASE 2: Thực hiện tìm kiếm theo các trường đã điền
            string idTerm = txtMaKhuyenMai.Text.Trim();
            string nameTerm = txtTenKhuyenMai.Text.Trim();
            string statusTerm = cboTrangThai.Text;

            // Câu lệnh cơ sở
            string sql = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai WHERE 1=1";
            
            // Xây dựng điều kiện động dựa trên input
            if (!string.IsNullOrEmpty(idTerm))
                sql += $" AND MaKhuyenMai LIKE '%{idTerm}%'";
                
            if (!string.IsNullOrEmpty(nameTerm))
                sql += $" AND TenKhuyenMai LIKE N'%{nameTerm}%'";
                
            if (!string.IsNullOrEmpty(statusTerm))
                sql += $" AND TrangThai = N'{statusTerm}'";

            if (dtNgayBatDau.Checked)
                sql += $" AND NgayBatDau = '{dtNgayBatDau.Value:yyyy-MM-dd}'";
                
            if (dtNgayHetHan.Checked)
                sql += $" AND NgayKetThuc = '{dtNgayHetHan.Value:yyyy-MM-dd}'";

            // Truy vấn và gắn dữ liệu mới vào lưới
            DataTable tblKM = DbContext.GetDataToTable(sql);
            dgvPromotion.DataSource = tblKM;

            if (tblKM.Rows.Count > 0)
            {
                ResetValues();
                MessageBox.Show($"Hoàn tất! Tìm thấy {tblKM.Rows.Count} bản ghi khớp yêu cầu.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResetValues();
                MessageBox.Show("Rất tiếc, không tìm thấy khuyến mãi nào khớp với các tiêu chí tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            btnCancel.Enabled = false; // Bắt buộc dùng nút Làm Mới để thoát tìm kiếm
        }

        /// <summary>
        /// Nút Làm mới (Refresh): Tải lại toàn bộ dữ liệu gốc, xóa bộ lọc tìm kiếm và reset form.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load_DataGridView(); // Kéo lại dữ liệu không lọc
            ResetValues();       // Xóa thông tin cũ trên form
            ToggleInputs(false); // Khóa form lại
            
            // Trả trạng thái nút bấm về mặc định
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaKhuyenMai.Enabled = false;
        }

        #endregion
    }
}
