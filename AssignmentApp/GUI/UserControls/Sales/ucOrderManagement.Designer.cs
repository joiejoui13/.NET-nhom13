namespace AssignmentApp.GUI.UserControls.Sales
{
    partial class ucOrderManagement
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

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvOrders = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtMaHD = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMaKH = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMaND = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMaKM = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtGiamGia = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMaGiaoHang = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTongTien = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbHinhThucThanhToan = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnAdd = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnViewDetails = new Guna.UI2.WinForms.Guna2Button();
            this.cbTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvOrders.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOrders.ColumnHeadersHeight = 35;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrders.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvOrders.Location = new System.Drawing.Point(0, 200);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.RowTemplate.Height = 35;
            this.dgvOrders.Size = new System.Drawing.Size(1000, 500);
            this.dgvOrders.TabIndex = 1;
            this.dgvOrders.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvOrders.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvOrders.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellClick);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.cbTrangThai);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.txtMaHD);
            this.pnlTop.Controls.Add(this.txtMaKH);
            this.pnlTop.Controls.Add(this.txtMaND);
            this.pnlTop.Controls.Add(this.txtMaKM);
            this.pnlTop.Controls.Add(this.txtGiamGia);
            this.pnlTop.Controls.Add(this.txtMaGiaoHang);
            this.pnlTop.Controls.Add(this.txtTongTien);
            this.pnlTop.Controls.Add(this.cbHinhThucThanhToan);
            this.pnlTop.Controls.Add(this.btnAdd);
            this.pnlTop.Controls.Add(this.btnUpdate);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnViewDetails);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 200);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(222, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ HÓA ĐƠN";
            // 
            // txtMaHD
            // 
            this.txtMaHD.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaHD.DefaultText = "";
            this.txtMaHD.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaHD.Location = new System.Drawing.Point(25, 60);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.PlaceholderText = "Mã Hóa Đơn (Tự tạo)";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(150, 36);
            this.txtMaHD.TabIndex = 1;
            // 
            // txtMaKH
            // 
            this.txtMaKH.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaKH.DefaultText = "";
            this.txtMaKH.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaKH.Location = new System.Drawing.Point(185, 60);
            this.txtMaKH.Name = "txtMaKH";
            this.txtMaKH.PlaceholderText = "Mã Khách Hàng";
            this.txtMaKH.Size = new System.Drawing.Size(150, 36);
            this.txtMaKH.TabIndex = 2;
            // 
            // txtMaND
            // 
            this.txtMaND.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaND.DefaultText = "";
            this.txtMaND.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaND.Location = new System.Drawing.Point(345, 60);
            this.txtMaND.Name = "txtMaND";
            this.txtMaND.PlaceholderText = "Mã Nhân Viên";
            this.txtMaND.Size = new System.Drawing.Size(150, 36);
            this.txtMaND.TabIndex = 3;
            // 
            // txtMaKM
            // 
            this.txtMaKM.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaKM.DefaultText = "";
            this.txtMaKM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaKM.Location = new System.Drawing.Point(505, 60);
            this.txtMaKM.Name = "txtMaKM";
            this.txtMaKM.PlaceholderText = "Mã Khuyến Mãi";
            this.txtMaKM.Size = new System.Drawing.Size(150, 36);
            this.txtMaKM.TabIndex = 4;
            // 
            // txtGiamGia
            // 
            this.txtGiamGia.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiamGia.DefaultText = "0";
            this.txtGiamGia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGiamGia.Location = new System.Drawing.Point(665, 60);
            this.txtGiamGia.Name = "txtGiamGia";
            this.txtGiamGia.PlaceholderText = "Giảm Giá";
            this.txtGiamGia.Size = new System.Drawing.Size(150, 36);
            this.txtGiamGia.TabIndex = 5;
            // 
            // txtMaGiaoHang
            // 
            this.txtMaGiaoHang.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaGiaoHang.DefaultText = "";
            this.txtMaGiaoHang.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaGiaoHang.Location = new System.Drawing.Point(825, 60);
            this.txtMaGiaoHang.Name = "txtMaGiaoHang";
            this.txtMaGiaoHang.PlaceholderText = "Mã Giao Hàng";
            this.txtMaGiaoHang.Size = new System.Drawing.Size(150, 36);
            this.txtMaGiaoHang.TabIndex = 6;
            // 
            // txtTongTien
            // 
            this.txtTongTien.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTongTien.DefaultText = "0";
            this.txtTongTien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTongTien.Location = new System.Drawing.Point(25, 105);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.PlaceholderText = "Tổng Tiền (Đọc)";
            this.txtTongTien.ReadOnly = true;
            this.txtTongTien.Size = new System.Drawing.Size(150, 36);
            this.txtTongTien.TabIndex = 7;
            // 
            // cbHinhThucThanhToan
            // 
            this.cbHinhThucThanhToan.BackColor = System.Drawing.Color.Transparent;
            this.cbHinhThucThanhToan.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbHinhThucThanhToan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHinhThucThanhToan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbHinhThucThanhToan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbHinhThucThanhToan.ItemHeight = 30;
            this.cbHinhThucThanhToan.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản",
            "Thẻ ngân hàng",
            "Ví điện tử"});
            this.cbHinhThucThanhToan.Location = new System.Drawing.Point(185, 105);
            this.cbHinhThucThanhToan.Name = "cbHinhThucThanhToan";
            this.cbHinhThucThanhToan.Size = new System.Drawing.Size(150, 36);
            this.cbHinhThucThanhToan.TabIndex = 8;
            // 
            // cbTrangThai
            // 
            this.cbTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cbTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbTrangThai.ItemHeight = 30;
            this.cbTrangThai.Items.AddRange(new object[] {
            "Chờ xử lý",
            "Đang giao",
            "Đã giao",
            "Hoàn thành",
            "Hủy"});
            this.cbTrangThai.Location = new System.Drawing.Point(345, 105);
            this.cbTrangThai.Name = "cbTrangThai";
            this.cbTrangThai.Size = new System.Drawing.Size(150, 36);
            this.cbTrangThai.TabIndex = 14;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(25, 150);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 36);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Thêm HĐ";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(135, 150);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 36);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Cập Nhật";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FillColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(245, 150);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 36);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.FillColor = System.Drawing.Color.Green;
            this.btnViewDetails.Location = new System.Drawing.Point(355, 150);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(120, 36);
            this.btnViewDetails.TabIndex = 12;
            this.btnViewDetails.Text = "Xem Chi Tiết";
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(485, 150);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 36);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Làm Mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ucOrderManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.pnlTop);
            this.Name = "ucOrderManagement";
            this.Size = new System.Drawing.Size(1000, 700);
            this.Load += new System.EventHandler(this.ucOrderManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvOrders;
        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtMaHD;
        private Guna.UI2.WinForms.Guna2TextBox txtMaKH;
        private Guna.UI2.WinForms.Guna2TextBox txtMaND;
        private Guna.UI2.WinForms.Guna2TextBox txtMaKM;
        private Guna.UI2.WinForms.Guna2TextBox txtGiamGia;
        private Guna.UI2.WinForms.Guna2TextBox txtMaGiaoHang;
        private Guna.UI2.WinForms.Guna2TextBox txtTongTien;
        private Guna.UI2.WinForms.Guna2ComboBox cbHinhThucThanhToan;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnViewDetails;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2ComboBox cbTrangThai;
    }
}

