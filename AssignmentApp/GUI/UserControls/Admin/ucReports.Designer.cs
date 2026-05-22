namespace AssignmentApp.GUI.UserControls.Admin
{
    partial class ucReports
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            lblEndDate = new Label();
            lblStartDate = new Label();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            btnFilter = new Guna.UI2.WinForms.Guna2Button();
            dtpEndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            dtpStartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            pnlCards = new Guna.UI2.WinForms.Guna2Panel();
            cardOrders = new Guna.UI2.WinForms.Guna2Panel();
            lblOrdersValue = new Label();
            lblOrdersTitle = new Label();
            cardRevenue = new Guna.UI2.WinForms.Guna2Panel();
            lblRevenueValue = new Label();
            lblRevenueTitle = new Label();
            pnlGrid = new Guna.UI2.WinForms.Guna2Panel();
            dgvReports = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlHeader.SuspendLayout();
            pnlCards.SuspendLayout();
            cardOrders.SuspendLayout();
            cardRevenue.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblEndDate);
            pnlHeader.Controls.Add(lblStartDate);
            pnlHeader.Controls.Add(btnExport);
            pnlHeader.Controls.Add(btnFilter);
            pnlHeader.Controls.Add(dtpEndDate);
            pnlHeader.Controls.Add(dtpStartDate);
            pnlHeader.CustomizableEdges = customizableEdges9;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(20, 20);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlHeader.Size = new Size(950, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblEndDate
            // 
            lblEndDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEndDate.AutoSize = true;
            lblEndDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEndDate.Location = new Point(430, 23);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(71, 19);
            lblEndDate.TabIndex = 6;
            lblEndDate.Text = "Đến ngày:";
            // 
            // lblStartDate
            // 
            lblStartDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStartDate.AutoSize = true;
            lblStartDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStartDate.Location = new Point(200, 23);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(61, 19);
            lblStartDate.TabIndex = 5;
            lblStartDate.Text = "Từ ngày:";
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BorderColor = Color.FromArgb(0, 126, 249);
            btnExport.BorderRadius = 5;
            btnExport.BorderThickness = 1;
            btnExport.Cursor = Cursors.Hand;
            btnExport.CustomizableEdges = customizableEdges1;
            btnExport.FillColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.FromArgb(0, 126, 249);
            btnExport.Location = new Point(800, 15);
            btnExport.Name = "btnExport";
            btnExport.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnExport.Size = new Size(130, 40);
            btnExport.TabIndex = 4;
            btnExport.Text = "Xuất báo cáo";
            // 
            // btnFilter
            // 
            btnFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFilter.BorderRadius = 5;
            btnFilter.Cursor = Cursors.Hand;
            btnFilter.CustomizableEdges = customizableEdges3;
            btnFilter.FillColor = Color.FromArgb(0, 126, 249);
            btnFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFilter.ForeColor = Color.White;
            btnFilter.Location = new Point(670, 15);
            btnFilter.Name = "btnFilter";
            btnFilter.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnFilter.Size = new Size(120, 40);
            btnFilter.TabIndex = 3;
            btnFilter.Text = "Lọc dữ liệu";
            // 
            // dtpEndDate
            // 
            dtpEndDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpEndDate.BorderRadius = 5;
            dtpEndDate.Checked = true;
            dtpEndDate.CustomizableEdges = customizableEdges5;
            dtpEndDate.FillColor = Color.White;
            dtpEndDate.Font = new Font("Segoe UI", 9F);
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(520, 15);
            dtpEndDate.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpEndDate.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.ShadowDecoration.CustomizableEdges = customizableEdges6;
            dtpEndDate.Size = new Size(140, 40);
            dtpEndDate.TabIndex = 2;
            dtpEndDate.Value = new DateTime(2026, 5, 20, 0, 0, 0, 0);
            // 
            // dtpStartDate
            // 
            dtpStartDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpStartDate.BorderRadius = 5;
            dtpStartDate.Checked = true;
            dtpStartDate.CustomizableEdges = customizableEdges7;
            dtpStartDate.FillColor = Color.White;
            dtpStartDate.Font = new Font("Segoe UI", 9F);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(280, 15);
            dtpStartDate.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpStartDate.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.ShadowDecoration.CustomizableEdges = customizableEdges8;
            dtpStartDate.Size = new Size(140, 40);
            dtpStartDate.TabIndex = 1;
            dtpStartDate.Value = new DateTime(2026, 5, 20, 0, 0, 0, 0);
            // 
            // pnlCards
            // 
            pnlCards.Controls.Add(cardOrders);
            pnlCards.Controls.Add(cardRevenue);
            pnlCards.CustomizableEdges = customizableEdges13;
            pnlCards.Dock = DockStyle.Top;
            pnlCards.Location = new Point(20, 90);
            pnlCards.Name = "pnlCards";
            pnlCards.ShadowDecoration.CustomizableEdges = customizableEdges14;
            pnlCards.Size = new Size(950, 120);
            pnlCards.TabIndex = 1;
            // 
            // cardOrders
            // 
            cardOrders.BorderRadius = 10;
            cardOrders.Controls.Add(lblOrdersValue);
            cardOrders.Controls.Add(lblOrdersTitle);
            cardOrders.CustomizableEdges = customizableEdges11;
            cardOrders.FillColor = Color.FromArgb(0, 126, 249);
            cardOrders.Location = new Point(320, 10);
            cardOrders.Name = "cardOrders";
            cardOrders.ShadowDecoration.CustomizableEdges = customizableEdges11;
            cardOrders.Size = new Size(300, 100);
            cardOrders.TabIndex = 1;
            // 
            // lblOrdersValue
            // 
            lblOrdersValue.AutoSize = true;
            lblOrdersValue.BackColor = Color.Transparent;
            lblOrdersValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrdersValue.ForeColor = Color.White;
            lblOrdersValue.Location = new Point(15, 45);
            lblOrdersValue.Name = "lblOrdersValue";
            lblOrdersValue.Size = new Size(33, 37);
            lblOrdersValue.TabIndex = 1;
            lblOrdersValue.Text = "0";
            // 
            // lblOrdersTitle
            // 
            lblOrdersTitle.AutoSize = true;
            lblOrdersTitle.BackColor = Color.Transparent;
            lblOrdersTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblOrdersTitle.ForeColor = Color.White;
            lblOrdersTitle.Location = new Point(15, 15);
            lblOrdersTitle.Name = "lblOrdersTitle";
            lblOrdersTitle.Size = new Size(116, 21);
            lblOrdersTitle.TabIndex = 0;
            lblOrdersTitle.Text = "Tổng đơn hàng";
            // 
            // cardRevenue
            // 
            cardRevenue.BorderRadius = 10;
            cardRevenue.Controls.Add(lblRevenueValue);
            cardRevenue.Controls.Add(lblRevenueTitle);
            cardRevenue.CustomizableEdges = customizableEdges12;
            cardRevenue.FillColor = Color.FromArgb(0, 126, 249);
            cardRevenue.Location = new Point(0, 10);
            cardRevenue.Name = "cardRevenue";
            cardRevenue.ShadowDecoration.CustomizableEdges = customizableEdges12;
            cardRevenue.Size = new Size(300, 100);
            cardRevenue.TabIndex = 0;
            // 
            // lblRevenueValue
            // 
            lblRevenueValue.AutoSize = true;
            lblRevenueValue.BackColor = Color.Transparent;
            lblRevenueValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRevenueValue.ForeColor = Color.White;
            lblRevenueValue.Location = new Point(15, 45);
            lblRevenueValue.Name = "lblRevenueValue";
            lblRevenueValue.Size = new Size(57, 37);
            lblRevenueValue.TabIndex = 1;
            lblRevenueValue.Text = "0 ₫";
            // 
            // lblRevenueTitle
            // 
            lblRevenueTitle.AutoSize = true;
            lblRevenueTitle.BackColor = Color.Transparent;
            lblRevenueTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRevenueTitle.ForeColor = Color.White;
            lblRevenueTitle.Location = new Point(15, 15);
            lblRevenueTitle.Name = "lblRevenueTitle";
            lblRevenueTitle.Size = new Size(120, 21);
            lblRevenueTitle.TabIndex = 0;
            lblRevenueTitle.Text = "Tổng doanh thu";
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvReports);
            pnlGrid.CustomizableEdges = customizableEdges15;
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(20, 210);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(0, 10, 0, 0);
            pnlGrid.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlGrid.Size = new Size(950, 453);
            pnlGrid.TabIndex = 2;
            // 
            // dgvReports
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvReports.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(0, 126, 249);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 126, 249);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvReports.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvReports.ColumnHeadersHeight = 45;
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvReports.DefaultCellStyle = dataGridViewCellStyle3;
            dgvReports.Dock = DockStyle.Fill;
            dgvReports.GridColor = Color.FromArgb(231, 229, 255);
            dgvReports.Location = new Point(0, 10);
            dgvReports.Name = "dgvReports";
            dgvReports.RowHeadersVisible = false;
            dgvReports.RowHeadersWidth = 51;
            dgvReports.RowTemplate.Height = 35;
            dgvReports.Size = new Size(950, 443);
            dgvReports.TabIndex = 0;
            dgvReports.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvReports.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvReports.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvReports.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvReports.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvReports.ThemeStyle.BackColor = Color.White;
            dgvReports.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvReports.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(0, 126, 249);
            dgvReports.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReports.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvReports.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvReports.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReports.ThemeStyle.HeaderStyle.Height = 45;
            dgvReports.ThemeStyle.ReadOnly = false;
            dgvReports.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvReports.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReports.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvReports.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvReports.ThemeStyle.RowsStyle.Height = 35;
            dgvReports.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvReports.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // ucReports
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(pnlGrid);
            Controls.Add(pnlCards);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ucReports";
            Padding = new Padding(20);
            Size = new Size(990, 683);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlCards.ResumeLayout(false);
            cardOrders.ResumeLayout(false);
            cardOrders.PerformLayout();
            cardRevenue.ResumeLayout(false);
            cardRevenue.PerformLayout();
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2Button btnFilter;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpEndDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private Guna.UI2.WinForms.Guna2Panel pnlCards;
        private Guna.UI2.WinForms.Guna2Panel cardRevenue;
        private System.Windows.Forms.Label lblRevenueValue;
        private System.Windows.Forms.Label lblRevenueTitle;
        private Guna.UI2.WinForms.Guna2Panel cardOrders;
        private System.Windows.Forms.Label lblOrdersValue;
        private System.Windows.Forms.Label lblOrdersTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlGrid;
        private Guna.UI2.WinForms.Guna2DataGridView dgvReports;
    }
}

