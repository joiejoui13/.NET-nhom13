using AssignmentApp.DAL.Core;
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
      
        DataTable dtReturn;         
        DataTable dtInvoiceDetails; 
        DataTable dtCart;          

        int maTraHangHienTai = 0;   
        bool isAdding = false;     
        bool isEditing = false;    

        public ucReturn()
        {
            InitializeComponent();
        }

        
        private void ucReturn_Load(object sender, EventArgs e)
        {
          
            btnSuaCT.Visible = false;
            guna2Button2.Visible = false;
            KhoiTaoGioHang();
            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

      
        private void KhoiTaoGioHang()
        {
            dtCart = new DataTable();
            dtCart.Columns.Add("colCurMaSP", typeof(int));
            dtCart.Columns.Add("colCurTenSP", typeof(string));
            dtCart.Columns.Add("colCurSoLuong", typeof(int));
            dtCart.Columns.Add("colCurDonGia", typeof(decimal));
            dtCart.Columns.Add("colCurTinhTrang", typeof(string));
            dtCart.Columns.Add("colCurThanhTien", typeof(decimal), "colCurSoLuong * colCurDonGia");

            colCurMaSP.DataPropertyName = "colCurMaSP";
            colCurTenSP.DataPropertyName = "colCurTenSP";
            colCurSoLuong.DataPropertyName = "colCurSoLuong";
            colCurDonGia.DataPropertyName = "colCurDonGia";
            colCurTinhTrang.DataPropertyName = "colCurTinhTrang";
            colCurThanhTien.DataPropertyName = "colCurThanhTien";

            dgvCurrentDetails.AutoGenerateColumns = false;
            dgvCurrentDetails.DataSource = dtCart;
            dgvCurrentDetails.AllowUserToAddRows = false;
        }

        private void SetTrangThaiBanDau()
        {
            isAdding = false;
            isEditing = false;

            KhoaONhapTab0(false);

            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = false;   
            btnCancel.Visible = false;  

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;  
            btnDelete.Enabled = false;   
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            KhoaTab1(true);
            XoaTrangTab0();
        }

  
        private void SetTrangThaiDangNhap()
        {
           
            KhoaONhapTab0(false);

    
            btnAdd.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

    
        private void KhoaONhapTab0(bool khoa)
        {
            txtMaHoaDon.ReadOnly = khoa;
            txtLyDo.ReadOnly = khoa;
            dtpNgayTra.Enabled = false;
            cboTrangThai.Enabled = false;
            cboLoaiGiaoDich.Enabled = !khoa;
            txtTongTienHoan.ReadOnly = true;
        }

    
        private void KhoaTab1(bool khoa)
        {
            btnAddToCart.Enabled = !khoa;
            btnRemoveFromCart.Enabled = !khoa;
            btnLuuCT.Enabled = !khoa;

            txtSelSoLuong.ReadOnly = khoa;
            txtSelTinhTrang.ReadOnly = khoa;

            if (khoa)
            {
                XoaTrangTab1();
            }
        }

   
        private void XoaTrangTab0()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0 đ";
            dtpNgayTra.Value = DateTime.Now; 

            
            cboTrangThai.SelectedIndex = 1;

            cboLoaiGiaoDich.SelectedIndex = -1; 
            lblKhachHang.Text = "Khách hàng: (Trống)";
            lblNhanVien.Text = "Nhân viên: (Trống)";
            maTraHangHienTai = 0;
        }

   
        private void XoaTrangTab1()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
            lblTotalAmount.Text = "TỔNG TIỀN HOÀN TRẢ TẠM TÍNH: 0 đ";

            if (dtCart != null)
                dtCart.Rows.Clear();

            if (dtInvoiceDetails != null)
                dtInvoiceDetails.Rows.Clear();

            lblReturnTitle.Text = "MÃ PHIẾU: ";
        }

    
        private void NapDanhSachPhieu()
        {
            string sql = @"
                SELECT 
                    th.MaTraHang,
                    th.MaHoaDon,
                    th.TrangThai,
                    th.LoaiGiaoDich,
                    th.TongTienHoan,
                    nd.TenNguoiDung AS NhanVien,
                    th.NgayTra,
                    th.LyDo,
                    kh.TenKhachHang AS KhachHang
                FROM TraHang th
                JOIN NguoiDung  nd ON th.MaNguoiDung = nd.MaNguoiDung
                JOIN HoaDon     hd ON th.MaHoaDon    = hd.MaHoaDon
                JOIN KhachHang  kh ON hd.MaKhachHang = kh.MaKhachHang
                ORDER BY th.MaTraHang DESC";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtReturn = new DataTable();
            da.Fill(dtReturn);

            colMaTraHang.DataPropertyName = "MaTraHang";
            colMaHoaDon.DataPropertyName = "MaHoaDon";
            colTrangThai.DataPropertyName = "TrangThai";
            colLoaiGiaoDich.DataPropertyName = "LoaiGiaoDich";
            colTongTienHoan.DataPropertyName = "TongTienHoan";
            colNhanVien.DataPropertyName = "NhanVien";
            colNgayTra.DataPropertyName = "NgayTra";
            colLyDo.DataPropertyName = "LyDo";

            dgvReturns.AutoGenerateColumns = false;
            dgvReturns.DataSource = dtReturn;
        }

   
        private void NapSanPhamHoaDon(int maHoaDon)
        {
            string sql = @"
                SELECT 
                    cthd.MaSanPham,
                    sp.TenSanPham,
                    cthd.SoLuong     AS SLMua,
                    ISNULL(SUM(ctth.SoLuong), 0) AS DaTra,
                    cthd.DonGia,
                    sp.Anh
                FROM ChiTietHoaDon cthd
                JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                LEFT JOIN ChiTietTraHang ctth ON ctth.MaSanPham = cthd.MaSanPham
                    AND ctth.MaTraHang IN (
                        SELECT MaTraHang FROM TraHang WHERE MaHoaDon = @mahd
                    )
                WHERE cthd.MaHoaDon = @mahd
                GROUP BY cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.Anh";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            da.SelectCommand.Parameters.AddWithValue("@mahd", maHoaDon);

            dtInvoiceDetails = new DataTable();
            da.Fill(dtInvoiceDetails);

            colSelMaSP.DataPropertyName = "MaSanPham";
            colSelTenSP.DataPropertyName = "TenSanPham";
            colSelSoLuongMua.DataPropertyName = "SLMua";
            colSelDaTra.DataPropertyName = "DaTra";
            colSelDonGia.DataPropertyName = "DonGia";

            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = dtInvoiceDetails;
        }

        private void TinhTongTienHoanTra()
        {
            decimal tongTien = 0;
            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (dtCart.Rows[i]["colCurThanhTien"] != DBNull.Value)
                    tongTien += Convert.ToDecimal(dtCart.Rows[i]["colCurThanhTien"]);
            }
            lblTotalAmount.Text = "TỔNG TIỀN HOÀN TRẢ TẠM TÍNH: " + tongTien.ToString("N0") + " đ";
        }

   
        private void dgvReturns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isAdding || isEditing)
            {
                MessageBox.Show("Đang ở chế độ nhập liệu! Hãy Lưu hoặc Bỏ qua trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.RowIndex < 0 || dtReturn == null || dtReturn.Rows.Count == 0) return;

            DataRow dong = dtReturn.Rows[e.RowIndex];

            txtMaHoaDon.Text = dong["MaHoaDon"].ToString();
            txtLyDo.Text = dong["LyDo"].ToString();
            txtTongTienHoan.Text = Convert.ToDecimal(dong["TongTienHoan"]).ToString("N0") + " đ";
            dtpNgayTra.Value = Convert.ToDateTime(dong["NgayTra"]);
            lblNhanVien.Text = "Nhân viên: " + dong["NhanVien"].ToString();
            lblKhachHang.Text = "Khách hàng: " + dong["KhachHang"].ToString();

            string trangThai = dong["TrangThai"].ToString().Trim();
            string loaiGD = dong["LoaiGiaoDich"].ToString().Trim();

            cboTrangThai.SelectedIndex = -1;
            for (int i = 0; i < cboTrangThai.Items.Count; i++)
            {
                if (cboTrangThai.Items[i].ToString().Trim() == trangThai)
                { cboTrangThai.SelectedIndex = i; break; }
            }

            cboLoaiGiaoDich.SelectedIndex = -1;
            for (int i = 0; i < cboLoaiGiaoDich.Items.Count; i++)
            {
                if (cboLoaiGiaoDich.Items[i].ToString().Trim() == loaiGD)
                { cboLoaiGiaoDich.SelectedIndex = i; break; }
            }

            maTraHangHienTai = Convert.ToInt32(dong["MaTraHang"]);
            lblReturnTitle.Text = "MÃ PHIẾU: " + maTraHangHienTai;

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "") return;

            int maHD;
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out maHD))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"
                SELECT nd.TenNguoiDung, kh.TenKhachHang
                FROM   HoaDon    hd
                JOIN   NguoiDung nd ON hd.MaNguoiDung = nd.MaNguoiDung
                JOIN   KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE  hd.MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", maHD);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblNhanVien.Text = "Nhân viên: " + reader["TenNguoiDung"].ToString();
                lblKhachHang.Text = "Khách hàng: " + reader["TenKhachHang"].ToString();

                reader.Close();
                NapSanPhamHoaDon(maHD);
            }
            else
            {
                reader.Close();
                MessageBox.Show("Không tìm thấy hóa đơn số " + maHD + "!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblNhanVien.Text = "Nhân viên: (Không tìm thấy)";
                lblKhachHang.Text = "Khách hàng: (Không tìm thấy)";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAdding = true;
            XoaTrangTab0();
            SetTrangThaiDangNhap();
            txtMaHoaDon.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu trả từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = true;
            SetTrangThaiDangNhap();
            txtMaHoaDon.ReadOnly = true; 
            txtLyDo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu trả từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Xác nhận xóa phiếu trả #" + maTraHangHienTai + " và toàn bộ chi tiết liên quan?",
                "Xác nhận xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlTransaction giaoDich = ((SqlConnection)DbContext.Conn).BeginTransaction();
            try
            {
                string sqlXoaCT = "DELETE FROM ChiTietTraHang WHERE MaTraHang = @matrahang";
                SqlCommand cmdCT = new SqlCommand(sqlXoaCT, (SqlConnection)DbContext.Conn, giaoDich);
                cmdCT.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdCT.ExecuteNonQuery();

                string sqlXoa = "DELETE FROM TraHang WHERE MaTraHang = @matrahang";
                SqlCommand cmd = new SqlCommand(sqlXoa, (SqlConnection)DbContext.Conn, giaoDich);
                cmd.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmd.ExecuteNonQuery();

                giaoDich.Commit();
                MessageBox.Show("Xóa phiếu trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                giaoDich.Rollback();
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn gốc!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            int maHD;
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out maHD))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập lý do trả hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }


            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn loại giao dịch!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            if (isAdding)
            {
                string sqlCheck = "SELECT COUNT(*) FROM TraHang WHERE MaHoaDon = @mahd";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, (SqlConnection)DbContext.Conn);
                cmdCheck.Parameters.AddWithValue("@mahd", maHD);
                int soPhieu = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (soPhieu > 0)
                {
                    MessageBox.Show("Hóa đơn này đã có phiếu trả rồi!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sqlInsert = @"
                    INSERT INTO TraHang (MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
                    VALUES (@mahd, 1, @lydo, 0, GETDATE(), N'Chờ xử lý', @loaigd);
                    SELECT SCOPE_IDENTITY();"; 

                SqlCommand cmdInsert = new SqlCommand(sqlInsert, (SqlConnection)DbContext.Conn);
                cmdInsert.Parameters.AddWithValue("@mahd", maHD);
                cmdInsert.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
          
                cmdInsert.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());

                maTraHangHienTai = Convert.ToInt32(cmdInsert.ExecuteScalar());

                MessageBox.Show(
                    "Đã khởi tạo phiếu trả #" + maTraHangHienTai + " thành công!\n" +
                    "Vui lòng chuyển sang Tab 'Chọn sản phẩm trả' để tiếp tục.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
              
                string sqlUpdate = @"
                    UPDATE TraHang SET
                        LyDo         = @lydo,
                        LoaiGiaoDich = @loaigd
                    WHERE MaTraHang = @matrahang";

                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, (SqlConnection)DbContext.Conn);
                cmdUpdate.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
                cmdUpdate.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());
                cmdUpdate.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdUpdate.ExecuteNonQuery();

                MessageBox.Show("Cập nhật phiếu trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            NapDanhSachPhieu();

            isAdding = false;
            isEditing = false;
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            KhoaONhapTab0(true);

            lblReturnTitle.Text = "MÃ PHIẾU: " + maTraHangHienTai;
            NapSanPhamHoaDon(maHD); 
            KhoaTab1(false);        

            tabMain.SelectedIndex = 1;
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetTrangThaiBanDau();
            NapDanhSachPhieu();
        }

     
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtMaHoaDon.Text.Trim();
            if (tuKhoa == "")
            {
                MessageBox.Show("Nhập mã hóa đơn để tìm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"
                SELECT 
                    th.MaTraHang, th.MaHoaDon, th.TrangThai, th.LoaiGiaoDich,
                    th.TongTienHoan, nd.TenNguoiDung AS NhanVien,
                    th.NgayTra, th.LyDo, kh.TenKhachHang AS KhachHang
                FROM TraHang th
                JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
                JOIN HoaDon    hd ON th.MaHoaDon    = hd.MaHoaDon
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE th.MaHoaDon LIKE @mahd OR th.MaTraHang LIKE @mahd";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            da.SelectCommand.Parameters.AddWithValue("@mahd", "%" + tuKhoa + "%");

            DataTable dtSearch = new DataTable();
            da.Fill(dtSearch);

            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phiếu trả phù hợp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvReturns.DataSource = dtSearch;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

      
        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtInvoiceDetails == null || dtInvoiceDetails.Rows.Count == 0) return;
            if (e.RowIndex < 0) return;

            DataRowView rowView = (DataRowView)dgvProductsSelection.Rows[e.RowIndex].DataBoundItem;

            txtSelMaSP.Text = rowView["MaSanPham"].ToString();
            txtSelTenSP.Text = rowView["TenSanPham"].ToString();
            txtSelDonGia.Text = rowView["DonGia"].ToString();
            txtSelSoLuong.Text = "";
            txtSelTinhTrang.Text = "";

            string tenAnh = rowView["Anh"]?.ToString();
            if (tenAnh != null && tenAnh != "")
            {
                string duongDan1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", tenAnh);
                string duongDan2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", tenAnh);

                if (System.IO.File.Exists(duongDan1)) picAnh.Image = System.Drawing.Image.FromFile(duongDan1);
                else if (System.IO.File.Exists(duongDan2)) picAnh.Image = System.Drawing.Image.FromFile(duongDan2);
                else picAnh.Image = null;
            }
            else
            {
                picAnh.Image = null;
            }

            txtSelSoLuong.Focus();
        }

        private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtCart == null || dtCart.Rows.Count == 0) return;
            if (e.RowIndex < 0) return;

            DataGridViewRow dong = dgvCurrentDetails.Rows[e.RowIndex];
            txtSelMaSP.Text = dong.Cells["colCurMaSP"].Value?.ToString();
            txtSelTenSP.Text = dong.Cells["colCurTenSP"].Value?.ToString();
            txtSelSoLuong.Text = dong.Cells["colCurSoLuong"].Value?.ToString();
            txtSelDonGia.Text = dong.Cells["colCurDonGia"].Value?.ToString();
            txtSelTinhTrang.Text = dong.Cells["colCurTinhTrang"].Value?.ToString();
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn sản phẩm từ danh sách bên trái!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuongTra;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuongTra) || soLuongTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            if (txtSelTinhTrang.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tình trạng hàng trả!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelTinhTrang.Focus();
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text.Trim());

            int slMua = 0;
            int daTra = 0;
            for (int i = 0; i < dgvProductsSelection.Rows.Count; i++)
            {
                if (dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value != null &&
                    Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value) == maSP)
                {
                    slMua = Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelSoLuongMua"].Value);
                    daTra = Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelDaTra"].Value);
                    break;
                }
            }

            int soLuongToiDa = slMua - daTra; 
            if (soLuongTra > soLuongToiDa)
            {
                MessageBox.Show(
                    "Số lượng trả vượt quá giới hạn!\nTối đa có thể trả thêm: " + soLuongToiDa + " sản phẩm.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal donGia = 0;
            decimal.TryParse(txtSelDonGia.Text.Trim(), out donGia);

            bool daCoTrong = false;
            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                {
                    
                    int slHienTai = Convert.ToInt32(dtCart.Rows[i]["colCurSoLuong"]);
                    if (slHienTai + soLuongTra > soLuongToiDa)
                    {
                        MessageBox.Show(
                            "Tổng số lượng trả vượt quá giới hạn! Hiện đã có " + slHienTai +
                            " trong giỏ, tối đa còn được thêm: " + (soLuongToiDa - slHienTai),
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

               
                    dtCart.Rows[i]["colCurSoLuong"] = slHienTai + soLuongTra;
                    dtCart.Rows[i]["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    daCoTrong = true;
                    break;
                }
            }

         
            if (!daCoTrong)
            {
                DataRow dongMoi = dtCart.NewRow();
                dongMoi["colCurMaSP"] = maSP;
                dongMoi["colCurTenSP"] = txtSelTenSP.Text.Trim();
                dongMoi["colCurSoLuong"] = soLuongTra;
                dongMoi["colCurDonGia"] = donGia;
                dongMoi["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                dtCart.Rows.Add(dongMoi);
            }

            TinhTongTienHoanTra();
            XoaTrangTab1SanPham(); 
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa từ danh sách hàng trả lại!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa sản phẩm này khỏi danh sách trả?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            int maSP = int.Parse(txtSelMaSP.Text.Trim());

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
            XoaTrangTab1SanPham();
        }

        private void XoaTrangTab1SanPham()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
        }

        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (dtCart == null || dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Danh sách hàng trả lại đang trống! Vui lòng thêm sản phẩm.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Chưa có phiếu trả! Vui lòng hoàn tất Tab 1 trước.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlTransaction giaoDich = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
         
                decimal tongTienHoan = 0;
                for (int i = 0; i < dtCart.Rows.Count; i++)
                    tongTienHoan += Convert.ToDecimal(dtCart.Rows[i]["colCurThanhTien"]);

             
                string sqlXoaCT = "DELETE FROM ChiTietTraHang WHERE MaTraHang = @matrahang";
                SqlCommand cmdXoa = new SqlCommand(sqlXoaCT, (SqlConnection)DbContext.Conn, giaoDich);
                cmdXoa.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdXoa.ExecuteNonQuery();

               
                for (int i = 0; i < dtCart.Rows.Count; i++)
                {
                    DataRow dong = dtCart.Rows[i];
                    int maSP = Convert.ToInt32(dong["colCurMaSP"]);
                    int soLuongTra = Convert.ToInt32(dong["colCurSoLuong"]);
                    decimal thanhTien = Convert.ToDecimal(dong["colCurThanhTien"]);
                    string tinhTrang = dong["colCurTinhTrang"].ToString();


                    string sqlCT = @"INSERT INTO ChiTietTraHang (MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang)
                                     VALUES (@matrahang, @masp, @sl, @tien, @tinhtrang)";
                    SqlCommand cmdCT = new SqlCommand(sqlCT, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdCT.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                    cmdCT.Parameters.AddWithValue("@masp", maSP);
                    cmdCT.Parameters.AddWithValue("@sl", soLuongTra);
                    cmdCT.Parameters.AddWithValue("@tien", thanhTien);
                    cmdCT.Parameters.AddWithValue("@tinhtrang", tinhTrang);
                    cmdCT.ExecuteNonQuery();

                  
                    string sqlTon = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @masp";
                    SqlCommand cmdTon = new SqlCommand(sqlTon, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdTon.Parameters.AddWithValue("@masp", maSP);
                    int tonKhoTruoc = Convert.ToInt32(cmdTon.ExecuteScalar());
                    int tonKhoSau = tonKhoTruoc + soLuongTra;

                    string sqlCapNhat = "UPDATE SanPham SET SoLuongTon = @sau WHERE MaSanPham = @masp";
                    SqlCommand cmdCapNhat = new SqlCommand(sqlCapNhat, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdCapNhat.Parameters.AddWithValue("@sau", tonKhoSau);
                    cmdCapNhat.Parameters.AddWithValue("@masp", maSP);
                    cmdCapNhat.ExecuteNonQuery();
                }

              
                decimal tienHoanThucTe = tongTienHoan; 

                string loaiGD = cboLoaiGiaoDich.Text.Trim();
                if (loaiGD == "Đổi hàng (1:1)")
                {
                    tienHoanThucTe = 0;
                }
            
                string sqlCapNhatPhieu = @"UPDATE TraHang SET
                    TongTienHoan = @tong,
                    TrangThai    = N'Hoàn thành',
                    NgayTra      = GETDATE()
                    WHERE MaTraHang = @matrahang";
                SqlCommand cmdPhieu = new SqlCommand(sqlCapNhatPhieu, (SqlConnection)DbContext.Conn, giaoDich);
                cmdPhieu.Parameters.AddWithValue("@tong", tienHoanThucTe);
                cmdPhieu.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdPhieu.ExecuteNonQuery();

                giaoDich.Commit();

                txtTongTienHoan.Text = tienHoanThucTe.ToString("N0") + " đ";
                for (int k = 0; k < cboTrangThai.Items.Count; k++)
                {
                    if (cboTrangThai.Items[k].ToString().Trim() == "Hoàn thành")
                    { cboTrangThai.SelectedIndex = k; break; }
                }
                dtpNgayTra.Value = DateTime.Now;

                MessageBox.Show("Lưu chi tiết phiếu trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                NapDanhSachPhieu();

                KhoaTab1(true);
                tabMain.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                giaoDich.Rollback();
                MessageBox.Show("Có lỗi khi lưu dữ liệu:\n" + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
        private void btnReturnSearch_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập Mã Hóa Đơn cần xem sản phẩm:", "Tìm kiếm hóa đơn", "");

            if (input.Trim() == "") return;

            int maHD;
            if (!int.TryParse(input.Trim(), out maHD))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NapSanPhamHoaDon(maHD);

            if (dtInvoiceDetails == null || dtInvoiceDetails.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn hoặc hóa đơn không có sản phẩm!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("Đã tìm thấy hóa đơn " + maHD + "! Chọn sản phẩm cần trả ở danh sách bên trái.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

     
        private void btnReturnRefresh_Click(object sender, EventArgs e)
        {
            XoaTrangTab1();
        }

       
        private void tabSelectionContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabSelectionContainer.SelectedIndex == 1 && txtSelMaSP.Text.Trim() == "")
                tabSelectionContainer.SelectedIndex = 0;
        }

        private void btnSelectProduct_Click(object sender, EventArgs e)
        {
            tabSelectionContainer.SelectedIndex = 0;
            txtSelSoLuong.Focus();
        }

     
        private void cboLoaiGiaoDich_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnChooseProducts_Click(object sender, EventArgs e) { }
        private void btnResetCartForm_Click(object sender, EventArgs e) { }
        private void tabPhieuTra_Click(object sender, EventArgs e) { }
        private void tabChonSanPham_Click(object sender, EventArgs e) { }
        private void tabProductDetail_Click(object sender, EventArgs e) { }
        private void pnlReturnTop_Paint(object sender, PaintEventArgs e) { }
        private void txtMaHoaDon_TextChanged(object sender, EventArgs e) { }

        private void dgvProductsSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { dgvProductsSelection_CellClick(sender, e); }

        private void dgvCurrentDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { dgvCurrentDetails_CellClick(sender, e); }

        private void dgvReturns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { dgvReturns_CellClick(sender, e); }

        private void btnSuaCT_Click(object sender, EventArgs e) { }
        private void btnBoquaCT_Click(object sender, EventArgs e) { btnReturnRefresh_Click(sender, e); }
    }
}