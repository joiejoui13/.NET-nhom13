using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturns : UserControl
    {
        // Khai báo cấu trúc lưu trữ thông tin sản phẩm tạm thời để tính toán
        private class TempInvoiceItem
        {
            public string MaSanPham { get; set; } = string.Empty;
            public string TenSanPham { get; set; } = string.Empty;
            public int SoLuongMua { get; set; }
            public int DaTra { get; set; }
            public decimal DonGia { get; set; }
        }

        // Programmatic controls for Tab 3 (Exchange POS)
        private TabPage tabExchangePOS = null!;
        private Label lblOffsetRefund = null!;
        private DataGridView dgvExchangeCart = null!;
        private Label lblExchangeSubtotal = null!;
        private Label lblExchangeOffset = null!;
        private Label lblExchangeTotal = null!;
        private ComboBox cboExchangePaymentMethod = null!;
        private Button btnExchangePay = null!;
        private TextBox txtExchangeSearch = null!;
        private ComboBox cboExchangeCategory = null!;
        private DataGridView dgvExchangeProducts = null!;

        public class ExchangeCartItem
        {
            public string MaSanPham { get; set; } = "";
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien => SoLuong * DonGia;
        }

        public class MockExchangeProduct
        {
            public string MaSanPham { get; set; } = "";
            public string TenSanPham { get; set; } = "";
            public string DanhMuc { get; set; } = "";
            public double DonGia { get; set; }
            public int TonKho { get; set; }
        }

        private double exchangeOffsetAmount = 0;
        private List<ExchangeCartItem> exchangeCart = new List<ExchangeCartItem>();
        private List<MockExchangeProduct> exchangeProducts = new List<MockExchangeProduct>();

        public ucReturns()
        {
            InitializeComponent();
        }

        private void ucReturns_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho giao diện khi tải
            txtMaTraHang.ReadOnly = true;
            txtTongTienHoan.ReadOnly = true;
            dtpNgayTra.Value = DateTime.Now;

            // Đổ dữ liệu vào các ComboBox nghiệp vụ
            cboLoaiGiaoDich.Items.Clear();
            cboLoaiGiaoDich.Items.AddRange(new object[] { "Trả hàng", "Đổi hàng" });
            cboLoaiGiaoDich.SelectedIndex = 0;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Hoàn thành", "Chờ xử lý", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;

            // Thiết lập Grid hiển thị danh sách sản phẩm trả ở chế độ chỉ đọc trên Grid, 
            // việc sửa đổi số lượng và tình trạng sẽ được làm thông qua Panel nhập chi tiết ở phía dưới.
            dgvReturnDetails.ReadOnly = true; 

            // Cấu hình ban đầu cho các ô nhập chi tiết sản phẩm
            ResetDetailInputFields();

            // Gán dữ liệu hiển thị mẫu ban đầu
            lblKhachHang.Text = "Khách hàng: (Chờ nhập hóa đơn...)";
            lblNhanVien.Text = "Nhân viên: Thu Ngân 1 (Mẫu)";
            txtTongTienHoan.Text = "0";

            // Khởi tạo tab đổi hàng mới (POS)
            InitializeExchangePOSTab();
            InitializeExchangeProducts();
            LoadExchangeProductsGrid();
        }

        private void InitializeExchangeProducts()
        {
            exchangeProducts.Clear();
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "1", TenSanPham = "Bút bi Thiên Long TL-027 Xanh", DanhMuc = "Bút các loại", DonGia = 5000, TonKho = 1000 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "2", TenSanPham = "Bút dạ quang Deli Macaron", DanhMuc = "Bút các loại", DonGia = 12000, TonKho = 300 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "3", TenSanPham = "Bút máy Hồng Hà Nét Hoa", DanhMuc = "Bút các loại", DonGia = 45000, TonKho = 150 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "4", TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", DanhMuc = "Sổ - Vở", DonGia = 9000, TonKho = 800 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "5", TenSanPham = "Vở ô ly Campus 96 trang", DanhMuc = "Sổ - Vở", DonGia = 12000, TonKho = 500 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "7", TenSanPham = "Giấy in Double A A4 70gsm", DanhMuc = "Giấy in - photo", DonGia = 80000, TonKho = 200 });
            exchangeProducts.Add(new MockExchangeProduct { MaSanPham = "12", TenSanPham = "Máy tính Casio FX-580VN X", DanhMuc = "Máy tính cầm tay", DonGia = 680000, TonKho = 50 });
        }

        private void InitializeExchangePOSTab()
        {
            // 1. Create TabPage
            tabExchangePOS = new TabPage();
            tabExchangePOS.Text = "Chọn hàng đổi mới";
            tabExchangePOS.BackColor = Color.FromArgb(240, 242, 245);
            tabReturnContainer.TabPages.Add(tabExchangePOS);

            // 2. Main layout: split left and right panels
            Panel pnlLeft = new Panel();
            pnlLeft.Width = 380;
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.BackColor = Color.White;
            pnlLeft.Padding = new Padding(15);
            tabExchangePOS.Controls.Add(pnlLeft);

            Panel pnlRight = new Panel();
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.BackColor = Color.FromArgb(240, 242, 245);
            pnlRight.Padding = new Padding(15);
            tabExchangePOS.Controls.Add(pnlRight);
            
            // Bring Left Panel to Front so Right panel docks to what remains
            pnlLeft.BringToFront();

            // ==========================================
            // LEFT PANEL CONTROLS (CART & PAYMENTS)
            // ==========================================
            lblOffsetRefund = new Label();
            lblOffsetRefund.Text = "Số tiền cấn trừ hàng cũ: -0 đ";
            lblOffsetRefund.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOffsetRefund.ForeColor = Color.FromArgb(220, 38, 38); // Red
            lblOffsetRefund.Dock = DockStyle.Top;
            lblOffsetRefund.Height = 30;
            pnlLeft.Controls.Add(lblOffsetRefund);

            Label lblCartTitle = new Label();
            lblCartTitle.Text = "GIỎ HÀNG ĐỔI MỚI";
            lblCartTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCartTitle.ForeColor = Color.FromArgb(0, 126, 249);
            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Height = 25;
            pnlLeft.Controls.Add(lblCartTitle);

            // DataGridView dgvExchangeCart
            dgvExchangeCart = new DataGridView();
            dgvExchangeCart.Dock = DockStyle.Fill;
            dgvExchangeCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExchangeCart.AllowUserToAddRows = false;
            dgvExchangeCart.ReadOnly = false;
            dgvExchangeCart.RowHeadersVisible = false;
            dgvExchangeCart.BackgroundColor = Color.White;
            dgvExchangeCart.BorderStyle = BorderStyle.None;
            dgvExchangeCart.CellClick += dgvExchangeCart_CellClick;
            dgvExchangeCart.CellValueChanged += dgvExchangeCart_CellValueChanged;
            pnlLeft.Controls.Add(dgvExchangeCart);

            // Configure Cart Columns
            dgvExchangeCart.Columns.Add("colExCartMaSP", "Mã SP");
            dgvExchangeCart.Columns["colExCartMaSP"].ReadOnly = true;
            dgvExchangeCart.Columns["colExCartMaSP"].Width = 50;

            dgvExchangeCart.Columns.Add("colExCartTenSP", "Tên sản phẩm");
            dgvExchangeCart.Columns["colExCartTenSP"].ReadOnly = true;

            dgvExchangeCart.Columns.Add("colExCartSoLuong", "SL");
            dgvExchangeCart.Columns["colExCartSoLuong"].Width = 40;

            dgvExchangeCart.Columns.Add("colExCartDonGia", "Đơn giá");
            dgvExchangeCart.Columns["colExCartDonGia"].ReadOnly = true;
            dgvExchangeCart.Columns["colExCartDonGia"].Width = 70;

            dgvExchangeCart.Columns.Add("colExCartThanhTien", "Thành tiền");
            dgvExchangeCart.Columns["colExCartThanhTien"].ReadOnly = true;
            dgvExchangeCart.Columns["colExCartThanhTien"].Width = 85;

            DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
            colDelete.Name = "colExCartDelete";
            colDelete.HeaderText = "Xóa";
            colDelete.Text = "X";
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Width = 35;
            dgvExchangeCart.Columns.Add(colDelete);

            // Bottom calculations panel in Left panel
            Panel pnlExCalc = new Panel();
            pnlExCalc.Dock = DockStyle.Bottom;
            pnlExCalc.Height = 220;
            pnlExCalc.Padding = new Padding(0, 10, 0, 0);
            pnlLeft.Controls.Add(pnlExCalc);
            
            // Make sure the calculation panel is placed below grid
            pnlExCalc.BringToFront();
            dgvExchangeCart.BringToFront(); // Grid takes remaining space

            lblExchangeSubtotal = new Label();
            lblExchangeSubtotal.Text = "Tạm tính hàng mới: 0 đ";
            lblExchangeSubtotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblExchangeSubtotal.Location = new Point(5, 15);
            lblExchangeSubtotal.Size = new Size(340, 20);
            pnlExCalc.Controls.Add(lblExchangeSubtotal);

            Label lblOffsetTitle = new Label();
            lblOffsetTitle.Text = "Khấu trừ hàng cũ:";
            lblOffsetTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblOffsetTitle.Location = new Point(5, 40);
            lblOffsetTitle.Size = new Size(130, 20);
            pnlExCalc.Controls.Add(lblOffsetTitle);

            lblExchangeOffset = new Label();
            lblExchangeOffset.Text = "- 0 đ";
            lblExchangeOffset.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblExchangeOffset.ForeColor = Color.FromArgb(220, 38, 38);
            lblExchangeOffset.Location = new Point(160, 40);
            lblExchangeOffset.Size = new Size(185, 20);
            lblExchangeOffset.TextAlign = ContentAlignment.MiddleRight;
            pnlExCalc.Controls.Add(lblExchangeOffset);

            lblExchangeTotal = new Label();
            lblExchangeTotal.Text = "TỔNG THANH TOÁN: 0 đ";
            lblExchangeTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExchangeTotal.ForeColor = Color.FromArgb(22, 163, 74); // Green
            lblExchangeTotal.Location = new Point(5, 70);
            lblExchangeTotal.Size = new Size(340, 25);
            pnlExCalc.Controls.Add(lblExchangeTotal);

            Label lblPaymentMethod = new Label();
            lblPaymentMethod.Text = "Hình thức thanh toán chênh lệch:";
            lblPaymentMethod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblPaymentMethod.Location = new Point(5, 110);
            lblPaymentMethod.Size = new Size(200, 20);
            pnlExCalc.Controls.Add(lblPaymentMethod);

            cboExchangePaymentMethod = new ComboBox();
            cboExchangePaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboExchangePaymentMethod.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản" });
            cboExchangePaymentMethod.SelectedIndex = 0;
            cboExchangePaymentMethod.Location = new Point(210, 107);
            cboExchangePaymentMethod.Size = new Size(135, 23);
            pnlExCalc.Controls.Add(cboExchangePaymentMethod);

            btnExchangePay = new Button();
            btnExchangePay.Text = "XÁC NHẬN ĐỔI HÀNG";
            btnExchangePay.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnExchangePay.BackColor = Color.FromArgb(22, 163, 74);
            btnExchangePay.ForeColor = Color.White;
            btnExchangePay.FlatStyle = FlatStyle.Flat;
            btnExchangePay.FlatAppearance.BorderSize = 0;
            btnExchangePay.Location = new Point(5, 150);
            btnExchangePay.Size = new Size(340, 45);
            btnExchangePay.Click += btnExchangePay_Click;
            pnlExCalc.Controls.Add(btnExchangePay);

            // ==========================================
            // RIGHT PANEL CONTROLS (PRODUCT LIST & FILTERS)
            // ==========================================
            Panel pnlRightFilters = new Panel();
            pnlRightFilters.Dock = DockStyle.Top;
            pnlRightFilters.Height = 50;
            pnlRight.Controls.Add(pnlRightFilters);

            txtExchangeSearch = new TextBox();
            txtExchangeSearch.PlaceholderText = "Nhập tên sản phẩm để tìm...";
            txtExchangeSearch.Location = new Point(5, 10);
            txtExchangeSearch.Size = new Size(250, 23);
            txtExchangeSearch.KeyDown += txtExchangeSearch_KeyDown;
            pnlRightFilters.Controls.Add(txtExchangeSearch);

            Button btnSearchProd = new Button();
            btnSearchProd.Text = "TÌM KIẾM";
            btnSearchProd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearchProd.BackColor = Color.FromArgb(0, 126, 249);
            btnSearchProd.ForeColor = Color.White;
            btnSearchProd.FlatStyle = FlatStyle.Flat;
            btnSearchProd.FlatAppearance.BorderSize = 0;
            btnSearchProd.Location = new Point(265, 8);
            btnSearchProd.Size = new Size(90, 27);
            btnSearchProd.Click += btnSearchProd_Click;
            pnlRightFilters.Controls.Add(btnSearchProd);

            cboExchangeCategory = new ComboBox();
            cboExchangeCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboExchangeCategory.Items.AddRange(new object[] {
                "Tất cả danh mục", "Bút các loại", "Sổ - Vở", "Giấy in - photo", "Bìa - File hồ sơ", "Dụng cụ học sinh", "Đồ dùng văn phòng", "Máy tính cầm tay"
            });
            cboExchangeCategory.SelectedIndex = 0;
            cboExchangeCategory.Location = new Point(370, 10);
            cboExchangeCategory.Size = new Size(160, 23);
            cboExchangeCategory.SelectedIndexChanged += cboExchangeCategory_SelectedIndexChanged;
            pnlRightFilters.Controls.Add(cboExchangeCategory);

            // DataGridView dgvExchangeProducts
            dgvExchangeProducts = new DataGridView();
            dgvExchangeProducts.Dock = DockStyle.Fill;
            dgvExchangeProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExchangeProducts.AllowUserToAddRows = false;
            dgvExchangeProducts.ReadOnly = true;
            dgvExchangeProducts.RowHeadersVisible = false;
            dgvExchangeProducts.BackgroundColor = Color.White;
            dgvExchangeProducts.BorderStyle = BorderStyle.None;
            dgvExchangeProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExchangeProducts.CellDoubleClick += dgvExchangeProducts_CellDoubleClick;
            pnlRight.Controls.Add(dgvExchangeProducts);

            // Configure Products Columns
            dgvExchangeProducts.Columns.Add("colExProdMaSP", "Mã SP");
            dgvExchangeProducts.Columns["colExProdMaSP"].Width = 50;

            dgvExchangeProducts.Columns.Add("colExProdTenSP", "Tên sản phẩm");
            
            dgvExchangeProducts.Columns.Add("colExProdDanhMuc", "Danh mục");
            dgvExchangeProducts.Columns["colExProdDanhMuc"].Width = 100;

            dgvExchangeProducts.Columns.Add("colExProdGiaBan", "Đơn giá");
            dgvExchangeProducts.Columns["colExProdGiaBan"].Width = 80;

            dgvExchangeProducts.Columns.Add("colExProdTonKho", "Tồn kho");
            dgvExchangeProducts.Columns["colExProdTonKho"].Width = 70;

            // Make sure grid displays below filters
            dgvExchangeProducts.BringToFront();
        }

        private void LoadExchangeProductsGrid(List<MockExchangeProduct>? dataSource = null)
        {
            dgvExchangeProducts.Rows.Clear();
            var list = dataSource ?? exchangeProducts;
            foreach (var prod in list)
            {
                dgvExchangeProducts.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.DanhMuc,
                    prod.DonGia.ToString("N0") + " đ",
                    prod.TonKho.ToString("N0")
                );
            }
        }

        private void LoadExchangeCartGrid()
        {
            dgvExchangeCart.Rows.Clear();
            foreach (var item in exchangeCart)
            {
                dgvExchangeCart.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
        }

        private void RecalculateExchangeTotals()
        {
            double subtotal = exchangeCart.Sum(i => i.ThanhTien);
            double total = subtotal - exchangeOffsetAmount;

            lblExchangeSubtotal.Text = $"Tạm tính hàng mới: {subtotal.ToString("N0")} đ";
            lblExchangeOffset.Text = $"- {exchangeOffsetAmount.ToString("N0")} đ";

            if (total == 0)
            {
                lblExchangeTotal.Text = "TỔNG THANH TOÁN: 0 đ (Đổi ngang)";
                lblExchangeTotal.ForeColor = Color.Black;
            }
            else if (total > 0)
            {
                lblExchangeTotal.Text = $"THU THÊM KHÁCH: {total.ToString("N0")} đ";
                lblExchangeTotal.ForeColor = Color.FromArgb(22, 163, 74); // Green
            }
            else
            {
                lblExchangeTotal.Text = $"THỐI LẠI KHÁCH: {Math.Abs(total).ToString("N0")} đ";
                lblExchangeTotal.ForeColor = Color.FromArgb(220, 38, 38); // Red
            }
        }

        private void dgvReturns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            MessageBox.Show("Hiển thị chi tiết phiếu trả hàng đã chọn trong lịch sử.", "Xem lịch sử", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset các control nhập liệu trên UI
            txtMaHoaDon.Text = "";
            txtMaTraHang.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0";
            lblKhachHang.Text = "Khách hàng: (Chờ nhập hóa đơn...)";
            cboLoaiGiaoDich.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            dgvReturnDetails.Rows.Clear();
            ResetDetailInputFields();
            txtMaHoaDon.Focus();

            exchangeCart.Clear();
            LoadExchangeCartGrid();
            exchangeOffsetAmount = 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Không hỗ trợ chỉnh sửa trực tiếp phiếu trả hàng đã hoàn tất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Không hỗ trợ xóa phiếu trả để bảo toàn lịch sử kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn gốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Giao dịch trả hàng đã được xác nhận thành công (Mô phỏng)!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn gốc và chọn hàng trả trước khi đổi hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtTongTienHoan.Text, out double refundAmt) || refundAmt <= 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm cần trả với số lượng hợp lệ (>0) để cấn trừ tiền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            exchangeOffsetAmount = refundAmt;
            lblOffsetRefund.Text = $"Số tiền cấn trừ hàng cũ: -{exchangeOffsetAmount.ToString("N0")} đ";
            lblExchangeOffset.Text = $"- {exchangeOffsetAmount.ToString("N0")} đ";

            // Switch to the newly created exchange POS tab
            tabReturnContainer.SelectedTab = tabExchangePOS;
            
            // Clear previous exchange cart
            exchangeCart.Clear();
            LoadExchangeCartGrid();
            RecalculateExchangeTotals();

            MessageBox.Show($"Đã tạo phiếu cấn trừ hàng cũ: {exchangeOffsetAmount.ToString("N0")} đ.\nTự động chuyển sang tab 'Chọn hàng đổi mới' để chọn hàng.", "Đổi hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã làm mới danh sách lịch sử phiếu trả hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FetchInvoiceDetailsStub();
        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            FetchInvoiceDetailsStub();
        }

        private void FetchInvoiceDetailsStub()
        {
            string maHoaDon = txtMaHoaDon.Text.Trim();
            if (string.IsNullOrEmpty(maHoaDon)) return;

            lblKhachHang.Text = $"Khách hàng: Nguyễn Văn A (Mẫu)";
            txtLyDo.PlaceholderText = "Nhập lý do trả hàng...";

            dgvReturnDetails.Rows.Clear();
            ResetDetailInputFields();

            // Mẫu sản phẩm 1
            int r1 = dgvReturnDetails.Rows.Add();
            var row1 = dgvReturnDetails.Rows[r1];
            row1.Cells["colReturnDetailMaSP"].Value = "1";
            row1.Cells["colReturnDetailTenSP"].Value = "Bút bi Thiên Long TL-027 Xanh";
            row1.Cells["colReturnDetailSoLuongMua"].Value = 5;
            row1.Cells["colReturnDetailDaTra"].Value = 1;
            row1.Cells["colReturnDetailSoLuong"].Value = 0;
            row1.Cells["colReturnDetailDonGia"].Value = 5000;
            row1.Cells["colReturnDetailTienHoan"].Value = 0;
            row1.Cells["colReturnDetailTinhTrang"].Value = "";
            row1.Tag = new TempInvoiceItem { MaSanPham = "1", TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuongMua = 5, DaTra = 1, DonGia = 5000 };

            // Mẫu sản phẩm 2
            int r2 = dgvReturnDetails.Rows.Add();
            var row2 = dgvReturnDetails.Rows[r2];
            row2.Cells["colReturnDetailMaSP"].Value = "4";
            row2.Cells["colReturnDetailTenSP"].Value = "Vở kẻ ngang Hồng Hà 72 trang";
            row2.Cells["colReturnDetailSoLuongMua"].Value = 10;
            row2.Cells["colReturnDetailDaTra"].Value = 0;
            row2.Cells["colReturnDetailSoLuong"].Value = 0;
            row2.Cells["colReturnDetailDonGia"].Value = 9000;
            row2.Cells["colReturnDetailTienHoan"].Value = 0;
            row2.Cells["colReturnDetailTinhTrang"].Value = "";
            row2.Tag = new TempInvoiceItem { MaSanPham = "4", TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuongMua = 10, DaTra = 0, DonGia = 9000 };

            tabReturnContainer.SelectedTab = tabReturnDetail;
        }

        private void dgvReturnDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvReturnDetails.Rows[e.RowIndex];
            var item = row.Tag as TempInvoiceItem;
            if (item != null)
            {
                // Load dữ liệu của sản phẩm đang chọn vào các ô nhập chi tiết phía dưới
                lblDetailProductName.Text = $"Sản phẩm đang chọn: {item.TenSanPham} (Mã SP: {item.MaSanPham})";
                lblDetailQtyInfo.Text = $"Số lượng đã mua: {item.SoLuongMua} | Đã trả ở các phiếu cũ: {item.DaTra}";
                
                txtDetailSoLuongTra.Text = row.Cells["colReturnDetailSoLuong"].Value?.ToString() ?? "0";
                txtDetailTinhTrang.Text = row.Cells["colReturnDetailTinhTrang"].Value?.ToString() ?? "";
                
                txtDetailSoLuongTra.Enabled = true;
                txtDetailTinhTrang.Enabled = true;
                btnUpdateDetail.Enabled = true;
            }
        }

        private void btnUpdateDetail_Click(object sender, EventArgs e)
        {
            if (dgvReturnDetails.CurrentRow == null) return;

            var row = dgvReturnDetails.CurrentRow;
            var item = row.Tag as TempInvoiceItem;
            if (item == null) return;

            string qtyStr = txtDetailSoLuongTra.Text.Trim();
            if (!int.TryParse(qtyStr, out int qty) || qty < 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên lớn hơn hoặc bằng 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDetailSoLuongTra.Focus();
                return;
            }

            int maxQty = item.SoLuongMua - item.DaTra;
            if (qty > maxQty)
            {
                MessageBox.Show($"Số lượng trả vượt quá giới hạn! Tối đa có thể trả thêm: {maxQty}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDetailSoLuongTra.Text = maxQty.ToString();
                txtDetailSoLuongTra.Focus();
                return;
            }

            // Cập nhật giá trị vào Grid hàng trả
            row.Cells["colReturnDetailSoLuong"].Value = qty;
            row.Cells["colReturnDetailTienHoan"].Value = qty * item.DonGia;
            row.Cells["colReturnDetailTinhTrang"].Value = txtDetailTinhTrang.Text.Trim();

            // Tính toán lại tổng tiền hoàn trả để hiển thị lên panel bên trái
            decimal totalRefund = 0;
            foreach (DataGridViewRow r in dgvReturnDetails.Rows)
            {
                if (r.Cells["colReturnDetailTienHoan"].Value != null)
                {
                    totalRefund += Convert.ToDecimal(r.Cells["colReturnDetailTienHoan"].Value);
                }
            }
            txtTongTienHoan.Text = totalRefund.ToString("N0");

            MessageBox.Show($"Đã cập nhật sản phẩm: {item.TenSanPham} với SL trả là {qty}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetDetailInputFields();
        }

        private void ResetDetailInputFields()
        {
            lblDetailProductName.Text = "Sản phẩm đang chọn: (Chưa chọn dòng sản phẩm trên Grid)";
            lblDetailQtyInfo.Text = "Số lượng đã mua: 0 | Đã trả ở các phiếu cũ: 0";
            txtDetailSoLuongTra.Text = "0";
            txtDetailTinhTrang.Text = "";
            txtDetailSoLuongTra.Enabled = false;
            txtDetailTinhTrang.Enabled = false;
            btnUpdateDetail.Enabled = false;
        }

        // ==========================================
        // TAB 3 (EXCHANGE POS) EVENT HANDLERS
        // ==========================================
        private void dgvExchangeProducts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dgvExchangeProducts.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var prod = exchangeProducts.FirstOrDefault(p => p.MaSanPham == id);
                if (prod != null)
                {
                    if (prod.TonKho <= 0)
                    {
                        MessageBox.Show("Sản phẩm đã hết hàng trong kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var item = exchangeCart.FirstOrDefault(i => i.MaSanPham == id);
                    if (item != null)
                    {
                        if (item.SoLuong >= prod.TonKho)
                        {
                            MessageBox.Show($"Không thể thêm! Số lượng trong giỏ hàng đã bằng giới hạn tồn kho ({prod.TonKho})", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        item.SoLuong++;
                    }
                    else
                    {
                        exchangeCart.Add(new ExchangeCartItem
                        {
                            MaSanPham = prod.MaSanPham,
                            TenSanPham = prod.TenSanPham,
                            SoLuong = 1,
                            DonGia = prod.DonGia
                        });
                    }

                    LoadExchangeCartGrid();
                    RecalculateExchangeTotals();
                }
            }
        }

        private void dgvExchangeCart_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                string id = dgvExchangeCart.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var item = exchangeCart.FirstOrDefault(i => i.MaSanPham == id);
                if (item != null)
                {
                    string valStr = dgvExchangeCart.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "1";
                    if (int.TryParse(valStr, out int qty) && qty > 0)
                    {
                        var prod = exchangeProducts.FirstOrDefault(p => p.MaSanPham == id);
                        if (prod != null && qty > prod.TonKho)
                        {
                            MessageBox.Show($"Số lượng yêu cầu vượt quá tồn kho hiện tại ({prod.TonKho})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            qty = prod.TonKho;
                        }
                        item.SoLuong = qty;
                    }
                    else
                    {
                        item.SoLuong = 1;
                    }
                }

                LoadExchangeCartGrid();
                RecalculateExchangeTotals();
            }
        }

        private void dgvExchangeCart_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 5)
            {
                string id = dgvExchangeCart.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var item = exchangeCart.FirstOrDefault(i => i.MaSanPham == id);
                if (item != null)
                {
                    exchangeCart.Remove(item);
                    LoadExchangeCartGrid();
                    RecalculateExchangeTotals();
                }
            }
        }

        private void btnSearchProd_Click(object? sender, EventArgs e)
        {
            string keyword = txtExchangeSearch.Text.Trim().ToLower();
            string category = cboExchangeCategory.Text;

            var filtered = exchangeProducts.Where(p =>
            {
                bool matchKeyword = string.IsNullOrEmpty(keyword) ||
                                    p.MaSanPham == keyword ||
                                    p.TenSanPham.ToLower().Contains(keyword);
                bool matchCategory = category == "Tất cả danh mục" || p.DanhMuc == category;
                return matchKeyword && matchCategory;
            }).ToList();

            LoadExchangeProductsGrid(filtered);
        }

        private void cboExchangeCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            btnSearchProd_Click(this, EventArgs.Empty);
        }

        private void txtExchangeSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearchProd_Click(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnExchangePay_Click(object? sender, EventArgs e)
        {
            if (exchangeCart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đổi mới rỗng! Vui lòng chọn sản phẩm mới cho khách đổi lấy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double subtotal = exchangeCart.Sum(i => i.ThanhTien);
            double difference = subtotal - exchangeOffsetAmount;
            string payment = cboExchangePaymentMethod.Text;

            Random rnd = new Random();
            int returnSlipId = rnd.Next(100, 999);
            int newInvoiceId = rnd.Next(5000, 9999);

            string paymentMsg = "";
            if (difference == 0)
            {
                paymentMsg = "\n[THANH TOÁN] Đổi ngang giá (chênh lệch 0 đ).";
            }
            else if (difference > 0)
            {
                paymentMsg = $"\n[THANH TOÁN] Khách hàng bù thêm tiền: {difference.ToString("N0")} đ bằng {payment}.";
            }
            else
            {
                paymentMsg = $"\n[THANH TOÁN] Thu ngân hoàn tiền thừa cho khách: {Math.Abs(difference).ToString("N0")} đ.";
            }

            MessageBox.Show($"Xác nhận giao dịch đổi trả hàng thành công!\n" +
                            $"- Đã tạo Phiếu trả hàng #{returnSlipId} (hoàn trả hàng cũ vào tồn kho).\n" +
                            $"- Đã tạo Hóa đơn đổi hàng #{newInvoiceId} (khấu trừ tồn kho hàng mới).\n" +
                            $"- Trạng thái hóa đơn mới: Đã hoàn thành (Loại: Đơn đổi hàng)." +
                            paymentMsg, 
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear inputs in return details tab
            txtMaHoaDon.Text = "";
            txtMaTraHang.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0";
            lblKhachHang.Text = "Khách hàng: (Chờ nhập hóa đơn...)";
            cboLoaiGiaoDich.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            dgvReturnDetails.Rows.Clear();
            ResetDetailInputFields();

            // Clear exchange cart
            exchangeCart.Clear();
            LoadExchangeCartGrid();
            exchangeOffsetAmount = 0;

            // Redirect back to return details tab
            tabReturnContainer.SelectedTab = tabReturnDetail;
        }
    }
}
