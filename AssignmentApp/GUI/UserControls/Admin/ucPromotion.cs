using System;
using System.Data;                                     // Thêm thư viện này để dùng DataTable
using System.Windows.Forms;
using AssignmentApp.DAL.Core;                         // Để gọi class DbContext (Tương đương class Functions của bạn)

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucPromotion : UserControl
    {
        public ucPromotion()
        {
            InitializeComponent();
        }

        // 5.2.2. Viết thủ tục Form_Load của ucPromotion
        private void ucPromotion_Load(object sender, EventArgs e)
        {
            DbContext.Ketnoi(); 
            Load_DataGridView();
            
            // Trạng thái ban đầu khi mới mở Form:
            ResetValues();
            txtMaKhuyenMai.Enabled = false; // Mã tự sinh nên khóa lại
            
            btnAdd.Enabled = true;          // Cho phép Thêm
            btnEdit.Enabled = false;        // Chưa chọn dòng nào thì không cho Sửa
            btnDelete.Enabled = false;      // Chưa chọn dòng nào thì không cho Xóa
            btnSave.Enabled = false;        // Chưa làm gì thì không cho Lưu
            btnCancel.Enabled = false;      // Chưa làm gì thì không cho Hủy
        }

        // 5.2.3. Viết thủ tục Load_DataGridView
        private void Load_DataGridView()
        {
            string sql = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai";
            DataTable tblKM = DbContext.GetDataToTable(sql);
            
            // Quan trọng: Tắt tính năng tự động đẻ thêm cột (gây lặp cột)
            dgvPromotion.AutoGenerateColumns = false;
            dgvPromotion.DataSource = tblKM;

            // Nối dữ liệu từ SQL vào các cột đã có sẵn trên Form
            dgvPromotion.Columns[0].DataPropertyName = "MaKhuyenMai";
            dgvPromotion.Columns[1].DataPropertyName = "TenKhuyenMai";
            dgvPromotion.Columns[2].DataPropertyName = "PhanTramGiamGia";
            dgvPromotion.Columns[3].DataPropertyName = "NgayBatDau";
            dgvPromotion.Columns[4].DataPropertyName = "NgayKetThuc";
            dgvPromotion.Columns[5].DataPropertyName = "TrangThai";

            // Đặt tên cột hiển thị rõ ràng
            dgvPromotion.Columns[0].HeaderText = "Mã KM";
            dgvPromotion.Columns[1].HeaderText = "Tên Khuyến Mãi";
            dgvPromotion.Columns[2].HeaderText = "% Giảm";
            dgvPromotion.Columns[3].HeaderText = "Ngày Bắt Đầu";
            dgvPromotion.Columns[4].HeaderText = "Ngày Kết Thúc";
            dgvPromotion.Columns[5].HeaderText = "Trạng Thái";

            // Định dạng lề và kích thước để không bị mất chữ
            dgvPromotion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvPromotion.Columns[0].Width = 80;
            dgvPromotion.Columns[0].MinimumWidth = 80;
            dgvPromotion.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvPromotion.Columns[1].MinimumWidth = 200;
            dgvPromotion.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Tên KM cho giãn tự động
            
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

            // Làm cho Header và Row chuẩn hóa giống ucUserManagement
            dgvPromotion.RowTemplate.Height = 40;
            dgvPromotion.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvPromotion.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvPromotion.ColumnHeadersHeight = 40; 
            dgvPromotion.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Không cho phép thêm mới dữ liệu trực tiếp trên lưới
            dgvPromotion.AllowUserToAddRows = false;
            // Không cho phép sửa dữ liệu trực tiếp trên lưới
            dgvPromotion.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // 5.2.4. Viết thủ tục ResetValues
        private void ResetValues()
        {
            txtMaKhuyenMai.Text = "";
            txtTenKhuyenMai.Text = "";
            txtPhanTramGiamGia.Text = "";
            txtMoTaKhuyenMai.Text = "";
            dtNgayBatDau.Value = DateTime.Now;
            dtNgayHetHan.Value = DateTime.Now; // Biến trên giao diện vẫn là dtNgayHetHan
            cboTrangThai.SelectedIndex = -1;
        }

        // 5.2.5. Viết thủ tục DataGridView_Click
        private void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhuyenMai.Focus();
                return;
            }
            if (dgvPromotion.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Đổ dữ liệu từ DataGridView lên TextBox bằng chỉ số cột (tránh lỗi sai tên cột)
            txtMaKhuyenMai.Text = dgvPromotion.CurrentRow.Cells[0].Value.ToString();
            txtTenKhuyenMai.Text = dgvPromotion.CurrentRow.Cells[1].Value.ToString();
            txtPhanTramGiamGia.Text = dgvPromotion.CurrentRow.Cells[2].Value.ToString();
            dtNgayBatDau.Value = Convert.ToDateTime(dgvPromotion.CurrentRow.Cells[3].Value);
            dtNgayHetHan.Value = Convert.ToDateTime(dgvPromotion.CurrentRow.Cells[4].Value);
            cboTrangThai.Text = dgvPromotion.CurrentRow.Cells[5].Value.ToString();
            
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        // 5.2.6. Viết thủ tục btnThem_Click (Nút Thêm mới)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Bước 1: Xóa trắng các ô nhập liệu
            ResetValues();
            
            // Bước 2: Bật/Tắt các nút bấm cho phù hợp với chế độ Thêm
            btnSave.Enabled = true;       // Cho phép bấm Lưu
            btnCancel.Enabled = true;     // Cho phép bấm Hủy (Bỏ qua)
            btnAdd.Enabled = false;       // Đang thêm thì ẩn nút Thêm đi
            btnEdit.Enabled = false;      // Không cho phép Sửa
            btnDelete.Enabled = false;    // Không cho phép Xóa

            // Bước 3: Đưa con trỏ chuột nhấp nháy vào ô Tên Khuyến Mãi
            txtMaKhuyenMai.Enabled = false; // Mã tự tăng nên không được nhập
            txtTenKhuyenMai.Focus();
        }

        // 5.2.7. Viết thủ tục btnLuu_Click (Nút Lưu thay đổi)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra dữ liệu đầu vào
            if (txtTenKhuyenMai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khuyến mãi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhuyenMai.Focus();
                return;
            }
            
            // Xử lý nếu % giảm giá để trống
            string phanTram = txtPhanTramGiamGia.Text.Trim();
            if (string.IsNullOrEmpty(phanTram)) phanTram = "0";
            
            // Bước 2: Tạo câu lệnh SQL INSERT (Không truyền MaKhuyenMai vì DB tự động sinh mã)
            string sql = $@"INSERT INTO KhuyenMai(TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) 
                            VALUES(N'{txtTenKhuyenMai.Text}', {phanTram}, '{dtNgayBatDau.Value:yyyy-MM-dd}', '{dtNgayHetHan.Value:yyyy-MM-dd}', N'{txtMoTaKhuyenMai.Text}', N'{cboTrangThai.Text}')";
                  
            // Bước 3: Thực thi và tải lại bảng
            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            
            // Bước 4: Chuyển các nút về trạng thái ban đầu
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaKhuyenMai.Enabled = false;
        }

        // 5.2.8. Viết thủ tục btnSua_Click (Nút Sửa)
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra xem có dữ liệu để sửa không
            if (dgvPromotion.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaKhuyenMai.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenKhuyenMai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khuyến mãi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhuyenMai.Focus();
                return;
            }
            
            // Nếu người dùng lỡ xóa trắng % giảm giá thì mặc định là 0
            string phanTram = txtPhanTramGiamGia.Text.Trim();
            if (string.IsNullOrEmpty(phanTram)) phanTram = "0";

            // Bước 2: Tạo câu lệnh SQL UPDATE
            string sql = $@"UPDATE KhuyenMai SET 
                            TenKhuyenMai = N'{txtTenKhuyenMai.Text}', 
                            PhanTramGiamGia = {phanTram}, 
                            NgayBatDau = '{dtNgayBatDau.Value:yyyy-MM-dd}', 
                            NgayKetThuc = '{dtNgayHetHan.Value:yyyy-MM-dd}', 
                            TrangThai = N'{cboTrangThai.Text}' 
                            WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";

            // Bước 3: Thực thi câu lệnh và tải lại bảng
            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            
            // Bước 4: Khóa lại các nút
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        // 5.2.9. Viết thủ tục btnXoa_Click (Nút Xóa)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra xem có dữ liệu không
            if (dgvPromotion.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaKhuyenMai.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Bước 2: Hỏi lại người dùng cho chắc chắn
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Bước 3: Tạo câu lệnh SQL DELETE
                string sql = $"DELETE KhuyenMai WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";
                
                // Thực thi và tải lại dữ liệu
                DbContext.RunSqlDel(sql);
                Load_DataGridView();
                ResetValues();
                
                // Khóa lại các nút
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        // 5.2.10. Viết thủ tục btnBoqua_Click (Nút Bỏ qua / Hủy)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false; // Hủy thì không có bản ghi nào được chọn
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaKhuyenMai.Enabled = false;
        }

        // 5.2.11. Viết thủ tục btnTimkiem_Click (Nút Tìm kiếm)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Mở khóa ô Mã Khuyến Mãi để người dùng có thể gõ mã vào tìm kiếm
            txtMaKhuyenMai.Enabled = true; 

            // Bước 1: Kiểm tra xem có ô nào được nhập dữ liệu chưa
            if (txtMaKhuyenMai.Text == "" && txtTenKhuyenMai.Text == "" && cboTrangThai.Text == "")
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!! (Ví dụ: Tên, Mã hoặc Trạng thái)", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            // Bước 2: Bắt đầu ghép câu lệnh SQL (chọn đúng 6 cột như lúc nạp lên Grid)
            string sql = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai WHERE 1=1";
            
            // Nếu người dùng có gõ Mã Khuyến Mãi
            if (txtMaKhuyenMai.Text != "")
                sql += $" AND MaKhuyenMai LIKE '%{txtMaKhuyenMai.Text}%'";
                
            // Nếu người dùng có gõ Tên Khuyến Mãi
            if (txtTenKhuyenMai.Text != "")
                sql += $" AND TenKhuyenMai LIKE N'%{txtTenKhuyenMai.Text}%'";
                
            // Nếu người dùng có chọn Trạng Thái
            if (cboTrangThai.Text != "")
                sql += $" AND TrangThai LIKE N'%{cboTrangThai.Text}%'";

            // Bước 3: Lấy dữ liệu và kiểm tra kết quả
            DataTable tblKM = DbContext.GetDataToTable(sql);
            if (tblKM.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Có {tblKM.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            // Bước 4: Đổ kết quả tìm được lên lưới DataGridView
            dgvPromotion.DataSource = tblKM;
            ResetValues();
            
            btnCancel.Enabled = true; // Mở nút Bỏ qua để người dùng có thể Reset lại bảng gốc
        }

        // 5.2.12. Viết thủ tục btnHienthi_Click (Nút Làm mới)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Tải lại toàn bộ dữ liệu gốc
            Load_DataGridView();
            ResetValues();
            
            // Đưa các nút về trạng thái mặc định
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
        }
    }
}
