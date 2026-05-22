namespace AssignmentApp.GUI.UserControls.Sales
{
    partial class ucCustomer
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dgvCustomers = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            lblTitle = new Label();
            txtMaKH = new Guna.UI2.WinForms.Guna2TextBox();
            txtTenKH = new Guna.UI2.WinForms.Guna2TextBox();
            txtSDT = new Guna.UI2.WinForms.Guna2TextBox();
            txtDiem = new Guna.UI2.WinForms.Guna2TextBox();
            btnAdd = new Guna.UI2.WinForms.Guna2Button();
            btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            guna2HtmlLabel1 = new System.Windows.Forms.Label();
            guna2HtmlLabel2 = new System.Windows.Forms.Label();
            guna2HtmlLabel3 = new System.Windows.Forms.Label();
            guna2HtmlLabel4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCustomers
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCustomers.ColumnHeadersHeight = 35;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCustomers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.GridColor = Color.FromArgb(231, 229, 255);
            dgvCustomers.Location = new Point(20, 220);
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowHeadersWidth = 51;
            dgvCustomers.RowTemplate.Height = 35;
            dgvCustomers.Size = new Size(960, 460);
            dgvCustomers.TabIndex = 1;
            dgvCustomers.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvCustomers.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvCustomers.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvCustomers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvCustomers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvCustomers.ThemeStyle.BackColor = Color.White;
            dgvCustomers.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvCustomers.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvCustomers.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCustomers.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvCustomers.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCustomers.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.ThemeStyle.HeaderStyle.Height = 35;
            dgvCustomers.ThemeStyle.ReadOnly = true;
            dgvCustomers.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCustomers.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCustomers.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvCustomers.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvCustomers.ThemeStyle.RowsStyle.Height = 35;
            dgvCustomers.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvCustomers.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.Controls.Add(guna2HtmlLabel4);
            pnlTop.Controls.Add(guna2HtmlLabel3);
            pnlTop.Controls.Add(guna2HtmlLabel2);
            pnlTop.Controls.Add(guna2HtmlLabel1);
            pnlTop.Controls.Add(txtMaKH);
            pnlTop.Controls.Add(txtTenKH);
            pnlTop.Controls.Add(txtSDT);
            pnlTop.Controls.Add(txtDiem);
            pnlTop.Controls.Add(btnAdd);
            pnlTop.Controls.Add(btnUpdate);
            pnlTop.Controls.Add(btnRefresh);
            pnlTop.CustomizableEdges = customizableEdges15;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(20, 20);
            pnlTop.Name = "pnlTop";
            pnlTop.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlTop.Size = new Size(960, 200);
            pnlTop.TabIndex = 0;
            pnlTop.Paint += pnlTop_Paint;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(43, 37);
            lblTitle.Margin = new Padding(6, 0, 6, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(574, 65);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ KHÁCH HÀNG";
            // 
            // txtMaKH
            // 
            txtMaKH.Cursor = Cursors.IBeam;
            txtMaKH.CustomizableEdges = customizableEdges1;
            txtMaKH.DefaultText = "";
            txtMaKH.Font = new Font("Segoe UI", 9F);
            txtMaKH.Location = new Point(150, 15);
            txtMaKH.Name = "txtMaKH";
            txtMaKH.PlaceholderText = "Mã Khách Hàng (Tự tạo)";
            txtMaKH.ReadOnly = true;
            txtMaKH.SelectedText = "";
            txtMaKH.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtMaKH.Size = new Size(200, 36);
            txtMaKH.TabIndex = 1;
            // 
            // txtTenKH
            // 
            txtTenKH.Cursor = Cursors.IBeam;
            txtTenKH.CustomizableEdges = customizableEdges3;
            txtTenKH.DefaultText = "";
            txtTenKH.Font = new Font("Segoe UI", 9F);
            txtTenKH.Location = new Point(520, 15);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.PlaceholderText = "Tên Khách Hàng";
            txtTenKH.SelectedText = "";
            txtTenKH.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtTenKH.Size = new Size(200, 36);
            txtTenKH.TabIndex = 2;
            // 
            // txtSDT
            // 
            txtSDT.Cursor = Cursors.IBeam;
            txtSDT.CustomizableEdges = customizableEdges5;
            txtSDT.DefaultText = "";
            txtSDT.Font = new Font("Segoe UI", 9F);
            txtSDT.Location = new Point(150, 65);
            txtSDT.Name = "txtSDT";
            txtSDT.PlaceholderText = "Số Điện Thoại";
            txtSDT.SelectedText = "";
            txtSDT.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtSDT.Size = new Size(200, 36);
            txtSDT.TabIndex = 3;
            // 
            // txtDiem
            // 
            txtDiem.Cursor = Cursors.IBeam;
            txtDiem.CustomizableEdges = customizableEdges7;
            txtDiem.DefaultText = "";
            txtDiem.Font = new Font("Segoe UI", 9F);
            txtDiem.Location = new Point(520, 65);
            txtDiem.Name = "txtDiem";
            txtDiem.PlaceholderText = "Điểm Tích Lũy (Số)";
            txtDiem.SelectedText = "";
            txtDiem.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtDiem.Size = new Size(200, 36);
            txtDiem.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.CustomizableEdges = customizableEdges9;
            btnAdd.Font = new Font("Segoe UI", 9F);
            btnAdd.ForeColor = Color.White;
            btnAdd.FillColor = Color.FromArgb(0, 126, 249);
            btnAdd.BorderRadius = 4;
            btnAdd.Location = new Point(150, 125);
            btnAdd.Name = "btnAdd";
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnAdd.Size = new Size(120, 42);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Thêm Mới";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.CustomizableEdges = customizableEdges11;
            btnUpdate.Font = new Font("Segoe UI", 9F);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FillColor = Color.FromArgb(0, 126, 249);
            btnUpdate.BorderRadius = 4;
            btnUpdate.Location = new Point(290, 125);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnUpdate.Size = new Size(120, 42);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "Cập Nhật";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.CustomizableEdges = customizableEdges13;
            btnRefresh.Font = new Font("Segoe UI", 9F);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FillColor = Color.FromArgb(0, 126, 249);
            btnRefresh.BorderRadius = 4;
            btnRefresh.Location = new Point(430, 125);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnRefresh.Size = new Size(120, 42);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Làm Mới";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Location = new Point(20, 23);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(120, 20);
            guna2HtmlLabel1.TabIndex = 8;
            guna2HtmlLabel1.Text = "Mã khách hàng";
            guna2HtmlLabel1.AutoSize = true;
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Location = new Point(20, 73);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(120, 20);
            guna2HtmlLabel2.TabIndex = 9;
            guna2HtmlLabel2.Text = "Số điện thoại";
            guna2HtmlLabel2.AutoSize = true;
            // 
            // guna2HtmlLabel3
            // 
            guna2HtmlLabel3.BackColor = Color.Transparent;
            guna2HtmlLabel3.Location = new Point(390, 23);
            guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            guna2HtmlLabel3.Size = new Size(120, 20);
            guna2HtmlLabel3.TabIndex = 10;
            guna2HtmlLabel3.Text = "Tên khách hàng";
            guna2HtmlLabel3.AutoSize = true;
            // 
            // guna2HtmlLabel4
            // 
            guna2HtmlLabel4.BackColor = Color.Transparent;
            guna2HtmlLabel4.Location = new Point(390, 73);
            guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            guna2HtmlLabel4.Size = new Size(120, 20);
            guna2HtmlLabel4.TabIndex = 11;
            guna2HtmlLabel4.Text = "Điểm tích lũy";
            guna2HtmlLabel4.AutoSize = true;
            // 
            // ucCustomer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(dgvCustomers);
            Controls.Add(pnlTop);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ucCustomer";
            Padding = new Padding(20);
            Size = new Size(1000, 700);
            Load += ucCustomer_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvCustomers;
        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtMaKH;
        private Guna.UI2.WinForms.Guna2TextBox txtTenKH;
        private Guna.UI2.WinForms.Guna2TextBox txtSDT;
        private Guna.UI2.WinForms.Guna2TextBox txtDiem;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.Label guna2HtmlLabel4;
        private System.Windows.Forms.Label guna2HtmlLabel3;
        private System.Windows.Forms.Label guna2HtmlLabel2;
        private System.Windows.Forms.Label guna2HtmlLabel1;
    }
}

