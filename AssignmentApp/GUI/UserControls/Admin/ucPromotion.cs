using System;
using System.Data; // Thêm thư viện này để dùng DataTable
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Để gọi class DbContext (Tương đương class Functions của bạn)

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucPromotion : UserControl
    {
        public ucPromotion()
        {
            InitializeComponent();

            // Extracted from Designer
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Không hoạt động" });
        }

        // 5.2.2. Viết thủ tục Form_Load của ucPromotion
        private void ucPromotion_Load(object sender, EventArgs e)
        {
            DbContext.Ketnoi(); 
            Load_DataGridView();
            
            // Trạng thái ban đầu khi mới mở Form:
            ResetValues();
            txtMaKhuyenMai.Enabled = false; // Mã tự sinh nên khóa lại
            ToggleInputs(false);
            
            btnAdd.Enabled = true;          // Cho phép Thêm
            btnEdit.Enabled = false;        // Chưa chọn dòng nào thì không cho Sửa
            btnDelete.Enabled = false;      // Chưa chọn dòng nào thì không cho Xóa
            btnSave.Enabled = false;        // Chưa làm gì thì không cho Lưu
            btnCancel.Enabled = false;      // Chưa làm gì thì không cho Hủy
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenKhuyenMai.Enabled = isEnabled;
            txtPhanTramGiamGia.Enabled = isEnabled;
            txtMoTaKhuyenMai.Enabled = isEnabled;
            dtNgayBatDau.Enabled = isEnabled;
            dtNgayHetHan.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
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
            dtNgayBatDau.Checked = true;
            dtNgayHetHan.Value = DateTime.Now; // Biến trên giao diện vẫn là dtNgayHetHan
            dtNgayHetHan.Checked = true;
            cboTrangThai.SelectedIndex = -1;
        }

        // 5.2.5. Viết thủ tục DataGridView_Click
        private void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Thoát chế độ tìm kiếm nếu đang ở chế độ tìm kiếm
                if (txtMaKhuyenMai.Enabled == true)
                {
                    txtMaKhuyenMai.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // Đổ dữ liệu từ DataGridView lên TextBox
                string id = dgvPromotion.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtMaKhuyenMai.Text = id;
                txtTenKhuyenMai.Text = dgvPromotion.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPhanTramGiamGia.Text = dgvPromotion.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtNgayBatDau.Value = Convert.ToDateTime(dgvPromotion.Rows[e.RowIndex].Cells[3].Value);
                dtNgayHetHan.Value = Convert.ToDateTime(dgvPromotion.Rows[e.RowIndex].Cells[4].Value);
                cboTrangThai.Text = dgvPromotion.Rows[e.RowIndex].Cells[5].Value.ToString();
                
                // Lấy Mô tả khuyến mãi (vì cột này không hiện trên lưới)
                string sqlDesc = $"SELECT MoTaKhuyenMai FROM KhuyenMai WHERE MaKhuyenMai = {id}";
                txtMoTaKhuyenMai.Text = DbContext.GetFieldValues(sqlDesc);
                
                ToggleInputs(true);
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        // 5.2.6. Viết thủ tục btnThem_Click (Nút Thêm mới)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues();
            
            txtMaKhuyenMai.Enabled = false;
            txtMaKhuyenMai.Text = "Tự động sinh";
            cboTrangThai.Text = "Hoạt động";
            
            ToggleInputs(true);
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            txtTenKhuyenMai.Focus();
        }

        private bool ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status)
        {
            name = txtTenKhuyenMai.Text.Trim();
            desc = txtMoTaKhuyenMai.Text.Trim();
            status = cboTrangThai.Text;
            startDate = dtNgayBatDau.Value.ToString("yyyy-MM-dd");
            endDate = dtNgayHetHan.Value.ToString("yyyy-MM-dd");
            percent = 0;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Bạn phải nhập tên khuyến mãi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhuyenMai.Focus();
                return false;
            }

            string phanTram = txtPhanTramGiamGia.Text.Trim();
            if (string.IsNullOrEmpty(phanTram)) phanTram = "0";
            if (!float.TryParse(phanTram, out percent) || percent < 0 || percent > 100)
            {
                MessageBox.Show("Phần trăm giảm giá phải là số từ 0 đến 100!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhanTramGiamGia.Focus();
                return false;
            }

            if (dtNgayBatDau.Value > dtNgayHetHan.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtNgayBatDau.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }
            
            return true;
        }

        // 5.2.7. Viết thủ tục btnLuu_Click (Nút Lưu thay đổi)
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status))
                return;
            
            string sql = $@"INSERT INTO KhuyenMai(TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) 
                            VALUES(N'{name}', {percent}, '{startDate}', '{endDate}', N'{desc}', N'{status}')";
                  
            DbContext.RunSql(sql);
            
            MessageBox.Show("Thêm mới khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Load_DataGridView();
            ResetValues();
            
            ToggleInputs(false);
            
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
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text) || txtMaKhuyenMai.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một khuyến mãi trong danh sách để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidatePromotionInputs(out string name, out float percent, out string desc, out string startDate, out string endDate, out string status))
                return;

            string sql = $@"UPDATE KhuyenMai SET 
                            TenKhuyenMai = N'{name}', 
                            PhanTramGiamGia = {percent}, 
                            NgayBatDau = '{startDate}', 
                            NgayKetThuc = '{endDate}', 
                            MoTaKhuyenMai = N'{desc}',
                            TrangThai = N'{status}' 
                            WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";

            DbContext.RunSql(sql);
            
            MessageBox.Show("Cập nhật thông tin khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Load_DataGridView();
            ResetValues();
            
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtMaKhuyenMai.Enabled = false;
        }

        // 5.2.9. Viết thủ tục btnXoa_Click (Nút Xóa)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text) || txtMaKhuyenMai.Text == "Tự động sinh")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Không hoạt động) khuyến mãi này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"UPDATE KhuyenMai SET TrangThai = N'Không hoạt động' WHERE MaKhuyenMai = {txtMaKhuyenMai.Text}";
                
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

        // 5.2.10. Viết thủ tục btnBoqua_Click (Nút Bỏ qua / Hủy)
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

        // 5.2.11. Viết thủ tục btnTimkiem_Click (Nút Tìm kiếm)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lần 1: Kích hoạt chế độ tìm kiếm
            if (txtMaKhuyenMai.Enabled == false && btnAdd.Enabled == true)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaKhuyenMai.Enabled = true;
                
                // Cho ngày tháng về null (không chọn) để không bắt buộc tìm theo ngày
                dtNgayBatDau.Checked = false;
                dtNgayHetHan.Checked = false;

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật! Vui lòng nhập thông tin cần tìm kiếm vào các ô dữ liệu rồi ấn Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhuyenMai.Focus();
                return;
            }

            // Lần 2: Bắt đầu tìm kiếm
            string idTerm = txtMaKhuyenMai.Text.Trim();
            string nameTerm = txtTenKhuyenMai.Text.Trim();
            string statusTerm = cboTrangThai.Text;

            string sql = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai WHERE 1=1";
            
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

            DataTable tblKM = DbContext.GetDataToTable(sql);
            dgvPromotion.DataSource = tblKM;

            if (tblKM.Rows.Count > 0)
            {
                ResetValues();
                MessageBox.Show($"Tìm thấy {tblKM.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResetValues();
                MessageBox.Show("Không tìm thấy bản ghi nào khớp với các tiêu chí tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            btnCancel.Enabled = false;
        }

        // 5.2.12. Viết thủ tục btnHienthi_Click (Nút Làm mới)
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
            txtMaKhuyenMai.Enabled = false;
        }
    }
}
