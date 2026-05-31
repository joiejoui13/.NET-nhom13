using AssignmentApp.DAL.Core;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Services.Sales;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
        #region 1. KHAI BÁO BIẾN & KHỞI TẠO (DÙNG CHUNG)
    
        // Bảng dữ liệu chứa danh sách các phiếu trả hàng
        BindingList<Return> listReturn;        
        // Bảng dữ liệu chứa danh sách chi tiết sản phẩm của hóa đơn gốc
        BindingList<ReturnInvoiceProduct> listInvoiceDetails; 
        // Bảng dữ liệu đóng vai trò như 'Giỏ hàng' chứa các sản phẩm được chọn để trả
        BindingList<ReturnDetail> listCart;           
        
        private readonly IReturnService _returnService;

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
            
            if (Program.ServiceProvider != null)
                _returnService = Program.ServiceProvider.GetRequiredService<IReturnService>();
            
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
            listCart = new BindingList<ReturnDetail>();

            // Liên kết các cột vừa tạo vào các cột trên giao diện DataGridView (Bảng bên phải ở Tab 2)
            colCurMaSP.DataPropertyName = "MaSanPham";
            colCurTenSP.DataPropertyName = "TenSanPham";
            colCurSoLuong.DataPropertyName = "SoLuong";
            colCurDonGia.DataPropertyName = "DonGia";
            colCurTinhTrang.DataPropertyName = "TinhTrang";
            
            // Note: DataGridView needs CellFormatting or a calculated property for ThanhTien if not mapped directly.
            // BindingList won't auto-calculate an expression column like DataTable did.
            // We will calculate it in code or DataGridView CellFormatting.
            colCurThanhTien.DataPropertyName = "TienHoan";

            dgvCurrentDetails.AutoGenerateColumns = false; // Tắt tự động sinh cột để dùng cột mình tự cấu hình
            dgvCurrentDetails.DataSource = listCart;         // Đổ dữ liệu của dtCart vào bảng
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
            if (listCart != null)
                listCart.Clear();

            // Xóa sạch danh sách sản phẩm hiển thị của hóa đơn gốc
            if (listInvoiceDetails != null)
                listInvoiceDetails.Clear();

            lblReturnTitle.Text = "MÃ PHIẾU: ";
        }

   
        // Hàm tải toàn bộ danh sách Phiếu trả hàng từ SQL Database lên bảng (Grid)
        private async void NapDanhSachPhieu()
        {
            if (_returnService == null) return;
            var returns = await _returnService.GetAllReturnsAsync();
            listReturn = new BindingList<Return>(new List<Return>(returns));

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
            dgvReturns.DataSource = listReturn;
        }

    
        private async void NapSanPhamHoaDon(int maHoaDon)
        {
            if (_returnService == null) return;
            var products = await _returnService.GetInvoiceProductsAsync(maHoaDon);
            listInvoiceDetails = new BindingList<ReturnInvoiceProduct>(new List<ReturnInvoiceProduct>(products));
        
            colSelMaSP.DataPropertyName = "MaSanPham";
            colSelTenSP.DataPropertyName = "TenSanPham";
            colSelSoLuongMua.DataPropertyName = "SLMua";
            colSelDaTra.DataPropertyName = "DaTra";
            colSelDonGia.DataPropertyName = "DonGia";

            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = listInvoiceDetails;
        }

     
        private void TinhTongTienHoanTra()
        {
            decimal tongTien = 0;
            if (listCart != null)
            {
                foreach (var item in listCart)
                {
                    tongTien += item.TienHoan;
                }
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

            if (e.RowIndex < 0 || listReturn == null || listReturn.Count == 0) return;

         
            var dong = listReturn[e.RowIndex];

            txtMaHoaDon.Text = dong.MaHoaDon.ToString();
            txtLyDo.Text = dong.LyDo;
            txtTongTienHoan.Text = dong.TongTienHoan.ToString("N0") + " đ";
            dtpNgayTra.Value = dong.NgayTra;
            txtNhanVien.Text = dong.NhanVien;
            txtKhachHang.Text = dong.KhachHang;

        
            string trangThai = dong.TrangThai.Trim();
            string loaiGD = dong.LoaiGiaoDich.Trim();

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

            maTraHangHienTai = dong.MaTraHang;
            int maHoaDonChon = dong.MaHoaDon;
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

    
        private async void NapChiTietDaTra(int maTraHang)
        {
            if (listCart != null)
                listCart.Clear();

            if (_returnService == null) return;
            var details = await _returnService.GetReturnDetailsAsync(maTraHang);
            
            foreach (var d in details)
            {
                listCart.Add(d);
            }

            TinhTongTienHoanTra();
        }

        private async void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "") return;

            int maHD;
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out maHD))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;
            string khachHang = await _returnService.GetCustomerNameByInvoiceAsync(maHD);

            if (!string.IsNullOrEmpty(khachHang))
            {
                if (AssignmentApp.BLL.Session.UserSession.CurrentUser != null)
                    txtNhanVien.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser.TenNguoiDung;
                else
                    txtNhanVien.Text = "(Không rõ)";

                txtKhachHang.Text = khachHang;

                NapSanPhamHoaDon(maHD);
            }
            else
            {
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

       
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu trả từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;

            if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu trả đã '" + cboTrangThai.Text + "', không thể sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn loại giao dịch!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            var r = new Return { MaTraHang = maTraHangHienTai, LyDo = txtLyDo.Text.Trim(), LoaiGiaoDich = cboLoaiGiaoDich.Text.Trim(), TrangThai = cboTrangThai.Text.Trim() };
            await _returnService.UpdateReturnAsync(r);

            MessageBox.Show("Cập nhật phiếu trả thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu trả từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;

            if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu trả đã '" + cboTrangThai.Text + "', không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Xác nhận chuyển trạng thái phiếu trả #" + maTraHangHienTai + " thành 'Đã hủy'?",
                "Xác nhận hủy phiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                var r = new Return { MaTraHang = maTraHangHienTai, LyDo = txtLyDo.Text.Trim(), LoaiGiaoDich = cboLoaiGiaoDich.Text.Trim(), TrangThai = "Đã hủy" };
                await _returnService.UpdateReturnAsync(r);

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
        private async void btnSave_Click(object sender, EventArgs e)
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

            if (_returnService == null) return;

            var returnObj = new Return
            {
                MaHoaDon = maHD,
                MaNguoiDung = AssignmentApp.BLL.Session.UserSession.CurrentUser.MaNguoiDung,
                LyDo = txtLyDo.Text.Trim(),
                TongTienHoan = 0,
                NgayTra = DateTime.Now,
                TrangThai = cboTrangThai.Text.Trim(),
                LoaiGiaoDich = cboLoaiGiaoDich.Text.Trim()
            };

            try 
            {
                int newMaTraHang = await _returnService.CreateReturnAsync(returnObj);

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetTrangThaiBanDau();
            NapDanhSachPhieu();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
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

            decimal? tongTien = null;
            if (tongTienStr != "" && tongTienStr != "0" && decimal.TryParse(tongTienStr, out decimal t))
            {
                tongTien = t;
            }

            DateTime? ngayTra = null;
            if (dtpNgayTra.Format != DateTimePickerFormat.Custom)
            {
                ngayTra = dtpNgayTra.Value.Date;
            }

            if (_returnService == null) return;
            var searchResults = await _returnService.SearchReturnsAsync(maHD, khach, nhanVien, lydo, cboTrangThai.Text, cboLoaiGiaoDich.Text, tongTien, ngayTra);

            listReturn = new BindingList<Return>(new List<Return>(searchResults));

            if (listReturn.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phiếu trả phù hợp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvReturns.DataSource = listReturn;
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
            if (listInvoiceDetails == null || listInvoiceDetails.Count == 0) return;
            if (e.RowIndex < 0) return;

            var rowView = listInvoiceDetails[e.RowIndex];

            txtSelMaSP.Text = rowView.MaSanPham.ToString();
            txtSelTenSP.Text = rowView.TenSanPham;
            txtSelDonGia.Text = rowView.DonGia.ToString();
            txtSelSoLuong.Text = "";
            txtSelTinhTrang.Text = "";

         
            string tenAnh = rowView.Anh;
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
            if (listCart == null || listCart.Count == 0) return;
            if (e.RowIndex < 0) return;

            var dong = listCart[e.RowIndex];
            txtSelMaSP.Text = dong.MaSanPham.ToString();
            txtSelTenSP.Text = dong.TenSanPham;
            txtSelSoLuong.Text = dong.SoLuong.ToString();
            txtSelDonGia.Text = dong.DonGia.ToString();
            txtSelTinhTrang.Text = dong.TinhTrang;
            
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
            foreach (var row in listInvoiceDetails)
            {
                if (row.MaSanPham == maSP)
                {
                    slMua = row.SLMua;
                    slDaTra = row.DaTra;
                    break;
                }
            }

            // Tính số lượng còn lại được phép trả
            int slDuocPhepTra = slMua - slDaTra;

            // Bước 3: Kiểm tra xem sản phẩm này đã có trong giỏ hàng tạm chưa
            bool daCoTrong = false;
            foreach (var r in listCart)
            {
                if (r.MaSanPham == maSP)
                {
                    daCoTrong = true;
                    int slCu = r.SoLuong;
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
                    r.SoLuong = tongSLSauKhiThem;
                    r.TinhTrang = txtSelTinhTrang.Text.Trim();
                    r.TienHoan = r.SoLuong * r.DonGia;
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
                var newDetail = new ReturnDetail
                {
                    MaSanPham = maSP,
                    TenSanPham = txtSelTenSP.Text.Trim(),
                    SoLuong = soLuongTra,
                    DonGia = donGia,
                    TinhTrang = txtSelTinhTrang.Text.Trim(),
                    TienHoan = soLuongTra * donGia
                };
                listCart.Add(newDetail);
            }

            // Đảm bảo UI update
            listCart.ResetBindings();

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

          
            for (int i = listCart.Count - 1; i >= 0; i--)
            {
                if (listCart[i].MaSanPham == maSP)
                {
                    listCart.RemoveAt(i);
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

        // Hàm xử lý khi bấm nút "Hoàn tất & Trở về"
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (listCart == null || listCart.Count == 0)
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
        private async void btnLuuCT_Click(object sender, EventArgs e)
        {
            if (listCart == null || listCart.Count == 0)
            {
                MessageBox.Show("Danh sách sản phẩm trống, không có gì để lưu!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;

            try
            {
                decimal tongTienHoan = 0;
                foreach (var item in listCart)
                {
                    tongTienHoan += item.TienHoan;
                }

                string loaiGD = cboLoaiGiaoDich.Text.Trim();
                if (loaiGD == "Đổi hàng (1:1)")
                {
                    tongTienHoan = 0;
                }

                await _returnService.SaveReturnDetailsTransactionAsync(maTraHangHienTai, new List<ReturnDetail>(listCart), tongTienHoan, cboLoaiGiaoDich.Text.Trim());

                // Cập nhật UI
                txtTongTienHoan.Text = tongTienHoan.ToString("N0") + " đ";
                dtpNgayTra.Value = DateTime.Now;
                
                isCartModified = false;
                btnLuuCT.Enabled = false;

                MessageBox.Show("Lưu thay đổi thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NapDanhSachPhieu();
            }
            catch (Exception ex)
            {
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
                if (listInvoiceDetails == null) return;

                string tuKhoaTen = txtSelTenSP.Text.Trim().ToLower();
                string tuKhoaMa = txtSelMaSP.Text.Trim();
                string tuKhoaSL = txtSelSoLuong.Text.Trim();
                
                if (tuKhoaTen == "" && tuKhoaMa == "" && tuKhoaSL == "")
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var filteredList = new List<ReturnInvoiceProduct>(listInvoiceDetails);

                if (tuKhoaMa != "") 
                {
                    if (int.TryParse(tuKhoaMa, out int ma))
                        filteredList = filteredList.Where(x => x.MaSanPham == ma).ToList();
                }

                if (tuKhoaTen != "")
                {
                    filteredList = filteredList.Where(x => x.TenSanPham.ToLower().Contains(tuKhoaTen)).ToList();
                }

                if (tuKhoaSL != "")
                {
                    if (int.TryParse(tuKhoaSL, out int sl))
                    {
                        filteredList = filteredList.Where(x => x.DaTra == sl).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Số lượng trả phải là số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                dgvProductsSelection.DataSource = new BindingList<ReturnInvoiceProduct>(filteredList);

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

            if (listInvoiceDetails != null)
            {
                dgvProductsSelection.DataSource = listInvoiceDetails;
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
            if (listInvoiceDetails != null)
            {
                foreach (var row in listInvoiceDetails)
                {
                    if (row.MaSanPham == maSP)
                    {
                        slMua = row.SLMua;
                        daTra = row.DaTra;
                        break;
                    }
                }
            }
            int soLuongToiDa = slMua - daTra;
            
            if (listCart != null)
            {
                foreach (var r in listCart)
                {
                    if (r.MaSanPham == maSP)
                    {
                        if (soLuongTra > soLuongToiDa)
                        {
                            MessageBox.Show("Số lượng trả vượt quá giới hạn! Tối đa: " + soLuongToiDa, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        r.SoLuong = soLuongTra;
                        r.TinhTrang = txtSelTinhTrang.Text.Trim();
                        r.TienHoan = r.SoLuong * r.DonGia;
                        break;
                    }
                }
                listCart.ResetBindings();
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