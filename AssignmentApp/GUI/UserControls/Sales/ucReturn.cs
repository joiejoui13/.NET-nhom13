using AssignmentApp.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
 
        DataTable dtCart;
        DataTable dtInvoiceDetails;

        public ucReturn()
        {
            InitializeComponent();
        }

        private void cboLoaiGiaoDich_SelectedIndexChanged(object? sender, EventArgs e)
        {

        }

        private void ucReturn_Load(object sender, EventArgs e)
        {
            btnReturnSearch.Enabled = true;
            btnReturnRefresh.Enabled = true;
            btnAddToCart.Enabled = false;
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            ResetValues1();
            KhoiTaoGioHangTamtinh();
            Load_DataGridView();
        }


        private void KhoiTaoGioHangTamtinh()
        {
            dtCart = new DataTable();
            dtCart.Columns.Add("colCurMaSP", typeof(int));
            dtCart.Columns.Add("colCurTenSP", typeof(string));
            dtCart.Columns.Add("colCurSoLuong", typeof(int));
            dtCart.Columns.Add("colCurDonGia", typeof(decimal));
            dtCart.Columns.Add("colCurTinhTrang", typeof(string));
            dtCart.Columns.Add("colCurThanhTien", typeof(decimal), "colCurSoLuong * colCurDonGia"); 

            dgvCurrentDetails.AutoGenerateColumns = false;

            if (dgvCurrentDetails.Columns.Count >= 6)
            {
                dgvCurrentDetails.Columns[0].DataPropertyName = "colCurMaSP";
                dgvCurrentDetails.Columns[1].DataPropertyName = "colCurTenSP";
                dgvCurrentDetails.Columns[2].DataPropertyName = "colCurSoLuong";
                dgvCurrentDetails.Columns[3].DataPropertyName = "colCurDonGia";
                dgvCurrentDetails.Columns[4].DataPropertyName = "colCurTinhTrang";
                dgvCurrentDetails.Columns[5].DataPropertyName = "colCurThanhTien";
            }

            dgvCurrentDetails.DataSource = dtCart;
            dgvCurrentDetails.AllowUserToAddRows = false;
        }

        private void ResetValues1()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";

            txtSelMaSP.Enabled = false;
            txtSelTenSP.Enabled = false;
            txtSelDonGia.Enabled = false;
        }


        DataTable dtReturn;
        private void Load_DataGridView()
        {
            string sql = @"
        SELECT 
            th.MaTraHang,
            th.MaHoaDon,
            th.TrangThai,
            th.LoaiGiaoDich,
            th.TongTienHoan,
            nd.TenNguoiDung AS MaNguoiDung,
            th.NgayTra,
            th.LyDo,
            nd.TenNguoiDung AS NhanVien,
            kh.TenKhachHang AS KhachHang
        FROM TraHang th
        JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
        JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
        JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtReturn = new DataTable();
            da.Fill(dtReturn);

            dgvReturns.AutoGenerateColumns = false;
            dgvReturns.DataSource = null;   // reset trước
            dgvReturns.DataSource = dtReturn;


            if (dgvReturns.Columns["NhanVien"] != null)
                dgvReturns.Columns["NhanVien"].Visible = false;
            if (dgvReturns.Columns["KhachHang"] != null)
                dgvReturns.Columns["KhachHang"].Visible = false;
        }

        // ==========================================
        // SỰ KIỆN TAB 1
        // ==========================================
        private void ResetValues()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0";
            dtpNgayTra.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            cboLoaiGiaoDich.SelectedIndex = -1;

            lblKhachHang.Text = "Khách hàng: (Trống)";
            lblNhanVien.Text = "Nhân viên: (Trống)";
        }
        private void btnAdd_Click(object? sender, EventArgs e)
        {
            string sql;

            if (txtMaHoaDon.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hóa đơn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            if (txtLyDo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập lý do trả hàng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            if (cboLoaiGiaoDich.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn loại giao dịch!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            sql = "SELECT MaHoaDon FROM TraHang WHERE MaHoaDon = N'"
                + txtMaHoaDon.Text.Trim() + "'";

            SqlCommand cmdCheck = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            SqlDataReader reader = cmdCheck.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                MessageBox.Show("Hóa đơn này đã có phiếu trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            reader.Close();

            sql = @"INSERT INTO TraHang 
            (MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
             VALUES
             (" + txtMaHoaDon.Text.Trim() + @",1,
             N'" + txtLyDo.Text.Trim() + @"',
             " + txtTongTienHoan.Text.Replace(" đ", "").Replace(",", "") + @",
             '" + dtpNgayTra.Value.ToString("yyyy-MM-dd HH:mm:ss") + @"',
             N'" + cboTrangThai.Text + @"',
             N'" + cboLoaiGiaoDich.Text + @"')";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.ExecuteNonQuery();


            Load_DataGridView();


            ResetValues();

            MessageBox.Show("Thêm phiếu trả hàng thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            string sql;

            if (dtReturn.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo");
                return;
            }

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa phiếu trả này không?",
                "Xác nhận",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = $@"DELETE FROM TraHang 
                 WHERE MaHoaDon = N'{txtMaHoaDon.Text}'";

                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Xóa thành công!", "Thông báo");

                Load_DataGridView();
                ResetValues();
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            string sql;

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Phải nhập mã hóa đơn!", "Thông báo");
                txtMaHoaDon.Focus();
                return;
            }

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Phải nhập lý do!", "Thông báo");
                txtLyDo.Focus();
                return;
            }

            sql = @"INSERT INTO TraHang
        (MaHoaDon, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
        VALUES
        (N'" + txtMaHoaDon.Text.Trim() +
                "', N'" + txtLyDo.Text.Trim() +
                "', " + txtTongTienHoan.Text.Replace("đ", "").Replace(",", "") +
                ", '" + dtpNgayTra.Value.ToString("yyyy-MM-dd HH:mm:ss") +
                "', N'" + cboTrangThai.Text +
                "', N'" + cboLoaiGiaoDich.Text + "')";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Thêm phiếu trả thành công!", "Thông báo");

            Load_DataGridView();
            ResetValues();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            ResetValues();
            Load_DataGridView();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Nhập mã hóa đơn để tìm!", "Thông báo");
                txtMaHoaDon.Focus();
                return;
            }

            string sql = @"
            SELECT 
              th.MaTraHang,
              th.MaHoaDon,
              th.TrangThai,
              th.LoaiGiaoDich,
              th.TongTienHoan,
              nd.TenNguoiDung AS NhanVien,
              kh.TenKhachHang AS KhachHang,
              th.NgayTra,
              th.LyDo
              FROM TraHang th
              JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
              JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
              JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
              WHERE th.MaHoaDon LIKE @mahd";

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);

            da.SelectCommand.Parameters.AddWithValue("@mahd", "%" + txtMaHoaDon.Text.Trim() + "%");

            DataTable dtSearch = new DataTable();
            da.Fill(dtSearch);

            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy!", "Thông báo");
                return;
            }

            dgvReturns.DataSource = dtSearch;
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues();
            Load_DataGridView();

            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo");
        }

        private void dgvReturns_CellClick(object? sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            string sql = @"
            SELECT 
              nd.TenNguoiDung,
               kh.TenKhachHang
                FROM HoaDon hd
                JOIN NguoiDung nd ON hd.MaNguoiDung = nd.MaNguoiDung
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE hd.MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblNhanVien.Text = "Nhân viên: " + reader["TenNguoiDung"].ToString();
                lblKhachHang.Text = "Khách hàng: " + reader["TenKhachHang"].ToString();
            }

            reader.Close();
        }

        private void btnChooseProducts_Click(object? sender, EventArgs e)
        {

        }

        // ==========================================
        // SỰ KIỆN TAB 2
        // ==========================================

        private void TinhTongTienHoanTra()
        {
            decimal total = 0;
            foreach (DataRow row in dtCart.Rows)
            {
                if (row["colCurThanhTien"] != DBNull.Value)
                {
                    total += Convert.ToDecimal(row["colCurThanhTien"]);
                }
            }

            lblTotalAmount.Text = total.ToString("N0") + " VNĐ";
        }

  
        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtInvoiceDetails == null || dtInvoiceDetails.Rows.Count == 0) return;

            if (e.RowIndex >= 0)
            {
                DataRowView rowView = (DataRowView)dgvProductsSelection.Rows[e.RowIndex].DataBoundItem;

           
                txtSelMaSP.Text = rowView["MaSanPham"].ToString();
                txtSelTenSP.Text = rowView["TenSanPham"].ToString();
                txtSelDonGia.Text = rowView["DonGia"].ToString();

                txtSelSoLuong.Text = "";
                txtSelTinhTrang.Text = "";


                string tenFileAnh = rowView["Anh"]?.ToString();

                if (!string.IsNullOrEmpty(tenFileAnh))
                {
                    string path1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", tenFileAnh);
                    string path2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", tenFileAnh);


                    if (System.IO.File.Exists(path1))
                    {
                    
                        picAnh.Image = System.Drawing.Image.FromFile(path1);
                    }
                    else if (System.IO.File.Exists(path2))
                    {
                        picAnh.Image = System.Drawing.Image.FromFile(path2);
                    }
                    else
                    {
                        picAnh.Image = null;
                    }
                }
                else
                {
                    picAnh.Image = null;
                }

                btnAddToCart.Enabled = true;
                btnEdit.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                txtSelSoLuong.Focus();
            }
        }
        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtCart.Rows.Count == 0) return;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCurrentDetails.Rows[e.RowIndex];

                txtSelMaSP.Text = row.Cells["colCurMaSP"].Value?.ToString();
                txtSelTenSP.Text = row.Cells["colCurTenSP"].Value?.ToString();
                txtSelSoLuong.Text = row.Cells["colCurSoLuong"].Value?.ToString();
                txtSelDonGia.Text = row.Cells["colCurDonGia"].Value?.ToString();
                txtSelTinhTrang.Text = row.Cells["colCurTinhTrang"].Value?.ToString();

                btnAddToCart.Enabled = false;
                btnEdit.Enabled = true;
                btnRemoveFromCart.Enabled = true;
            }
        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào để trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSelSoLuong.Text.Trim().Length == 0 || !int.TryParse(txtSelSoLuong.Text.Trim(), out int slTra) || slTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            if (txtSelTinhTrang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tình trạng hàng trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelTinhTrang.Focus();
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

 
            int slMuaGoc = 0;
            foreach (DataGridViewRow r in dgvProductsSelection.Rows)
            {
                if (r.Cells[0].Value != null && Convert.ToInt32(r.Cells[0].Value) == maSP)
                {
                    slMuaGoc = Convert.ToInt32(r.Cells[2].Value);
                    break;
                }
            }

            if (slTra > slMuaGoc)
            {
                MessageBox.Show($"Số lượng trả không được lớn hơn số lượng đã mua ({slMuaGoc})!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        
            foreach (DataRow row in dtCart.Rows)
            {
                if (Convert.ToInt32(row["colCurMaSP"]) == maSP)
                {
                    MessageBox.Show("Sản phẩm này đã nằm trong danh sách trả! Nếu muốn thay đổi số lượng, hãy chọn dòng bên phải và bấm SỬA.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

        
            DataRow newRow = dtCart.NewRow();
            newRow["colCurMaSP"] = maSP;
            newRow["colCurTenSP"] = txtSelTenSP.Text;
            newRow["colCurSoLuong"] = slTra;
            decimal donGia = 0;
            decimal.TryParse(txtSelDonGia.Text, out donGia);
            newRow["colCurDonGia"] = donGia;
            newRow["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
            dtCart.Rows.Add(newRow);

          
            TinhTongTienHoanTra();
            ResetValues1();
            btnAddToCart.Enabled = false;
        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            if (MessageBox.Show("Bạn có muốn bỏ sản phẩm này khỏi danh sách hoàn trả không?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int maSP = int.Parse(txtSelMaSP.Text);

                for (int i = dtCart.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                    {
                        dtCart.Rows[i].Delete();
                        break;
                    }
                }
                dtCart.AcceptChanges();

                TinhTongTienHoanTra();
                ResetValues1();
                btnEdit.Enabled = false;
                btnRemoveFromCart.Enabled = false;
            }
        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {

        }

        private void btnReturnSearch_Click(object? sender, EventArgs e)
        {
            string maHDInput = Microsoft.VisualBasic.Interaction.InputBox("Nhập Mã Hóa Đơn cần trả hàng:", "Tìm kiếm hóa đơn", "");
            if (string.IsNullOrEmpty(maHDInput.Trim())) return;

            string sql = $@"SELECT cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.MoTa, sp.Anh 
                    FROM ChiTietHoaDon cthd
                    INNER JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                    WHERE cthd.MaHoaDon = {maHDInput.Trim()}";

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtInvoiceDetails = new DataTable();
            da.Fill(dtInvoiceDetails);

            if (dtInvoiceDetails.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn hoặc hóa đơn không có sản phẩm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

  
            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = dtInvoiceDetails;

     
            pnlReturnTop.Tag = maHDInput.Trim();

  
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();

            btnLuuCT.Enabled = true;
            MessageBox.Show($"Đã tìm thấy hóa đơn {maHDInput}! Hãy chọn sản phẩm cần trả ở danh sách bên trái.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReturnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues1();
            if (dtInvoiceDetails != null) dtInvoiceDetails.Rows.Clear();
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();

            btnAddToCart.Enabled = false;
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            pnlReturnTop.Tag = null;
        }

        private void tabSelectionContainer_SelectedIndexChanged(object? sender, EventArgs e)
        {

        }

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            if (dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng hoàn trả đang trống! Không thể lưu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pnlReturnTop.Tag == null) return;
            int maHoaDonGoc = Convert.ToInt32(pnlReturnTop.Tag);

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlTransaction transaction = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
                decimal tongTienHoan = 0;
                foreach (DataRow r in dtCart.Rows)
                {
                    tongTienHoan += Convert.ToDecimal(r["colCurThanhTien"]);
                }

                int maNhanVien = 1;

            
                string sqlInsertTraHang = $@"INSERT INTO TraHang (MaHoaDon, MaNguoiDung, NgayTra, TongTienHoan, LyDo)
                                     VALUES ({maHoaDonGoc}, {maNhanVien}, GETDATE(), {tongTienHoan}, N'Khách trả hàng theo yêu cầu');
                                     SELECT SCOPE_IDENTITY();";

                SqlCommand cmdTraHang = new SqlCommand(sqlInsertTraHang, (SqlConnection)DbContext.Conn, transaction);
                int maTraHangMoi = Convert.ToInt32(cmdTraHang.ExecuteScalar());

               
                foreach (DataRow row in dtCart.Rows)
                {
                    int maSP = Convert.ToInt32(row["colCurMaSP"]);
                    int soLuongTra = Convert.ToInt32(row["colCurSoLuong"]);
                    decimal thanhTienDong = Convert.ToDecimal(row["colCurThanhTien"]);
                    string tinhTrang = row["colCurTinhTrang"].ToString();

                  
                    string sqlInsertChiTiet = $@"INSERT INTO ChiTietTraHang (MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang)
                                         VALUES ({maTraHangMoi}, {maSP}, {soLuongTra}, {thanhTienDong}, N'{tinhTrang}')";
                    SqlCommand cmdChiTiet = new SqlCommand(sqlInsertChiTiet, (SqlConnection)DbContext.Conn, transaction);
                    cmdChiTiet.ExecuteNonQuery();

                   
                    string sqlTonKho = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
                    SqlCommand cmdTonKho = new SqlCommand(sqlTonKho, (SqlConnection)DbContext.Conn, transaction);
                    int tonKhoHienTai = Convert.ToInt32(cmdTonKho.ExecuteScalar());
                    int tonKhoSau = tonKhoHienTai + soLuongTra;

             
                    string sqlUpdateKho = $"UPDATE SanPham SET SoLuongTon = {tonKhoSau} WHERE MaSanPham = {maSP}";
                    SqlCommand cmdUpdateKho = new SqlCommand(sqlUpdateKho, (SqlConnection)DbContext.Conn, transaction);
                    cmdUpdateKho.ExecuteNonQuery();

                   
                    string sqlInsertLichSu = $@"INSERT INTO LichSuNhapKho 
                                        (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai)
                                        VALUES ({maSP}, GETDATE(), {soLuongTra}, {tonKhoHienTai}, {tonKhoSau}, N'Trả hàng', {maTraHangMoi}, N'Hoàn tất')";
                    SqlCommand cmdLichSu = new SqlCommand(sqlInsertLichSu, (SqlConnection)DbContext.Conn, transaction);
                    cmdLichSu.ExecuteNonQuery();
                }

        
                transaction.Commit();
                MessageBox.Show("Xử lý và lưu đơn hàng hoàn trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

        
                btnCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
             
                transaction.Rollback();
                MessageBox.Show("Có lỗi xảy ra trong quá trình lưu dữ liệu: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectProduct_Click(object? sender, EventArgs e)
        {

        }

        private void dgvReturns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHoaDon.Focus();
                return;
            }

            if (dgvReturns.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaHoaDon.Text = dgvReturns.CurrentRow.Cells["colMaHoaDon"].Value.ToString();
            txtLyDo.Text = dgvReturns.CurrentRow.Cells["colLyDo"].Value.ToString();
            txtTongTienHoan.Text = dgvReturns.CurrentRow.Cells["colTongTienHoan"].Value.ToString();

            string trangThai = dgvReturns.CurrentRow.Cells["colTrangThai"].Value.ToString().Trim();
            string loaiGD = dgvReturns.CurrentRow.Cells["colLoaiGiaoDich"].Value.ToString().Trim();

            cboTrangThai.SelectedIndex = -1;
            foreach (var item in cboTrangThai.Items)
                if (item.ToString().Trim() == trangThai)
                { cboTrangThai.SelectedItem = item; break; }

            cboLoaiGiaoDich.SelectedIndex = -1;
            foreach (var item in cboLoaiGiaoDich.Items)
                if (item.ToString().Trim() == loaiGD)
                { cboLoaiGiaoDich.SelectedItem = item; break; }

            dtpNgayTra.Value = Convert.ToDateTime(dgvReturns.CurrentRow.Cells["colNgayTra"].Value);

            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dtReturn.Rows.Count)
            {
                lblNhanVien.Text = "Nhân viên: " + dtReturn.Rows[rowIndex]["NhanVien"].ToString();
                lblKhachHang.Text = "Khách hàng: " + dtReturn.Rows[rowIndex]["KhachHang"].ToString();
            }


            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void tabPhieuTra_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            Load_DataGridView();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string sql;

            if (dtReturn.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtLyDo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập lý do trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
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
             UPDATE TraHang SET
             LyDo = N'{txtLyDo.Text.Trim()}',
             TongTienHoan = {txtTongTienHoan.Text.Replace("đ", "").Replace(",", "")},
             NgayTra = '{dtpNgayTra.Value:yyyy-MM-dd HH:mm:ss}',
             TrangThai = N'{cboTrangThai.Text}',
             LoaiGiaoDich = N'{cboLoaiGiaoDich.Text}'
             WHERE MaHoaDon = N'{txtMaHoaDon.Text}'";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Sửa phiếu trả thành công!", "Thông báo");

            Load_DataGridView();
            ResetValues();
        }

        private void txtMaHoaDon_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProductsSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductsSelection_CellClick(sender, e);
        }

        private void tabChonSanPham_Click(object sender, EventArgs e)
        {
        }

        private void dgvCurrentDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCurrentDetails_CellClick(sender, e);
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            if (txtSelSoLuong.Text.Trim().Length == 0 || !int.TryParse(txtSelSoLuong.Text.Trim(), out int slTra) || slTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

     
            int slMuaGoc = 0;
            foreach (DataGridViewRow r in dgvProductsSelection.Rows)
            {
                if (Convert.ToInt32(r.Cells["colMaSP"].Value) == maSP)
                {
                    slMuaGoc = Convert.ToInt32(r.Cells["colSoLuong"].Value);
                    break;
                }
            }

            if (slTra > slMuaGoc)
            {
                MessageBox.Show($"Số lượng trả vượt quá số lượng gốc ({slMuaGoc})!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

      
            foreach (DataRow row in dtCart.Rows)
            {
                if (Convert.ToInt32(row["colCurMaSP"]) == maSP)
                {
                    row["colCurSoLuong"] = slTra;
                    row["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    break;
                }
            }

            TinhTongTienHoanTra();
            ResetValues();
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
        }

        private void btnBoquaCT_Click(object sender, EventArgs e)
        {
            btnReturnRefresh_Click(sender, e);
        }

        private void pnlReturnTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabProductDetail_Click(object sender, EventArgs e)
        {

        }
    }
}
