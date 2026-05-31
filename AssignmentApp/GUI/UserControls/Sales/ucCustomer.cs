using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : UserControl
    {
        private bool isAddingNew = false;

        public ucCustomer()
        {
            InitializeComponent();
        }

        private void ucCustomer_Load(object sender, EventArgs e)
        {
            dtpNgayTao.ValueChanged -= dtpNgayTao_ValueChanged;
            dtpNgayTao.ValueChanged += dtpNgayTao_ValueChanged;
            LoadData();
            ResetState();
        }

        private void dtpNgayTao_ValueChanged(object sender, EventArgs e)
        {
            if (dtpNgayTao.CustomFormat == " ")
            {
                dtpNgayTao.Format = DateTimePickerFormat.Short;
            }
        }

        private void LoadData()
        {
            dgvCustomers.Rows.Clear();
            try
            {
                AssignmentApp.DAL.Core.DbContext.Ketnoi();
                System.Data.DataTable dt = AssignmentApp.DAL.Core.DbContext.GetDataToTable("SELECT * FROM KhachHang");
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string maKhachHang = row["MaKhachHang"].ToString();
                    string tenKhachHang = row["TenKhachHang"].ToString();
                    string sdt = row["SoDienThoai"] != DBNull.Value ? row["SoDienThoai"].ToString() : "";
                    string email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
                    string diaChi = row["DiaChi"] != DBNull.Value ? row["DiaChi"].ToString() : "";
                    string ngayTao = row["NgayTao"] != DBNull.Value ? Convert.ToDateTime(row["NgayTao"]).ToString("dd/MM/yyyy") : "";
                    string trangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString() : "";

                    dgvCustomers.Rows.Add(maKhachHang, tenKhachHang, sdt, email, diaChi, ngayTao, trangThai);
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi nếu bảng chưa tồn tại
            }
        }

        private void ResetValues()
        {
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            dtpNgayTao.Format = DateTimePickerFormat.Short;
            dtpNgayTao.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenKhachHang.Enabled = isEnabled;
            txtSoDienThoai.Enabled = isEnabled;
            txtEmail.Enabled = isEnabled;
            txtDiaChi.Enabled = isEnabled;
            dtpNgayTao.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        private void ResetState()
        {
            isAddingNew = false;
            ResetValues();

            txtMaKhachHang.Enabled = false;
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["colMaKhachHang"].Value?.ToString();
                txtTenKhachHang.Text = row.Cells["colTenKhachHang"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["colSoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["colEmail"].Value?.ToString();
                txtDiaChi.Text = row.Cells["colDiaChi"].Value?.ToString();
                cboTrangThai.Text = row.Cells["colTrangThai"].Value?.ToString();
                
                try
                {
                    string ngayTaoStr = row.Cells["colNgayTao"].Value?.ToString();
                    if (!string.IsNullOrEmpty(ngayTaoStr))
                    {
                        dtpNgayTao.Value = DateTime.ParseExact(ngayTaoStr, "dd/MM/yyyy", null);
                    }
                } catch { }

                ToggleInputs(true);
                txtMaKhachHang.Enabled = false;

                btnAdd.Enabled = false;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                btnSave.Enabled = false;
                btnCancel.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            ResetValues();
            ToggleInputs(true);

            // Lấy mã tự sinh
            string nextId = AssignmentApp.DAL.Core.DbContext.GetFieldValues("SELECT ISNULL(MAX(MaKhachHang), 0) + 1 FROM KhachHang");

            txtMaKhachHang.Text = nextId;
            txtMaKhachHang.Enabled = false; // khóa không cho click
            dtpNgayTao.Enabled = false; // khóa vì tự động lấy ngày hiện tại
            cboTrangThai.Text = "Hoạt động"; // trạng thái mặc định

            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            
            txtTenKhachHang.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text) || isAddingNew)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa!");
                return;
            }

            string ten = txtTenKhachHang.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string trangThai = cboTrangThai.Text;
            string maKH = txtMaKhachHang.Text;

            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            string sql = $"UPDATE KhachHang SET TenKhachHang = N'{ten}', SoDienThoai = '{sdt}', Email = '{email}', DiaChi = N'{diaChi}', TrangThai = N'{trangThai}' WHERE MaKhachHang = '{maKH}'";
            AssignmentApp.DAL.Core.DbContext.RunSql(sql);
            MessageBox.Show("Lưu thay đổi thành công!");

            LoadData();
            ResetState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isAddingNew) return;

            string ten = txtTenKhachHang.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string ngay = DateTime.Now.ToString("yyyy-MM-dd");
            string trangThai = cboTrangThai.Text;

            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            string sql = $"INSERT INTO KhachHang (TenKhachHang, SoDienThoai, Email, DiaChi, NgayTao, TrangThai) VALUES (N'{ten}', '{sdt}', '{email}', N'{diaChi}', '{ngay}', N'{trangThai}')";
            AssignmentApp.DAL.Core.DbContext.RunSql(sql);
            MessageBox.Show("Thêm khách hàng thành công! Mã khách hàng đã được tự động sinh.");
            
            LoadData();
            ResetState();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Ngừng hoạt động) khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                AssignmentApp.DAL.Core.DbContext.RunSql($"UPDATE KhachHang SET TrangThai = N'Ngừng hoạt động' WHERE MaKhachHang = '{txtMaKhachHang.Text}'");
                MessageBox.Show("Đã xóa bản ghi (chuyển trạng thái) thành công!");
                LoadData();
                ResetState();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ResetState();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaKhachHang.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaKhachHang.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                
                dtpNgayTao.Format = DateTimePickerFormat.Custom;
                dtpNgayTao.CustomFormat = " "; // Đặt ngày tạo về null

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập các tiêu chí cần lọc vào ô nhập liệu rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMaKhachHang.Text.Trim()) &&
                string.IsNullOrEmpty(txtTenKhachHang.Text.Trim()) &&
                string.IsNullOrEmpty(txtSoDienThoai.Text.Trim()) &&
                string.IsNullOrEmpty(txtEmail.Text.Trim()) &&
                string.IsNullOrEmpty(txtDiaChi.Text.Trim()) &&
                cboTrangThai.SelectedIndex == -1 &&
                dtpNgayTao.CustomFormat == " ")
            {
                MessageBox.Show("Vui lòng nhập/chọn ít nhất một thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvCustomers.Rows.Clear();
            string sql = "SELECT * FROM KhachHang WHERE 1=1 ";
            
            if (!string.IsNullOrEmpty(txtMaKhachHang.Text.Trim()))
                sql += $" AND MaKhachHang = '{txtMaKhachHang.Text.Trim()}' ";
            if (!string.IsNullOrEmpty(txtTenKhachHang.Text.Trim()))
                sql += $" AND TenKhachHang LIKE N'%{txtTenKhachHang.Text.Trim()}%' ";
            if (!string.IsNullOrEmpty(txtSoDienThoai.Text.Trim()))
                sql += $" AND SoDienThoai LIKE '%{txtSoDienThoai.Text.Trim()}%' ";
            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
                sql += $" AND Email LIKE '%{txtEmail.Text.Trim()}%' ";
            if (!string.IsNullOrEmpty(txtDiaChi.Text.Trim()))
                sql += $" AND DiaChi LIKE N'%{txtDiaChi.Text.Trim()}%' ";
            if (cboTrangThai.SelectedIndex != -1)
                sql += $" AND TrangThai = N'{cboTrangThai.Text}' ";
            if (dtpNgayTao.CustomFormat != " ")
                sql += $" AND CONVERT(date, NgayTao) = '{dtpNgayTao.Value.ToString("yyyy-MM-dd")}' ";

            try
            {
                System.Data.DataTable dt = AssignmentApp.DAL.Core.DbContext.GetDataToTable(sql);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string maKhachHang = row["MaKhachHang"].ToString();
                    string tenKhachHang = row["TenKhachHang"].ToString();
                    string sdt = row["SoDienThoai"] != DBNull.Value ? row["SoDienThoai"].ToString() : "";
                    string email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
                    string diaChi = row["DiaChi"] != DBNull.Value ? row["DiaChi"].ToString() : "";
                    string ngayTao = row["NgayTao"] != DBNull.Value ? Convert.ToDateTime(row["NgayTao"]).ToString("dd/MM/yyyy") : "";
                    string trangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString() : "";

                    dgvCustomers.Rows.Add(maKhachHang, tenKhachHang, sdt, email, diaChi, ngayTao, trangThai);
                }
            }
            catch { }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetState();
        }
    }
}
