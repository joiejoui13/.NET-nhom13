namespace AssignmentApp.GUI.UserControls.Sales
{
    partial class ucPOS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            btnLogout = new Guna.UI2.WinForms.Guna2Button();
            pnlProducts = new Guna.UI2.WinForms.Guna2Panel();
            flowProducts = new FlowLayoutPanel();
            pnlCart = new Guna.UI2.WinForms.Guna2Panel();
            dgvCart = new Guna.UI2.WinForms.Guna2DataGridView();
            lblSubtotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblDiscount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblVAT = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnPay = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            btnNew = new Guna.UI2.WinForms.Guna2Button();
            pnlTop.SuspendLayout();
            pnlProducts.SuspendLayout();
            pnlCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.BorderRadius = 15;
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(btnLogout);
            pnlTop.CustomizableEdges = customizableEdges5;
            pnlTop.Location = new Point(20, 20);
            pnlTop.Name = "pnlTop";
            pnlTop.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnlTop.Size = new Size(1360, 80);
            pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(25, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(241, 56);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "POS SYSTEM";
            // 
            // txtSearch
            // 
            txtSearch.BorderRadius = 12;
            txtSearch.CustomizableEdges = customizableEdges1;
            txtSearch.DefaultText = "";
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(420, 18);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm sản phẩm...";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch.Size = new Size(500, 45);
            txtSearch.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.BorderRadius = 10;
            btnLogout.CustomizableEdges = customizableEdges3;
            btnLogout.FillColor = Color.FromArgb(220, 38, 38);
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(1180, 18);
            btnLogout.Name = "btnLogout";
            btnLogout.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLogout.Size = new Size(150, 45);
            btnLogout.TabIndex = 2;
            btnLogout.Text = "ĐĂNG XUẤT";
            // 
            // pnlProducts
            // 
            pnlProducts.BackColor = Color.White;
            pnlProducts.BorderRadius = 20;
            pnlProducts.Controls.Add(flowProducts);
            pnlProducts.CustomizableEdges = customizableEdges7;
            pnlProducts.Location = new Point(20, 120);
            pnlProducts.Name = "pnlProducts";
            pnlProducts.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pnlProducts.Size = new Size(850, 700);
            pnlProducts.TabIndex = 1;
            // 
            // flowProducts
            // 
            flowProducts.AutoScroll = true;
            flowProducts.BackColor = Color.White;
            flowProducts.Dock = DockStyle.Fill;
            flowProducts.Location = new Point(0, 0);
            flowProducts.Name = "flowProducts";
            flowProducts.Padding = new Padding(20);
            flowProducts.Size = new Size(850, 700);
            flowProducts.TabIndex = 0;
            // 
            // pnlCart
            // 
            pnlCart.BackColor = Color.White;
            pnlCart.BorderRadius = 20;
            pnlCart.Controls.Add(dgvCart);
            pnlCart.Controls.Add(lblSubtotal);
            pnlCart.Controls.Add(lblDiscount);
            pnlCart.Controls.Add(lblVAT);
            pnlCart.Controls.Add(lblTotal);
            pnlCart.Controls.Add(btnPay);
            pnlCart.Controls.Add(btnCancel);
            pnlCart.Controls.Add(btnNew);
            pnlCart.CustomizableEdges = customizableEdges15;
            pnlCart.Location = new Point(890, 120);
            pnlCart.Name = "pnlCart";
            pnlCart.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlCart.Size = new Size(490, 700);
            pnlCart.TabIndex = 2;
            // 
            // dgvCart
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvCart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCart.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCart.GridColor = Color.FromArgb(229, 231, 235);
            dgvCart.Location = new Point(20, 20);
            dgvCart.Name = "dgvCart";
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 62;
            dgvCart.RowTemplate.Height = 40;
            dgvCart.Size = new Size(450, 350);
            dgvCart.TabIndex = 0;
            dgvCart.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvCart.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvCart.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvCart.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvCart.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvCart.ThemeStyle.BackColor = Color.White;
            dgvCart.ThemeStyle.GridColor = Color.FromArgb(229, 231, 235);
            dgvCart.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvCart.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCart.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCart.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCart.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCart.ThemeStyle.HeaderStyle.Height = 40;
            dgvCart.ThemeStyle.ReadOnly = false;
            dgvCart.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCart.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCart.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCart.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvCart.ThemeStyle.RowsStyle.Height = 40;
            dgvCart.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvCart.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // lblSubtotal
            // 
            lblSubtotal.BackColor = Color.Transparent;
            lblSubtotal.Font = new Font("Segoe UI", 11F);
            lblSubtotal.Location = new Point(25, 400);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(167, 32);
            lblSubtotal.TabIndex = 1;
            lblSubtotal.Text = "Tạm tính : 0 VNĐ";
            // 
            // lblDiscount
            // 
            lblDiscount.BackColor = Color.Transparent;
            lblDiscount.Font = new Font("Segoe UI", 11F);
            lblDiscount.Location = new Point(25, 440);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Size = new Size(168, 32);
            lblDiscount.TabIndex = 2;
            lblDiscount.Text = "Giảm giá : 0 VNĐ";
            // 
            // lblVAT
            // 
            lblVAT.BackColor = Color.Transparent;
            lblVAT.Font = new Font("Segoe UI", 11F);
            lblVAT.Location = new Point(25, 480);
            lblVAT.Name = "lblVAT";
            lblVAT.Size = new Size(123, 32);
            lblVAT.TabIndex = 3;
            lblVAT.Text = "VAT : 0 VNĐ";
            // 
            // lblTotal
            // 
            lblTotal.BackColor = Color.Transparent;
            lblTotal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(22, 163, 74);
            lblTotal.Location = new Point(25, 530);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(136, 62);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "0 VNĐ";
            // 
            // btnPay
            // 
            btnPay.BorderRadius = 12;
            btnPay.CustomizableEdges = customizableEdges9;
            btnPay.FillColor = Color.FromArgb(22, 163, 74);
            btnPay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(20, 598);
            btnPay.Name = "btnPay";
            btnPay.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnPay.Size = new Size(140, 77);
            btnPay.TabIndex = 5;
            btnPay.Text = "THANH TOÁN";
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 12;
            btnCancel.CustomizableEdges = customizableEdges11;
            btnCancel.FillColor = Color.FromArgb(107, 114, 128);
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(166, 598);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnCancel.Size = new Size(144, 77);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "HỦY ĐƠN";
            // 
            // btnNew
            // 
            btnNew.BorderRadius = 12;
            btnNew.CustomizableEdges = customizableEdges13;
            btnNew.FillColor = Color.FromArgb(37, 99, 235);
            btnNew.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnNew.ForeColor = Color.White;
            btnNew.Location = new Point(325, 598);
            btnNew.Name = "btnNew";
            btnNew.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnNew.Size = new Size(135, 77);
            btnNew.TabIndex = 7;
            btnNew.Text = "TẠO MỚI";
            // 
            // ucPOS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            BackColor = Color.FromArgb(243, 244, 246);
            Controls.Add(pnlTop);
            Controls.Add(pnlProducts);
            Controls.Add(pnlCart);
            Name = "ucPOS";
            Size = new Size(1400, 850);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlProducts.ResumeLayout(false);
            pnlCart.ResumeLayout(false);
            pnlCart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private Guna.UI2.WinForms.Guna2Panel pnlProducts;
        private Guna.UI2.WinForms.Guna2Panel pnlCart;

        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubtotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDiscount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVAT;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal;

        private Guna.UI2.WinForms.Guna2TextBox txtSearch;

        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private Guna.UI2.WinForms.Guna2Button btnPay;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2Button btnNew;

        private Guna.UI2.WinForms.Guna2DataGridView dgvCart;

        private FlowLayoutPanel flowProducts;
    }
}

