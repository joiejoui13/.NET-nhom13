using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Import DbContext kết nối SQL

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        // Biến toàn cục dùng để lưu đường dẫn ảnh hiện tại của sản phẩm
        private string currentImagePath = "";

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo UserControl Danh sách Sản phẩm.
        /// Chạy đầu tiên để vẽ giao diện và gán các cấu hình tĩnh.
        /// </summary>
        public ucProductList()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Thêm các tùy chọn cố định (Tách từ Designer)
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang bán", "Ngưng bán" });
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; // Cấm người dùng gõ phím tinh tinh
        }

        /// <summary>
        /// Sự kiện Load Form: Kích hoạt khi giao diện hiển thị.
        /// Chuyên dùng để kết nối CSDL, kéo dữ liệu vào lưới và ComboBox.
        /// </summary>
        private void ucProductList_Load(object sender, EventArgs e)
        {
            // 1. Khởi tạo kết nối DB
            DbContext.Ketnoi();

            // 2. Nạp dữ liệu vào ComboBox Danh Mục từ CSDL
            Load_cboDanhMuc();

            // 3. Tải toàn bộ danh sách Sản phẩm lên DataGridView
            LoadProductsGrid(null);

            // 4. Khóa các ô nhập liệu và xóa sạch dữ liệu cũ trên form
            ClearInputs();
            ToggleInputs(false);
            
            // 5. Cấu hình trạng thái các nút bấm ban đầu
            btnAdd.Enabled = true;          // Được quyền bấm Thêm mới
            btnEdit.Enabled = false;        // Chưa chọn SP nào nên cấm Sửa
            btnDelete.Enabled = false;      // Cấm Xóa
            btnSave.Enabled = false;        // Đang không thao tác nên cấm Lưu
            btnCancel.Enabled = false;      // Cấm Hủy
            txtMaSanPham.Enabled = false;   // Mã SP do CSDL cấp, cấm gõ tay
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Tải toàn bộ Danh mục từ bảng DanhMuc lên ComboBox để người dùng có thể phân loại sản phẩm.
        /// </summary>
        private void Load_cboDanhMuc()
        {
            string sql = "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY TenDanhMuc ASC";
            DataTable tblDanhMuc = DbContext.GetDataToTable(sql);
            
            cboDanhMuc.DataSource = tblDanhMuc;
            cboDanhMuc.DisplayMember = "TenDanhMuc"; // Tên hiển thị ra cho người dùng đọc
            cboDanhMuc.ValueMember = "MaDanhMuc";   // Mã chìm bên dưới để lưu CSDL
            cboDanhMuc.SelectedIndex = -1;          // Mặc định không chọn danh mục nào
        }

        /// <summary>
        /// Kéo dữ liệu Sản Phẩm từ CSDL và hiển thị lên lưới.
        /// </summary>
        private void LoadProductsGrid(DataTable customTable)
        {
            DataTable tblProducts;

            // Nếu người dùng có truyền customTable (Ví dụ: dữ liệu sau khi Tìm kiếm) thì dùng nó
            if (customTable != null)
            {
                tblProducts = customTable;
            }
            else
            {
                // Truy vấn JOIN 2 bảng để lấy Tên Danh Mục thay vì chỉ hiện Mã Danh Mục vô hồn
                string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                               FROM SanPham s 
                               LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc
                               ORDER BY s.NgayTao DESC";
                tblProducts = DbContext.GetDataToTable(sql);
            }

            // Tắt chức năng tự đẻ cột của DataGridView để không làm hỏng thiết kế
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.DataSource = tblProducts;

            // BINDING DỮ LIỆU: Cột nào ăn theo dữ liệu nào
            if (dgvSanPham.Columns.Contains("colMaSanPham")) dgvSanPham.Columns["colMaSanPham"].DataPropertyName = "MaSanPham";
            if (dgvSanPham.Columns.Contains("colTenSanPham")) dgvSanPham.Columns["colTenSanPham"].DataPropertyName = "TenSanPham";
            if (dgvSanPham.Columns.Contains("colMaDanhMuc")) dgvSanPham.Columns["colMaDanhMuc"].DataPropertyName = "TenDanhMuc"; 
            if (dgvSanPham.Columns.Contains("colGiaNhap")) dgvSanPham.Columns["colGiaNhap"].DataPropertyName = "GiaNhap";
            if (dgvSanPham.Columns.Contains("colGiaBan")) dgvSanPham.Columns["colGiaBan"].DataPropertyName = "GiaBan";
            if (dgvSanPham.Columns.Contains("colSoLuongTon")) dgvSanPham.Columns["colSoLuongTon"].DataPropertyName = "SoLuongTon";
            if (dgvSanPham.Columns.Contains("colTrangThai")) dgvSanPham.Columns["colTrangThai"].DataPropertyName = "TrangThai";
            if (dgvSanPham.Columns.Contains("colNgayTao")) dgvSanPham.Columns["colNgayTao"].DataPropertyName = "NgayTao";

            // ĐỊNH DẠNG SỐ TIỀN VÀ NGÀY THÁNG
            if (dgvSanPham.Columns.Contains("colGiaNhap")) dgvSanPham.Columns["colGiaNhap"].DefaultCellStyle.Format = "N0"; // "N0" tự động thêm dấu phẩy ngăn cách hàng nghìn
            if (dgvSanPham.Columns.Contains("colGiaBan")) dgvSanPham.Columns["colGiaBan"].DefaultCellStyle.Format = "N0";
            if (dgvSanPham.Columns.Contains("colNgayTao")) dgvSanPham.Columns["colNgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // TẮT CHẾ ĐỘ AUTOSIZE TOÀN CỤC VÀ ĐỊNH RỘNG LẠI TỪNG CỘT
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dgvSanPham.Columns.Contains("colMaSanPham"))
            {
                dgvSanPham.Columns["colMaSanPham"].Width = 70;
                dgvSanPham.Columns["colMaSanPham"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvSanPham.Columns.Contains("colTenSanPham"))
            {
                dgvSanPham.Columns["colTenSanPham"].MinimumWidth = 180;
                dgvSanPham.Columns["colTenSanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Tự co giãn cột tên sản phẩm
            }
            if (dgvSanPham.Columns.Contains("colMaDanhMuc"))
            {
                dgvSanPham.Columns["colMaDanhMuc"].Width = 110;
                dgvSanPham.Columns["colMaDanhMuc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvSanPham.Columns.Contains("colGiaNhap"))
            {
                dgvSanPham.Columns["colGiaNhap"].Width = 95;
                dgvSanPham.Columns["colGiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvSanPham.Columns.Contains("colGiaBan"))
            {
                dgvSanPham.Columns["colGiaBan"].Width = 95;
                dgvSanPham.Columns["colGiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvSanPham.Columns.Contains("colSoLuongTon"))
            {
                dgvSanPham.Columns["colSoLuongTon"].Width = 70;
                dgvSanPham.Columns["colSoLuongTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvSanPham.Columns.Contains("colTrangThai"))
            {
                dgvSanPham.Columns["colTrangThai"].Width = 100;
                dgvSanPham.Columns["colTrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvSanPham.Columns.Contains("colNgayTao"))
            {
                dgvSanPham.Columns["colNgayTao"].Width = 110;
                dgvSanPham.Columns["colNgayTao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Đồng bộ giao diện Row/Header
            dgvSanPham.RowTemplate.Height = 40;
            dgvSanPham.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvSanPham.ColumnHeadersHeight = 40;
            dgvSanPham.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; 
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        /// <summary>
        /// Mở / Khóa các ô TextBox để người dùng nhập liệu.
        /// </summary>
        private void ToggleInputs(bool isEnabled)
        {
            txtTenSanPham.Enabled = isEnabled;
            txtGiaNhap.Enabled = isEnabled;
            txtGiaBan.Enabled = isEnabled;
            txtSoLuongTon.Enabled = isEnabled;
            txtMoTa.Enabled = isEnabled;
            cboDanhMuc.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
            btnChonAnh.Enabled = isEnabled;
        }

        /// <summary>
        /// Dọn dẹp trắng tinh mọi thứ.
        /// </summary>
        private void ClearInputs()
        {
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtGiaNhap.Text = "0";
            txtGiaBan.Text = "0";
            txtSoLuongTon.Text = "0";
            txtMoTa.Text = "";
            
            if (cboDanhMuc.Items.Count > 0) 
            {
                cboDanhMuc.SelectedIndex = -1;
            }
            
            if (cboTrangThai.Items.Count > 0) 
            {
                cboTrangThai.SelectedIndex = 0; // Mặc định Đang bán
            }

            currentImagePath = "";
            LoadProductImage("");
        }

        /// <summary>
        /// Khi người dùng click chuột vào lưới, hàm này sẽ lấy dữ liệu của nguyên dòng đó từ DB đổ lên Panel bên phải.
        /// </summary>
        private void SelectProductRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSanPham.Rows.Count) 
            {
                return;
            }

            dgvSanPham.ClearSelection();
            dgvSanPham.Rows[rowIndex].Selected = true;

            // Ép kiểu an toàn (int.TryParse)
            int prodId = 0;
            if (int.TryParse(dgvSanPham.Rows[rowIndex].Cells["colMaSanPham"].Value.ToString(), out prodId) == false)
            {
                return;
            }
            
            string sql = $@"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE s.MaSanPham = {prodId}";
            DataTable tbl = DbContext.GetDataToTable(sql);

            if (tbl.Rows.Count > 0)
            {
                DataRow r = tbl.Rows[0];
                
                // Sử dụng cấu trúc IF cơ bản, tránh dùng toán tử lạ "??" hoặc "?." để thân thiện với newbie
                txtMaSanPham.Text = r["MaSanPham"].ToString();
                
                if (r["TenSanPham"] != DBNull.Value) txtTenSanPham.Text = r["TenSanPham"].ToString();
                else txtTenSanPham.Text = "";

                if (r["GiaNhap"] != DBNull.Value) txtGiaNhap.Text = r["GiaNhap"].ToString();
                else txtGiaNhap.Text = "0";

                if (r["GiaBan"] != DBNull.Value) txtGiaBan.Text = r["GiaBan"].ToString();
                else txtGiaBan.Text = "0";

                if (r["SoLuongTon"] != DBNull.Value) txtSoLuongTon.Text = r["SoLuongTon"].ToString();
                else txtSoLuongTon.Text = "0";

                if (r["MoTa"] != DBNull.Value) txtMoTa.Text = r["MoTa"].ToString();
                else txtMoTa.Text = "";
                
                if (r["MaDanhMuc"] != DBNull.Value)
                {
                    cboDanhMuc.SelectedValue = Convert.ToInt32(r["MaDanhMuc"]);
                }
                else
                {
                    cboDanhMuc.SelectedIndex = -1;
                }

                if (r["TrangThai"] != DBNull.Value) cboTrangThai.Text = r["TrangThai"].ToString();
                else cboTrangThai.Text = "Đang bán";

                if (r["Anh"] != DBNull.Value) currentImagePath = r["Anh"].ToString();
                else currentImagePath = "";

                LoadProductImage(currentImagePath);

                // Cập nhật Nhãn Tab Chi Tiết Bên Phải (Product Details Card)
                double giaBan = 0;
                if (r["GiaBan"] != DBNull.Value) giaBan = Convert.ToDouble(r["GiaBan"]);

                int soLuongTon = 0;
                if (r["SoLuongTon"] != DBNull.Value) soLuongTon = Convert.ToInt32(r["SoLuongTon"]);

                string tenSP = "";
                if (r["TenSanPham"] != DBNull.Value) tenSP = r["TenSanPham"].ToString();

                string tenDanhMuc = "Không rõ";
                if (r["TenDanhMuc"] != DBNull.Value) tenDanhMuc = r["TenDanhMuc"].ToString();

                double giaNhap = 0;
                if (r["GiaNhap"] != DBNull.Value) giaNhap = Convert.ToDouble(r["GiaNhap"]);

                string trangThai = "Đang bán";
                if (r["TrangThai"] != DBNull.Value) trangThai = r["TrangThai"].ToString();

                lblProductDetailName.Text = tenSP.ToUpper();
                lblProductDetailPrice.Text = $"Giá bán: {giaBan.ToString("N0")} VNĐ";
                lblProductDetailStock.Text = $"Số lượng tồn: {soLuongTon.ToString("N0")}";
                
                lblProductDetailDesc.Text = $"Mã sản phẩm: {prodId}\n" +
                                            $"Danh mục: {tenDanhMuc}\n" +
                                            $"Giá nhập: {giaNhap.ToString("N0")} VNĐ\n" +
                                            $"Trạng thái: {trangThai}";
            }
        }

        /// <summary>
        /// Thủ tục tải ảnh an toàn bằng MemoryStream.
        /// Chống hiện tượng bị khóa file ảnh (Lock file) trên hệ điều hành khi muốn sửa/xóa ảnh sau này.
        /// </summary>
        private void LoadProductImage(string imagePath)
        {
            // Thu hồi bộ nhớ rác của C#
            if (picProductImage.Image != null)
            {
                picProductImage.Image.Dispose();
                picProductImage.Image = null;
            }
            if (picProductDetailImage.Image != null)
            {
                picProductDetailImage.Image.Dispose();
                picProductDetailImage.Image = null;
            }

            if (string.IsNullOrEmpty(imagePath) == false && File.Exists(imagePath) == true)
            {
                try
                {
                    // Đọc file ảnh thông qua mảng Byte để không dính líu vật lý tới file gốc trên ổ cứng
                    byte[] bytes = File.ReadAllBytes(imagePath);
                    
                    MemoryStream ms1 = new MemoryStream(bytes);
                    picProductImage.Image = Image.FromStream(ms1);

                    MemoryStream ms2 = new MemoryStream(bytes);
                    picProductDetailImage.Image = Image.FromStream(ms2);
                }
                catch
                {
                    // Lỗi ảnh hỏng thì gán thành rỗng
                    picProductImage.Image = null;
                    picProductDetailImage.Image = null;
                }
            }
        }

        /// <summary>
        /// Hàm tập trung sức mạnh: Kiểm duyệt toàn bộ dữ liệu người dùng gõ vào.
        /// Vừa bắt lỗi, vừa nhả (out) các biến đã ép kiểu thành công ra ngoài cho nút Lưu/Sửa xài chung.
        /// </summary>
        private bool ValidateProductInputs(out string name, out string desc, out string status, out double importPrice, out double salesPrice, out int stock, out int catId)
        {
            // Khởi tạo các giá trị đầu ra (Bắt buộc phải khởi tạo khi dùng từ khóa 'out')
            name = txtTenSanPham.Text.Trim();
            desc = txtMoTa.Text.Trim();
            status = cboTrangThai.Text;
            importPrice = 0; 
            salesPrice = 0; 
            stock = 0; 
            catId = 0;

            if (string.IsNullOrEmpty(name) == true) 
            { 
                MessageBox.Show("Tên sản phẩm không được phép để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                txtTenSanPham.Focus(); 
                return false; 
            }

            if (cboDanhMuc.SelectedIndex == -1 || cboDanhMuc.SelectedValue == null) 
            { 
                MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                cboDanhMuc.Focus(); 
                return false; 
            }

            if (double.TryParse(txtGiaNhap.Text, out importPrice) == false || importPrice < 0) 
            { 
                MessageBox.Show("Giá nhập kho phải là số hợp lệ và lớn hơn hoặc bằng 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                txtGiaNhap.Focus(); 
                return false; 
            }

            if (double.TryParse(txtGiaBan.Text, out salesPrice) == false || salesPrice <= 0) 
            { 
                MessageBox.Show("Giá bán lẻ phải là số hợp lệ và lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                txtGiaBan.Focus(); 
                return false; 
            }

            if (salesPrice < importPrice) 
            { 
                MessageBox.Show("Giá bán lẻ không được phép nhỏ hơn giá nhập kho!", "Lỗi kinh doanh", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                txtGiaBan.Focus(); 
                return false; 
            }

            if (int.TryParse(txtSoLuongTon.Text, out stock) == false || stock < 0) 
            { 
                MessageBox.Show("Số lượng tồn kho phải là số nguyên không âm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                txtSoLuongTon.Focus(); 
                return false; 
            }

            // Ép kiểu danh mục
            catId = Convert.ToInt32(cboDanhMuc.SelectedValue);

            return true; // Qua hết các ải, dữ liệu xanh sạch!
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Kích hoạt khi bấm chuột trái vào bất kỳ ô nào trên bảng Sản phẩm.
        /// </summary>
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Thoát chế độ tìm kiếm khẩn cấp nếu người dùng đang tìm kiếm mà bấm vào lưới
                if (txtMaSanPham.Enabled == true)
                {
                    txtMaSanPham.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // Gọi hàm lấy dữ liệu siêu to khổng lồ
                SelectProductRow(e.RowIndex);
                
                // Mở khóa các textbox
                ToggleInputs(true);
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        #endregion

        #region 4. CÁC HÀM XỬ LÝ NÚT BẤM (BUTTON CLICK HANDLERS)

        /// <summary>
        /// Nút Thêm mới sản phẩm.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearInputs();

            txtMaSanPham.Enabled = false;
            txtMaSanPham.Text = "Tự động sinh";
            cboTrangThai.Text = "Đang bán";
            
            ToggleInputs(true);
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            
            txtTenSanPham.Focus();
        }

        /// <summary>
        /// Nút Lưu sản phẩm (Thêm mới dữ liệu vào bảng SanPham - INSERT).
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra ải Validation. Sử dụng từ khóa 'out' để hứng dữ liệu đã bóp vụn bên trong.
            string name = "";
            string desc = "";
            string status = "";
            double importPrice = 0;
            double salesPrice = 0;
            int stock = 0;
            int catId = 0;

            if (ValidateProductInputs(out name, out desc, out status, out importPrice, out salesPrice, out stock, out catId) == false)
            {
                return;
            }

            // 2. Viết câu SQL Insert thẳng thắn, không màu mè
            string sqlInsert = $@"INSERT INTO SanPham (TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) 
                                  VALUES (N'{name}', {catId}, {importPrice}, {salesPrice}, {stock}, N'{desc}', N'{currentImagePath}', N'{status}', GETDATE())";
            DbContext.RunSql(sqlInsert);

            MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 3. Khóa và làm mới
            ToggleInputs(false);
            LoadProductsGrid(null);

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        /// <summary>
        /// Nút Sửa thông tin sản phẩm (Cập nhật dữ liệu vào bảng SanPham - UPDATE).
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.Rows.Count == 0 || string.IsNullOrEmpty(txtMaSanPham.Text) || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách lưới để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Kiểm duyệt tương tự nút Lưu
            string name = "";
            string desc = "";
            string status = "";
            double importPrice = 0;
            double salesPrice = 0;
            int stock = 0;
            int catId = 0;

            if (ValidateProductInputs(out name, out desc, out status, out importPrice, out salesPrice, out stock, out catId) == false)
            {
                return;
            }

            // 2. Viết câu SQL Update
            int prodId = Convert.ToInt32(txtMaSanPham.Text);
            string sqlUpdate = $@"UPDATE SanPham 
                                  SET TenSanPham = N'{name}', 
                                      MaDanhMuc = {catId}, 
                                      GiaNhap = {importPrice}, 
                                      GiaBan = {salesPrice}, 
                                      SoLuongTon = {stock}, 
                                      MoTa = N'{desc}', 
                                      Anh = N'{currentImagePath}', 
                                      TrangThai = N'{status}', 
                                      NgayCapNhat = GETDATE() 
                                  WHERE MaSanPham = {prodId}";
            DbContext.RunSql(sqlUpdate);

            MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ToggleInputs(false);
            LoadProductsGrid(null);

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        /// <summary>
        /// Nút Xóa (Xóa Mềm): Chuyển trạng thái sang Ngưng bán.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSanPham.Text) == true || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int prodId = Convert.ToInt32(txtMaSanPham.Text);
            string name = txtTenSanPham.Text;

            // Hỏi ý kiến lần cuối
            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Ngưng bán) sản phẩm '{name}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                string sql = $"UPDATE SanPham SET TrangThai = N'Ngưng bán' WHERE MaSanPham = {prodId}";
                DbContext.RunSql(sql);

                MessageBox.Show("Chuyển trạng thái sản phẩm thành Ngưng bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadProductsGrid(null);
                ClearInputs();
                ToggleInputs(false);

                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        /// <summary>
        /// Nút Làm mới (Tải lại).
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadProductsGrid(null);
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        /// <summary>
        /// Nút Bỏ qua thao tác Thêm/Sửa đang làm dở dang.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearInputs();
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        /// <summary>
        /// Nút Tìm Kiếm (2 Giai đoạn).
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lần 1: Kích hoạt chế độ điền thông tin tìm kiếm
            if (txtMaSanPham.Enabled == false)
            {
                ClearInputs();
                ToggleInputs(true);
                txtMaSanPham.Enabled = true; // Mở luôn mã SP để có thể tìm theo mã nếu thích

                // Xóa các số 0 mặc định để người dùng đỡ bị tìm nhầm
                txtGiaNhap.Text = "";
                txtGiaBan.Text = "";
                txtSoLuongTon.Text = "";
                cboTrangThai.SelectedIndex = -1;

                // Khóa bảo vệ các nút
                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập thông tin (Tên SP, Giá, Danh mục...) vào các ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaSanPham.Focus();
                return;
            }

            // Lần 2: Ráp nối chuỗi SQL dựa trên các ô người dùng có nhập
            string idTerm = txtMaSanPham.Text.Trim();
            string nameTerm = txtTenSanPham.Text.Trim();
            
            int selectedCatId = -1;
            if (cboDanhMuc.SelectedValue != null)
            {
                if (int.TryParse(cboDanhMuc.SelectedValue.ToString(), out selectedCatId) == false)
                {
                    selectedCatId = -1;
                }
            }

            string statusTerm = cboTrangThai.Text;

            // Tìm nâng cao: Limit (Lấy các sản phẩm có giá nhỏ hơn hoặc bằng giá nhập vào)
            double priceLimit = 0;
            double.TryParse(txtGiaBan.Text, out priceLimit);

            int stockLimit = 0;
            int.TryParse(txtSoLuongTon.Text, out stockLimit);

            // Bắt đầu chuỗi gốc
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE 1=1";

            if (string.IsNullOrEmpty(idTerm) == false)
            {
                sql += $" AND s.MaSanPham LIKE N'%{idTerm}%'";
            }
            if (string.IsNullOrEmpty(nameTerm) == false)
            {
                sql += $" AND (s.TenSanPham LIKE N'%{nameTerm}%' OR s.MoTa LIKE N'%{nameTerm}%')";
            }
            if (cboDanhMuc.SelectedIndex != -1 && selectedCatId > 0)
            {
                sql += $" AND s.MaDanhMuc = {selectedCatId}";
            }
            if (cboTrangThai.SelectedIndex != -1 && string.IsNullOrEmpty(statusTerm) == false)
            {
                sql += $" AND s.TrangThai = N'{statusTerm}'";
            }
            if (priceLimit > 0)
            {
                sql += $" AND s.GiaBan <= {priceLimit}";
            }
            if (stockLimit > 0)
            {
                sql += $" AND s.SoLuongTon <= {stockLimit}";
            }

            sql += " ORDER BY s.NgayTao DESC";

            // Truy vấn và nạp lên lưới
            DataTable tblSearch = DbContext.GetDataToTable(sql);
            LoadProductsGrid(tblSearch);

            if (dgvSanPham.Rows.Count > 0)
            {
                MessageBox.Show($"Tìm thấy {dgvSanPham.Rows.Count} sản phẩm phù hợp!", "Tìm kiếm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào khớp với thông tin đã nhập!", "Thông báo rỗng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Nút tải ảnh đại diện từ hệ thống Windows (OpenFileDialog).
        /// </summary>
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            // Sử dụng khối using để đảm bảo hộp thoại tự động tắt và trả bộ nhớ khi xong
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Chỉ cho phép chọn file ảnh
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentImagePath = ofd.FileName;
                        LoadProductImage(currentImagePath); // Gọi hàm tải ảnh an toàn bằng byte đã viết ở trên
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hệ thống không thể tải tập tin ảnh này.\nChi tiết lỗi: " + ex.Message, "Lỗi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region 5. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        #endregion
    }
}
