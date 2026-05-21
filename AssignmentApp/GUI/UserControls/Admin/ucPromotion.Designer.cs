namespace AssignmentApp.GUI.UserControls.Admin
{
    partial class ucPromotion
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            btnAddNewPromotion = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            btnFilters = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            dgvPromotion = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlPagination = new Guna.UI2.WinForms.Guna2Panel();
            btnNext = new Guna.UI2.WinForms.Guna2Button();
            btnPrev = new Guna.UI2.WinForms.Guna2Button();
            lblPageInfo = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPromotion).BeginInit();
            pnlPagination.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnAddNewPromotion);
            pnlHeader.Controls.Add(btnExport);
            pnlHeader.Controls.Add(btnFilters);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.CustomizableEdges = customizableEdges9;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(31, 41);
            pnlHeader.Margin = new Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlHeader.Size = new Size(1081, 107);
            pnlHeader.TabIndex = 0;
            // 
            // btnAddNewPromotion
            // 
            btnAddNewPromotion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewPromotion.BorderRadius = 5;
            btnAddNewPromotion.Cursor = Cursors.Hand;
            btnAddNewPromotion.CustomizableEdges = customizableEdges1;
            btnAddNewPromotion.FillColor = Color.FromArgb(67, 97, 238);
            btnAddNewPromotion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddNewPromotion.ForeColor = Color.White;
            btnAddNewPromotion.Location = new Point(807, 27);
            btnAddNewPromotion.Margin = new Padding(3, 4, 3, 4);
            btnAddNewPromotion.Name = "btnAddNewPromotion";
            btnAddNewPromotion.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAddNewPromotion.Size = new Size(251, 53);
            btnAddNewPromotion.TabIndex = 2;
            btnAddNewPromotion.Text = "+ Thêm mới Khuyến mãi";
            btnAddNewPromotion.Click += btnAddNewPromotion_Click;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BorderColor = Color.LightGray;
            btnExport.BorderRadius = 5;
            btnExport.BorderThickness = 1;
            btnExport.Cursor = Cursors.Hand;
            btnExport.CustomizableEdges = customizableEdges3;
            btnExport.FillColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.FromArgb(64, 64, 64);
            btnExport.Location = new Point(681, 27);
            btnExport.Margin = new Padding(3, 4, 3, 4);
            btnExport.Name = "btnExport";
            btnExport.ShadowDecoration.CustomizableEdges = customizableEdges4;
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
            btnFilters.CustomizableEdges = customizableEdges5;
            btnFilters.FillColor = Color.White;
            btnFilters.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFilters.ForeColor = Color.FromArgb(64, 64, 64);
            btnFilters.Location = new Point(555, 27);
            btnFilters.Margin = new Padding(3, 4, 3, 4);
            btnFilters.Name = "btnFilters";
            btnFilters.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnFilters.Size = new Size(114, 53);
            btnFilters.TabIndex = 4;
            btnFilters.Text = "Bộ lọc";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.BorderRadius = 5;
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.CustomizableEdges = customizableEdges7;
            txtSearch.DefaultText = "";
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(235, 27);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm khuyến mãi...";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtSearch.Size = new Size(309, 53);
            txtSearch.TabIndex = 1;
            // 
            // dgvPromotion
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvPromotion.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvPromotion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(67, 97, 238);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(67, 97, 238);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPromotion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPromotion.ColumnHeadersHeight = 45;
            dgvPromotion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPromotion.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPromotion.GridColor = Color.FromArgb(231, 229, 255);
            dgvPromotion.Location = new Point(266, 164);
            dgvPromotion.Margin = new Padding(3, 4, 3, 4);
            dgvPromotion.Name = "dgvPromotion";
            dgvPromotion.RowHeadersVisible = false;
            dgvPromotion.RowHeadersWidth = 51;
            dgvPromotion.RowTemplate.Height = 35;
            dgvPromotion.Size = new Size(712, 644);
            dgvPromotion.TabIndex = 1;
            dgvPromotion.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvPromotion.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvPromotion.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvPromotion.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvPromotion.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvPromotion.ThemeStyle.BackColor = Color.White;
            dgvPromotion.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvPromotion.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(67, 97, 238);
            dgvPromotion.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPromotion.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPromotion.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPromotion.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvPromotion.ThemeStyle.HeaderStyle.Height = 45;
            dgvPromotion.ThemeStyle.ReadOnly = false;
            dgvPromotion.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvPromotion.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPromotion.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvPromotion.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvPromotion.ThemeStyle.RowsStyle.Height = 35;
            dgvPromotion.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvPromotion.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dgvPromotion.CellContentClick += dgvPromotion_CellContentClick;
            // 
            // pnlPagination
            // 
            pnlPagination.Controls.Add(btnNext);
            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.CustomizableEdges = customizableEdges15;
            pnlPagination.Dock = DockStyle.Bottom;
            pnlPagination.Location = new Point(31, 816);
            pnlPagination.Margin = new Padding(3, 4, 3, 4);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlPagination.Size = new Size(1081, 76);
            pnlPagination.TabIndex = 2;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNext.BorderColor = Color.LightGray;
            btnNext.BorderRadius = 5;
            btnNext.BorderThickness = 1;
            btnNext.Cursor = Cursors.Hand;
            btnNext.CustomizableEdges = customizableEdges11;
            btnNext.FillColor = Color.White;
            btnNext.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNext.ForeColor = Color.FromArgb(64, 64, 64);
            btnNext.Location = new Point(944, 13);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.ShadowDecoration.CustomizableEdges = customizableEdges12;
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
            btnPrev.CustomizableEdges = customizableEdges13;
            btnPrev.FillColor = Color.White;
            btnPrev.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrev.ForeColor = Color.FromArgb(64, 64, 64);
            btnPrev.Location = new Point(818, 13);
            btnPrev.Margin = new Padding(3, 4, 3, 4);
            btnPrev.Name = "btnPrev";
            btnPrev.ShadowDecoration.CustomizableEdges = customizableEdges14;
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
            // ucPromotion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(pnlPagination);
            Controls.Add(dgvPromotion);
            Controls.Add(pnlHeader);
            Margin = new Padding(5, 7, 5, 7);
            Name = "ucPromotion";
            Padding = new Padding(31, 41, 31, 41);
            Size = new Size(1143, 933);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPromotion).EndInit();
            pnlPagination.ResumeLayout(false);
            pnlPagination.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2Button btnAddNewPromotion;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2Button btnFilters;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPromotion;
        private Guna.UI2.WinForms.Guna2Panel pnlPagination;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnPrev;
        private System.Windows.Forms.Label lblPageInfo;
    }
}

