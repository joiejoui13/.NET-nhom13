namespace AssignmentApp.GUI.UserControls.Admin
{
    partial class ucUserManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.cboRole = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnAddNewUser = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.btnFilters = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvUsers = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlPagination = new Guna.UI2.WinForms.Guna2Panel();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.btnPrev = new Guna.UI2.WinForms.Guna2Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.pnlPagination.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.cboRole);
            this.pnlHeader.Controls.Add(this.btnAddNewUser);
            this.pnlHeader.Controls.Add(this.btnExport);
            this.pnlHeader.Controls.Add(this.btnFilters);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // cboRole
            // 
            this.cboRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRole.BackColor = System.Drawing.Color.Transparent;
            this.cboRole.BorderRadius = 5;
            this.cboRole.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.cboRole.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.cboRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboRole.ItemHeight = 34;
            this.cboRole.Items.AddRange(new object[] {
            "Tất cả vai trò",
            "Quản trị viên",
            "Nhân viên",
            "Khách hàng"});
            this.cboRole.Location = new System.Drawing.Point(190, 20);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(150, 40);
            this.cboRole.StartIndex = 0;
            this.cboRole.TabIndex = 5;
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewUser.BorderRadius = 5;
            this.btnAddNewUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddNewUser.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnAddNewUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddNewUser.ForeColor = System.Drawing.Color.White;
            this.btnAddNewUser.Location = new System.Drawing.Point(780, 20);
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.Size = new System.Drawing.Size(200, 40);
            this.btnAddNewUser.TabIndex = 2;
            this.btnAddNewUser.Text = "+ Thêm Người dùng";
            this.btnAddNewUser.Click += new System.EventHandler(this.btnAddNewUser_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BorderColor = System.Drawing.Color.LightGray;
            this.btnExport.BorderRadius = 5;
            this.btnExport.BorderThickness = 1;
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FillColor = System.Drawing.Color.White;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExport.Location = new System.Drawing.Point(670, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 40);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất file";
            // 
            // btnFilters
            // 
            this.btnFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilters.BorderColor = System.Drawing.Color.LightGray;
            this.btnFilters.BorderRadius = 5;
            this.btnFilters.BorderThickness = 1;
            this.btnFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilters.FillColor = System.Drawing.Color.White;
            this.btnFilters.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFilters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFilters.Location = new System.Drawing.Point(560, 20);
            this.btnFilters.Name = "btnFilters";
            this.btnFilters.Size = new System.Drawing.Size(100, 40);
            this.btnFilters.TabIndex = 4;
            this.btnFilters.Text = "Bộ lọc";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderRadius = 5;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.Location = new System.Drawing.Point(350, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderText = "Tìm kiếm tên, email...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(200, 40);
            this.txtSearch.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(225, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Người dùng";
            // 
            // dgvUsers
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUsers.ColumnHeadersHeight = 45;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUsers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvUsers.Location = new System.Drawing.Point(20, 100);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.Size = new System.Drawing.Size(956, 520);
            this.dgvUsers.TabIndex = 1;
            this.dgvUsers.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvUsers.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvUsers.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvUsers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvUsers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvUsers.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvUsers.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvUsers.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.dgvUsers.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvUsers.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvUsers.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvUsers.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvUsers.ThemeStyle.HeaderStyle.Height = 45;
            this.dgvUsers.ThemeStyle.ReadOnly = false;
            this.dgvUsers.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvUsers.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvUsers.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvUsers.ThemeStyle.RowsStyle.Height = 35;
            this.dgvUsers.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvUsers.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // pnlPagination
            // 
            this.pnlPagination.Controls.Add(this.btnNext);
            this.pnlPagination.Controls.Add(this.btnPrev);
            this.pnlPagination.Controls.Add(this.lblPageInfo);
            this.pnlPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPagination.Location = new System.Drawing.Point(0, 640);
            this.pnlPagination.Name = "pnlPagination";
            this.pnlPagination.Size = new System.Drawing.Size(1000, 60);
            this.pnlPagination.TabIndex = 2;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BorderColor = System.Drawing.Color.LightGray;
            this.btnNext.BorderRadius = 5;
            this.btnNext.BorderThickness = 1;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FillColor = System.Drawing.Color.White;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnNext.Location = new System.Drawing.Point(880, 10);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 40);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Trang sau";
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.BorderColor = System.Drawing.Color.LightGray;
            this.btnPrev.BorderRadius = 5;
            this.btnPrev.BorderThickness = 1;
            this.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrev.FillColor = System.Drawing.Color.White;
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPrev.Location = new System.Drawing.Point(770, 10);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(100, 40);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.Text = "Trang trước";
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPageInfo.Location = new System.Drawing.Point(670, 20);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(84, 19);
            this.lblPageInfo.TabIndex = 7;
            this.lblPageInfo.Text = "Trang 1 / 10";
            // 
            // ucUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.pnlPagination);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ucUserManagement";
            this.Size = new System.Drawing.Size(1000, 700);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.pnlPagination.ResumeLayout(false);
            this.pnlPagination.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2ComboBox cboRole;
        private Guna.UI2.WinForms.Guna2Button btnAddNewUser;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2Button btnFilters;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvUsers;
        private Guna.UI2.WinForms.Guna2Panel pnlPagination;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnPrev;
        private System.Windows.Forms.Label lblPageInfo;
    }
}

