using AssignmentApp.DAL.Core;
using System;
using System.Data;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucCategory : UserControl
    {
        public ucCategory()
        {
            InitializeComponent();
        }

        private void ucCategory_Load(object sender, EventArgs e)
        {
            // Đồng bộ dữ liệu cho ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Hoạt động");
            cboTrangThai.Items.Add("Đã hủy");
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; // Ngăn người dùng gõ linh tinh

            DbContext.Ketnoi();
            Load_DataGridView();

            ResetValues();
            txtMaDanhMuc.Enabled = false; 
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void Load_DataGridView()
        {
            string sql = "SELECT MaDanhMuc, TenDanhMuc, MoTa, TrangThai, NgayTao, NgayCapNhat FROM DanhMuc";
            DataTable tblDM = DbContext.GetDataToTable(sql);
            
            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.DataSource = tblDM;
            
            if (dgvDanhMuc.Columns.Contains("colNgayTao"))
                dgvDanhMuc.Columns["colNgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            
            if (dgvDanhMuc.Columns.Contains("colNgayCapNhat"))
                dgvDanhMuc.Columns["colNgayCapNhat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenDanhMuc.Enabled = isEnabled;
            txtMoTa.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        private void ResetValues()
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = -1;
        }

        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Thoát chế độ tìm kiếm nếu đang ở chế độ tìm kiếm
                if (txtMaDanhMuc.Enabled == true)
                {
                    txtMaDanhMuc.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // Gán dữ liệu vào Textbox
                DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
                txtMaDanhMuc.Text = row.Cells[0].Value.ToString();
                txtTenDanhMuc.Text = row.Cells[1].Value.ToString();
                txtMoTa.Text = row.Cells[2].Value.ToString();
                cboTrangThai.Text = row.Cells[3].Value.ToString();
                
                // Mở khóa các ô nhập liệu
                ToggleInputs(true);
                
                // Bật tắt các nút
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues();
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            txtMaDanhMuc.Enabled = false;
            txtMaDanhMuc.Text = "(Tự động sinh)";
            cboTrangThai.Text = "Hoạt động";
            
            ToggleInputs(true);
            txtTenDanhMuc.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaDanhMuc.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return;
            }
            if (txtMoTa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mô tả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return;
            }
            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            string sql = $@"UPDATE DanhMuc SET 
                            TenDanhMuc = N'{txtTenDanhMuc.Text.Trim()}', 
                            MoTa = N'{txtMoTa.Text.Trim()}', 
                            TrangThai = N'{cboTrangThai.Text.Trim()}',
                            NgayCapNhat = GETDATE()
                            WHERE MaDanhMuc = {txtMaDanhMuc.Text}";

            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            
            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaDanhMuc.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Đã hủy) danh mục này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"UPDATE DanhMuc SET TrangThai = N'Đã hủy' WHERE MaDanhMuc = {txtMaDanhMuc.Text}";
                
                DbContext.RunSql(sql);
                Load_DataGridView();
                ResetValues();
                
                ToggleInputs(false);
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // LẦN 1: Kích hoạt chế độ tìm kiếm (khi Mã Danh Mục đang bị khóa)
            if (txtMaDanhMuc.Enabled == false)
            {
                ResetValues();
                txtMaDanhMuc.Enabled = true; // Mở khóa mã danh mục để điền
                ToggleInputs(true);

                // Ẩn/khóa các nút khác
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật! Vui lòng nhập thông tin (Mã, Tên, Mô tả...) rồi ấn nút Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaDanhMuc.Focus();
                return;
            }

            // LẦN 2: Thực hiện tìm kiếm (khi Mã Danh Mục đang được mở)
            if (txtMaDanhMuc.Text == "" && txtTenDanhMuc.Text == "" && txtMoTa.Text == "" && cboTrangThai.Text == "")
            {
                MessageBox.Show("Hãy nhập ít nhất một điều kiện tìm kiếm!!!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

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
                MessageBox.Show($"Có {dtSearch.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            dgvDanhMuc.DataSource = dtSearch;
            // Lưu ý: Vẫn giữ giao diện tìm kiếm cho đến khi click vào DataGridView hoặc bấm Làm mới
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return;
            }

            if (txtMoTa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mô tả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            string sqlCheck = $"SELECT TenDanhMuc FROM DanhMuc WHERE TenDanhMuc = N'{txtTenDanhMuc.Text.Trim()}'";
            DataTable dtCheck = DbContext.GetDataToTable(sqlCheck);
            if (dtCheck.Rows.Count > 0)
            {
                MessageBox.Show("Tên danh mục đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return;
            }

            string sql = $@"INSERT INTO DanhMuc(TenDanhMuc, MoTa, TrangThai, NgayTao) 
                            VALUES(N'{txtTenDanhMuc.Text.Trim()}', N'{txtMoTa.Text.Trim()}', N'{cboTrangThai.Text.Trim()}', GETDATE())";
            
            DbContext.RunSql(sql);
            Load_DataGridView();
            ResetValues();

            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaDanhMuc.Enabled = false;
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvDanhMuc_CellClick(sender, e);
        }
    }
}
