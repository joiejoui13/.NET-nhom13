using System;
using System.Data; // Thêm thư viện thao tác DataTable
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Thư viện kết nối CSDL DbContext

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucCategory : UserControl
    {
        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo UserControl Danh mục Sản phẩm.
        /// Chạy đầu tiên để vẽ giao diện và gán các cấu hình tĩnh.
        /// </summary>
        public ucCategory()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Danh sách tùy chọn trạng thái
            cboTrangThai.Items.Clear(); // Dọn dẹp dữ liệu cũ nếu có
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Đã hủy" });
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; // Khóa chức năng gõ phím, bắt buộc phải click chọn từ danh sách thả xuống
        }

        /// <summary>
        /// Sự kiện Load Form: Kích hoạt khi giao diện vừa hiển thị lên màn hình.
        /// Chuyên dùng để lấy dữ liệu từ DB lên lưới và khóa form ban đầu.
        /// </summary>
        private void ucCategory_Load(object sender, EventArgs e)
        {
            // 1. Khởi tạo kết nối với CSDL
            DbContext.Ketnoi();
            
            // 2. Kéo dữ liệu vào lưới DataGridView
            Load_DataGridView();

            // 3. Reset các ô nhập liệu và đưa Form về chế độ nghỉ (Read-only)
            ResetValues();
            txtMaDanhMuc.Enabled = false; // Mã danh mục là cột tự tăng Identity trong SQL nên không cho phép tự nhập tay
            ToggleInputs(false);

            // 4. Cấu hình trạng thái các nút bấm mặc định
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Tải danh sách Danh mục từ Database và cấu hình hiển thị lưới DataGridView.
        /// </summary>
        private void Load_DataGridView()
        {
            // Bước 1: Ánh xạ cột (Chỉ ánh xạ nếu cột đó có tồn tại trên Designer để tránh lỗi văng app)
            if (dgvDanhMuc.Columns.Contains("colMaDanhMuc")) dgvDanhMuc.Columns["colMaDanhMuc"].DataPropertyName = "MaDanhMuc";
            if (dgvDanhMuc.Columns.Contains("colTenDanhMuc")) dgvDanhMuc.Columns["colTenDanhMuc"].DataPropertyName = "TenDanhMuc";
            if (dgvDanhMuc.Columns.Contains("colMoTa")) dgvDanhMuc.Columns["colMoTa"].DataPropertyName = "MoTa";
            if (dgvDanhMuc.Columns.Contains("colTrangThai")) dgvDanhMuc.Columns["colTrangThai"].DataPropertyName = "TrangThai";
            if (dgvDanhMuc.Columns.Contains("colNgayTao")) dgvDanhMuc.Columns["colNgayTao"].DataPropertyName = "NgayTao";
            if (dgvDanhMuc.Columns.Contains("colNgayCapNhat")) dgvDanhMuc.Columns["colNgayCapNhat"].DataPropertyName = "NgayCapNhat";

            // Bước 2: Kéo dữ liệu từ SQL Server về DataTable
            string sql = "SELECT MaDanhMuc, TenDanhMuc, MoTa, TrangThai, NgayTao, NgayCapNhat FROM DanhMuc";
            DataTable tblDM = DbContext.GetDataToTable(sql);
            
            // Bước 3: Đẩy vào DataGridView
            dgvDanhMuc.AutoGenerateColumns = false; // Bắt buộc tắt để lưới không tự đẻ thêm cột rác
            dgvDanhMuc.DataSource = tblDM;
            
            // Bước 4: Định dạng hiển thị ngày tháng cho thân thiện với người Việt (dd/MM/yyyy)
            if (dgvDanhMuc.Columns.Contains("colNgayTao"))
                dgvDanhMuc.Columns["colNgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            
            if (dgvDanhMuc.Columns.Contains("colNgayCapNhat"))
                dgvDanhMuc.Columns["colNgayCapNhat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            // Bước 5: Cấu hình khóa bảng, chống thao tác làm hỏng giao diện
            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        /// <summary>
        /// Xóa trắng nội dung của tất cả các ô nhập liệu trên màn hình.
        /// </summary>
        private void ResetValues()
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = -1; // -1 nghĩa là không chọn mục nào cả
        }

        /// <summary>
        /// Mở hoặc khóa các ô TextBox và ComboBox.
        /// </summary>
        private void ToggleInputs(bool isEnabled)
        {
            txtTenDanhMuc.Enabled = isEnabled;
            txtMoTa.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        /// <summary>
        /// Hàm kiểm duyệt (Validate) thông tin trước khi lưu hoặc cập nhật vào CSDL.
        /// Nếu thông tin sai, hàm hiện thông báo lỗi và trả về false để ngắt quá trình xử lý.
        /// </summary>
        private bool ValidateCategoryInputs(bool isAddingNew)
        {
            // Kiểm tra TextBox Tên Danh mục
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return false;
            }

            // Kiểm tra TextBox Mô tả
            if (txtMoTa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mô tả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return false;
            }

            // Kiểm tra ComboBox Trạng thái
            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }

            // Nếu đang trong chế độ Thêm Mới, phải kiểm tra trùng lặp Tên Danh Mục trong CSDL
            if (isAddingNew == true)
            {
                string sqlCheck = $"SELECT TenDanhMuc FROM DanhMuc WHERE TenDanhMuc = N'{txtTenDanhMuc.Text.Trim()}'";
                DataTable dtCheck = DbContext.GetDataToTable(sqlCheck);
                
                // Nếu số dòng trả về lớn hơn 0 nghĩa là tên này đã bị ai đó dùng rồi
                if (dtCheck.Rows.Count > 0)
                {
                    MessageBox.Show("Tên danh mục đã tồn tại, vui lòng chọn tên khác!", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDanhMuc.Focus();
                    return false;
                }
            }

            return true; // Dữ liệu hoàn toàn hợp lệ
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Xử lý việc click chuột vào một dòng bất kỳ trên lưới DataGridView.
        /// Cập nhật thông tin dòng đó lên các TextBox để người dùng có thể Xem/Sửa/Xóa.
        /// </summary>
        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu click đúng vào 1 dòng dữ liệu (không phải click vào tiêu đề cột rowIndex = -1)
            if (e.RowIndex >= 0)
            {
                // Hủy bỏ chế độ Tìm kiếm nếu đang dùng
                if (txtMaDanhMuc.Enabled == true)
                {
                    txtMaDanhMuc.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // Gán dữ liệu từ DataGridView lên TextBox
                DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
                txtMaDanhMuc.Text = row.Cells[0].Value.ToString();
                txtTenDanhMuc.Text = row.Cells[1].Value.ToString();
                txtMoTa.Text = row.Cells[2].Value.ToString();
                cboTrangThai.Text = row.Cells[3].Value.ToString();
                
                // Mở khóa nhập liệu
                ToggleInputs(true);
                
                // Bật tắt các nút chức năng tương ứng
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
        /// Nút Thêm: Đưa Form vào chế độ "Chuẩn bị thêm mới".
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues();
            
            txtMaDanhMuc.Enabled = false;
            txtMaDanhMuc.Text = "(Tự động sinh)";
            cboTrangThai.Text = "Hoạt động"; // Đặt mặc định trạng thái
            
            ToggleInputs(true);
            txtTenDanhMuc.Focus(); // Bắt con trỏ chuột nhảy vào ô đầu tiên để gõ phím ngay

            // Chuyển chế độ nút bấm
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        /// <summary>
        /// Nút Lưu: Thực hiện chèn (INSERT) dòng dữ liệu mới vào bảng DanhMuc trong DB.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra kỹ lưỡng đầu vào (truyền tham số true vì đang Thêm mới)
            if (ValidateCategoryInputs(true) == false)
            {
                return; // Nếu có lỗi, kết thúc hàm ngay lập tức
            }

            // 2. Viết câu lệnh Insert dữ liệu, chú ý chữ N phía trước để lưu tiếng Việt có dấu
            string sql = $@"INSERT INTO DanhMuc(TenDanhMuc, MoTa, TrangThai, NgayTao) 
                            VALUES(N'{txtTenDanhMuc.Text.Trim()}', N'{txtMoTa.Text.Trim()}', N'{cboTrangThai.Text.Trim()}', GETDATE())";
            
            // 3. Thực thi
            DbContext.RunSql(sql);
            MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 4. Dọn dẹp form
            Load_DataGridView();
            ResetValues();
            ToggleInputs(false);

            // 5. Cài đặt lại giao diện nút
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaDanhMuc.Enabled = false;
        }

        /// <summary>
        /// Nút Sửa: Thực hiện sửa thông tin (UPDATE) dựa theo mã Danh mục đang chọn.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã nhấp chuột chọn danh mục nào chưa
            if (dgvDanhMuc.Rows.Count == 0 || txtMaDanhMuc.Text == "" || txtMaDanhMuc.Text == "(Tự động sinh)")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Kiểm duyệt đầu vào (truyền tham số false vì đang là Sửa, không cần check trùng lặp tên với chính nó)
            if (ValidateCategoryInputs(false) == false)
            {
                return;
            }

            // 3. Cập nhật dữ liệu
            string sql = $@"UPDATE DanhMuc SET 
                            TenDanhMuc = N'{txtTenDanhMuc.Text.Trim()}', 
                            MoTa = N'{txtMoTa.Text.Trim()}', 
                            TrangThai = N'{cboTrangThai.Text.Trim()}',
                            NgayCapNhat = GETDATE()
                            WHERE MaDanhMuc = {txtMaDanhMuc.Text}";

            DbContext.RunSql(sql);
            MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 4. Trả form về trạng thái nghỉ
            Load_DataGridView();
            ResetValues();
            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
        }

        /// <summary>
        /// Nút Xóa: Thực thi quy trình Xóa Mềm, không xóa hẳn mà chuyển trạng thái thành Đã hủy để giữ log hệ thống.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count == 0 || txtMaDanhMuc.Text == "" || txtMaDanhMuc.Text == "(Tự động sinh)")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Đã hủy) danh mục này không?", "Cảnh báo xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"UPDATE DanhMuc SET TrangThai = N'Đã hủy' WHERE MaDanhMuc = {txtMaDanhMuc.Text}";
                
                DbContext.RunSql(sql);
                MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        /// Nút Hủy: Bỏ qua quá trình thêm mới / sửa dở dang và đưa form về trạng thái ban đầu.
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
            txtMaDanhMuc.Enabled = false;
        }

        /// <summary>
        /// Nút Tìm Kiếm: Mở khóa các ô để người dùng điền tiêu chí lọc, hoặc thực thi lọc nếu đã điền xong.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // PHASE 1: Kích hoạt chế độ tìm kiếm
            if (txtMaDanhMuc.Enabled == false)
            {
                ResetValues();
                txtMaDanhMuc.Enabled = true; // Mở khóa mã danh mục để điền
                ToggleInputs(true);

                // Ẩn/khóa các nút chức năng tránh thao tác nhầm
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập thông tin (Mã, Tên, Mô tả...) vào ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaDanhMuc.Focus();
                return;
            }

            // PHASE 2: Bắt đầu dò tìm
            // Kiểm tra xem người dùng đã nhập gì chưa, tránh tìm kiếm trống tốn thời gian
            if (txtMaDanhMuc.Text == "" && txtTenDanhMuc.Text == "" && txtMoTa.Text == "" && cboTrangThai.Text == "")
            {
                MessageBox.Show("Hãy nhập ít nhất một điều kiện tìm kiếm!!!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            // Cấu trúc if nối chuỗi cơ bản cho beginner
            string sql = "SELECT MaDanhMuc, TenDanhMuc, MoTa, TrangThai, NgayTao, NgayCapNhat FROM DanhMuc WHERE 1=1";
            
            if (txtMaDanhMuc.Text != "")
                sql += $" AND MaDanhMuc LIKE '%{txtMaDanhMuc.Text.Trim()}%'";
                
            if (txtTenDanhMuc.Text != "")
                sql += $" AND TenDanhMuc LIKE N'%{txtTenDanhMuc.Text.Trim()}%'";
                
            if (txtMoTa.Text != "")
                sql += $" AND MoTa LIKE N'%{txtMoTa.Text.Trim()}%'";
                
            if (cboTrangThai.Text != "")
                sql += $" AND TrangThai = N'{cboTrangThai.Text}'";

            DataTable dtSearch = DbContext.GetDataToTable(sql);
            
            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Tìm thấy {dtSearch.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            dgvDanhMuc.DataSource = dtSearch;
        }

        /// <summary>
        /// Nút Làm mới: Tải lại toàn bộ dữ liệu (Bỏ lọc tìm kiếm) và khóa form.
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
            txtMaDanhMuc.Enabled = false;
        }

        #endregion

        #region 5. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do lỡ tay click ở giao diện Designer, có thể ủy quyền cho CellClick xử lý chung
            dgvDanhMuc_CellClick(sender, e);
        }

        #endregion
    }
}
