using AssignmentApp.DAL.Core;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.BLL.Services.Warehouse;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
        #region 1. KHAI BÁO BI?N & KH?I T?O (DÙNG CHUNG)
    
        // B?ng d? li?u ch?a danh sách các phi?u tr? hàng
        BindingList<Return> listReturn;        
        // B?ng d? li?u ch?a danh sách chi ti?t s?n ph?m c?a hóa don g?c
        BindingList<ReturnInvoiceProduct> listInvoiceDetails; 
        // B?ng d? li?u dóng vai trò nhu 'Gi? hàng' ch?a các s?n ph?m du?c ch?n d? tr?
        BindingList<ReturnDetail> listCart;           
        
        private readonly IReturnService _returnService;
        private readonly IProductService _productService;

        // Bi?n luu tr? mã c?a Phi?u Tr? Hàng dang du?c thao tác (n?u = 0 nghia là chua ch?n phi?u nào)
        int maTraHangHienTai = 0;  
        // C? dánh d?u h? th?ng dang ? ch? d? Thêm m?i phi?u
        bool isAdding = false;   
        // C? dánh d?u h? th?ng dang ? ch? d? S?a phi?u
        bool isEditing = false;    
        // C? dánh d?u h? th?ng dang ? ch? d? Tìm ki?m phi?u (Tab 1)
        bool isSearching = false;    
        // C? dánh d?u xem gi? hàng dã b? thay d?i (thêm/s?a/xóa) hay chua
        bool isCartModified = false;
        // C? dánh d?u h? th?ng dang ? ch? d? Tìm ki?m s?n ph?m (Tab 2)
        bool isReturnSearching = false;

        // Hàm kh?i t?o (Constructor): Ðu?c g?i t? d?ng d?u tiên khi giao di?n (ucReturn) du?c t?o ra
        public ucReturn()
        {
            // L?nh b?t bu?c d? v? các thành ph?n giao di?n (nút, b?ng, ch?,...)
            InitializeComponent();
            
            if (Program.ServiceProvider != null)
                _returnService = Program.ServiceProvider.GetRequiredService<IReturnService>();
            if (Program.ServiceProvider != null)
                _productService = Program.ServiceProvider.GetRequiredService<IProductService>();
            
            // Ðang ký s? ki?n: Khi ngu?i dùng d?i ngày tr? hàng thì g?i hàm dtpNgayTra_ValueChanged
            dtpNgayTra.ValueChanged += dtpNgayTra_ValueChanged;
            // Ðang ký s? ki?n: Khi ngu?i dùng chuy?n qua l?i gi?a các Tab (Tab 1 và Tab 2) thì g?i hàm tabMain_Selecting
            tabMain.Selecting += tabMain_Selecting;
        }

        // Hàm x? lý s? ki?n: Khi d?i ngày tr? hàng
        private void dtpNgayTra_ValueChanged(object sender, EventArgs e)
        {
            // Ð?m b?o d?nh d?ng hi?n th? ngày tháng luôn là ki?u Short (Vd: 15/08/2026)
            if (dtpNgayTra.Format == DateTimePickerFormat.Custom)
            {
                dtpNgayTra.Format = DateTimePickerFormat.Short;
            }
        }

        // Hàm x? lý s? ki?n: Khi b?m ch?n chuy?n Tab
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // N?u ngu?i dùng d?nh b?m sang Tab 2 (Ch?n s?n ph?m) nhung chua có phi?u nào du?c ch?n (maTraHangHienTai == 0)
            if (e.TabPage == tabChonSanPham && maTraHangHienTai == 0)
            {
                e.Cancel = true; // H?y thao tác chuy?n Tab
                MessageBox.Show("Vui lòng ch?n ho?c t?o m?i m?t phi?u tr? tru?c khi chuy?n sang tab ch?n s?n ph?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm x? lý s? ki?n Load form: Ch?y 1 l?n duy nh?t khi giao di?n v?a m? lên
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }

        private void ucReturn_Load(object sender, EventArgs e)
        {
            KhoiTaoGioHang();      // Bu?c 1: T?o c?u trúc cho gi? hàng
            NapDanhSachPhieu();    // Bu?c 2: L?y d? li?u phi?u t? co s? d? li?u lên b?ng (Grid)
            SetTrangThaiBanDau();  // Bu?c 3: Ðua giao di?n v? tr?ng thái khóa/m?c d?nh ban d?u
        }

        // Hàm kh?i t?o gi? hàng (Ch? t?o c?u trúc các c?t, chua có d? li?u)
        private void KhoiTaoGioHang()
        {
            listCart = new BindingList<ReturnDetail>();

            // Liên k?t các c?t v?a t?o vào các c?t trên giao di?n DataGridView (B?ng bên ph?i ? Tab 2)
            colCurMaSP.DataPropertyName = "MaSanPham";
            colCurTenSP.DataPropertyName = "TenSanPham";
            colCurSoLuong.DataPropertyName = "SoLuong";
            colCurDonGia.DataPropertyName = "DonGia";
            colCurTinhTrang.DataPropertyName = "TinhTrang";
            
            // Note: DataGridView needs CellFormatting or a calculated property for ThanhTien if not mapped directly.
            // BindingList won't auto-calculate an expression column like DataTable did.
            // We will calculate it in code or DataGridView CellFormatting.
            colCurThanhTien.DataPropertyName = "TienHoan";

            dgvCurrentDetails.AutoGenerateColumns = false; // T?t t? d?ng sinh c?t d? dùng c?t mình t? c?u hình
            dgvCurrentDetails.DataSource = listCart;         // Ð? d? li?u c?a dtCart vào b?ng
            dgvCurrentDetails.AllowUserToAddRows = false;  // Không cho ngu?i dùng t? gõ thêm hàng tr?ng vào cu?i b?ng
        }

        #endregion

        #region 2. CÁC HÀM TI?N ÍCH & TR?NG THÁI (DÙNG CHUNG)

        // Hàm thi?t l?p tr?ng thái m?c d?nh ban d?u cho toàn b? giao di?n
        private void SetTrangThaiBanDau()
        {
            // T?t toàn b? các c? tr?ng thái
            isAdding = false;
            isEditing = false;
            isSearching = false;

            // Khóa các ô nh?p li?u ? Tab 1 d? ngan ngu?i dùng gõ linh tinh khi chua b?m nút
            KhoaONhapTab0(true);

            // B?t nút Thêm, t?t các nút s?a/xóa/luu (Vì chua ch?n phi?u nào thì không th? s?a xóa)
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;   
            btnRefresh.Enabled = true;

            // Khóa luôn các nút ? Tab 2
            KhoaTab1(true);
            
            // Xóa s?ch ch? ? các ô nh?p li?u
            XoaTrangTab0();
            XoaTrangTab1();
        }

      
        // Hàm thi?t l?p tr?ng thái khi ngu?i dùng dang Thêm ho?c S?a phi?u
        private void SetTrangThaiDangNhap()
        {
            // M? khóa các ô nh?p li?u d? ngu?i dùng gõ
            KhoaONhapTab0(false);

            // Khóa các nút Thêm, S?a, Xóa d? tránh xung d?t
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            // B?t nút Luu và B? qua
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        // Hàm ?n/hi?n (Khóa/m?) các ô nh?p li?u ? Tab 1
        private void KhoaONhapTab0(bool khoa)
        {
            // N?u khoa = true -> ReadOnly = true (Ch? d?c), Enabled = false (B? m? di)
            txtMaHoaDon.ReadOnly = khoa;
            txtMaHoaDon.Enabled = !khoa;
            txtLyDo.ReadOnly = khoa;
            txtLyDo.Enabled = !khoa;
            dtpNgayTra.Enabled = false; // Ngày tr? h? th?ng t? l?y ngày hi?n t?i nên luôn khóa
            cboTrangThai.Enabled = !khoa;
            cboLoaiGiaoDich.Enabled = !khoa;
            
            // T?ng ti?n, Khách hàng, Nhân viên là do h? th?ng t? tính/t? l?y, ngu?i dùng không du?c gõ
            txtTongTienHoan.ReadOnly = true;
            txtTongTienHoan.Enabled = false;
            txtKhachHang.ReadOnly = true;
            txtKhachHang.Enabled = false;
            txtNhanVien.ReadOnly = true;
            txtNhanVien.Enabled = false;
        }

      
        // Hàm khóa toàn b? các nút ch?c nang ? Tab 2
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

      
        // Hàm xóa s?ch n?i dung các ô nh?p ? Tab 1
        private void XoaTrangTab0()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0 d";
            dtpNgayTra.Value = DateTime.Now; // Reset v? ngày hi?n t?i
            dtpNgayTra.Format = DateTimePickerFormat.Short;

            cboTrangThai.SelectedIndex = 1; // M?c d?nh ch?n tr?ng thái ? dòng s? 2 (Ðang x? lý)

            cboLoaiGiaoDich.SelectedIndex = -1; // B? ch?n
            txtKhachHang.Text = "";
            txtNhanVien.Text = "";
            maTraHangHienTai = 0; // Xóa mã phi?u dang nh?
        }

       
        // Hàm xóa s?ch n?i dung các ô nh?p ? Tab 2
        private void XoaTrangTab1()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
            lblTotalAmount.Text = "T?NG TI?N HOÀN TR? T?M TÍNH: 0 d";
            lblProductDetailDesc.Text = "Mã SP: --\nThông tin chi ti?t v? s?n ph?m s? du?c c?p nh?t ? dây.";
            if (picAnh.Image != null)
            {
                picAnh.Image.Dispose();
                picAnh.Image = null;
            }

            // Xóa s?ch gi? hàng t?m
            if (listCart != null)
                listCart.Clear();

            // Xóa s?ch danh sách s?n ph?m hi?n th? c?a hóa don g?c
            if (listInvoiceDetails != null)
                listInvoiceDetails.Clear();

            lblReturnTitle.Text = "MÃ PHI?U: ";
        }

   
        // Hàm t?i toàn b? danh sách Phi?u tr? hàng t? SQL Database lên b?ng (Grid)
        private async void NapDanhSachPhieu()
        {
            if (_returnService == null) return;
            var returns = await _returnService.GetAllReturnsAsync();
            listReturn = new BindingList<Return>(new List<Return>(returns));

            // G?n các c?t d? li?u vào giao di?n b?ng (Grid)
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

    
        /// <summary>
        /// [NGHI?P V?] T?i danh sách các s?n ph?m (chi ti?t) thu?c m?t Hóa don c? th?.
        /// D? li?u này dùng d? ch?n dua vào gi? hàng tr? l?i.
        /// </summary>
        /// <param name="maHoaDon">Mã hóa don c?n l?y danh sách s?n ph?m</param>
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

     
        /// <summary>
        /// [TI?N ÍCH] Duy?t qua danh sách các s?n ph?m trong gi? hàng (listCart) 
        /// d? tính t?ng s? ti?n hoàn tr? t?m tính và c?p nh?t lên nhãn (Label) hi?n th?.
        /// </summary>
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
            lblTotalAmount.Text = "T?NG TI?N HOÀN TR? T?M TÍNH: " + tongTien.ToString("N0") + " d";
        }

        #endregion

        #region 3. TAB 1: QU?N LÝ PHI?U TR? HÀNG

        /// <summary>
        /// [S? KI?N GIAO DI?N] X? lý khi ngu?i dùng click vào m?t dòng trên b?ng Danh sách Phi?u tr? (Tab 1).
        /// - Ð?y d? li?u t? dòng du?c ch?n lên các ô nh?p li?u (TextBox, ComboBox).
        /// - Ch?n thao tác n?u h? th?ng dang ? ch? d? Thêm/S?a.
        /// </summary>
        private void dgvReturns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isAdding || isEditing)
            {
                MessageBox.Show("Ðang ? ch? d? nh?p li?u! Hãy Luu ho?c B? qua tru?c.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.RowIndex < 0 || listReturn == null || listReturn.Count == 0) return;

         
            var dong = listReturn[e.RowIndex];

            txtMaHoaDon.Text = dong.MaHoaDon.ToString();
            txtLyDo.Text = dong.LyDo;
            txtTongTienHoan.Text = dong.TongTienHoan.ToString("N0") + " d";
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
            lblReturnTitle.Text = "MÃ PHI?U: " + maTraHangHienTai;

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

          
            if (trangThai == "Hoàn thành" || trangThai == "Ðã h?y")
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
                MessageBox.Show("Mã hóa don ph?i là s? nguyên!", "C?nh báo",
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
                MessageBox.Show("Không tìm th?y hóa don s? " + maHD + "!", "C?nh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhanVien.Text = "(Không tìm th?y)";
                txtKhachHang.Text = "(Không tìm th?y)";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAdding = true;
            XoaTrangTab0();
            SetTrangThaiDangNhap();
            cboLoaiGiaoDich.Text = "Tr? hàng";
            txtMaHoaDon.Focus();
        }

       
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng ch?n m?t phi?u tr? t? danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;

            if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Ðã h?y")
            {
                MessageBox.Show("Phi?u tr? dã '" + cboTrangThai.Text + "', không th? s?a!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng ch?n lo?i giao d?ch!", "C?nh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            var r = new Return { MaTraHang = maTraHangHienTai, LyDo = txtLyDo.Text.Trim(), LoaiGiaoDich = cboLoaiGiaoDich.Text.Trim(), TrangThai = cboTrangThai.Text.Trim() };
            await _returnService.UpdateReturnAsync(r);

            MessageBox.Show("C?p nh?t phi?u tr? thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Vui lòng ch?n m?t phi?u tr? t? danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_returnService == null) return;

            if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Ðã h?y")
            {
                MessageBox.Show("Phi?u tr? dã '" + cboTrangThai.Text + "', không th? xóa!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Xác nh?n chuy?n tr?ng thái phi?u tr? #" + maTraHangHienTai + " thành 'Ðã h?y'?",
                "Xác nh?n h?y phi?u", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                var r = new Return { MaTraHang = maTraHangHienTai, LyDo = txtLyDo.Text.Trim(), LoaiGiaoDich = cboLoaiGiaoDich.Text.Trim(), TrangThai = "Ðã h?y" };
                await _returnService.UpdateReturnAsync(r);

                MessageBox.Show("H?y phi?u tr? thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("L?i khi h?y: " + ex.Message, "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NapDanhSachPhieu();
            SetTrangThaiBanDau();
        }

      
        // Hàm x? lý s? ki?n khi b?m nút Luu ? Tab 1 (Dùng d? T?o m?i phi?u tr?)
        /// <summary>
        /// [S? KI?N GIAO DI?N] Khi ngu?i dùng b?m nút LUU THAY Ð?I (Tab 1).
        /// - N?u dang Thêm M?i: T?o m?t th?c th? Return m?i và insert xu?ng Database, l?y mã v?a t?o và chuy?n sang Tab 2 d? nh?p chi ti?t.
        /// - N?u dang S?a: C?p nh?t thông tin phi?u (Lý do, Tr?ng thái, LoaiGiaoDich) xu?ng Database, sau dó c?p nh?t l?i Grid.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            // Bu?c 1: Ki?m tra d? li?u d?u vào (Validate)
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nh?p mã hóa don g?c!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            int maHD;
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out maHD))
            {
                MessageBox.Show("Mã hóa don ph?i là s? nguyên!", "C?nh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nh?p lý do tr? hàng!", "C?nh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng ch?n lo?i giao d?ch!", "C?nh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            // Bu?c 2: Ki?m tra phiên dang nh?p (Ai dang thao tác)
            if (AssignmentApp.BLL.Session.UserSession.CurrentUser == null)
            {
                MessageBox.Show("L?i: Không tìm th?y phiên dang nh?p!", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    "Ðã kh?i t?o phi?u tr? #" + newMaTraHang + " thành công!\n" +
                    "Vui lòng chuy?n sang Tab 'Ch?n s?n ph?m tr?' d? ti?p t?c.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Bu?c 5: C?p nh?t l?i giao di?n
                NapDanhSachPhieu(); // T?i l?i danh sách d? phi?u m?i hi?n lên grid
                SetTrangThaiBanDau(); // Reset form
                
                // Gi? nguyên phi?u v?a t?o d? thao tác ti?p
                maTraHangHienTai = newMaTraHang;
                cboTrangThai.Text = "Ðang x? lý";
                KhoaONhapTab0(false); // M? khóa các ô nh?p

                // Hi?n th? tiêu d? phi?u bên Tab 2
                lblReturnTitle.Text = "MÃ PHI?U: " + maTraHangHienTai;
                // T?i danh sách s?n ph?m thu?c hóa don này lên Tab 2
                NapSanPhamHoaDon(maHD); 
                KhoaTab1(false); // M? khóa các nút bên Tab 2

                // Bu?c 6: T? d?ng chuy?n sang Tab 2 d? ch?n s?n ph?m
                tabMain.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("L?i khi thêm phi?u: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
                lblReturnTitle.Text = "CH? Ð? TÌM KI?M";
                MessageBox.Show("Ðã chuy?n sang ch? d? tìm ki?m.\nVui lòng nh?p thông tin tìm ki?m vào các ô tuong ?ng và ?n TÌM KI?M l?n n?a!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                txtMaHoaDon.Focus();
                return;
            }

            string maHD = txtMaHoaDon.Text.Trim();
            string khach = txtKhachHang.Text.Trim();
            string nhanVien = txtNhanVien.Text.Trim();
            string lydo = txtLyDo.Text.Trim();
            string tongTienStr = txtTongTienHoan.Text.Replace(" d", "").Replace(",", "").Replace(".", "").Trim();

            bool isAnyFieldFilled = maHD != "" || khach != "" || nhanVien != "" || lydo != "" || 
                                    (tongTienStr != "" && tongTienStr != "0") || 
                                    cboTrangThai.SelectedIndex != -1 || 
                                    cboLoaiGiaoDich.SelectedIndex != -1 || 
                                    dtpNgayTra.Format != DateTimePickerFormat.Custom;

            if (!isAnyFieldFilled)
            {
                MessageBox.Show("Vui lòng nh?p ho?c ch?n ít nh?t m?t thông tin d? tìm ki?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Không tìm th?y phi?u tr? phù h?p!", "Thông báo",
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

        #region 4. TAB 2: QU?N LÝ CHI TI?T S?N PH?M TR?

        /// <summary>
        /// [S? KI?N GIAO DI?N] Khi ngu?i dùng ch?n m?t dòng bên danh sách s?n ph?m c?a hóa don g?c.
        /// - L?y thông tin s?n ph?m (Mã, Tên, Ðon giá).
        /// - Tìm và load ?nh s?n ph?m t? du?ng d?n tuy?t d?i (tránh file lock) ho?c t? thu m?c Resources d? phòng.
        /// - C?p nh?t n?i dung mô t? chi ti?t s?n ph?m và s? lu?ng t?i da có th? tr?.
        /// </summary>
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

            string imagePath = rowView.Anh;
            if (picAnh.Image != null)
            {
                picAnh.Image.Dispose();
                picAnh.Image = null;
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (System.IO.File.Exists(imagePath))
                {
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(imagePath);
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                        picAnh.Image = System.Drawing.Image.FromStream(ms);
                    }
                    catch { picAnh.Image = null; }
                }
                else
                {
                    // Fallback to Resources folder if it's just a filename
                    string duongDan1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", imagePath);
                    string duongDan2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", imagePath);
                    if (System.IO.File.Exists(duongDan1)) picAnh.Image = System.Drawing.Image.FromFile(duongDan1);
                    else if (System.IO.File.Exists(duongDan2)) picAnh.Image = System.Drawing.Image.FromFile(duongDan2);
                }
            }

            lblProductDetailDesc.Text = $"Mã s?n ph?m: {rowView.MaSanPham}\n" +
                                        $"Tên s?n ph?m: {rowView.TenSanPham ?? "Không rõ"}\n" +
                                        $"Ðon giá mua: {rowView.DonGia.ToString("N0")} VNÐ\n" +
                                        $"S? lu?ng dã mua: {rowView.SLMua} | S? lu?ng dã tr? tru?c dây: {rowView.DaTra}\n" +
                                        $"B?n có th? tr? t?i da: {rowView.SLMua - rowView.DaTra} s?n ph?m n?a.";

            tabSelectionContainer.SelectedIndex = 1;
            
            // State management
            if (maTraHangHienTai != 0)
            {
                if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Ðã h?y")
                {
                    MessageBox.Show("Phi?u tr? dã '" + cboTrangThai.Text + "', không th? thêm s?n ph?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            
            int slMua = 0;
            int slDaTra = 0;
            if (listInvoiceDetails != null)
            {
                foreach (var row in listInvoiceDetails)
                {
                    if (row.MaSanPham == dong.MaSanPham)
                    {
                        slMua = row.SLMua;
                        slDaTra = row.DaTra;
                        
                        string imagePath = row.Anh;
                        if (picAnh.Image != null)
                        {
                            picAnh.Image.Dispose();
                            picAnh.Image = null;
                        }

                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            if (System.IO.File.Exists(imagePath))
                            {
                                try
                                {
                                    byte[] bytes = System.IO.File.ReadAllBytes(imagePath);
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                                    picAnh.Image = System.Drawing.Image.FromStream(ms);
                                }
                                catch { picAnh.Image = null; }
                            }
                            else
                            {
                                string duongDan1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", imagePath);
                                string duongDan2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", imagePath);
                                if (System.IO.File.Exists(duongDan1)) picAnh.Image = System.Drawing.Image.FromFile(duongDan1);
                                else if (System.IO.File.Exists(duongDan2)) picAnh.Image = System.Drawing.Image.FromFile(duongDan2);
                            }
                        }
                        break;
                    }
                }
            }

            lblProductDetailDesc.Text = $"Mã s?n ph?m: {dong.MaSanPham}\n" +
                                        $"Tên s?n ph?m: {dong.TenSanPham ?? "Không rõ"}\n" +
                                        $"Ðon giá mua: {dong.DonGia.ToString("N0")} VNÐ\n" +
                                        $"S? lu?ng dã mua: {slMua} | S? lu?ng dã tr? tru?c dây: {slDaTra}\n" +
                                        $"B?n có th? tr? t?i da: {slMua - slDaTra} s?n ph?m n?a.";

            tabSelectionContainer.SelectedIndex = 1;
            
            // State management
            if (maTraHangHienTai != 0)
            {
                if (cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Ðã h?y")
                {
                    MessageBox.Show("Phi?u tr? dã '" + cboTrangThai.Text + "', không th? s?a ho?c xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

     
        // Hàm x? lý khi b?m nút "Thêm vào gi?" ? Tab 2
        /// <summary>
        /// [S? KI?N GIAO DI?N] Khi ngu?i dùng b?m nút THÊM s?n ph?m vào gi? hàng tr? (Tab 2).
        /// - Validate (Ki?m tra) s? lu?ng nh?p vào (ph?i là s? h?p l?, > 0).
        /// - Ki?m tra xem s? lu?ng mu?n tr? có vu?t quá (S? lu?ng dã mua - S? lu?ng dã tr? tru?c dó) hay không.
        /// - C?p nh?t gi? hàng: N?u dã có thì c?ng d?n s? lu?ng, n?u chua thì thêm dòng m?i.
        /// </summary>
        private async void btnAddToCart_Click(object sender, EventArgs e)
        {
            // Bu?c 1: Ki?m tra tính h?p l?
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng ch?n m?t s?n ph?m t? danh sách hóa don!",
                    "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text.Trim());
            int soLuongTra = 0;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuongTra) || soLuongTra <= 0)
            {
                MessageBox.Show("S? lu?ng tr? ph?i l?n hon 0!",
                    "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            decimal donGia = decimal.Parse(txtSelDonGia.Text.Trim());

            // Bu?c 2: L?y s? lu?ng dã mua và dã tr? tru?c dó d? d?i chi?u
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

            // Tính s? lu?ng còn l?i du?c phép tr?
            int slDuocPhepTra = slMua - slDaTra;

            // Bu?c 3: Ki?m tra xem s?n ph?m này dã có trong gi? hàng t?m chua
            bool daCoTrong = false;
            foreach (var r in listCart)
            {
                if (r.MaSanPham == maSP)
                {
                    daCoTrong = true;
                    int slCu = r.SoLuong;
                    int tongSLSauKhiThem = slCu + soLuongTra;

                    // N?u t?ng s? lu?ng dòi tr? vu?t quá s? lu?ng du?c phép tr? -> Báo l?i
                    if (tongSLSauKhiThem > slDuocPhepTra)
                    {
                        MessageBox.Show("T?ng s? lu?ng tr? (" + tongSLSauKhiThem +
                            ") vu?t quá s? lu?ng du?c phép tr? (" + slDuocPhepTra + ")!",
                            "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    // C?p nh?t s? lu?ng và tình tr?ng m?i
                    r.SoLuong = tongSLSauKhiThem;
                    r.TinhTrang = txtSelTinhTrang.Text.Trim();
                    r.TienHoan = r.SoLuong * r.DonGia;
                    break;
                }
            }

            // N?u chua có trong gi? mà s? lu?ng dòi tr? l?n hon s? du?c phép -> Báo l?i
            if (!daCoTrong && soLuongTra > slDuocPhepTra)
            {
                MessageBox.Show("S? lu?ng tr? vu?t quá s? lu?ng du?c phép tr? (" + slDuocPhepTra + ")!",
                    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bu?c 4: Thêm vào gi? hàng n?u h?p l?
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

            // Ð?m b?o UI update
            listCart.ResetBindings();

            // B?t nút Luu thay d?i
            btnLuuCT.Enabled = true;
            // Tính l?i t?ng ti?n
            TinhTongTienHoanTra();
            // Xóa tr?ng form nh?p
            XoaTrangTab1SanPham();
            
            // Khóa các nút d? ch? thao tác ti?p theo
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Ðã h?y")
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
                MessageBox.Show("Vui lòng ch?n s?n ph?m c?n xóa t? danh sách hàng tr? l?i!",
                    "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa s?n ph?m này kh?i danh sách tr??", "Xác nh?n",
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
            
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Ðã h?y")
            {
                btnAddToCart.Enabled = false;
                btnSuaCT.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBoquaCT.Enabled = false;
            }
        }

        // Hàm x? lý khi b?m nút "Hoàn t?t & Tr? v?"
        /// <summary>
        /// [S? KI?N GIAO DI?N] Khi ngu?i dùng b?m nút LUU CHI TI?T PHI?U (Tab 2).
        /// - Xóa toàn b? chi ti?t cu (n?u có) c?a phi?u dang thao tác.
        /// - L?p qua gi? hàng (listCart) và luu t?ng s?n ph?m vào b?ng ChiTietTraHang.
        /// - K?t thúc quá trình s?a chi ti?t, reload l?i danh sách bên Tab 1 và làm m?i giao di?n.
        /// </summary>
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (listCart == null || listCart.Count == 0)
            {
                MessageBox.Show("Danh sách hàng tr? l?i dang tr?ng! Vui lòng thêm s?n ph?m.",
                    "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maTraHangHienTai == 0)
            {
                MessageBox.Show("Chua có phi?u tr?! Vui lòng hoàn t?t Tab 1 tru?c.", "C?nh báo",
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
            lblProductDetailDesc.Text = "Mã SP: --\nThông tin chi ti?t v? s?n ph?m s? du?c c?p nh?t ? dây.";
            if (picAnh.Image != null)
            {
                picAnh.Image.Dispose();
                picAnh.Image = null;
            }
        }

        
        // Hàm luu toàn b? gi? hàng vào Database
        private async void btnLuuCT_Click(object sender, EventArgs e)
        {
            if (listCart == null || listCart.Count == 0)
            {
                MessageBox.Show("Danh sách s?n ph?m tr?ng, không có gì d? luu!",
                    "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (loaiGD == "Ð?i hàng (1:1)")
                {
                    tongTienHoan = 0;
                }

                await _returnService.SaveReturnDetailsTransactionAsync(maTraHangHienTai, new List<ReturnDetail>(listCart), tongTienHoan, cboLoaiGiaoDich.Text.Trim());

                // C?p nh?t UI
                txtTongTienHoan.Text = tongTienHoan.ToString("N0") + " d";
                dtpNgayTra.Value = DateTime.Now;
                
                isCartModified = false;
                btnLuuCT.Enabled = false;

                MessageBox.Show("Luu thay d?i thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NapDanhSachPhieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có l?i khi luu d? li?u:\n" + ex.Message, "L?i h? th?ng",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturnSearch_Click(object sender, EventArgs e)
        {
            if (maTraHangHienTai == 0 || cboTrangThai.Text == "Hoàn thành" || cboTrangThai.Text == "Ðã h?y")
            {
                MessageBox.Show("Vui lòng ch?n m?t phi?u dang x? lý tru?c khi tìm ki?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show("Vui lòng nh?p Mã, Tên s?n ph?m ho?c S? lu?ng dã tr? vào ô tuong ?ng bên ph?i, sau dó ?n TÌM KI?M l?n n?a!", "Hu?ng d?n", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Vui lòng nh?p thông tin tìm ki?m!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("S? lu?ng tr? ph?i là s? nguyên!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                dgvProductsSelection.DataSource = new BindingList<ReturnInvoiceProduct>(filteredList);

                if (dgvProductsSelection.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm th?y s?n ph?m nào phù h?p trong hóa don!", "K?t qu? tìm ki?m", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Ðã h?y")
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

        private async void btnSuaCT_Click(object sender, EventArgs e) 
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng ch?n s?n ph?m c?n s?a t? gi? hàng!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int soLuongTra;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuongTra) || soLuongTra <= 0)
            {
                MessageBox.Show("S? lu?ng tr? ph?i là s? nguyên duong!", "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            
            if (cboLoaiGiaoDich.Text.Trim() == "Ð?i hàng")
            {
                if (_productService != null)
                {
                    var product = await _productService.GetProductByIdAsync(maSP);
                    if (product == null || product.SoLuongTon < soLuongTra)
                    {
                        MessageBox.Show($"Trong kho ch? còn {product?.SoLuongTon ?? 0} s?n ph?m m?i d? d?i. Không d? s? lu?ng!", "C?nh báo t?n kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
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
                            MessageBox.Show("S? lu?ng tr? vu?t quá gi?i h?n! T?i da: " + soLuongToiDa, "C?nh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Ðã h?y")
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
            if (maTraHangHienTai != 0 && cboTrangThai.Text != "Hoàn thành" && cboTrangThai.Text != "Ðã h?y")
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

