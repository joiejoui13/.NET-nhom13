$designerCode = @"
namespace AssignmentApp.GUI.UserControls.Sales
{
    partial class ucOrderManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tabMainControl = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabOrderManagement = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvOrders = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaHoaDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKhachHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNguoiDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayTao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOrderDetails = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colDetailMaSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailTenSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblOrdersTitle = new System.Windows.Forms.Label();
            this.lblDetailsTitle = new System.Windows.Forms.Label();
            
            this.tabPOS = new System.Windows.Forms.TabPage();
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblMaHoaDon = new System.Windows.Forms.Label();
            this.txtMaHoaDonSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearchOrder = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLeftTitle = new System.Windows.Forms.Label();
            this.tabProducts = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabProductList = new System.Windows.Forms.TabPage();
            this.dgvProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaSP_Prod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSP_Prod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaBan_Prod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRight = new Guna.UI2.WinForms.Guna2Panel();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.dgvCart = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaSP_Cart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSP_Cart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSL_Cart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia_Cart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien_Cart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMaSP_Input = new System.Windows.Forms.Label();
            this.txtMaSP = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTenSP_Input = new System.Windows.Forms.Label();
            this.txtTenSP = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSoLuong_Input = new System.Windows.Forms.Label();
            this.txtSoLuong = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDonGia_Input = new System.Windows.Forms.Label();
            this.txtDonGia = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.btnAdd = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();

