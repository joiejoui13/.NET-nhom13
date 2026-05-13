namespace AssignmentApp.GUI.Forms
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlMenuAdmin = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAdmin_Users = new Guna.UI2.WinForms.Guna2Button();
            this.btnAdmin_Promo = new Guna.UI2.WinForms.Guna2Button();
            this.btnAdmin_Reports = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMenuSales = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSales_POS = new Guna.UI2.WinForms.Guna2Button();
            this.btnSales_Orders = new Guna.UI2.WinForms.Guna2Button();
            this.btnSales_Delivery = new Guna.UI2.WinForms.Guna2Button();
            this.btnSales_Returns = new Guna.UI2.WinForms.Guna2Button();
            this.btnSales_Customers = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMenuWarehouse = new Guna.UI2.WinForms.Guna2Panel();
            this.btnWarehouse_Goods = new Guna.UI2.WinForms.Guna2Button();
            this.btnWarehouse_Category = new Guna.UI2.WinForms.Guna2Button();
            this.btnWarehouse_StockIn = new Guna.UI2.WinForms.Guna2Button();
            this.btnWarehouse_Inventory = new Guna.UI2.WinForms.Guna2Button();
            this.pnlLogo = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnDangXuat = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlContainer = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.pnlSidebar.SuspendLayout();
            this.pnlMenuAdmin.SuspendLayout();
            this.pnlMenuSales.SuspendLayout();
            this.pnlMenuWarehouse.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlSidebar.Controls.Add(this.pnlMenuAdmin);
            this.pnlSidebar.Controls.Add(this.pnlMenuSales);
            this.pnlSidebar.Controls.Add(this.pnlMenuWarehouse);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 720);
            this.pnlSidebar.TabIndex = 0;
            // 
            // pnlMenuAdmin
            // 
            this.pnlMenuAdmin.Controls.Add(this.btnAdmin_Users);
            this.pnlMenuAdmin.Controls.Add(this.btnAdmin_Promo);
            this.pnlMenuAdmin.Controls.Add(this.btnAdmin_Reports);
            this.pnlMenuAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenuAdmin.Location = new System.Drawing.Point(0, 100);
            this.pnlMenuAdmin.Name = "pnlMenuAdmin";
            this.pnlMenuAdmin.Size = new System.Drawing.Size(220, 620);
            this.pnlMenuAdmin.TabIndex = 1;
            this.pnlMenuAdmin.Visible = false;
            // 
            // btnAdmin_Users
            // 
            this.btnAdmin_Users.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdmin_Users.FillColor = System.Drawing.Color.Transparent;
            this.btnAdmin_Users.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdmin_Users.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnAdmin_Users.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnAdmin_Users.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnAdmin_Users.Location = new System.Drawing.Point(0, 100);
            this.btnAdmin_Users.Name = "btnAdmin_Users";
            this.btnAdmin_Users.Size = new System.Drawing.Size(220, 50);
            this.btnAdmin_Users.TabIndex = 2;
            this.btnAdmin_Users.Text = "QUẢN LÝ NHÂN VIÊN";
            this.btnAdmin_Users.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAdmin_Users.TextOffset = new System.Drawing.Point(20, 0);
            this.btnAdmin_Users.Click += new System.EventHandler(this.btnAdmin_Users_Click);
            // 
            // btnAdmin_Promo
            // 
            this.btnAdmin_Promo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdmin_Promo.FillColor = System.Drawing.Color.Transparent;
            this.btnAdmin_Promo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdmin_Promo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnAdmin_Promo.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnAdmin_Promo.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnAdmin_Promo.Location = new System.Drawing.Point(0, 50);
            this.btnAdmin_Promo.Name = "btnAdmin_Promo";
            this.btnAdmin_Promo.Size = new System.Drawing.Size(220, 50);
            this.btnAdmin_Promo.TabIndex = 1;
            this.btnAdmin_Promo.Text = "KHUYẾN MÃI";
            this.btnAdmin_Promo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAdmin_Promo.TextOffset = new System.Drawing.Point(20, 0);
            this.btnAdmin_Promo.Click += new System.EventHandler(this.btnAdmin_Promo_Click);
            // 
            // btnAdmin_Reports
            // 
            this.btnAdmin_Reports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdmin_Reports.FillColor = System.Drawing.Color.Transparent;
            this.btnAdmin_Reports.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdmin_Reports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnAdmin_Reports.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnAdmin_Reports.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnAdmin_Reports.Location = new System.Drawing.Point(0, 0);
            this.btnAdmin_Reports.Name = "btnAdmin_Reports";
            this.btnAdmin_Reports.Size = new System.Drawing.Size(220, 50);
            this.btnAdmin_Reports.TabIndex = 0;
            this.btnAdmin_Reports.Text = "BÁO CÁO TỔNG QUAN";
            this.btnAdmin_Reports.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAdmin_Reports.TextOffset = new System.Drawing.Point(20, 0);
            this.btnAdmin_Reports.Click += new System.EventHandler(this.btnAdmin_Reports_Click);
            // 
            // pnlMenuSales
            // 
            this.pnlMenuSales.Controls.Add(this.btnSales_Customers);
            this.pnlMenuSales.Controls.Add(this.btnSales_Returns);
            this.pnlMenuSales.Controls.Add(this.btnSales_Delivery);
            this.pnlMenuSales.Controls.Add(this.btnSales_Orders);
            this.pnlMenuSales.Controls.Add(this.btnSales_POS);
            this.pnlMenuSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenuSales.Location = new System.Drawing.Point(0, 100);
            this.pnlMenuSales.Name = "pnlMenuSales";
            this.pnlMenuSales.Size = new System.Drawing.Size(220, 620);
            this.pnlMenuSales.TabIndex = 2;
            this.pnlMenuSales.Visible = false;
            // 
            // btnSales_POS
            // 
            this.btnSales_POS.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSales_POS.FillColor = System.Drawing.Color.Transparent;
            this.btnSales_POS.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSales_POS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnSales_POS.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnSales_POS.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnSales_POS.Location = new System.Drawing.Point(0, 0);
            this.btnSales_POS.Name = "btnSales_POS";
            this.btnSales_POS.Size = new System.Drawing.Size(220, 50);
            this.btnSales_POS.TabIndex = 0;
            this.btnSales_POS.Text = "BÁN HÀNG (POS)";
            this.btnSales_POS.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSales_POS.TextOffset = new System.Drawing.Point(20, 0);
            this.btnSales_POS.Click += new System.EventHandler(this.btnSales_POS_Click);
            // 
            // btnSales_Orders
            // 
            this.btnSales_Orders.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSales_Orders.FillColor = System.Drawing.Color.Transparent;
            this.btnSales_Orders.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSales_Orders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnSales_Orders.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnSales_Orders.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnSales_Orders.Location = new System.Drawing.Point(0, 50);
            this.btnSales_Orders.Name = "btnSales_Orders";
            this.btnSales_Orders.Size = new System.Drawing.Size(220, 50);
            this.btnSales_Orders.TabIndex = 1;
            this.btnSales_Orders.Text = "ĐƠN HÀNG";
            this.btnSales_Orders.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSales_Orders.TextOffset = new System.Drawing.Point(20, 0);
            this.btnSales_Orders.Click += new System.EventHandler(this.btnSales_Orders_Click);
            // 
            // btnSales_Delivery
            // 
            this.btnSales_Delivery.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSales_Delivery.FillColor = System.Drawing.Color.Transparent;
            this.btnSales_Delivery.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSales_Delivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnSales_Delivery.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnSales_Delivery.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnSales_Delivery.Location = new System.Drawing.Point(0, 100);
            this.btnSales_Delivery.Name = "btnSales_Delivery";
            this.btnSales_Delivery.Size = new System.Drawing.Size(220, 50);
            this.btnSales_Delivery.TabIndex = 2;
            this.btnSales_Delivery.Text = "GIAO HÀNG";
            this.btnSales_Delivery.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSales_Delivery.TextOffset = new System.Drawing.Point(20, 0);
            this.btnSales_Delivery.Click += new System.EventHandler(this.btnSales_Delivery_Click);
            // 
            // btnSales_Returns
            // 
            this.btnSales_Returns.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSales_Returns.FillColor = System.Drawing.Color.Transparent;
            this.btnSales_Returns.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSales_Returns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnSales_Returns.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnSales_Returns.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnSales_Returns.Location = new System.Drawing.Point(0, 150);
            this.btnSales_Returns.Name = "btnSales_Returns";
            this.btnSales_Returns.Size = new System.Drawing.Size(220, 50);
            this.btnSales_Returns.TabIndex = 3;
            this.btnSales_Returns.Text = "TRẢ HÀNG";
            this.btnSales_Returns.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSales_Returns.TextOffset = new System.Drawing.Point(20, 0);
            this.btnSales_Returns.Click += new System.EventHandler(this.btnSales_Returns_Click);
            // 
            // btnSales_Customers
            // 
            this.btnSales_Customers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSales_Customers.FillColor = System.Drawing.Color.Transparent;
            this.btnSales_Customers.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSales_Customers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnSales_Customers.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnSales_Customers.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnSales_Customers.Location = new System.Drawing.Point(0, 200);
            this.btnSales_Customers.Name = "btnSales_Customers";
            this.btnSales_Customers.Size = new System.Drawing.Size(220, 50);
            this.btnSales_Customers.TabIndex = 4;
            this.btnSales_Customers.Text = "KHÁCH HÀNG";
            this.btnSales_Customers.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSales_Customers.TextOffset = new System.Drawing.Point(20, 0);
            this.btnSales_Customers.Click += new System.EventHandler(this.btnSales_Customers_Click);
            // 
            // pnlMenuWarehouse
            // 
            this.pnlMenuWarehouse.Controls.Add(this.btnWarehouse_Inventory);
            this.pnlMenuWarehouse.Controls.Add(this.btnWarehouse_StockIn);
            this.pnlMenuWarehouse.Controls.Add(this.btnWarehouse_Category);
            this.pnlMenuWarehouse.Controls.Add(this.btnWarehouse_Goods);
            this.pnlMenuWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenuWarehouse.Location = new System.Drawing.Point(0, 100);
            this.pnlMenuWarehouse.Name = "pnlMenuWarehouse";
            this.pnlMenuWarehouse.Size = new System.Drawing.Size(220, 620);
            this.pnlMenuWarehouse.TabIndex = 3;
            this.pnlMenuWarehouse.Visible = false;
            // 
            // btnWarehouse_Goods
            // 
            this.btnWarehouse_Goods.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnWarehouse_Goods.FillColor = System.Drawing.Color.Transparent;
            this.btnWarehouse_Goods.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnWarehouse_Goods.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnWarehouse_Goods.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnWarehouse_Goods.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnWarehouse_Goods.Location = new System.Drawing.Point(0, 0);
            this.btnWarehouse_Goods.Name = "btnWarehouse_Goods";
            this.btnWarehouse_Goods.Size = new System.Drawing.Size(220, 50);
            this.btnWarehouse_Goods.TabIndex = 0;
            this.btnWarehouse_Goods.Text = "HÀNG HÓA";
            this.btnWarehouse_Goods.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse_Goods.TextOffset = new System.Drawing.Point(20, 0);
            this.btnWarehouse_Goods.Click += new System.EventHandler(this.btnWarehouse_Goods_Click);
            // 
            // btnWarehouse_Category
            // 
            this.btnWarehouse_Category.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnWarehouse_Category.FillColor = System.Drawing.Color.Transparent;
            this.btnWarehouse_Category.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnWarehouse_Category.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnWarehouse_Category.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnWarehouse_Category.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnWarehouse_Category.Location = new System.Drawing.Point(0, 50);
            this.btnWarehouse_Category.Name = "btnWarehouse_Category";
            this.btnWarehouse_Category.Size = new System.Drawing.Size(220, 50);
            this.btnWarehouse_Category.TabIndex = 1;
            this.btnWarehouse_Category.Text = "PHÂN LOẠI";
            this.btnWarehouse_Category.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse_Category.TextOffset = new System.Drawing.Point(20, 0);
            this.btnWarehouse_Category.Click += new System.EventHandler(this.btnWarehouse_Category_Click);
            // 
            // btnWarehouse_StockIn
            // 
            this.btnWarehouse_StockIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnWarehouse_StockIn.FillColor = System.Drawing.Color.Transparent;
            this.btnWarehouse_StockIn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnWarehouse_StockIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnWarehouse_StockIn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnWarehouse_StockIn.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnWarehouse_StockIn.Location = new System.Drawing.Point(0, 100);
            this.btnWarehouse_StockIn.Name = "btnWarehouse_StockIn";
            this.btnWarehouse_StockIn.Size = new System.Drawing.Size(220, 50);
            this.btnWarehouse_StockIn.TabIndex = 2;
            this.btnWarehouse_StockIn.Text = "NHẬP HÀNG";
            this.btnWarehouse_StockIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse_StockIn.TextOffset = new System.Drawing.Point(20, 0);
            this.btnWarehouse_StockIn.Click += new System.EventHandler(this.btnWarehouse_StockIn_Click);
            // 
            // btnWarehouse_Inventory
            // 
            this.btnWarehouse_Inventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnWarehouse_Inventory.FillColor = System.Drawing.Color.Transparent;
            this.btnWarehouse_Inventory.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnWarehouse_Inventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(164)))), ((int)(((byte)(177)))));
            this.btnWarehouse_Inventory.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btnWarehouse_Inventory.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnWarehouse_Inventory.Location = new System.Drawing.Point(0, 150);
            this.btnWarehouse_Inventory.Name = "btnWarehouse_Inventory";
            this.btnWarehouse_Inventory.Size = new System.Drawing.Size(220, 50);
            this.btnWarehouse_Inventory.TabIndex = 3;
            this.btnWarehouse_Inventory.Text = "TỒN KHO";
            this.btnWarehouse_Inventory.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse_Inventory.TextOffset = new System.Drawing.Point(20, 0);
            this.btnWarehouse_Inventory.Click += new System.EventHandler(this.btnWarehouse_Inventory_Click);
            // 
            // pnlLogo
            // 
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(220, 100);
            this.pnlLogo.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnDangXuat);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(220, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1060, 60);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDangXuat.BorderRadius = 5;
            this.btnDangXuat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.Location = new System.Drawing.Point(940, 12);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(100, 35);
            this.btnDangXuat.TabIndex = 1;
            this.btnDangXuat.Text = "ĐĂNG XUẤT";
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(121, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DASHBOARD";
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(220, 60);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(1060, 660);
            this.pnlContainer.TabIndex = 2;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.TargetControl = this.pnlHeader;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);

            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý Văn phòng phẩm";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlMenuAdmin.ResumeLayout(false);
            this.pnlMenuSales.ResumeLayout(false);
            this.pnlMenuWarehouse.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private Guna.UI2.WinForms.Guna2Panel pnlLogo;
        private Guna.UI2.WinForms.Guna2Panel pnlMenuAdmin;
        private Guna.UI2.WinForms.Guna2Button btnAdmin_Reports;
        private Guna.UI2.WinForms.Guna2Button btnAdmin_Promo;
        private Guna.UI2.WinForms.Guna2Button btnAdmin_Users;
        private Guna.UI2.WinForms.Guna2Panel pnlMenuSales;
        private Guna.UI2.WinForms.Guna2Button btnSales_POS;
        private Guna.UI2.WinForms.Guna2Button btnSales_Orders;
        private Guna.UI2.WinForms.Guna2Button btnSales_Delivery;
        private Guna.UI2.WinForms.Guna2Button btnSales_Returns;
        private Guna.UI2.WinForms.Guna2Button btnSales_Customers;
        private Guna.UI2.WinForms.Guna2Panel pnlMenuWarehouse;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse_Goods;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse_Category;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse_StockIn;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse_Inventory;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Button btnDangXuat;
        private Guna.UI2.WinForms.Guna2Panel pnlContainer;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
    }
}