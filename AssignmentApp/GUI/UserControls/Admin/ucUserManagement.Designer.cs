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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            cboRole = new Guna.UI2.WinForms.Guna2ComboBox();
            btnAddNewUser = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            btnFilters = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            dgvUsers = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlPagination = new Guna.UI2.WinForms.Guna2Panel();
            btnNext = new Guna.UI2.WinForms.Guna2Button();
            btnPrev = new Guna.UI2.WinForms.Guna2Button();
            lblPageInfo = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlPagination.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(cboRole);
            pnlHeader.Controls.Add(btnAddNewUser);
            pnlHeader.Controls.Add(btnExport);
            pnlHeader.Controls.Add(btnFilters);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.CustomizableEdges = customizableEdges11;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(31, 41);
            pnlHeader.Margin = new Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlHeader.Size = new Size(1081, 107);
            pnlHeader.TabIndex = 0;
            // 
            // cboRole
            // 
            cboRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboRole.BackColor = Color.Transparent;
            cboRole.BorderRadius = 5;
            cboRole.CustomizableEdges = customizableEdges1;
            cboRole.DrawMode = DrawMode.OwnerDrawFixed;
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.FocusedColor = Color.FromArgb(0, 126, 249);
            cboRole.FocusedState.BorderColor = Color.FromArgb(0, 126, 249);
            cboRole.Font = new Font("Segoe UI", 10F);
            cboRole.ForeColor = Color.FromArgb(68, 88, 112);
            cboRole.ItemHeight = 34;
            cboRole.Items.AddRange(new object[] { "Tất cả vai trò", "Quản trị viên", "Nhân viên", "Khách hàng" });
            cboRole.Location = new Point(155, 27);
            cboRole.Margin = new Padding(3, 4, 3, 4);
            cboRole.Name = "cboRole";
            cboRole.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cboRole.Size = new Size(171, 40);
            cboRole.StartIndex = 0;
            cboRole.TabIndex = 5;
            // 
            // btnAddNewUser
            // 
            btnAddNewUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewUser.BorderRadius = 5;
            btnAddNewUser.Cursor = Cursors.Hand;
            btnAddNewUser.CustomizableEdges = customizableEdges3;
            btnAddNewUser.FillColor = Color.FromArgb(0, 126, 249);
            btnAddNewUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddNewUser.ForeColor = Color.White;
            btnAddNewUser.Location = new Point(829, 27);
            btnAddNewUser.Margin = new Padding(3, 4, 3, 4);
            btnAddNewUser.Name = "btnAddNewUser";
            btnAddNewUser.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAddNewUser.Size = new Size(229, 53);
            btnAddNewUser.TabIndex = 2;
            btnAddNewUser.Text = "+ Thêm Người dùng";
            btnAddNewUser.Click += btnAddNewUser_Click;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BorderColor = Color.LightGray;
            btnExport.BorderRadius = 5;
            btnExport.BorderThickness = 1;
            btnExport.Cursor = Cursors.Hand;
            btnExport.CustomizableEdges = customizableEdges5;
            btnExport.FillColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.FromArgb(64, 64, 64);
            btnExport.Location = new Point(704, 27);
            btnExport.Margin = new Padding(3, 4, 3, 4);
            btnExport.Name = "btnExport";
            btnExport.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExport.Size = new Size(114, 53);
            btnExport.TabIndex = 3;
            btnExport.Text = "Xuất file";
            // 
            // btnFilters
            // 
            btnFilters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFilters.BorderColor = Color.LightGray;
            btnFilters.BorderRadius = 5;
            btnFilters.BorderThickness = 1;
            btnFilters.Cursor = Cursors.Hand;
            btnFilters.CustomizableEdges = customizableEdges7;
            btnFilters.FillColor = Color.White;
            btnFilters.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFilters.ForeColor = Color.FromArgb(64, 64, 64);
            btnFilters.Location = new Point(578, 27);
            btnFilters.Margin = new Padding(3, 4, 3, 4);
            btnFilters.Name = "btnFilters";
            btnFilters.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnFilters.Size = new Size(114, 53);
            btnFilters.TabIndex = 4;
            btnFilters.Text = "Bộ lọc";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.BorderRadius = 5;
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.CustomizableEdges = customizableEdges9;
            txtSearch.DefaultText = "";
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(338, 27);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm tên, email...";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtSearch.Size = new Size(229, 53);
            txtSearch.TabIndex = 1;
            // 
            // dgvUsers
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(0, 126, 249);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 126, 249);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.ColumnHeadersHeight = 45;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvUsers.GridColor = Color.FromArgb(231, 229, 255);
            dgvUsers.Location = new Point(23, 133);
            dgvUsers.Margin = new Padding(3, 4, 3, 4);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.RowTemplate.Height = 35;
            dgvUsers.Size = new Size(712, 693);
            dgvUsers.TabIndex = 1;
            dgvUsers.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvUsers.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvUsers.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvUsers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvUsers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvUsers.ThemeStyle.BackColor = Color.White;
            dgvUsers.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvUsers.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(0, 126, 249);
            dgvUsers.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsers.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvUsers.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvUsers.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvUsers.ThemeStyle.HeaderStyle.Height = 45;
            dgvUsers.ThemeStyle.ReadOnly = false;
            dgvUsers.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvUsers.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsers.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvUsers.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvUsers.ThemeStyle.RowsStyle.Height = 35;
            dgvUsers.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvUsers.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dgvUsers.CellContentClick += dgvUsers_CellContentClick;
            // 
            // pnlPagination
            // 
            pnlPagination.Controls.Add(btnNext);
            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.CustomizableEdges = customizableEdges17;
            pnlPagination.Dock = DockStyle.Bottom;
            pnlPagination.Location = new Point(31, 812);
            pnlPagination.Margin = new Padding(3, 4, 3, 4);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.ShadowDecoration.CustomizableEdges = customizableEdges18;
            pnlPagination.Size = new Size(1081, 80);
            pnlPagination.TabIndex = 2;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNext.BorderColor = Color.LightGray;
            btnNext.BorderRadius = 5;
            btnNext.BorderThickness = 1;
            btnNext.Cursor = Cursors.Hand;
            btnNext.CustomizableEdges = customizableEdges13;
            btnNext.FillColor = Color.White;
            btnNext.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNext.ForeColor = Color.FromArgb(64, 64, 64);
            btnNext.Location = new Point(944, 13);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnNext.Size = new Size(114, 53);
            btnNext.TabIndex = 5;
            btnNext.Text = "Trang sau";
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrev.BorderColor = Color.LightGray;
            btnPrev.BorderRadius = 5;
            btnPrev.BorderThickness = 1;
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.CustomizableEdges = customizableEdges15;
            btnPrev.FillColor = Color.White;
            btnPrev.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrev.ForeColor = Color.FromArgb(64, 64, 64);
            btnPrev.Location = new Point(818, 13);
            btnPrev.Margin = new Padding(3, 4, 3, 4);
            btnPrev.Name = "btnPrev";
            btnPrev.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnPrev.Size = new Size(114, 53);
            btnPrev.TabIndex = 6;
            btnPrev.Text = "Trang trước";
            // 
            // lblPageInfo
            // 
            lblPageInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPageInfo.AutoSize = true;
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPageInfo.ForeColor = Color.FromArgb(64, 64, 64);
            lblPageInfo.Location = new Point(704, 27);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(102, 23);
            lblPageInfo.TabIndex = 7;
            lblPageInfo.Text = "Trang 1 / 10";
            // 
            // ucUserManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(pnlPagination);
            Controls.Add(dgvUsers);
            Controls.Add(pnlHeader);
            Margin = new Padding(5, 7, 5, 7);
            Name = "ucUserManagement";
            Padding = new Padding(31, 41, 31, 41);
            Size = new Size(1143, 933);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            pnlPagination.ResumeLayout(false);
            pnlPagination.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2ComboBox cboRole;
        private Guna.UI2.WinForms.Guna2Button btnAddNewUser;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2Button btnFilters;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvUsers;
        private Guna.UI2.WinForms.Guna2Panel pnlPagination;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnPrev;
        private System.Windows.Forms.Label lblPageInfo;
    }
}

