using AssignmentApp.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.SqlClient;
using System.Data;


namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucCategory : UserControl
    {
        DataTable dtDanhMuc;

        public ucCategory()
        {
            InitializeComponent();
        }

        private void ucCategory_Load(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            Load_DataGridView();
        }
        private void Load_DataGridView()
        {
            string sql = @"SELECT MaDanhMuc, TenDanhMuc, MoTa,
                   TrangThai, NgayTao, NgayCapNhat
                   FROM DanhMuc";

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlDataAdapter da =
          new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);

            dtDanhMuc = new DataTable();
            da.Fill(dtDanhMuc);
            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.DataSource = dtDanhMuc;
            dgvDanhMuc.Columns["colNgayTao"].DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            dgvDanhMuc.Columns["colNgayCapNhat"].DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            dgvDanhMuc.AllowUserToAddRows = false;

            dgvDanhMuc.EditMode =
                DataGridViewEditMode.EditProgrammatically;
        }

        private void ResetValues()
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = -1;
        }


        private void dgvDanhMuc_CellClick(object? sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAdd.Enabled = false;
            ResetValues();
            txtMaDanhMuc.Enabled = false;
            txtTenDanhMuc.Focus();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            string sql;

            if (dtDanhMuc.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtMaDanhMuc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return;
            }

            if (txtMoTa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mô tả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            sql = $@"
        UPDATE DanhMuc
        SET 
            TenDanhMuc = N'{txtTenDanhMuc.Text.Trim()}',
            MoTa = N'{txtMoTa.Text.Trim()}',
            TrangThai = N'{cboTrangThai.Text}',
            NgayCapNhat = GETDATE()
        WHERE MaDanhMuc = {txtMaDanhMuc.Text}";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Sửa danh mục thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView();

            ResetValues();

            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            string sql;

            if (dtDanhMuc.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (txtMaDanhMuc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào!", "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                "Bạn có muốn xóa danh mục này không?",
                "Thông báo",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = $@"DELETE FROM DanhMuc
                 WHERE MaDanhMuc = {txtMaDanhMuc.Text}";

                SqlCommand cmd =
                    new SqlCommand(sql, (SqlConnection)DbContext.Conn);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Xóa danh mục thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Load_DataGridView();

                ResetValues();
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues();

            Load_DataGridView();

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            txtMaDanhMuc.Enabled = false;

            MessageBox.Show("Đã làm mới dữ liệu!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            ResetValues();

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;

            txtMaDanhMuc.Enabled = false;

            Load_DataGridView();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            string sql;

            if (txtTenDanhMuc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn hãy nhập tên danh mục cần tìm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDanhMuc.Focus();
                return;
            }

            sql = $@"
        SELECT MaDanhMuc, TenDanhMuc, MoTa,
               TrangThai, NgayTao, NgayCapNhat
        FROM DanhMuc
        WHERE TenDanhMuc LIKE N'%{txtTenDanhMuc.Text.Trim()}%'";

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlDataAdapter da =
                new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);

            DataTable dtSearch = new DataTable();

            da.Fill(dtSearch);

            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy danh mục nào!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            dgvDanhMuc.DataSource = dtSearch;

            MessageBox.Show("Đã tìm thấy " + dtSearch.Rows.Count +
                " danh mục!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            string sql;

         
            if (txtTenDanhMuc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên danh mục!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDanhMuc.Focus();
                return;
            }

         
            if (txtMoTa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mô tả!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMoTa.Focus();
                return;
            }

         
            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTrangThai.Focus();
                return;
            }

           
            sql = @"SELECT TenDanhMuc
            FROM DanhMuc
            WHERE TenDanhMuc = N'" +
                    txtTenDanhMuc.Text.Trim() + "'";

            SqlCommand cmdCheck =
                new SqlCommand(sql, (SqlConnection)DbContext.Conn);

            SqlDataReader reader = cmdCheck.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();

                MessageBox.Show("Tên danh mục đã tồn tại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenDanhMuc.Focus();
                return;
            }

            reader.Close();

           
            sql = @"INSERT INTO DanhMuc
            (TenDanhMuc, MoTa, TrangThai, NgayTao)
            VALUES
            (N'" + txtTenDanhMuc.Text.Trim() +
                    "', N'" + txtMoTa.Text.Trim() +
                    "', N'" + cboTrangThai.Text.Trim() +
                    "', GETDATE())";

            SqlCommand cmd =
                new SqlCommand(sql, (SqlConnection)DbContext.Conn);

            cmd.ExecuteNonQuery();

            MessageBox.Show("Thêm danh mục thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Load_DataGridView();

            ResetValues();

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                txtMaDanhMuc.Focus();
                return;
            }
            if (dtDanhMuc.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            txtMaDanhMuc.Text = dgvDanhMuc.CurrentRow.Cells["colMaDanhMuc"].Value.ToString();

            txtTenDanhMuc.Text = dgvDanhMuc.CurrentRow.Cells["colTenDanhMuc"].Value.ToString();

            txtMoTa.Text = dgvDanhMuc.CurrentRow.Cells["colMoTa"].Value.ToString();

            cboTrangThai.Text = dgvDanhMuc.CurrentRow.Cells["colTrangThai"].Value.ToString();

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

    }
}

  

