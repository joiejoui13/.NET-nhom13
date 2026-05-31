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
            LoadData();
            ResetState();
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

                    dgvCustomers.Rows.Add(maKhachHang, tenKhachHang, sdt, email, diaChi, ngayTao);
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi nếu bảng chưa tồn tại
            }
        }

        private void ResetState()
        {
            isAddingNew = false;

            // Trắng thông tin
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            dtpNgayTao.Value = DateTime.Now;

            // Các nút: thêm, tìm kiếm, làm mới hoạt động
            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            // Nút sửa, xóa, lưu, bỏ qua bị vô hiệu hóa
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            // Cho phép click để tìm kiếm
            txtMaKhachHang.Enabled = true;
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
                
                try
                {
                    string ngayTaoStr = row.Cells["colNgayTao"].Value?.ToString();
                    if (!string.IsNullOrEmpty(ngayTaoStr))
                    {
                        dtpNgayTao.Value = DateTime.ParseExact(ngayTaoStr, "dd/MM/yyyy", null);
                    }
                } catch { }

                // Khi click vào: nút hoạt động bình thường, trừ nút lưu/bỏ qua
                btnAdd.Enabled = true;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;

            // Lấy mã tự sinh
            string nextId = AssignmentApp.DAL.Core.DbContext.GetFieldValues("SELECT ISNULL(MAX(MaKhachHang), 0) + 1 FROM KhachHang");

            // Làm trắng thông tin
            txtMaKhachHang.Text = nextId;
            txtMaKhachHang.Enabled = false; // khóa không cho click
            
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            dtpNgayTao.Value = DateTime.Now;

            // Nút lưu, bỏ qua hoạt động
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            // Tắt nút khác
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa!");
                return;
            }

            isAddingNew = false;

            // Khóa mã khi sửa
            txtMaKhachHang.Enabled = false;

            // Nút lưu, bỏ qua hoạt động
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ten = txtTenKhachHang.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string ngay = DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            if (isAddingNew)
            {
                string sql = $"INSERT INTO KhachHang (TenKhachHang, SoDienThoai, Email, DiaChi, NgayTao) VALUES (N'{ten}', '{sdt}', '{email}', N'{diaChi}', '{ngay}')";
                AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                MessageBox.Show("Thêm khách hàng thành công! Mã khách hàng đã được tự động sinh.");
            }
            else
            {
                if (string.IsNullOrEmpty(txtMaKhachHang.Text))
                {
                    MessageBox.Show("Không có khách hàng nào đang được chỉnh sửa!");
                    return;
                }
                
                string maKH = txtMaKhachHang.Text;
                string sql = $"UPDATE KhachHang SET TenKhachHang = N'{ten}', SoDienThoai = '{sdt}', Email = '{email}', DiaChi = N'{diaChi}' WHERE MaKhachHang = '{maKH}'";
                AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                MessageBox.Show("Lưu thay đổi thành công!");
            }

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

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                AssignmentApp.DAL.Core.DbContext.RunSql($"DELETE FROM KhachHang WHERE MaKhachHang = '{txtMaKhachHang.Text}'");
                MessageBox.Show("Xóa thành công!");
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
            dgvCustomers.Rows.Clear();
            string sql = "SELECT * FROM KhachHang WHERE 1=1 ";
            
            if (!string.IsNullOrEmpty(txtMaKhachHang.Text))
                sql += $" AND MaKhachHang = '{txtMaKhachHang.Text}' ";
            if (!string.IsNullOrEmpty(txtSoDienThoai.Text))
                sql += $" AND SoDienThoai LIKE '%{txtSoDienThoai.Text}%' ";
            if (!string.IsNullOrEmpty(txtEmail.Text))
                sql += $" AND Email LIKE '%{txtEmail.Text}%' ";

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

                    dgvCustomers.Rows.Add(maKhachHang, tenKhachHang, sdt, email, diaChi, ngayTao);
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
