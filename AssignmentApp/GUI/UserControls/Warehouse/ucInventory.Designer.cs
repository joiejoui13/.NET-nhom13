namespace AssignmentApp.GUI.UserControls.Warehouse
{
    partial class ucInventory
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            cboCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            btnEdit = new Guna.UI2.WinForms.Guna2Button();
            btndelete = new Guna.UI2.WinForms.Guna2Button();
            btnAdd = new Guna.UI2.WinForms.Guna2Button();
            dgvInventory = new Guna.UI2.WinForms.Guna2DataGridView();
            colProductID = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colProductName = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.CustomizableEdges = customizableEdges1;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(67, 102);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "tìm kiếm văn phòng phẩm";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch.Size = new Size(286, 60);
            txtSearch.TabIndex = 0;
            // 
            // cboCategory
            // 
            cboCategory.BackColor = Color.Transparent;
            cboCategory.CustomizableEdges = customizableEdges3;
            cboCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.FocusedColor = Color.FromArgb(94, 148, 255);
            cboCategory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cboCategory.Font = new Font("Segoe UI", 10F);
            cboCategory.ForeColor = Color.FromArgb(68, 88, 112);
            cboCategory.ItemHeight = 30;
            cboCategory.Location = new Point(639, 117);
            cboCategory.Name = "cboCategory";
            cboCategory.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cboCategory.Size = new Size(175, 36);
            cboCategory.TabIndex = 1;
            // 
            // btnEdit
            // 
            btnEdit.CustomizableEdges = customizableEdges5;
            btnEdit.DisabledState.BorderColor = Color.DarkGray;
            btnEdit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEdit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEdit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEdit.Font = new Font("Segoe UI", 9F);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(376, 460);
            btnEdit.Name = "btnEdit";
            btnEdit.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEdit.Size = new Size(225, 56);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "Sửa thông tin";
            // 
            // btndelete
            // 
            btndelete.CustomizableEdges = customizableEdges7;
            btndelete.DisabledState.BorderColor = Color.DarkGray;
            btndelete.DisabledState.CustomBorderColor = Color.DarkGray;
            btndelete.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btndelete.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btndelete.Font = new Font("Segoe UI", 9F);
            btndelete.ForeColor = Color.White;
            btndelete.Location = new Point(668, 460);
            btndelete.Name = "btndelete";
            btndelete.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btndelete.Size = new Size(225, 56);
            btndelete.TabIndex = 4;
            btndelete.Text = "Xóa sản phẩm";
            // 
            // btnAdd
            // 
            btnAdd.CustomizableEdges = customizableEdges9;
            btnAdd.DisabledState.BorderColor = Color.DarkGray;
            btnAdd.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAdd.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAdd.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAdd.Font = new Font("Segoe UI", 9F);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(85, 460);
            btnAdd.Name = "btnAdd";
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnAdd.Size = new Size(225, 56);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Thêm sản phẩm";
            // 
            // dgvInventory
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvInventory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvInventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvInventory.ColumnHeadersHeight = 22;
            dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvInventory.Columns.AddRange(new DataGridViewColumn[] { colProductID, colUnit, colPrice, colQuantity, colProductName, colCategory });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvInventory.DefaultCellStyle = dataGridViewCellStyle3;
            dgvInventory.Dock = DockStyle.Fill;
            dgvInventory.GridColor = Color.FromArgb(231, 229, 255);
            dgvInventory.Location = new Point(27, 31);
            dgvInventory.Name = "dgvInventory";
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.RowHeadersWidth = 51;
            dgvInventory.Size = new Size(946, 638);
            dgvInventory.TabIndex = 6;
            dgvInventory.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvInventory.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvInventory.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvInventory.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvInventory.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvInventory.ThemeStyle.BackColor = Color.White;
            dgvInventory.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvInventory.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvInventory.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvInventory.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvInventory.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvInventory.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvInventory.ThemeStyle.HeaderStyle.Height = 22;
            dgvInventory.ThemeStyle.ReadOnly = false;
            dgvInventory.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvInventory.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInventory.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvInventory.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvInventory.ThemeStyle.RowsStyle.Height = 29;
            dgvInventory.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvInventory.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // colProductID
            // 
            colProductID.HeaderText = "Mã sản phẩm";
            colProductID.MinimumWidth = 6;
            colProductID.Name = "colProductID";
            // 
            // colUnit
            // 
            colUnit.HeaderText = "Đơn vị tính";
            colUnit.MinimumWidth = 6;
            colUnit.Name = "colUnit";
            // 
            // colPrice
            // 
            colPrice.HeaderText = "Giá bán";
            colPrice.MinimumWidth = 6;
            colPrice.Name = "colPrice";
            // 
            // colQuantity
            // 
            colQuantity.HeaderText = "Số lượng tồn";
            colQuantity.MinimumWidth = 6;
            colQuantity.Name = "colQuantity";
            // 
            // colProductName
            // 
            colProductName.HeaderText = "Tên sản phẩm";
            colProductName.MinimumWidth = 6;
            colProductName.Name = "colProductName";
            // 
            // colCategory
            // 
            colCategory.HeaderText = "Danh mục";
            colCategory.MinimumWidth = 6;
            colCategory.Name = "colCategory";
            // 
            // ucInventory
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            Controls.Add(btnAdd);
            Controls.Add(btndelete);
            Controls.Add(btnEdit);
            Controls.Add(cboCategory);
            Controls.Add(txtSearch);
            Controls.Add(dgvInventory);
            Name = "ucInventory";
            Size = new Size(1000, 700);
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cboCategory;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btndelete;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2DataGridView dgvInventory;
        private DataGridViewTextBoxColumn colProductID;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colQuantity;
        private DataGridViewTextBoxColumn colProductName;
        private DataGridViewTextBoxColumn colCategory;
    }
}
