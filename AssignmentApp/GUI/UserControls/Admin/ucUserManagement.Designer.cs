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
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            cboRole = new Guna.UI2.WinForms.Guna2ComboBox();
            btnAddNewUser = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            btnFilters = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            dgvUsers = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
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
            pnlHeader.Location = new Point(20, 20);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlHeader.Size = new Size(960, 70);
            pnlHeader.TabIndex = 0;
            // 
            // cboRole
            // 
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
            cboRole.Location = new Point(0, 15);
            cboRole.Name = "cboRole";
            cboRole.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cboRole.Size = new Size(160, 40);
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
            btnAddNewUser.Location = new Point(710, 15);
            btnAddNewUser.Name = "btnAddNewUser";
            btnAddNewUser.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAddNewUser.Size = new Size(250, 40);
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
            btnExport.Location = new Point(600, 15);
            btnExport.Name = "btnExport";
            btnExport.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExport.Size = new Size(100, 40);
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
            btnFilters.Location = new Point(490, 15);
            btnFilters.Name = "btnFilters";
            btnFilters.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnFilters.Size = new Size(100, 40);
            btnFilters.TabIndex = 4;
            btnFilters.Text = "Bộ lọc";
            // 
            // txtSearch
            // 
            txtSearch.BorderRadius = 5;
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.CustomizableEdges = customizableEdges9;
            txtSearch.DefaultText = "";
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(170, 15);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm tên, email...";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtSearch.Size = new Size(200, 40);
            txtSearch.TabIndex = 1;
            // 
            // dgvUsers
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.GridColor = Color.FromArgb(231, 229, 255);
            dgvUsers.Location = new Point(20, 20);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.RowTemplate.Height = 35;
            dgvUsers.Size = new Size(960, 660);
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
            // ucUserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(pnlHeader);
            Controls.Add(dgvUsers);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ucUserManagement";
            Padding = new Padding(20);
            Size = new Size(1000, 700);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
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
    }
}

