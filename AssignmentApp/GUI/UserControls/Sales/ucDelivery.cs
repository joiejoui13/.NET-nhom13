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

            dtpNgayGiao.ValueChanged -= dtpNgayGiao_ValueChanged;
            dtpNgayGiao.ValueChanged += dtpNgayGiao_ValueChanged;

            LoadData();
            ResetState();
        }

        private void dtpNgayGiao_ValueChanged(object sender, EventArgs e)
        {
            if (dtpNgayGiao.CustomFormat == " ")
            {
                dtpNgayGiao.Format = DateTimePickerFormat.Short;
            }
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
                    string hienThiMa = !string.IsNullOrEmpty(maHoaDon) ? "HĐ: " + maHoaDon : "ĐH: " + maTraHang;

                    string diaChi = row["DiaChiGiao"] != DBNull.Value ? row["DiaChiGiao"].ToString() : "";
                    string trangThai = row["TrangThaiGiao"] != DBNull.Value ? row["TrangThaiGiao"].ToString() : "";
                    string ngayGiao = row["NgayGiao"] != DBNull.Value ? Convert.ToDateTime(row["NgayGiao"]).ToString("dd/MM/yyyy") : "";

                    dgvDeliveries.Rows.Add(maGiaoHang, hienThiMa, diaChi, trangThai, ngayGiao);
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi
            }
        }

        private void ResetValues()
        {
            txtMaGiaoHang.Text = "";
            txtMaHoaDon.Text = "";
            guna2TextBox1.Text = "";
            txtDiaChiGiao.Text = "";
            
            cboTrangThaiGiao.SelectedIndex = -1;
            dtpNgayGiao.Format = DateTimePickerFormat.Short;
            dtpNgayGiao.Value = DateTime.Now;

            guna2CustomRadioButton1.Checked = true;
        }

        private void ToggleInputs(bool isEnabled)
        {
            guna2CustomRadioButton1.Enabled = isEnabled;
            guna2CustomRadioButton2.Enabled = isEnabled;

            if (isEnabled)
            {
                if (guna2CustomRadioButton1.Checked)
                {
                    txtMaHoaDon.Enabled = true;
                    guna2TextBox1.Enabled = false;
                }
                else
                {
                    txtMaHoaDon.Enabled = false;
                    guna2TextBox1.Enabled = true;
                }
            }
            else
            {
                txtMaHoaDon.Enabled = false;
                guna2TextBox1.Enabled = false;
            }

            txtDiaChiGiao.Enabled = isEnabled;
            cboTrangThaiGiao.Enabled = isEnabled;
            dtpNgayGiao.Enabled = isEnabled;
        }

        private void ResetState()
        {
            isAddingNew = false;
            ResetValues();

            txtMaGiaoHang.Enabled = false;
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Chỉ đổi trạng thái input khi form đang mở khóa nhập liệu (txtDiaChiGiao mở)
            if (txtDiaChiGiao.Enabled)
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
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDeliveries.Rows[e.RowIndex];
                txtMaGiaoHang.Text = row.Cells["colMaGiaoHang"].Value?.ToString();

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
                            guna2TextBox1.Text = "";
                        }
                        else
                        {
                            guna2CustomRadioButton2.Checked = true;
                            guna2TextBox1.Text = dr["MaTraHang"].ToString();
                            txtMaHoaDon.Text = "";
                        }
                        txtDiaChiGiao.Text = dr["DiaChiGiao"].ToString();
                        cboTrangThaiGiao.Text = dr["TrangThaiGiao"]?.ToString() ?? "Chờ giao";
                        if (dr["NgayGiao"] != DBNull.Value) dtpNgayGiao.Value = Convert.ToDateTime(dr["NgayGiao"]);
                    }
                }
                catch { }

                ToggleInputs(true);
                txtMaGiaoHang.Enabled = false;

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

            string nextId = AssignmentApp.DAL.Core.DbContext.GetFieldValues("SELECT ISNULL(MAX(MaGiaoHang), 0) + 1 FROM GiaoHang");
            txtMaGiaoHang.Text = nextId;
            txtMaGiaoHang.Enabled = false; 

            dtpNgayGiao.Enabled = false; // Tự động lấy ngày hiện tại
            cboTrangThaiGiao.Text = "Chờ giao"; // Mặc định

            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;

            if (txtMaHoaDon.Enabled) txtMaHoaDon.Focus();
            else guna2TextBox1.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaGiaoHang.Text) || isAddingNew)
            {
                MessageBox.Show("Vui lòng chọn một đơn giao hàng để chỉnh sửa!");
                return;
            }

            string trangThai = cboTrangThaiGiao.Text;
            string ngay = dtpNgayGiao.Value.ToString("yyyy-MM-dd");
            string diaChi = txtDiaChiGiao.Text.Replace("'", "''");
            string maGH = txtMaGiaoHang.Text;

            if (guna2CustomRadioButton1.Checked)
            {
                string maHD = txtMaHoaDon.Text.Trim();
                if (!AssignmentApp.DAL.Core.DbContext.CheckKey($"SELECT * FROM HoaDon WHERE MaHoaDon = '{maHD}'"))
                {
                    MessageBox.Show("Mã hóa đơn không tồn tại trong hệ thống!");
                    return;
                }
                string sql = $"UPDATE GiaoHang SET MaHoaDon = '{maHD}', MaTraHang = NULL, DiaChiGiao = N'{diaChi}', TrangThaiGiao = N'{trangThai}', NgayGiao = '{ngay}' WHERE MaGiaoHang = '{maGH}'";
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
                string sql = $"UPDATE GiaoHang SET MaTraHang = '{maDH}', MaHoaDon = NULL, DiaChiGiao = N'{diaChi}', TrangThaiGiao = N'{trangThai}', NgayGiao = '{ngay}' WHERE MaGiaoHang = '{maGH}'";
                AssignmentApp.DAL.Core.DbContext.RunSql(sql);
            }

            MessageBox.Show("Lưu thay đổi thành công!");

            LoadData();
            ResetState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isAddingNew) return;

            string trangThai = cboTrangThaiGiao.Text;
            string ngay = DateTime.Now.ToString("yyyy-MM-dd");
            string diaChi = txtDiaChiGiao.Text.Replace("'", "''");

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

                string sql = $"INSERT INTO GiaoHang (MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES ('{maHD}', N'{diaChi}', N'{trangThai}', '{ngay}')";
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

                string sql = $"INSERT INTO GiaoHang (MaTraHang, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES ('{maDH}', N'{diaChi}', N'{trangThai}', '{ngay}')";
                AssignmentApp.DAL.Core.DbContext.RunSql(sql);
            }

            MessageBox.Show("Thêm đơn giao hàng thành công! Mã giao hàng đã được tự động sinh.");
            
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

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Đã hủy) đơn giao hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                AssignmentApp.DAL.Core.DbContext.RunSql($"UPDATE GiaoHang SET TrangThaiGiao = N'Đã hủy' WHERE MaGiaoHang = '{txtMaGiaoHang.Text}'");
                MessageBox.Show("Đã xóa (hủy) đơn giao hàng thành công!");
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
            if (txtMaGiaoHang.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaGiaoHang.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                
                dtpNgayGiao.Format = DateTimePickerFormat.Custom;
                dtpNgayGiao.CustomFormat = " ";

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập các tiêu chí cần lọc vào ô nhập liệu rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaGiaoHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMaGiaoHang.Text.Trim()) &&
                string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()) &&
                string.IsNullOrEmpty(guna2TextBox1.Text.Trim()) &&
                string.IsNullOrEmpty(txtDiaChiGiao.Text.Trim()) &&
                cboTrangThaiGiao.SelectedIndex == -1 &&
                dtpNgayGiao.CustomFormat == " ")
            {
                MessageBox.Show("Vui lòng nhập/chọn ít nhất một thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvDeliveries.Rows.Clear();
            string sql = "SELECT * FROM GiaoHang WHERE 1=1 ";
            
            if (!string.IsNullOrEmpty(txtMaGiaoHang.Text.Trim()))
                sql += $" AND MaGiaoHang = '{txtMaGiaoHang.Text.Trim()}' ";
            if (!string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
                sql += $" AND MaHoaDon = '{txtMaHoaDon.Text.Trim()}' ";
            if (!string.IsNullOrEmpty(guna2TextBox1.Text.Trim()))
                sql += $" AND MaTraHang = '{guna2TextBox1.Text.Trim()}' ";
            if (!string.IsNullOrEmpty(txtDiaChiGiao.Text.Trim()))
                sql += $" AND DiaChiGiao LIKE N'%{txtDiaChiGiao.Text.Trim()}%' ";
            if (cboTrangThaiGiao.SelectedIndex != -1)
                sql += $" AND TrangThaiGiao = N'{cboTrangThaiGiao.Text}' ";
            if (dtpNgayGiao.CustomFormat != " ")
                sql += $" AND CONVERT(date, NgayGiao) = '{dtpNgayGiao.Value.ToString("yyyy-MM-dd")}' ";

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
                    string trangThai = row["TrangThaiGiao"] != DBNull.Value ? row["TrangThaiGiao"].ToString() : "";
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

        private void pnlGridCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
