using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : UserControl
    {
        private bool isAddingNew = false;

        public ucDelivery()
        {
            InitializeComponent();
        }

        private void ucDelivery_Load(object sender, EventArgs e)
        {
            // Thiết lập ComboBox trạng thái
            cboTrangThaiGiao.Items.Clear();
            cboTrangThaiGiao.Items.AddRange(new object[] { "Chờ giao", "Đang giao", "Đã giao", "Đã hủy" });
            cboTrangThaiGiao.SelectedIndex = 0;

            // Thiết lập sự kiện cho RadioButtons
            guna2CustomRadioButton1.CheckedChanged += RadioButton_CheckedChanged;
            guna2CustomRadioButton2.CheckedChanged += RadioButton_CheckedChanged;

            // Mặc định chọn Mã hóa đơn
            guna2CustomRadioButton1.Checked = true;

            // Đổi tên cột trong GridView
            dgvDeliveries.Columns["colMaHoaDon"].HeaderText = "Mã HĐ/ĐH";

            LoadData();
            ResetState();
        }

        private void LoadData()
        {
            dgvDeliveries.Rows.Clear();
            try
            {
                AssignmentApp.DAL.Core.DbContext.Ketnoi();
                System.Data.DataTable dt = AssignmentApp.DAL.Core.DbContext.GetDataToTable("SELECT * FROM GiaoHang");
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string maGiaoHang = row["MaGiaoHang"].ToString();
                    string maHoaDon = row["MaHoaDon"] != DBNull.Value ? row["MaHoaDon"].ToString() : "";
                    string maTraHang = row["MaTraHang"] != DBNull.Value ? row["MaTraHang"].ToString() : "";
                    string hienThiMa = !string.IsNullOrEmpty(maHoaDon) ? "HĐ: " + maHoaDon : "ĐH: " + maTraHang; // Hiển thị phân biệt
                    
                    string diaChi = row["DiaChiGiao"] != DBNull.Value ? row["DiaChiGiao"].ToString() : "";
                    string trangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString() : "";
                    string ngayGiao = row["NgayGiao"] != DBNull.Value ? Convert.ToDateTime(row["NgayGiao"]).ToString("dd/MM/yyyy") : "";

                    dgvDeliveries.Rows.Add(maGiaoHang, hienThiMa, diaChi, trangThai, ngayGiao);
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi nếu bảng chưa tồn tại hoặc lỗi kết nối
            }
        }

        private void ResetState()
        {
            isAddingNew = false;
            // Trắng thông tin
            txtMaGiaoHang.Text = "";
            txtMaHoaDon.Text = "";
            guna2TextBox1.Text = "";
            txtDiaChiGiao.Text = "";
            cboTrangThaiGiao.SelectedIndex = 0;
            dtpNgayGiao.Value = DateTime.Now;

            // Các nút: thêm, tìm kiếm, làm mới hoạt động
            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            // Nút sửa, xóa, lưu, bỏ qua bị vô hiệu hóa
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            // Mã giao hàng không cho nhập tay để tự sinh (hoặc chỉ dùng tìm kiếm)
            txtMaGiaoHang.Enabled = true;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomRadioButton1.Checked)
            {
                txtMaHoaDon.Enabled = true;
                guna2TextBox1.Enabled = false;
                guna2TextBox1.Text = "";
            }
            else
            {
                txtMaHoaDon.Enabled = false;
                guna2TextBox1.Enabled = true;
                txtMaHoaDon.Text = "";
            }
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDeliveries.Rows[e.RowIndex];
                txtMaGiaoHang.Text = row.Cells["colMaGiaoHang"].Value?.ToString();
                
                // Lấy dữ liệu chi tiết từ DB để biết là Hóa Đơn hay Đổi Hàng
                try
                {
                    System.Data.DataTable dt = AssignmentApp.DAL.Core.DbContext.GetDataToTable($"SELECT * FROM GiaoHang WHERE MaGiaoHang = '{txtMaGiaoHang.Text}'");
                    if (dt.Rows.Count > 0)
                    {
                        System.Data.DataRow dr = dt.Rows[0];
                        if (dr["MaHoaDon"] != DBNull.Value && !string.IsNullOrEmpty(dr["MaHoaDon"].ToString()))
                        {
                            guna2CustomRadioButton1.Checked = true;
                            txtMaHoaDon.Text = dr["MaHoaDon"].ToString();
                        }
                        else
                        {
                            guna2CustomRadioButton2.Checked = true;
                            guna2TextBox1.Text = dr["MaTraHang"].ToString();
                        }
                        txtDiaChiGiao.Text = dr["DiaChiGiao"].ToString();
                        cboTrangThaiGiao.Text = dr["TrangThai"]?.ToString() ?? "Chưa giao";
                        if (dr["NgayGiao"] != DBNull.Value) dtpNgayGiao.Value = Convert.ToDateTime(dr["NgayGiao"]);
                    }
                }
                catch { }

                // Khi click vào đơn hàng: nút hoạt động bình thường, trừ nút lưu/bỏ qua
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
            
            // Lấy mã giao hàng tự sinh
            string nextId = AssignmentApp.DAL.Core.DbContext.GetFieldValues("SELECT ISNULL(MAX(MaGiaoHang), 0) + 1 FROM GiaoHang");

            // Làm trắng thông tin và khóa mã giao hàng
            txtMaGiaoHang.Text = nextId;
            txtMaGiaoHang.Enabled = false;
            txtMaHoaDon.Text = "";
            guna2TextBox1.Text = "";
            txtDiaChiGiao.Text = "";
            cboTrangThaiGiao.SelectedIndex = 0;
            dtpNgayGiao.Value = DateTime.Now;

            // Nút lưu, bỏ qua hoạt động bình thường
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            // Tắt các nút khác để tránh xung đột
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaGiaoHang.Text))
            {
                MessageBox.Show("Vui lòng chọn một đơn giao hàng để chỉnh sửa!");
                return;
            }
            
            isAddingNew = false;
            
            // Khóa mã giao hàng khi sửa
            txtMaGiaoHang.Enabled = false;
            
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
            string trangThai = cboTrangThaiGiao.Text;
            string ngay = dtpNgayGiao.Value.ToString("yyyy-MM-dd");
            string diaChi = txtDiaChiGiao.Text.Replace("'", "''");

            if (isAddingNew)
            {
                // Xử lý thêm mới
                if (guna2CustomRadioButton1.Checked)
                {
                    string maHD = txtMaHoaDon.Text.Trim();
                    if (string.IsNullOrEmpty(maHD))
                    {
                        MessageBox.Show("Vui lòng nhập mã hóa đơn!");
                        return;
                    }
                    if (!AssignmentApp.DAL.Core.DbContext.CheckKey($"SELECT * FROM HoaDon WHERE MaHoaDon = '{maHD}'"))
                    {
                        MessageBox.Show("Mã hóa đơn không tồn tại trong hệ thống!");
                        return;
                    }

                    string sql = $"INSERT INTO GiaoHang (MaHoaDon, DiaChiGiao, TrangThai, NgayGiao) VALUES ('{maHD}', N'{diaChi}', N'{trangThai}', '{ngay}')";
                    AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                }
                else
                {
                    string maDH = guna2TextBox1.Text.Trim();
                    if (string.IsNullOrEmpty(maDH))
                    {
                        MessageBox.Show("Vui lòng nhập mã đổi hàng!");
                        return;
                    }
                    if (!AssignmentApp.DAL.Core.DbContext.CheckKey($"SELECT * FROM TraHang WHERE MaTraHang = '{maDH}'"))
                    {
                        MessageBox.Show("Mã đổi hàng không tồn tại trong hệ thống!");
                        return;
                    }

                    string sql = $"INSERT INTO GiaoHang (MaTraHang, DiaChiGiao, TrangThai, NgayGiao) VALUES ('{maDH}', N'{diaChi}', N'{trangThai}', '{ngay}')";
                    AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                }

                MessageBox.Show("Thêm đơn giao hàng thành công! Mã giao hàng đã được tự động sinh.");
            }
            else
            {
                // Xử lý cập nhật
                if (string.IsNullOrEmpty(txtMaGiaoHang.Text))
                {
                    MessageBox.Show("Không có đơn hàng nào đang được chỉnh sửa!");
                    return;
                }

                string maGH = txtMaGiaoHang.Text;

                if (guna2CustomRadioButton1.Checked)
                {
                    string maHD = txtMaHoaDon.Text.Trim();
                    if (!AssignmentApp.DAL.Core.DbContext.CheckKey($"SELECT * FROM HoaDon WHERE MaHoaDon = '{maHD}'"))
                    {
                        MessageBox.Show("Mã hóa đơn không tồn tại trong hệ thống!");
                        return;
                    }
                    string sql = $"UPDATE GiaoHang SET MaHoaDon = '{maHD}', MaTraHang = NULL, DiaChiGiao = N'{diaChi}', TrangThai = N'{trangThai}', NgayGiao = '{ngay}' WHERE MaGiaoHang = '{maGH}'";
                    AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                }
                else
                {
                    string maDH = guna2TextBox1.Text.Trim();
                    if (!AssignmentApp.DAL.Core.DbContext.CheckKey($"SELECT * FROM TraHang WHERE MaTraHang = '{maDH}'"))
                    {
                        MessageBox.Show("Mã đổi hàng không tồn tại trong hệ thống!");
                        return;
                    }
                    string sql = $"UPDATE GiaoHang SET MaTraHang = '{maDH}', MaHoaDon = NULL, DiaChiGiao = N'{diaChi}', TrangThai = N'{trangThai}', NgayGiao = '{ngay}' WHERE MaGiaoHang = '{maGH}'";
                    AssignmentApp.DAL.Core.DbContext.RunSql(sql);
                }

                MessageBox.Show("Lưu thay đổi thành công!");
            }

            LoadData();
            ResetState();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaGiaoHang.Text))
            {
                MessageBox.Show("Vui lòng chọn một đơn giao hàng để xóa!");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn giao hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                AssignmentApp.DAL.Core.DbContext.RunSql($"DELETE FROM GiaoHang WHERE MaGiaoHang = '{txtMaGiaoHang.Text}'");
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
            dgvDeliveries.Rows.Clear();
            string sql = "SELECT * FROM GiaoHang WHERE 1=1 ";
            
            if (!string.IsNullOrEmpty(txtMaGiaoHang.Text))
                sql += $" AND MaGiaoHang = '{txtMaGiaoHang.Text}' ";
            if (!string.IsNullOrEmpty(txtMaHoaDon.Text))
                sql += $" AND MaHoaDon = '{txtMaHoaDon.Text}' ";
            if (!string.IsNullOrEmpty(guna2TextBox1.Text))
                sql += $" AND MaTraHang = '{guna2TextBox1.Text}' ";

            try
            {
                System.Data.DataTable dt = AssignmentApp.DAL.Core.DbContext.GetDataToTable(sql);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string maGiaoHang = row["MaGiaoHang"].ToString();
                    string maHoaDon = row["MaHoaDon"] != DBNull.Value ? row["MaHoaDon"].ToString() : "";
                    string maTraHang = row["MaTraHang"] != DBNull.Value ? row["MaTraHang"].ToString() : "";
                    string hienThiMa = !string.IsNullOrEmpty(maHoaDon) ? "HĐ: " + maHoaDon : "ĐH: " + maTraHang;
                    
                    string diaChi = row["DiaChiGiao"] != DBNull.Value ? row["DiaChiGiao"].ToString() : "";
                    string trangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString() : "";
                    string ngayGiao = row["NgayGiao"] != DBNull.Value ? Convert.ToDateTime(row["NgayGiao"]).ToString("dd/MM/yyyy") : "";

                    dgvDeliveries.Rows.Add(maGiaoHang, hienThiMa, diaChi, trangThai, ngayGiao);
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
