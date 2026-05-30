
import os

designer_code = """namespace AssignmentApp.GUI.UserControls.Sales
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
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblMaHoaDon = new System.Windows.Forms.Label();
            this.txtMaHoaDonSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearchOrder = new Guna.UI2.WinForms.Guna2Button();
            this.btnSearchOrder2 = new Guna.UI2.WinForms.Guna2Button();
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
            this.tabProductDetails = new System.Windows.Forms.TabPage();
            
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

            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.tabProducts.SuspendLayout();
            this.tabProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            
            // pnlTop
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnSearchOrder2);
            this.pnlTop.Controls.Add(this.btnSearchOrder);
            this.pnlTop.Controls.Add(this.txtMaHoaDonSearch);
            this.pnlTop.Controls.Add(this.lblMaHoaDon);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 60);
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
            
            // btnSearchOrder2
            this.btnSearchOrder2.Location = new System.Drawing.Point(560, 12);
            this.btnSearchOrder2.Name = "btnSearchOrder2";
            this.btnSearchOrder2.Size = new System.Drawing.Size(80, 36);
            this.btnSearchOrder2.Text = "TÌM";
            this.btnSearchOrder2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(650, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 36);
            this.btnRefresh.Text = "LÀM MỚI";
            this.btnRefresh.FillColor = System.Drawing.Color.Gray;
            
            // pnlMain
            this.pnlMain.ColumnCount = 2;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.pnlMain.Controls.Add(this.pnlLeft, 0, 0);
            this.pnlMain.Controls.Add(this.pnlRight, 1, 0);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 60);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 1;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Size = new System.Drawing.Size(1000, 640);
            this.pnlMain.TabIndex = 1;
            
            // pnlLeft
            this.pnlLeft.Controls.Add(this.tabProducts);
            this.pnlLeft.Controls.Add(this.lblLeftTitle);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(10, 10);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(10);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(430, 620);
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
            this.tabProducts.Controls.Add(this.tabProductDetails);
            this.tabProducts.Location = new System.Drawing.Point(10, 40);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.SelectedIndex = 0;
            this.tabProducts.Size = new System.Drawing.Size(410, 570);
            this.tabProducts.TabIndex = 1;
            this.tabProducts.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.tabProducts.TabButtonIdleState.InnerColor = System.Drawing.Color.Transparent;
            this.tabProducts.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            
            // tabProductList
            this.tabProductList.Controls.Add(this.dgvProducts);
            this.tabProductList.Location = new System.Drawing.Point(4, 44);
            this.tabProductList.Name = "tabProductList";
            this.tabProductList.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductList.Size = new System.Drawing.Size(402, 522);
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
            this.dgvProducts.Size = new System.Drawing.Size(396, 516);
            this.dgvProducts.TabIndex = 0;
            this.dgvProducts.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvProducts.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            
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
            
            // tabProductDetails
            this.tabProductDetails.Location = new System.Drawing.Point(4, 44);
            this.tabProductDetails.Name = "tabProductDetails";
            this.tabProductDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductDetails.Size = new System.Drawing.Size(402, 522);
            this.tabProductDetails.TabIndex = 1;
            this.tabProductDetails.Text = "Chi tiết sản phẩm";
            this.tabProductDetails.UseVisualStyleBackColor = true;
            
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
            this.pnlRight.Location = new System.Drawing.Point(460, 10);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(10);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(530, 620);
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
            this.dgvCart.Size = new System.Drawing.Size(510, 250);
            this.dgvCart.TabIndex = 1;
            this.dgvCart.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvCart.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            
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
            this.lblMaSP_Input.Location = new System.Drawing.Point(10, 310);
            this.lblMaSP_Input.Name = "lblMaSP_Input";
            this.lblMaSP_Input.Size = new System.Drawing.Size(82, 15);
            this.lblMaSP_Input.Text = "Mã sản phẩm:";
            // txtMaSP
            this.txtMaSP.Location = new System.Drawing.Point(10, 330);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.ReadOnly = true;
            this.txtMaSP.Size = new System.Drawing.Size(510, 36);
            this.txtMaSP.TabIndex = 3;
            // lblTenSP_Input
            this.lblTenSP_Input.AutoSize = true;
            this.lblTenSP_Input.Location = new System.Drawing.Point(10, 370);
            this.lblTenSP_Input.Name = "lblTenSP_Input";
            this.lblTenSP_Input.Size = new System.Drawing.Size(83, 15);
            this.lblTenSP_Input.Text = "Tên sản phẩm:";
            // txtTenSP
            this.txtTenSP.Location = new System.Drawing.Point(10, 390);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.ReadOnly = true;
            this.txtTenSP.Size = new System.Drawing.Size(510, 36);
            this.txtTenSP.TabIndex = 5;
            // lblSoLuong_Input
            this.lblSoLuong_Input.AutoSize = true;
            this.lblSoLuong_Input.Location = new System.Drawing.Point(10, 430);
            this.lblSoLuong_Input.Name = "lblSoLuong_Input";
            this.lblSoLuong_Input.Size = new System.Drawing.Size(57, 15);
            this.lblSoLuong_Input.Text = "Số lượng:";
            // txtSoLuong
            this.txtSoLuong.Location = new System.Drawing.Point(10, 450);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(510, 36);
            this.txtSoLuong.TabIndex = 7;
            // lblDonGia_Input
            this.lblDonGia_Input.AutoSize = true;
            this.lblDonGia_Input.Location = new System.Drawing.Point(10, 490);
            this.lblDonGia_Input.Name = "lblDonGia_Input";
            this.lblDonGia_Input.Size = new System.Drawing.Size(51, 15);
            this.lblDonGia_Input.Text = "Đơn giá:";
            // txtDonGia
            this.txtDonGia.Location = new System.Drawing.Point(10, 510);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(510, 36);
            this.txtDonGia.TabIndex = 9;
            
            // lblTotalAmount
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTotalAmount.Location = new System.Drawing.Point(10, 560);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(206, 21);
            this.lblTotalAmount.Text = "TỔNG TIỀN TẠM TÍNH: 0 đ";
            
            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(230, 555);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 36);
            this.btnAdd.Text = "THÊM";
            this.btnAdd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            
            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(330, 555);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 36);
            this.btnUpdate.Text = "SỬA";
            this.btnUpdate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(430, 555);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 36);
            this.btnDelete.Text = "XÓA";
            this.btnDelete.FillColor = System.Drawing.Color.Red;
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(230, 600);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 36);
            this.btnSave.Text = "LƯU THAY ĐỔI";
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(380, 600);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 36);
            this.btnCancel.Text = "BỎ QUA";
            this.btnCancel.FillColor = System.Drawing.Color.Gray;

            // ucOrderManagement
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Name = "ucOrderManagement";
            this.Size = new System.Drawing.Size(1000, 700);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
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

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblMaHoaDon;
        private Guna.UI2.WinForms.Guna2TextBox txtMaHoaDonSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearchOrder;
        private Guna.UI2.WinForms.Guna2Button btnSearchOrder2;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private Guna.UI2.WinForms.Guna2Panel pnlLeft;
        private System.Windows.Forms.Label lblLeftTitle;
        private Guna.UI2.WinForms.Guna2TabControl tabProducts;
        private System.Windows.Forms.TabPage tabProductList;
        private System.Windows.Forms.TabPage tabProductDetails;
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
"""

with open("AssignmentApp/GUI/UserControls/Sales/ucOrderManagement.Designer.cs", "w", encoding="utf-8") as f:
    f.write(designer_code)
