using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Import DbContext

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {

        private string currentImagePath = "";

        public ucProductList()
        {
            InitializeComponent();
        }

        // 5.2.1. Viết thủ tục ucProductList_Load khi tải Control
        private void ucProductList_Load(object sender, EventArgs e)
        {
            // Kết nối cơ sở dữ liệu nếu chưa kết nối
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Tải danh sách Danh mục lên combobox cboDanhMuc
            Load_cboDanhMuc();

            // Khởi tạo ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang bán", "Ngưng bán" });
            cboTrangThai.SelectedIndex = 0;

            // Tải lưới sản phẩm
            LoadProductsGrid();

            // Khóa các ô nhập liệu và thiết lập trạng thái nút bấm ban đầu
            ToggleInputs(false);
            
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        // 5.2.2. Viết thủ tục nạp combobox Danh Mục từ Database
        private void Load_cboDanhMuc()
        {
            string sql = "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY TenDanhMuc ASC";
            DataTable tblDanhMuc = DbContext.GetDataToTable(sql);
            
            cboDanhMuc.DataSource = tblDanhMuc;
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";
            cboDanhMuc.SelectedIndex = -1; // Để trống mặc định ban đầu
        }

        // 5.2.3. Viết thủ tục LoadProductsGrid tải dữ liệu sản phẩm lên lưới
        private void LoadProductsGrid(DataTable customTable = null)
        {
            DataTable tblProducts;
            if (customTable != null)
            {
                tblProducts = customTable;
            }
            else
            {
                string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                               FROM SanPham s 
                               LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc
                               ORDER BY s.NgayTao DESC";
                tblProducts = DbContext.GetDataToTable(sql);
            }

            // Tắt tự động tạo cột trên DataGridView
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.DataSource = tblProducts;

            // Gán DataPropertyName cho từng cột đã tạo sẵn trong Designer
            if (dgvSanPham.Columns.Contains("colMaSanPham")) dgvSanPham.Columns["colMaSanPham"].DataPropertyName = "MaSanPham";
            if (dgvSanPham.Columns.Contains("colTenSanPham")) dgvSanPham.Columns["colTenSanPham"].DataPropertyName = "TenSanPham";
            if (dgvSanPham.Columns.Contains("colMaDanhMuc")) dgvSanPham.Columns["colMaDanhMuc"].DataPropertyName = "TenDanhMuc"; // Hiển thị tên DM cho dễ nhìn
            if (dgvSanPham.Columns.Contains("colGiaNhap")) dgvSanPham.Columns["colGiaNhap"].DataPropertyName = "GiaNhap";
            if (dgvSanPham.Columns.Contains("colGiaBan")) dgvSanPham.Columns["colGiaBan"].DataPropertyName = "GiaBan";
            if (dgvSanPham.Columns.Contains("colSoLuongTon")) dgvSanPham.Columns["colSoLuongTon"].DataPropertyName = "SoLuongTon";
            if (dgvSanPham.Columns.Contains("colTrangThai")) dgvSanPham.Columns["colTrangThai"].DataPropertyName = "TrangThai";
            if (dgvSanPham.Columns.Contains("colNgayTao")) dgvSanPham.Columns["colNgayTao"].DataPropertyName = "NgayTao";

            // Định dạng cột hiển thị tiền tệ và ngày tháng
            if (dgvSanPham.Columns.Contains("colGiaNhap")) dgvSanPham.Columns["colGiaNhap"].DefaultCellStyle.Format = "N0";
            if (dgvSanPham.Columns.Contains("colGiaBan")) dgvSanPham.Columns["colGiaBan"].DefaultCellStyle.Format = "N0";
            if (dgvSanPham.Columns.Contains("colNgayTao")) dgvSanPham.Columns["colNgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // Thiết lập độ rộng cột và căn chỉnh văn bản đẹp đẽ giống ucPromotion
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

            // Đồng bộ giao diện cao cấp
            dgvSanPham.RowTemplate.Height = 40;
            dgvSanPham.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvSanPham.ColumnHeadersHeight = 40;
            dgvSanPham.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; // Tránh tự động xuống dòng gây mất chữ ở tiêu đề
        }

        // 5.2.4. Viết thủ tục chọn sản phẩm hiển thị thông tin chi tiết
        private void SelectProductRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSanPham.Rows.Count) return;

            dgvSanPham.ClearSelection();
            dgvSanPham.Rows[rowIndex].Selected = true;

            int prodId = Convert.ToInt32(dgvSanPham.Rows[rowIndex].Cells["colMaSanPham"].Value);
            
            string sql = $@"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE s.MaSanPham = {prodId}";
            DataTable tbl = DbContext.GetDataToTable(sql);

            if (tbl.Rows.Count > 0)
            {
                DataRow r = tbl.Rows[0];
                txtMaSanPham.Text = r["MaSanPham"].ToString();
                txtTenSanPham.Text = r["TenSanPham"]?.ToString() ?? "";
                txtGiaNhap.Text = r["GiaNhap"]?.ToString() ?? "0";
                txtGiaBan.Text = r["GiaBan"]?.ToString() ?? "0";
                txtSoLuongTon.Text = r["SoLuongTon"]?.ToString() ?? "0";
                txtMoTa.Text = r["MoTa"]?.ToString() ?? "";
                
                if (r["MaDanhMuc"] != DBNull.Value)
                {
                    cboDanhMuc.SelectedValue = Convert.ToInt32(r["MaDanhMuc"]);
                }
                else
                {
                    cboDanhMuc.SelectedIndex = -1;
                }
                cboTrangThai.Text = r["TrangThai"]?.ToString() ?? "Đang bán";

                currentImagePath = r["Anh"]?.ToString() ?? "";
                LoadProductImage(currentImagePath);

                // Cập nhật nhãn tab chi tiết sản phẩm phía bên phải
                double giaBan = r["GiaBan"] == DBNull.Value ? 0 : Convert.ToDouble(r["GiaBan"]);
                int soLuongTon = r["SoLuongTon"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoLuongTon"]);
                string tenSP = r["TenSanPham"]?.ToString() ?? "";
                string tenDanhMuc = r["TenDanhMuc"]?.ToString() ?? "Không rõ";
                double giaNhap = r["GiaNhap"] == DBNull.Value ? 0 : Convert.ToDouble(r["GiaNhap"]);
                string trangThai = r["TrangThai"]?.ToString() ?? "Đang bán";

                lblProductDetailName.Text = tenSP.ToUpper();
                lblProductDetailPrice.Text = $"Giá bán: {giaBan.ToString("N0")} VNĐ";
                lblProductDetailStock.Text = $"Số lượng tồn: {soLuongTon.ToString("N0")}";
                
                lblProductDetailDesc.Text = $"Mã sản phẩm: {prodId}\n" +
                                            $"Danh mục: {tenDanhMuc}\n" +
                                            $"Giá nhập: {giaNhap.ToString("N0")} VNĐ\n" +
                                            $"Trạng thái: {trangThai}";
            }
        }

        // 5.2.5. Thủ tục tải ảnh sản phẩm an toàn tránh bị khóa file trên hệ thống
        private void LoadProductImage(string imagePath)
        {
            // Thu hồi bộ nhớ ảnh cũ nếu có
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

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try
                {
                    // Đọc file ảnh dưới dạng byte array để không chiếm dụng, khóa file ảnh trên ổ cứng
                    byte[] bytes = File.ReadAllBytes(imagePath);
                    using (MemoryStream ms1 = new MemoryStream(bytes))
                    {
                        picProductImage.Image = Image.FromStream(ms1);
                    }
                    using (MemoryStream ms2 = new MemoryStream(bytes))
                    {
                        picProductDetailImage.Image = Image.FromStream(ms2);
                    }
                }
                catch
                {
                    picProductImage.Image = null;
                    picProductDetailImage.Image = null;
                }
            }
        }

        // 5.2.6. Thủ tục ToggleInputs thay đổi trạng thái bật tắt các trường thông tin
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

        // 5.2.7. Thủ tục dọn dẹp các trường nhập dữ liệu
        private void ClearInputs()
        {
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtGiaNhap.Text = "0";
            txtGiaBan.Text = "0";
            txtSoLuongTon.Text = "0";
            txtMoTa.Text = "";
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;

            currentImagePath = "";
            LoadProductImage("");
        }

        // 5.2.8. Sự kiện click vào ô trên lưới sản phẩm
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Thoát chế độ tìm kiếm nếu đang ở chế độ tìm kiếm
                if (txtMaSanPham.Enabled == true)
                {
                    txtMaSanPham.Enabled = false;
                    btnAdd.Enabled = true;
                }

                SelectProductRow(e.RowIndex);
                
                ToggleInputs(true);
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        // 5.2.9. Sự kiện click nút Thêm mới
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

        private bool ValidateProductInputs(out string name, out string desc, out string status, out double importPrice, out double salesPrice, out int stock, out int catId)
        {
            name = txtTenSanPham.Text.Trim();
            desc = txtMoTa.Text.Trim();
            status = cboTrangThai.Text;
            importPrice = 0; salesPrice = 0; stock = 0; catId = 0;

            if (string.IsNullOrEmpty(name)) { MessageBox.Show("Tên sản phẩm không được phép để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); txtTenSanPham.Focus(); return false; }
            if (cboDanhMuc.SelectedIndex == -1 || cboDanhMuc.SelectedValue == null) { MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); cboDanhMuc.Focus(); return false; }
            if (!double.TryParse(txtGiaNhap.Text, out importPrice) || importPrice < 0) { MessageBox.Show("Giá nhập kho phải lớn hơn hoặc bằng 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); txtGiaNhap.Focus(); return false; }
            if (!double.TryParse(txtGiaBan.Text, out salesPrice) || salesPrice <= 0) { MessageBox.Show("Giá bán lẻ phải lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); txtGiaBan.Focus(); return false; }
            if (salesPrice < importPrice) { MessageBox.Show("Giá bán lẻ không được nhỏ hơn giá nhập kho!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error); txtGiaBan.Focus(); return false; }
            if (!int.TryParse(txtSoLuongTon.Text, out stock) || stock < 0) { MessageBox.Show("Số lượng tồn kho phải là số không âm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSoLuongTon.Focus(); return false; }
            catId = Convert.ToInt32(cboDanhMuc.SelectedValue);
            return true;
        }

        // 5.2.10. Sự kiện click nút Sửa
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.Rows.Count == 0 || string.IsNullOrEmpty(txtMaSanPham.Text) || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateProductInputs(out string name, out string desc, out string status, out double importPrice, out double salesPrice, out int stock, out int catId))
                return;

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
            LoadProductsGrid();

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        // 5.2.11. Sự kiện click nút Xóa sản phẩm
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSanPham.Text) || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int prodId = Convert.ToInt32(txtMaSanPham.Text);
            string name = txtTenSanPham.Text;

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Ngưng bán) sản phẩm '{name}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                string sql = $"UPDATE SanPham SET TrangThai = N'Ngưng bán' WHERE MaSanPham = {prodId}";
                DbContext.RunSql(sql);

                MessageBox.Show("Chuyển trạng thái sản phẩm thành Ngưng bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadProductsGrid();
                ClearInputs();
                ToggleInputs(false);

                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        // 5.2.12. Sự kiện nút Làm mới (Tải lại bảng)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadProductsGrid();
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        // 5.2.13. Sự kiện click nút Bỏ qua thay đổi
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

        // 5.2.14. Sự kiện click nút Lưu thay đổi
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductInputs(out string name, out string desc, out string status, out double importPrice, out double salesPrice, out int stock, out int catId))
                return;

            string sqlInsert = $@"INSERT INTO SanPham (TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) 
                                  VALUES (N'{name}', {catId}, {importPrice}, {salesPrice}, {stock}, N'{desc}', N'{currentImagePath}', N'{status}', GETDATE())";
            DbContext.RunSql(sqlInsert);

            MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ToggleInputs(false);
            LoadProductsGrid();

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

        // 5.2.15. Sự kiện nút Tìm kiếm linh động nâng cao theo nhiều điều kiện
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lần 1: Kích hoạt chế độ tìm kiếm
            if (txtMaSanPham.Enabled == false && btnAdd.Enabled == true)
            {
                ClearInputs();
                ToggleInputs(true);
                txtMaSanPham.Enabled = true; // Mở luôn mã SP để có thể tìm theo mã

                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật! Vui lòng nhập thông tin cần tìm kiếm vào các ô dữ liệu rồi ấn Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaSanPham.Focus();
                return;
            }

            // Lần 2: Bắt đầu tìm kiếm
            string idTerm = txtMaSanPham.Text.Trim();
            string nameTerm = txtTenSanPham.Text.Trim();
            int selectedCatId = cboDanhMuc.SelectedValue is int id ? id : -1;
            string statusTerm = cboTrangThai.Text;

            // Cho phép tìm nâng cao: các sản phẩm có giá <= limit nhập và tồn <= limit nhập
            double.TryParse(txtGiaBan.Text, out double priceLimit);
            int.TryParse(txtSoLuongTon.Text, out int stockLimit);

            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE 1=1";

            if (!string.IsNullOrEmpty(idTerm))
            {
                sql += $" AND s.MaSanPham LIKE N'%{idTerm}%'";
            }
            if (!string.IsNullOrEmpty(nameTerm))
            {
                sql += $" AND (s.TenSanPham LIKE N'%{nameTerm}%' OR s.MoTa LIKE N'%{nameTerm}%')";
            }
            if (cboDanhMuc.SelectedIndex != -1 && selectedCatId > 0)
            {
                sql += $" AND s.MaDanhMuc = {selectedCatId}";
            }
            if (cboTrangThai.SelectedIndex != -1 && !string.IsNullOrEmpty(statusTerm))
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

            DataTable tblSearch = DbContext.GetDataToTable(sql);
            LoadProductsGrid(tblSearch);

            if (dgvSanPham.Rows.Count > 0)
            {
                ClearInputs();
                MessageBox.Show($"Tìm thấy {dgvSanPham.Rows.Count} sản phẩm phù hợp!", "Tìm kiếm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ClearInputs();
                MessageBox.Show("Không tìm thấy sản phẩm nào khớp với các tiêu chí tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            btnCancel.Enabled = true;
        }

        // 5.2.16. Sự kiện chọn ảnh đại diện sản phẩm từ máy tính
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentImagePath = ofd.FileName;
                        LoadProductImage(currentImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải tập tin ảnh này: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