            this.tabMainControl.SuspendLayout();
            this.tabOrderManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).BeginInit();
            this.tabPOS.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.tabProducts.SuspendLayout();
            this.tabProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();

            // tabMainControl
            this.tabMainControl.Controls.Add(this.tabOrderManagement);
            this.tabMainControl.Controls.Add(this.tabPOS);
            this.tabMainControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMainControl.Location = new System.Drawing.Point(0, 0);
            this.tabMainControl.Name = "tabMainControl";
            this.tabMainControl.SelectedIndex = 0;
            this.tabMainControl.Size = new System.Drawing.Size(1000, 700);
            this.tabMainControl.TabIndex = 0;
            this.tabMainControl.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.tabMainControl.TabButtonIdleState.InnerColor = System.Drawing.Color.Transparent;
            this.tabMainControl.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));

            // tabOrderManagement
            this.tabOrderManagement.Controls.Add(this.splitContainer1);
            this.tabOrderManagement.Location = new System.Drawing.Point(4, 44);
            this.tabOrderManagement.Name = "tabOrderManagement";
            this.tabOrderManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrderManagement.Size = new System.Drawing.Size(992, 652);
            this.tabOrderManagement.TabIndex = 0;
            this.tabOrderManagement.Text = "QUẢN LÝ HÓA ĐƠN";
            this.tabOrderManagement.UseVisualStyleBackColor = true;

            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // splitContainer1.Panel1
            this.splitContainer1.Panel1.Controls.Add(this.dgvOrders);
            this.splitContainer1.Panel1.Controls.Add(this.lblOrdersTitle);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // splitContainer1.Panel2
            this.splitContainer1.Panel2.Controls.Add(this.dgvOrderDetails);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetailsTitle);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(986, 646);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 0;

            // lblOrdersTitle
            this.lblOrdersTitle.AutoSize = true;
            this.lblOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblOrdersTitle.Location = new System.Drawing.Point(10, 10);
            this.lblOrdersTitle.Name = "lblOrdersTitle";
            this.lblOrdersTitle.Size = new System.Drawing.Size(180, 21);
            this.lblOrdersTitle.Text = "DANH SÁCH HÓA ĐƠN";

            // dgvOrders
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.ColumnHeadersHeight = 35;
            this.dgvOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHoaDon,
            this.colTenKhachHang,
            this.colTenNguoiDung,
            this.colTongTien,
            this.colTrangThai,
            this.colNgayTao});
            this.dgvOrders.Location = new System.Drawing.Point(10, 40);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(966, 270);
            this.dgvOrders.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.dgvOrders.TabIndex = 1;
            this.dgvOrders.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvOrders.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvOrders.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellClick);
            this.dgvOrders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellDoubleClick);

            // colMaHoaDon
            this.colMaHoaDon.HeaderText = "Mã HĐ";
            this.colMaHoaDon.Name = "colMaHoaDon";
            this.colMaHoaDon.ReadOnly = true;
            // colTenKhachHang
            this.colTenKhachHang.HeaderText = "Tên Khách Hàng";
            this.colTenKhachHang.Name = "colTenKhachHang";
            this.colTenKhachHang.ReadOnly = true;
            // colTenNguoiDung
            this.colTenNguoiDung.HeaderText = "Nhân Viên";
            this.colTenNguoiDung.Name = "colTenNguoiDung";
            this.colTenNguoiDung.ReadOnly = true;
            // colTongTien
            this.colTongTien.HeaderText = "Tổng Tiền";
            this.colTongTien.Name = "colTongTien";
            this.colTongTien.ReadOnly = true;
            // colTrangThai
            this.colTrangThai.HeaderText = "Trạng Thái";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            // colNgayTao
            this.colNgayTao.HeaderText = "Ngày Tạo";
            this.colNgayTao.Name = "colNgayTao";
            this.colNgayTao.ReadOnly = true;

            // lblDetailsTitle
            this.lblDetailsTitle.AutoSize = true;
            this.lblDetailsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDetailsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblDetailsTitle.Location = new System.Drawing.Point(10, 10);
            this.lblDetailsTitle.Name = "lblDetailsTitle";
            this.lblDetailsTitle.Size = new System.Drawing.Size(155, 21);
            this.lblDetailsTitle.Text = "CHI TIẾT HÓA ĐƠN";

            // dgvOrderDetails
            this.dgvOrderDetails.AllowUserToAddRows = false;
            this.dgvOrderDetails.AllowUserToDeleteRows = false;
            this.dgvOrderDetails.ColumnHeadersHeight = 35;
            this.dgvOrderDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailMaSP,
            this.colDetailTenSP,
            this.colDetailSoLuong,
            this.colDetailDonGia,
            this.colDetailThanhTien});
            this.dgvOrderDetails.Location = new System.Drawing.Point(10, 40);
            this.dgvOrderDetails.Name = "dgvOrderDetails";
            this.dgvOrderDetails.ReadOnly = true;
            this.dgvOrderDetails.RowHeadersVisible = false;
            this.dgvOrderDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderDetails.Size = new System.Drawing.Size(966, 270);
            this.dgvOrderDetails.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.dgvOrderDetails.TabIndex = 1;
            this.dgvOrderDetails.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvOrderDetails.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;

            // colDetailMaSP
            this.colDetailMaSP.HeaderText = "Mã SP";
            this.colDetailMaSP.Name = "colDetailMaSP";
            this.colDetailMaSP.ReadOnly = true;
            // colDetailTenSP
            this.colDetailTenSP.HeaderText = "Tên SP";
            this.colDetailTenSP.Name = "colDetailTenSP";
            this.colDetailTenSP.ReadOnly = true;
            // colDetailSoLuong
            this.colDetailSoLuong.HeaderText = "Số Lượng";
            this.colDetailSoLuong.Name = "colDetailSoLuong";
            this.colDetailSoLuong.ReadOnly = true;
            // colDetailDonGia
            this.colDetailDonGia.HeaderText = "Đơn Giá";
            this.colDetailDonGia.Name = "colDetailDonGia";
            this.colDetailDonGia.ReadOnly = true;
            // colDetailThanhTien
            this.colDetailThanhTien.HeaderText = "Thành Tiền";
            this.colDetailThanhTien.Name = "colDetailThanhTien";
            this.colDetailThanhTien.ReadOnly = true;

            // tabPOS
            this.tabPOS.Controls.Add(this.pnlMain);
            this.tabPOS.Controls.Add(this.pnlTop);
            this.tabPOS.Location = new System.Drawing.Point(4, 44);
            this.tabPOS.Name = "tabPOS";
            this.tabPOS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPOS.Size = new System.Drawing.Size(992, 652);
            this.tabPOS.TabIndex = 1;
            this.tabPOS.Text = "BÁN HÀNG (POS)";
            this.tabPOS.UseVisualStyleBackColor = true;

            // pnlTop
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnSearchOrder);
            this.pnlTop.Controls.Add(this.txtMaHoaDonSearch);
            this.pnlTop.Controls.Add(this.lblMaHoaDon);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(986, 60);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.FillColor = System.Drawing.Color.White;

            // lblMaHoaDon
            this.lblMaHoaDon.AutoSize = true;
            this.lblMaHoaDon.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMaHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblMaHoaDon.Location = new System.Drawing.Point(20, 20);
            this.lblMaHoaDon.Name = "lblMaHoaDon";
            this.lblMaHoaDon.Size = new System.Drawing.Size(120, 21);
            this.lblMaHoaDon.Text = "MÃ HÓA ĐƠN:";
            
            // txtMaHoaDonSearch
            this.txtMaHoaDonSearch.Location = new System.Drawing.Point(150, 12);
            this.txtMaHoaDonSearch.Name = "txtMaHoaDonSearch";
            this.txtMaHoaDonSearch.Size = new System.Drawing.Size(300, 36);
            this.txtMaHoaDonSearch.TabIndex = 1;
            
            // btnSearchOrder
            this.btnSearchOrder.Location = new System.Drawing.Point(470, 12);
            this.btnSearchOrder.Name = "btnSearchOrder";
            this.btnSearchOrder.Size = new System.Drawing.Size(80, 36);
            this.btnSearchOrder.Text = "TÌM";
            this.btnSearchOrder.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSearchOrder.Click += new System.EventHandler(this.btnSearchOrder_Click);
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(560, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 36);
            this.btnRefresh.Text = "LÀM MỚI";
            this.btnRefresh.FillColor = System.Drawing.Color.Gray;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // pnlMain
            this.pnlMain.ColumnCount = 2;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.pnlMain.Controls.Add(this.pnlLeft, 0, 0);
            this.pnlMain.Controls.Add(this.pnlRight, 1, 0);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(3, 63);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 1;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Size = new System.Drawing.Size(986, 586);
            this.pnlMain.TabIndex = 1;

            // pnlLeft
            this.pnlLeft.Controls.Add(this.tabProducts);
            this.pnlLeft.Controls.Add(this.lblLeftTitle);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(10, 10);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(10);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(423, 566);
            this.pnlLeft.FillColor = System.Drawing.Color.White;

            // lblLeftTitle
            this.lblLeftTitle.AutoSize = true;
            this.lblLeftTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLeftTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblLeftTitle.Location = new System.Drawing.Point(10, 10);
            this.lblLeftTitle.Name = "lblLeftTitle";
            this.lblLeftTitle.Size = new System.Drawing.Size(200, 19);
            this.lblLeftTitle.Text = "1. CHỌN SẢN PHẨM CẦN BÁN";

            // tabProducts
            this.tabProducts.Controls.Add(this.tabProductList);
            this.tabProducts.Location = new System.Drawing.Point(10, 40);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.SelectedIndex = 0;
            this.tabProducts.Size = new System.Drawing.Size(403, 516);
            this.tabProducts.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.tabProducts.TabIndex = 1;
            this.tabProducts.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.tabProducts.TabButtonIdleState.InnerColor = System.Drawing.Color.Transparent;
            this.tabProducts.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));

            // tabProductList
            this.tabProductList.Controls.Add(this.dgvProducts);
            this.tabProductList.Location = new System.Drawing.Point(4, 44);
            this.tabProductList.Name = "tabProductList";
            this.tabProductList.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductList.Size = new System.Drawing.Size(395, 468);
            this.tabProductList.TabIndex = 0;
            this.tabProductList.Text = "Danh sách sản phẩm";
            this.tabProductList.UseVisualStyleBackColor = true;

            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.ColumnHeadersHeight = 35;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSP_Prod,
            this.colTenSP_Prod,
            this.colGiaBan_Prod});
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.Location = new System.Drawing.Point(3, 3);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(389, 462);
            this.dgvProducts.TabIndex = 0;
            this.dgvProducts.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvProducts.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);

            // colMaSP_Prod
            this.colMaSP_Prod.HeaderText = "Mã SP";
            this.colMaSP_Prod.Name = "colMaSP_Prod";
            this.colMaSP_Prod.ReadOnly = true;
            // colTenSP_Prod
            this.colTenSP_Prod.HeaderText = "Tên Sản Phẩm";
            this.colTenSP_Prod.Name = "colTenSP_Prod";
            this.colTenSP_Prod.ReadOnly = true;
            // colGiaBan_Prod
            this.colGiaBan_Prod.HeaderText = "Giá bán";
            this.colGiaBan_Prod.Name = "colGiaBan_Prod";
            this.colGiaBan_Prod.ReadOnly = true;

            // pnlRight
            this.pnlRight.Controls.Add(this.btnCancel);
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Controls.Add(this.btnUpdate);
            this.pnlRight.Controls.Add(this.btnAdd);
            this.pnlRight.Controls.Add(this.lblTotalAmount);
            this.pnlRight.Controls.Add(this.txtDonGia);
            this.pnlRight.Controls.Add(this.lblDonGia_Input);
            this.pnlRight.Controls.Add(this.txtSoLuong);
            this.pnlRight.Controls.Add(this.lblSoLuong_Input);
            this.pnlRight.Controls.Add(this.txtTenSP);
            this.pnlRight.Controls.Add(this.lblTenSP_Input);
            this.pnlRight.Controls.Add(this.txtMaSP);
            this.pnlRight.Controls.Add(this.lblMaSP_Input);
            this.pnlRight.Controls.Add(this.dgvCart);
            this.pnlRight.Controls.Add(this.lblRightTitle);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(453, 10);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(10);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(523, 566);
            this.pnlRight.FillColor = System.Drawing.Color.White;

            // lblRightTitle
            this.lblRightTitle.AutoSize = true;
            this.lblRightTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblRightTitle.Location = new System.Drawing.Point(10, 10);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(180, 19);
            this.lblRightTitle.Text = "2. GIỎ HÀNG THÔNG TIN";

            // dgvCart
            this.dgvCart.AllowUserToAddRows = false;
            this.dgvCart.AllowUserToDeleteRows = false;
            this.dgvCart.ColumnHeadersHeight = 35;
            this.dgvCart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSP_Cart,
            this.colTenSP_Cart,
            this.colSL_Cart,
            this.colDonGia_Cart,
            this.colThanhTien_Cart});
            this.dgvCart.Location = new System.Drawing.Point(10, 40);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.ReadOnly = true;
            this.dgvCart.RowHeadersVisible = false;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.Size = new System.Drawing.Size(503, 190);
            this.dgvCart.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.dgvCart.TabIndex = 1;
            this.dgvCart.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvCart.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCart_CellClick);

            // colMaSP_Cart
            this.colMaSP_Cart.HeaderText = "Mã SP";
            this.colMaSP_Cart.Name = "colMaSP_Cart";
            this.colMaSP_Cart.ReadOnly = true;
            // colTenSP_Cart
            this.colTenSP_Cart.HeaderText = "Tên";
            this.colTenSP_Cart.Name = "colTenSP_Cart";
            this.colTenSP_Cart.ReadOnly = true;
            // colSL_Cart
            this.colSL_Cart.HeaderText = "SL";
            this.colSL_Cart.Name = "colSL_Cart";
            this.colSL_Cart.ReadOnly = true;
            // colDonGia_Cart
            this.colDonGia_Cart.HeaderText = "Đơn Giá";
            this.colDonGia_Cart.Name = "colDonGia_Cart";
            this.colDonGia_Cart.ReadOnly = true;
            // colThanhTien_Cart
            this.colThanhTien_Cart.HeaderText = "Thành Tiền";
            this.colThanhTien_Cart.Name = "colThanhTien_Cart";
            this.colThanhTien_Cart.ReadOnly = true;

            // lblMaSP_Input
            this.lblMaSP_Input.AutoSize = true;
            this.lblMaSP_Input.Location = new System.Drawing.Point(10, 240);
            this.lblMaSP_Input.Name = "lblMaSP_Input";
            this.lblMaSP_Input.Size = new System.Drawing.Size(82, 15);
            this.lblMaSP_Input.Text = "Mã sản phẩm:";
            // txtMaSP
            this.txtMaSP.Location = new System.Drawing.Point(10, 260);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.ReadOnly = true;
            this.txtMaSP.Size = new System.Drawing.Size(503, 36);
            this.txtMaSP.TabIndex = 3;
            // lblTenSP_Input
            this.lblTenSP_Input.AutoSize = true;
            this.lblTenSP_Input.Location = new System.Drawing.Point(10, 300);
            this.lblTenSP_Input.Name = "lblTenSP_Input";
            this.lblTenSP_Input.Size = new System.Drawing.Size(83, 15);
            this.lblTenSP_Input.Text = "Tên sản phẩm:";
            // txtTenSP
            this.txtTenSP.Location = new System.Drawing.Point(10, 320);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.ReadOnly = true;
            this.txtTenSP.Size = new System.Drawing.Size(503, 36);
            this.txtTenSP.TabIndex = 5;
            // lblSoLuong_Input
            this.lblSoLuong_Input.AutoSize = true;
            this.lblSoLuong_Input.Location = new System.Drawing.Point(10, 360);
            this.lblSoLuong_Input.Name = "lblSoLuong_Input";
            this.lblSoLuong_Input.Size = new System.Drawing.Size(57, 15);
            this.lblSoLuong_Input.Text = "Số lượng:";
            // txtSoLuong
            this.txtSoLuong.Location = new System.Drawing.Point(10, 380);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(503, 36);
            this.txtSoLuong.TabIndex = 7;
            // lblDonGia_Input
            this.lblDonGia_Input.AutoSize = true;
            this.lblDonGia_Input.Location = new System.Drawing.Point(10, 420);
            this.lblDonGia_Input.Name = "lblDonGia_Input";
            this.lblDonGia_Input.Size = new System.Drawing.Size(51, 15);
            this.lblDonGia_Input.Text = "Đơn giá:";
            // txtDonGia
            this.txtDonGia.Location = new System.Drawing.Point(10, 440);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(503, 36);
            this.txtDonGia.TabIndex = 9;
            
            // lblTotalAmount
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTotalAmount.Location = new System.Drawing.Point(10, 490);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(206, 21);
            this.lblTotalAmount.Text = "TỔNG TIỀN TẠM TÍNH: 0 đ";
            
            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(230, 485);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 36);
            this.btnAdd.Text = "THÊM";
            this.btnAdd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(325, 485);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(85, 36);
            this.btnUpdate.Text = "SỬA";
            this.btnUpdate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(420, 485);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 36);
            this.btnDelete.Text = "XÓA";
            this.btnDelete.FillColor = System.Drawing.Color.Red;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(230, 525);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 36);
            this.btnSave.Text = "LƯU THAY ĐỔI";
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(375, 525);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 36);
            this.btnCancel.Text = "BỎ QUA";
            this.btnCancel.FillColor = System.Drawing.Color.Gray;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ucOrderManagement
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMainControl);
            this.Name = "ucOrderManagement";
            this.Size = new System.Drawing.Size(1000, 700);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.Load += new System.EventHandler(this.ucOrderManagement_Load);
            
            this.tabMainControl.ResumeLayout(false);
            this.tabOrderManagement.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).EndInit();
            this.tabPOS.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.tabProducts.ResumeLayout(false);
            this.tabProductList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TabControl tabMainControl;
        private System.Windows.Forms.TabPage tabOrderManagement;
        private System.Windows.Forms.TabPage tabPOS;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvOrders;
        private Guna.UI2.WinForms.Guna2DataGridView dgvOrderDetails;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.Label lblDetailsTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHoaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKhachHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNguoiDung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayTao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailMaSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailTenSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailThanhTien;

        // POS
        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblMaHoaDon;
        private Guna.UI2.WinForms.Guna2TextBox txtMaHoaDonSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearchOrder;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private Guna.UI2.WinForms.Guna2Panel pnlLeft;
        private System.Windows.Forms.Label lblLeftTitle;
        private Guna.UI2.WinForms.Guna2TabControl tabProducts;
        private System.Windows.Forms.TabPage tabProductList;
        private Guna.UI2.WinForms.Guna2DataGridView dgvProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSP_Prod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSP_Prod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaBan_Prod;
        private Guna.UI2.WinForms.Guna2Panel pnlRight;
        private System.Windows.Forms.Label lblRightTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSP_Cart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSP_Cart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSL_Cart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia_Cart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien_Cart;
        private System.Windows.Forms.Label lblMaSP_Input;
        private Guna.UI2.WinForms.Guna2TextBox txtMaSP;
        private System.Windows.Forms.Label lblTenSP_Input;
        private Guna.UI2.WinForms.Guna2TextBox txtTenSP;
        private System.Windows.Forms.Label lblSoLuong_Input;
        private Guna.UI2.WinForms.Guna2TextBox txtSoLuong;
        private System.Windows.Forms.Label lblDonGia_Input;
        private Guna.UI2.WinForms.Guna2TextBox txtDonGia;
        private System.Windows.Forms.Label lblTotalAmount;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
    }
}
"@
$designerCode | Out-File -FilePath "AssignmentApp/GUI/UserControls/Sales/ucOrderManagement.Designer.cs" -Encoding UTF8
