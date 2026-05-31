using AssignmentApp.DAL.Core;
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
        #region 1. KHAI BÁO BIẾN & KHỞI TẠO (DÙNG CHUNG)
    
        // Bảng dữ liệu chứa danh sách các phiếu trả hàng
        DataTable dtReturn;        
        // Bảng dữ liệu chứa danh sách chi tiết sản phẩm của hóa đơn gốc
        DataTable dtInvoiceDetails; 
        // Bảng dữ liệu đóng vai trò như 'Giỏ hàng' chứa các sản phẩm được chọn để trả
        DataTable dtCart;           

        // Biến lưu trữ mã của Phiếu Trả Hàng đang được thao tác (nếu = 0 nghĩa là chưa chọn phiếu nào)
        int maTraHangHienTai = 0;  
        // Cờ đánh dấu hệ thống đang ở chế độ Thêm mới phiếu
        bool isAdding = false;   
        // Cờ đánh dấu hệ thống đang ở chế độ Sửa phiếu
        bool isEditing = false;    
        // Cờ đánh dấu hệ thống đang ở chế độ Tìm kiếm phiếu (Tab 1)
        bool isSearching = false;    
        // Cờ đánh dấu xem giỏ hàng đã bị thay đổi (thêm/sửa/xóa) hay chưa
        bool isCartModified = false;
        // Cờ đánh dấu hệ thống đang ở chế độ Tìm kiếm sản phẩm (Tab 2)
        bool isReturnSearching = false;

        // Hàm khởi tạo (Constructor): Được gọi tự động đầu tiên khi giao diện (ucReturn) được tạo ra
        public ucReturn()
        {
            // Lệnh bắt buộc để vẽ các thành phần giao diện (nút, bảng, chữ,...)
            InitializeComponent();
            
            // Đăng ký sự kiện: Khi người dùng đổi ngày trả hàng thì gọi hàm dtpNgayTra_ValueChanged
            dtpNgayTra.ValueChanged += dtpNgayTra_ValueChanged;
            // Đăng ký sự kiện: Khi người dùng chuyển qua lại giữa các Tab (Tab 1 và Tab 2) thì gọi hàm tabMain_Selecting
            tabMain.Selecting += tabMain_Selecting;
        }

        // Hàm xử lý sự kiện: Khi đổi ngày trả hàng
        private void dtpNgayTra_ValueChanged(object sender, EventArgs e)
        {
            // Đảm bảo định dạng hiển thị ngày tháng luôn là kiểu Short (Vd: 15/08/2026)
            if (dtpNgayTra.Format == DateTimePickerFormat.Custom)
            {
                dtpNgayTra.Format = DateTimePickerFormat.Short;
            }
        }

        // Hàm xử lý sự kiện: Khi bấm chọn chuyển Tab
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Nếu người dùng định bấm sang Tab 2 (Chọn sản phẩm) nhưng chưa có phiếu nào được chọn (maTraHangHienTai == 0)
            if (e.TabPage == tabChonSanPham && maTraHangHienTai == 0)
            {
                e.Cancel = true; // Hủy thao tác chuyển Tab
                MessageBox.Show("Vui lòng chọn hoặc tạo mới một phiếu trả trước khi chuyển sang tab chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm xử lý sự kiện Load form: Chạy 1 lần duy nhất khi giao diện vừa mở lên
        private void ucReturn_Load(object sender, EventArgs e)
        {
            KhoiTaoGioHang();      // Bước 1: Tạo cấu trúc cho giỏ hàng
            NapDanhSachPhieu();    // Bước 2: Lấy dữ liệu phiếu từ cơ sở dữ liệu lên bảng (Grid)
            SetTrangThaiBanDau();  // Bước 3: Đưa giao diện về trạng thái khóa/mặc định ban đầu
        }

        // Hàm khởi tạo giỏ hàng (Chỉ tạo cấu trúc các cột, chưa có dữ liệu)
        private void KhoiTaoGioHang()
        {
            dtCart = new DataTable();
            // Khai báo các cột cho bảng giỏ hàng với kiểu dữ liệu tương ứng (int, string, decimal)
            dtCart.Columns.Add("colCurMaSP", typeof(int));
            dtCart.Columns.Add("colCurTenSP", typeof(string));
            dtCart.Columns.Add("colCurSoLuong", typeof(int));
            dtCart.Columns.Add("colCurDonGia", typeof(decimal));
            dtCart.Columns.Add("colCurTinhTrang", typeof(string));
            // Cột Thành tiền tự động tính bằng Số lượng x Đơn giá
            dtCart.Columns.Add("colCurThanhTien", typeof(decimal), "colCurSoLuong * colCurDonGia");

            // Liên kết các cột vừa tạo vào các cột trên giao diện DataGridView (Bảng bên phải ở Tab 2)
            colCurMaSP.DataPropertyName = "colCurMaSP";
            colCurTenSP.DataPropertyName = "colCurTenSP";
            colCurSoLuong.DataPropertyName = "colCurSoLuong";
            colCurDonGia.DataPropertyName = "colCurDonGia";
            colCurTinhTrang.DataPropertyName = "colCurTinhTrang";
            colCurThanhTien.DataPropertyName = "colCurThanhTien";

            dgvCurrentDetails.AutoGenerateColumns = false; // Tắt tự động sinh cột để dùng cột mình tự cấu hình
            dgvCurrentDetails.DataSource = dtCart;         // Đổ dữ liệu của dtCart vào bảng
            dgvCurrentDetails.AllowUserToAddRows = false;  // Không cho người dùng tự gõ thêm hàng trống vào cuối bảng
        }

        #endregion

        #region 2. CÁC HÀM TIỆN ÍCH & TRẠNG THÁI (DÙNG CHUNG)

        // Hàm thiết lập trạng thái mặc định ban đầu cho toàn bộ giao diện
        private void SetTrangThaiBanDau()
        {
            // Tắt toàn bộ các cờ trạng thái
            isAdding = false;
            isEditing = false;
            isSearching = false;

            // Khóa các ô nhập liệu ở Tab 1 để ngăn người dùng gõ linh tinh khi chưa bấm nút
            KhoaONhapTab0(true);

            // Bật nút Thêm, tắt các nút sửa/xóa/lưu (Vì chưa chọn phiếu nào thì không thể sửa xóa)
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;   
            btnRefresh.Enabled = true;

            // Khóa luôn các nút ở Tab 2
            KhoaTab1(true);
            
            // Xóa sạch chữ ở các ô nhập liệu
            XoaTrangTab0();
            XoaTrangTab1();
        }

      
        // Hàm thiết lập trạng thái khi người dùng đang Thêm hoặc Sửa phiếu
        private void SetTrangThaiDangNhap()
        {
            // Mở khóa các ô nhập liệu để người dùng gõ
            KhoaONhapTab0(false);

            // Khóa các nút Thêm, Sửa, Xóa để tránh xung đột
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            // Bật nút Lưu và Bỏ qua
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        // Hàm ẩn/hiện (Khóa/mở) các ô nhập liệu ở Tab 1
        private void KhoaONhapTab0(bool khoa)
        {
            // Nếu khoa = true -> ReadOnly = true (Chỉ đọc), Enabled = false (Bị mờ đi)
            txtMaHoaDon.ReadOnly = khoa;
            txtMaHoaDon.Enabled = !khoa;
            txtLyDo.ReadOnly = khoa;
            txtLyDo.Enabled = !khoa;
            dtpNgayTra.Enabled = false; // Ngày trả hệ thống tự lấy ngày hiện tại nên luôn khóa
            cboTrangThai.Enabled = !khoa;
            cboLoaiGiaoDich.Enabled = !khoa;
            
            // Tổng tiền, Khách hàng, Nhân viên là do hệ thống tự tính/tự lấy, người dùng không được gõ
            txtTongTienHoan.ReadOnly = true;
            txtTongTienHoan.Enabled = false;
            txtKhachHang.ReadOnly = true;
            txtKhachHang.Enabled = false;
            txtNhanVien.ReadOnly = true;
            txtNhanVien.Enabled = false;
        }

      
        // Hàm khóa toàn bộ các nút chức năng ở Tab 2
        private void KhoaTab1(bool khoa)
        {
            btnAddToCart.Enabled = false;
            btnSuaCT.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            btnBoquaCT.Enabled = false;

            txtSelSoLuong.ReadOnly = khoa;
            txtSelTinhTrang.ReadOnly = khoa;
        }

      
        // Hàm xóa sạch nội dung các ô nhập ở Tab 1
        private void XoaTrangTab0()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0 đ";
            dtpNgayTra.Value = DateTime.Now; // Reset về ngày hiện tại
            dtpNgayTra.Format = DateTimePickerFormat.Short;

            cboTrangThai.SelectedIndex = 1; // Mặc định chọn trạng thái ở dòng số 2 (Đang xử lý)

            cboLoaiGiaoDich.SelectedIndex = -1; // Bỏ chọn
            txtKhachHang.Text = "";
            txtNhanVien.Text = "";
            maTraHangHienTai = 0; // Xóa mã phiếu đang nhớ
        }

       
        // Hàm xóa sạch nội dung các ô nhập ở Tab 2
        private void XoaTrangTab1()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
            lblTotalAmount.Text = "TỔNG TIỀN HOÀN TRẢ TẠM TÍNH: 0 đ";

            // Xóa sạch giỏ hàng tạm
            if (dtCart != null)
                dtCart.Rows.Clear();

            // Xóa sạch danh sách sản phẩm hiển thị của hóa đơn gốc
            if (dtInvoiceDetails != null)
                dtInvoiceDetails.Rows.Clear();

            lblReturnTitle.Text = "MÃ PHIẾU: ";
        }

   
        // Hàm tải toàn bộ danh sách Phiếu trả hàng từ SQL Database lên bảng (Grid)
        private void NapDanhSachPhieu()
        {
            // Câu lệnh SQL: Lấy thông tin phiếu trả, join với các bảng khác để lấy tên nhân viên, khách hàng
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
                ORDER BY th.MaTraHang DESC"; // Sắp xếp phiếu mới nhất lên đầu

            // Kiểm tra kết nối Database, nếu đóng thì mở ra
            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            // Dùng DataAdapter để kéo dữ liệu từ SQL về
            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtReturn = new DataTable();
            da.Fill(dtReturn); // Đổ dữ liệu vào bảng dtReturn

            // Gắn các cột dữ liệu vào giao diện bảng (Grid)
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

        #endregion

        #region 3. TAB 1: QUẢN LÝ PHIẾU TRẢ HÀNG

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
            txtNhanVien.Text = dong["NhanVien"].ToString();
            txtKhachHang.Text = dong["KhachHang"].ToString();

        
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
            int maHoaDonChon = Convert.ToInt32(dong["MaHoaDon"]);
            lblReturnTitle.Text = "MÃ PHIẾU: " + maTraHangHienTai;

            KhoaONhapTab0(false);
            txtMaHoaDon.ReadOnly = true;
            txtMaHoaDon.Enabled = false;

            btnAdd.Enabled = false;
            btnSave.Enabled = false;

            btnEdit.Enabled = true; 
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;

          
            NapSanPhamHoaDon(maHoaDonChon);

         
            NapChiTietDaTra(maTraHangHienTai);

          
            if (trangThai == "Hoàn thành" || trangThai == "Đã hủy")
            {
                
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnLuuCT.Enabled = false;
                btnBoquaCT.Enabled = false;
                txtSelSoLuong.ReadOnly = true;
                txtSelTinhTrang.ReadOnly = true;
            }
            else 
            {
             
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnLuuCT.Enabled = false;
                btnBoquaCT.Enabled = false;
                txtSelSoLuong.ReadOnly = false;
                txtSelTinhTrang.ReadOnly = false;
                isCartModified = false;
            }
        }

    
        private void NapChiTietDaTra(int maTraHang)
        {
            
            if (dtCart != null)
                dtCart.Rows.Clear();

            string sql = @"
                SELECT 
                    ctth.MaSanPham,
                    sp.TenSanPham,
                    ctth.SoLuong,
                    ctth.TienHoan,
                    ctth.TinhTrang
                FROM ChiTietTraHang ctth
                JOIN SanPham sp ON ctth.MaSanPham = sp.MaSanPham
                WHERE ctth.MaTraHang = @matrahang";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            da.SelectCommand.Parameters.AddWithValue("@matrahang", maTraHang);

            DataTable dtChiTiet = new DataTable();
            da.Fill(dtChiTiet);

            for (int i = 0; i < dtChiTiet.Rows.Count; i++)
            {
                DataRow nguon = dtChiTiet.Rows[i];
                int soLuong = Convert.ToInt32(nguon["SoLuong"]);
                decimal tienHoan = Convert.ToDecimal(nguon["TienHoan"]);
                decimal donGia = 0;
                if (soLuong > 0)
                {
                    donGia = tienHoan / soLuong;
                }
                else
                {
                    donGia = 0;
                }

                DataRow dongMoi = dtCart.NewRow();
                dongMoi["colCurMaSP"] = Convert.ToInt32(nguon["MaSanPham"]);
                dongMoi["colCurTenSP"] = nguon["TenSanPham"].ToString();
                dongMoi["colCurSoLuong"] = soLuong;
                dongMoi["colCurDonGia"] = donGia;
                dongMoi["colCurTinhTrang"] = nguon["TinhTrang"].ToString();
                dtCart.Rows.Add(dongMoi);
            }

            TinhTongTienHoanTra();
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
                SELECT kh.TenKhachHang
                FROM   HoaDon    hd
                JOIN   KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE  hd.MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", maHD);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (AssignmentApp.BLL.Session.UserSession.CurrentUser != null)
                    txtNhanVien.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser.TenNguoiDung;
                else
                    txtNhanVien.Text = "(Không rõ)";

                txtKhachHang.Text = reader["TenKhachHang"].ToString();

                reader.Close();
                NapSanPhamHoaDon(maHD);
            }
            else
            {
                reader.Close();
                MessageBox.Show("Không tìm thấy hóa đơn số " + maHD + "!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhanVien.Text = "(Không tìm thấy)";
                txtKhachHang.Text = "(Không tìm thấy)";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAdding = true;
            XoaTrangTab0();
            SetTrangThaiDangNhap();
            cboLoaiGiaoDich.Text = "Trả hàng";
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

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            string sqlCheck = "SELECT TrangThai FROM TraHang WHERE MaTraHang = @matrahang";
            SqlCommand cmdCheck = new SqlCommand(sqlCheck, (SqlConnection)DbContext.Conn);
            cmdCheck.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
            string trangThaiHienTai = cmdCheck.ExecuteScalar()?.ToString();

            if (trangThaiHienTai == "Hoàn thành" || trangThaiHienTai == "Đã hủy")
            {
                MessageBox.Show("Phiếu trả đã '" + trangThaiHienTai + "', không thể sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            string sqlUpdate = @"
                UPDATE TraHang SET
                    LyDo         = @lydo,
                    LoaiGiaoDich = @loaigd,
                    TrangThai    = @trangthai
                WHERE MaTraHang = @matrahang";

            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, (SqlConnection)DbContext.Conn);
            cmdUpdate.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
            cmdUpdate.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());
            cmdUpdate.Parameters.AddWithValue("@trangthai", cboTrangThai.Text.Trim());
            cmdUpdate.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
            cmdUpdate.ExecuteNonQuery();

            MessageBox.Show("Cập nhật phiếu trả thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu trả từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            string sqlCheck = "SELECT TrangThai FROM TraHang WHERE MaTraHang = @matrahang";
            SqlCommand cmdCheck = new SqlCommand(sqlCheck, (SqlConnection)DbContext.Conn);
            cmdCheck.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
            string trangThaiHienTai = cmdCheck.ExecuteScalar()?.ToString();

            if (trangThaiHienTai == "Hoàn thành" || trangThaiHienTai == "Đã hủy")
            {
                MessageBox.Show("Phiếu trả đã '" + trangThaiHienTai + "', không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Xác nhận chuyển trạng thái phiếu trả #" + maTraHangHienTai + " thành 'Đã hủy'?",
                "Xác nhận hủy phiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            try
            {
                string sqlUpdate = "UPDATE TraHang SET TrangThai = N'Đã hủy' WHERE MaTraHang = @matrahang";
                SqlCommand cmd = new SqlCommand(sqlUpdate, (SqlConnection)DbContext.Conn);
                cmd.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Hủy phiếu trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

      
        // Hàm xử lý sự kiện khi bấm nút Lưu ở Tab 1 (Dùng để Tạo mới phiếu trả)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra dữ liệu đầu vào (Validate)
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn gốc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Bước 2: Kiểm tra phiên đăng nhập (Ai đang thao tác)
            if (AssignmentApp.BLL.Session.UserSession.CurrentUser == null)
            {
                MessageBox.Show("Lỗi: Không tìm thấy phiên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bước 3: Mở kết nối Database
            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            // Bước 4: Tạo câu lệnh SQL Insert (Thêm phiếu mới vào bảng TraHang)
            string sqlInsert = @"
                INSERT INTO TraHang (MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
                VALUES (@mahd, @manguoidung, @lydo, 0, GETDATE(), @trangthai, @loaigd);
                SELECT SCOPE_IDENTITY();"; // SCOPE_IDENTITY() trả về ID của phiếu vừa được tạo

            SqlCommand cmdInsert = new SqlCommand(sqlInsert, (SqlConnection)DbContext.Conn);
            cmdInsert.Parameters.AddWithValue("@mahd", maHD);
            cmdInsert.Parameters.AddWithValue("@manguoidung", AssignmentApp.BLL.Session.UserSession.CurrentUser.MaNguoiDung);
            cmdInsert.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
            cmdInsert.Parameters.AddWithValue("@trangthai", cboTrangThai.Text.Trim());
            cmdInsert.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());

            // Thực thi và lưu lại Mã Phiếu vừa tạo vào biến maTraHangHienTai
            int newMaTraHang = Convert.ToInt32(cmdInsert.ExecuteScalar());

            MessageBox.Show(
                "Đã khởi tạo phiếu trả #" + newMaTraHang + " thành công!\n" +
                "Vui lòng chuyển sang Tab 'Chọn sản phẩm trả' để tiếp tục.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Bước 5: Cập nhật lại giao diện
            NapDanhSachPhieu(); // Tải lại danh sách để phiếu mới hiện lên grid
            SetTrangThaiBanDau(); // Reset form
            
            // Giữ nguyên phiếu vừa tạo để thao tác tiếp
            maTraHangHienTai = newMaTraHang;
            cboTrangThai.Text = "Đang xử lý";
            KhoaONhapTab0(false); // Mở khóa các ô nhập

            // Hiển thị tiêu đề phiếu bên Tab 2
            lblReturnTitle.Text = "MÃ PHIẾU: " + maTraHangHienTai;
            // Tải danh sách sản phẩm thuộc hóa đơn này lên Tab 2
            NapSanPhamHoaDon(maHD); 
            KhoaTab1(false); // Mở khóa các nút bên Tab 2

            // Bước 6: Tự động chuyển sang Tab 2 để chọn sản phẩm
            tabMain.SelectedIndex = 1;
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetTrangThaiBanDau();
            NapDanhSachPhieu();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnAdd.Enabled || btnEdit.Enabled || btnDelete.Enabled || btnSave.Enabled || btnCancel.Enabled)
            {
                isSearching = true;

                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                KhoaONhapTab0(false);
                XoaTrangTab0();
                
                txtKhachHang.Enabled = true;
                txtKhachHang.ReadOnly = false;
                txtNhanVien.Enabled = true;
                txtNhanVien.ReadOnly = false;
                txtTongTienHoan.Enabled = true;
                txtTongTienHoan.ReadOnly = false;
                dtpNgayTra.Enabled = true;
                dtpNgayTra.ShowCheckBox = false;
                dtpNgayTra.Format = DateTimePickerFormat.Custom;
                dtpNgayTra.CustomFormat = " ";
                
                cboTrangThai.SelectedIndex = -1;
                cboLoaiGiaoDich.SelectedIndex = -1;
                
                lblReturnTitle.Text = "CHẾ ĐỘ TÌM KIẾM";
                MessageBox.Show("Đã chuyển sang chế độ tìm kiếm.\nVui lòng nhập thông tin tìm kiếm vào các ô tương ứng và ấn TÌM KIẾM lần nữa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                txtMaHoaDon.Focus();
                return;
            }

            string maHD = txtMaHoaDon.Text.Trim();
            string khach = txtKhachHang.Text.Trim();
            string nhanVien = txtNhanVien.Text.Trim();
            string lydo = txtLyDo.Text.Trim();
            string tongTienStr = txtTongTienHoan.Text.Replace(" đ", "").Replace(",", "").Replace(".", "").Trim();

            bool isAnyFieldFilled = maHD != "" || khach != "" || nhanVien != "" || lydo != "" || 
                                    (tongTienStr != "" && tongTienStr != "0") || 
                                    cboTrangThai.SelectedIndex != -1 || 
                                    cboLoaiGiaoDich.SelectedIndex != -1 || 
                                    dtpNgayTra.Format != DateTimePickerFormat.Custom;

            if (!isAnyFieldFilled)
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn ít nhất một thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                WHERE 1=1 ";

            if (maHD != "") sql += " AND (th.MaHoaDon LIKE @mahd OR th.MaTraHang LIKE @mahd) ";
            if (khach != "") sql += " AND kh.TenKhachHang LIKE @khach ";
            if (nhanVien != "") sql += " AND nd.TenNguoiDung LIKE @nhanVien ";
            if (lydo != "") sql += " AND th.LyDo LIKE @lydo ";
            if (cboTrangThai.SelectedIndex != -1) sql += " AND th.TrangThai = @trangthai ";
            if (cboLoaiGiaoDich.SelectedIndex != -1) sql += " AND th.LoaiGiaoDich = @loaigd ";
            
            if (tongTienStr != "" && tongTienStr != "0")
            {
                if (decimal.TryParse(tongTienStr, out decimal tien))
                    sql += " AND th.TongTienHoan = @tien ";
            }

            if (dtpNgayTra.Format != DateTimePickerFormat.Custom)
            {
                sql += " AND CAST(th.NgayTra AS DATE) = @ngaytra ";
            }

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            if (maHD != "") da.SelectCommand.Parameters.AddWithValue("@mahd", "%" + maHD + "%");
            if (khach != "") da.SelectCommand.Parameters.AddWithValue("@khach", "%" + khach + "%");
            if (nhanVien != "") da.SelectCommand.Parameters.AddWithValue("@nhanVien", "%" + nhanVien + "%");
            if (lydo != "") da.SelectCommand.Parameters.AddWithValue("@lydo", "%" + lydo + "%");
            if (cboTrangThai.SelectedIndex != -1) da.SelectCommand.Parameters.AddWithValue("@trangthai", cboTrangThai.Text);
            if (cboLoaiGiaoDich.SelectedIndex != -1) da.SelectCommand.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text);
            
            if (tongTienStr != "" && tongTienStr != "0" && decimal.TryParse(tongTienStr, out decimal t))
                da.SelectCommand.Parameters.AddWithValue("@tien", t);
                
            if (dtpNgayTra.Format != DateTimePickerFormat.Custom)
                da.SelectCommand.Parameters.AddWithValue("@ngaytra", dtpNgayTra.Value.Date);

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

        #endregion

        #region 4. TAB 2: QUẢN LÝ CHI TIẾT SẢN PHẨM TRẢ

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

            // ... load image ...
            tabSelectionContainer.SelectedIndex = 1;
            
            // State management
            if (maTraHangHienTai != 0)
            {
                if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Đã hủy")
                {
                    MessageBox.Show("Phiếu trả đã '" + cboTrangThai.Text + "', không thể thêm sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnAddToCart.Enabled = true;
                    btnSuaCT.Enabled = false;
                    btnRemoveFromCart.Enabled = false;
                    btnBoquaCT.Enabled = true;
                }
            }
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
            
            // State management
            if (maTraHangHienTai != 0)
            {
                if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Đã hủy")
                {
                    MessageBox.Show("Phiếu trả đã '" + cboTrangThai.Text + "', không thể sửa hoặc xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnAddToCart.Enabled = false;
                    btnSuaCT.Enabled = true;
                    btnRemoveFromCart.Enabled = true;
                    btnBoquaCT.Enabled = true;
                }
            }
        }

     
        // Hàm xử lý khi bấm nút "Thêm vào giỏ" ở Tab 2
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra tính hợp lệ
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách hóa đơn!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text.Trim());
            int soLuongTra = 0;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuongTra) || soLuongTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải lớn hơn 0!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            decimal donGia = decimal.Parse(txtSelDonGia.Text.Trim());

            // Bước 2: Lấy số lượng đã mua và đã trả trước đó để đối chiếu
            int slMua = 0;
            int slDaTra = 0;
            foreach (DataRow row in dtInvoiceDetails.Rows)
            {
                if (Convert.ToInt32(row["MaSanPham"]) == maSP)
                {
                    slMua = Convert.ToInt32(row["SLMua"]);
                    slDaTra = Convert.ToInt32(row["DaTra"]);
                    break;
                }
            }

            // Tính số lượng còn lại được phép trả
            int slDuocPhepTra = slMua - slDaTra;

            // Bước 3: Kiểm tra xem sản phẩm này đã có trong giỏ hàng tạm chưa
            bool daCoTrong = false;
            foreach (DataRow r in dtCart.Rows)
            {
                if (Convert.ToInt32(r["colCurMaSP"]) == maSP)
                {
                    daCoTrong = true;
                    int slCu = Convert.ToInt32(r["colCurSoLuong"]);
                    int tongSLSauKhiThem = slCu + soLuongTra;

                    // Nếu tổng số lượng đòi trả vượt quá số lượng được phép trả -> Báo lỗi
                    if (tongSLSauKhiThem > slDuocPhepTra)
                    {
                        MessageBox.Show("Tổng số lượng trả (" + tongSLSauKhiThem +
                            ") vượt quá số lượng được phép trả (" + slDuocPhepTra + ")!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    // Cập nhật số lượng và tình trạng mới
                    r["colCurSoLuong"] = tongSLSauKhiThem;
                    r["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    break;
                }
            }

            // Nếu chưa có trong giỏ mà số lượng đòi trả lớn hơn số được phép -> Báo lỗi
            if (!daCoTrong && soLuongTra > slDuocPhepTra)
            {
                MessageBox.Show("Số lượng trả vượt quá số lượng được phép trả (" + slDuocPhepTra + ")!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bước 4: Thêm vào giỏ hàng nếu hợp lệ
            if (!daCoTrong)
            {
                DataRow dongMoi = dtCart.NewRow();
                dongMoi["colCurMaSP"] = maSP;
                dongMoi["colCurTenSP"] = txtSelTenSP.Text.Trim();
                dongMoi["colCurSoLuong"] = soLuongTra;
                dongMoi["colCurDonGia"] = donGia;
                dongMoi["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                dtCart.Rows.Add(dongMoi); // Đẩy dòng mới vào bảng giỏ hàng
            }

            // Bật nút Lưu thay đổi
            btnLuuCT.Enabled = true;
            // Tính lại tổng tiền
            TinhTongTienHoanTra();
            // Xóa trắng form nhập
            XoaTrangTab1SanPham();
            
            // Khóa các nút để chờ thao tác tiếp theo
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Đã hủy")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
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

            isCartModified = true;
            btnLuuCT.Enabled = true;
            TinhTongTienHoanTra();
            XoaTrangTab1SanPham();
            
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Đã hủy")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
        }

        // Hàm xử lý khi bấm nút "Hoàn tất & Trở về"
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

            tabMain.SelectedIndex = 0;
        }

        private void XoaTrangTab1SanPham()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
        }

        
        // Hàm lưu toàn bộ giỏ hàng vào Database
        private void btnLuuCT_Click(object sender, EventArgs e)
        {
            if (dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Danh sách sản phẩm trống, không có gì để lưu!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            // Sử dụng Transaction (Giao dịch): Đảm bảo hoặc là lưu thành công TẤT CẢ, hoặc là KHÔNG LƯU gì cả nếu có lỗi (tránh lưu nửa chừng)
            SqlTransaction giaoDich = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
                decimal tongTienHoan = 0;
                // Tính tổng tiền hoàn thực tế từ giỏ hàng
                for (int i = 0; i < dtCart.Rows.Count; i++)
                    tongTienHoan += Convert.ToDecimal(dtCart.Rows[i]["colCurThanhTien"]);

                // Bước 1: Khôi phục tồn kho cũ (trả lại kho) của các sản phẩm TRƯỚC KHI XÓA
                // (Vì mình sẽ xóa sạch chi tiết cũ rồi chèn lại toàn bộ chi tiết mới từ giỏ)
                string sqlKhoiPhucTon = @"
                    UPDATE SanPham
                    SET SoLuongTon = SoLuongTon - ct.SoLuong
                    FROM SanPham sp
                    JOIN ChiTietTraHang ct ON sp.MaSanPham = ct.MaSanPham
                    WHERE ct.MaTraHang = @matrahang";
                SqlCommand cmdKhoiPhuc = new SqlCommand(sqlKhoiPhucTon, (SqlConnection)DbContext.Conn, giaoDich);
                cmdKhoiPhuc.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdKhoiPhuc.ExecuteNonQuery();

                // Bước 2: Xóa sạch các chi tiết trả hàng cũ của mã phiếu này trong Database
                string sqlXoaCT = "DELETE FROM ChiTietTraHang WHERE MaTraHang = @matrahang";
                SqlCommand cmdXoa = new SqlCommand(sqlXoaCT, (SqlConnection)DbContext.Conn, giaoDich);
                cmdXoa.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdXoa.ExecuteNonQuery();

                // Bước 3: Lưu lại toàn bộ các sản phẩm đang có trong giỏ hàng (dtCart) vào Database
                for (int i = 0; i < dtCart.Rows.Count; i++)
                {
                    DataRow dong = dtCart.Rows[i];
                    int msp = Convert.ToInt32(dong["colCurMaSP"]);
                    int sl = Convert.ToInt32(dong["colCurSoLuong"]);
                    decimal dg = Convert.ToDecimal(dong["colCurDonGia"]);
                    string tt = dong["colCurTinhTrang"]?.ToString();
                    decimal tienHoan = Convert.ToDecimal(dong["colCurThanhTien"]);

                    // Thêm vào bảng ChiTietTraHang
                    string sqlThemCT = @"INSERT INTO ChiTietTraHang(MaTraHang, MaSanPham, SoLuong, TinhTrang, TienHoan)
                                         VALUES(@matrahang, @masp, @soluong, @tinhtrang, @tienhoan)";
                    SqlCommand cmdThemCT = new SqlCommand(sqlThemCT, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdThemCT.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                    cmdThemCT.Parameters.AddWithValue("@masp", msp);
                    cmdThemCT.Parameters.AddWithValue("@soluong", sl);
                    cmdThemCT.Parameters.AddWithValue("@tinhtrang", tt);
                    cmdThemCT.Parameters.AddWithValue("@tienhoan", tienHoan);
                    cmdThemCT.ExecuteNonQuery();

                    // Cập nhật lại tồn kho (+ số lượng trả vào số lượng tồn)
                    string sqlCapNhatKho = @"UPDATE SanPham SET SoLuongTon = SoLuongTon + @soluong WHERE MaSanPham = @masp";
                    SqlCommand cmdKho = new SqlCommand(sqlCapNhatKho, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdKho.Parameters.AddWithValue("@soluong", sl);
                    cmdKho.Parameters.AddWithValue("@masp", msp);
                    cmdKho.ExecuteNonQuery();
                }

                decimal tienHoanThucTe = tongTienHoan; 

                string loaiGD = cboLoaiGiaoDich.Text.Trim();
                if (loaiGD == "Đổi hàng (1:1)")
                {
                    tienHoanThucTe = 0;
                }

                // Bước 4: Cập nhật lại tổng tiền cho phiếu trả gốc
                string sqlCapNhatPhieu = @"UPDATE TraHang SET
                    TongTienHoan = @tong,
                    NgayTra      = GETDATE()
                    WHERE MaTraHang = @matrahang";
                SqlCommand cmdPhieu = new SqlCommand(sqlCapNhatPhieu, (SqlConnection)DbContext.Conn, giaoDich);
                cmdPhieu.Parameters.AddWithValue("@tong", tienHoanThucTe);
                cmdPhieu.Parameters.AddWithValue("@matrahang", maTraHangHienTai);
                cmdPhieu.ExecuteNonQuery();

                // Xác nhận lưu dữ liệu (Commit Transaction)
                giaoDich.Commit();

                // Cập nhật UI
                txtTongTienHoan.Text = tienHoanThucTe.ToString("N0") + " đ";
                dtpNgayTra.Value = DateTime.Now;
                
                isCartModified = false;
                btnLuuCT.Enabled = false;

                MessageBox.Show("Lưu thay đổi thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NapDanhSachPhieu();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, hủy toàn bộ giao dịch (Rollback)
                giaoDich.Rollback();
                MessageBox.Show("Có lỗi khi lưu dữ liệu:\n" + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturnSearch_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0 || cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Vui lòng chọn một phiếu đang xử lý trước khi tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isReturnSearching)
            {
                isReturnSearching = true;
                XoaTrangTab1SanPham();
                
                txtSelMaSP.Enabled = true;
                txtSelMaSP.ReadOnly = false;
                txtSelTenSP.Enabled = true;
                txtSelTenSP.ReadOnly = false;
                txtSelSoLuong.Enabled = true;
                txtSelSoLuong.ReadOnly = false;

                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
                txtSelTinhTrang.Enabled = false;
                txtSelTinhTrang.ReadOnly = true;

                MessageBox.Show("Vui lòng nhập Mã, Tên sản phẩm hoặc Số lượng đã trả vào ô tương ứng bên phải, sau đó ấn TÌM KIẾM lần nữa!", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSelTenSP.Focus();
            }
            else
            {
                if (dtInvoiceDetails == null) return;

                string tuKhoaTen = txtSelTenSP.Text.Trim().ToLower();
                string tuKhoaMa = txtSelMaSP.Text.Trim();
                string tuKhoaSL = txtSelSoLuong.Text.Trim();
                
                if (tuKhoaTen == "" && tuKhoaMa == "" && tuKhoaSL == "")
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string filter = "";
                if (tuKhoaMa != "") filter += "MaSanPham = " + tuKhoaMa;
                if (tuKhoaTen != "")
                {
                    if (filter != "") filter += " AND ";
                    filter += "TenSanPham LIKE '%" + tuKhoaTen + "%'";
                }
                if (tuKhoaSL != "")
                {
                    if (int.TryParse(tuKhoaSL, out int sl))
                    {
                        if (filter != "") filter += " AND ";
                        filter += "DaTra = " + sl;
                    }
                    else
                    {
                        MessageBox.Show("Số lượng trả phải là số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                dtInvoiceDetails.DefaultView.RowFilter = filter;
                dgvProductsSelection.DataSource = dtInvoiceDetails.DefaultView.ToTable();

                if (dgvProductsSelection.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp trong hóa đơn!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnReturnRefresh_Click(object sender, EventArgs e)
        {
            isReturnSearching = false;
            XoaTrangTab1SanPham();
            
            txtSelMaSP.Enabled = false;
            txtSelMaSP.ReadOnly = true;
            txtSelTenSP.Enabled = false;
            txtSelTenSP.ReadOnly = true;
            txtSelTinhTrang.Enabled = true;
            txtSelTinhTrang.ReadOnly = false;

            if (dtInvoiceDetails != null)
            {
                dtInvoiceDetails.DefaultView.RowFilter = "";
                dgvProductsSelection.DataSource = dtInvoiceDetails;
            }

            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Đã hủy")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
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

        private void btnSuaCT_Click(object sender, EventArgs e) 
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa từ giỏ hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int soLuongTra;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuongTra) || soLuongTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            
            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                {
                    if (soLuongTra > soLuongToiDa)
                    {
                        MessageBox.Show("Số lượng trả vượt quá giới hạn! Tối đa: " + soLuongToiDa, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    dtCart.Rows[i]["colCurSoLuong"] = soLuongTra;
                    dtCart.Rows[i]["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    break;
                }
            }
            
            isCartModified = true;
            btnLuuCT.Enabled = true;
            TinhTongTienHoanTra();
            XoaTrangTab1SanPham();
            
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Đã hủy")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
        }
        
        private void btnBoquaCT_Click(object sender, EventArgs e) 
        { 
            XoaTrangTab1SanPham();
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Đã hủy")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
        }

        #endregion
    }
}