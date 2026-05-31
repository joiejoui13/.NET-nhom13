using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Import DbContext để dùng trực tiếp Database

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucStockIn : UserControl
    {
        public class StockInDetailModel
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double GiaNhap { get; set; }
            public double ThanhTien => SoLuong * GiaNhap;
        }

        private List<StockInDetailModel> currentDetails = new List<StockInDetailModel>();
        private bool isEditing = false;
        private bool isAddingNew = false;
        private bool isAddingDetail = false;
        private bool isSearching = false;
        private int activeUserId = 1;      // Mặc định người dùng hệ thống là Admin
        private string activeUserName = "Admin";

        public ucStockIn()
        {
            InitializeComponent();

            // Extracted from Designer
            cboTrangThai.Items.AddRange(new object[] { "Chờ xử lý", "Đã hoàn thành", "Đã hủy" });
        }

        // 5.2.1. Nạp dữ liệu ban đầu khi Load UserControl
        private void ucStockIn_Load(object sender, EventArgs e)
        {
            // Tab 2 dynamic bindings
            guna2Button1.Click += btnStockInSearch_Click;
            guna2Button2.Click += btnStockInRefresh_Click;
            tabMain.Selecting += tabMain_Selecting;

            // Kết nối Database nếu chưa kết nối
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            LoadProductsSelectionGrid();

            // Tải thông tin người dùng đang đăng nhập (hoặc mặc định) từ Database
            LoadActiveUser();

            // Thiết lập ComboBox Trạng thái phiếu nhập
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Chờ xử lý", "Đã hoàn thành", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;

            // Đặt tiêu đề lưới hiển thị danh sách phiếu nhập
            lblGridTitle.Text = "DANH SÁCH PHIẾU NHẬP";
            dgvDetails.Columns[0].HeaderText = "Mã Phiếu Nhập";
            dgvDetails.Columns[1].HeaderText = "Mã NV";
            dgvDetails.Columns[2].HeaderText = "Ngày Nhập";
            dgvDetails.Columns[3].HeaderText = "Trạng Thái";
            dgvDetails.Columns[4].HeaderText = "Tổng Tiền";

            // Wire up cell click dynamically giống mock code
            dgvDetails.CellClick += dgvDetails_CellClick;
            dgvProductsSelection.CellClick += dgvProductsSelection_CellClick;

            // Tải lưới danh sách phiếu nhập
            LoadReceiptsGrid();

            // Đưa các nút và các trường thông tin về trạng thái khóa
            ResetTab1State();
            ResetTab2State();
        }

        // 5.2.2. Lấy thông tin tài khoản người dùng hoạt động
        private void LoadActiveUser()
        {
            try
            {
                string sqlUser = "SELECT TOP 1 MaNguoiDung, TenNguoiDung FROM NguoiDung";
                DataTable tblUser = DbContext.GetDataToTable(sqlUser);
                if (tblUser.Rows.Count > 0)
                {
                    activeUserId = Convert.ToInt32(tblUser.Rows[0]["MaNguoiDung"]);
                    activeUserName = tblUser.Rows[0]["TenNguoiDung"]?.ToString() ?? "Admin";
                }
            }
            catch
            {
                activeUserId = 1;
                activeUserName = "Admin";
            }
        }

        // 5.2.3. Tải danh sách phiếu nhập lên DataGridView dgvDetails
        private void LoadReceiptsGrid(DataTable customTable = null)
        {
            dgvDetails.Rows.Clear();
            DataTable tbl;
            if (customTable != null)
            {
                tbl = customTable;
            }
            else
            {
                string sql = @"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                               FROM PhieuNhap p 
                               ORDER BY p.NgayNhap DESC";
                tbl = DbContext.GetDataToTable(sql);
            }

            // Tải dữ liệu thủ công vào lưới dgvDetails giống hệt cấu trúc của mock code ban đầu
            foreach (DataRow r in tbl.Rows)
            {
                int id = Convert.ToInt32(r["MaPhieuNhap"]);
                string user = r["MaNguoiDung"]?.ToString() ?? "";
                DateTime date = r["NgayNhap"] != DBNull.Value ? Convert.ToDateTime(r["NgayNhap"]) : DateTime.Now;
                string status = r["TrangThai"]?.ToString() ?? "Chờ xử lý";
                double total = r["TongTien"] != DBNull.Value ? Convert.ToDouble(r["TongTien"]) : 0;

                dgvDetails.Rows.Add(
                    id,
                    user,
                    date.ToString("dd/MM/yyyy HH:mm"),
                    status,
                    total.ToString("N0") + " đ"
                );
            }

            // Căn chỉnh giao diện đẹp đẽ giống các UserControl khác
            dgvDetails.RowTemplate.Height = 40;
            dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDetails.ColumnHeadersHeight = 40;
            dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }

        // 5.2.4. Chọn dòng hiển thị chi tiết phiếu nhập
        private void SelectReceiptRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDetails.Rows.Count) return;

            lblChonRightTitle.Text = $"CHI TIẾT PHIẾU NHẬP {dgvDetails.Rows[rowIndex].Cells[0].Value}";
            lblStockInTitle.Text = $"MÃ PHIẾU: {dgvDetails.Rows[rowIndex].Cells[0].Value}";

            dgvDetails.ClearSelection();
            dgvDetails.Rows[rowIndex].Selected = true;

            int receiptId = Convert.ToInt32(dgvDetails.Rows[rowIndex].Cells[0].Value);

            string sql = $@"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai 
                           FROM PhieuNhap p 
                           WHERE p.MaPhieuNhap = {receiptId}";
            DataTable tbl = DbContext.GetDataToTable(sql);

            if (tbl.Rows.Count > 0)
            {
                DataRow r = tbl.Rows[0];
                txtMaPhieuNhap.Text = r["MaPhieuNhap"].ToString();
                txtNguoiDung.Text = r["MaNguoiDung"]?.ToString() ?? "";
                dtNgayNhap.Value = r["NgayNhap"] != DBNull.Value ? Convert.ToDateTime(r["NgayNhap"]) : DateTime.Now;
                cboTrangThai.Text = r["TrangThai"]?.ToString() ?? "Chờ xử lý";

                // Tải chi tiết sản phẩm thuộc phiếu nhập từ Database
                string sqlDet = $@"SELECT c.MaSanPham, s.TenSanPham, c.SoLuong, c.DonGia 
                                   FROM ChiTietNhapHang c 
                                   LEFT JOIN SanPham s ON c.MaSanPham = s.MaSanPham 
                                   WHERE c.MaPhieuNhap = {receiptId}";
                DataTable tblDet = DbContext.GetDataToTable(sqlDet);

                currentDetails.Clear();
                foreach (DataRow rDet in tblDet.Rows)
                {
                    currentDetails.Add(new StockInDetailModel
                    {
                        MaSanPham = Convert.ToInt32(rDet["MaSanPham"]),
                        TenSanPham = rDet["TenSanPham"]?.ToString() ?? "Sản phẩm ẩn",
                        SoLuong = Convert.ToInt32(rDet["SoLuong"]),
                        GiaNhap = Convert.ToDouble(rDet["DonGia"])
                    });
                }
                LoadCurrentDetailsGrid();
            }
        }

        // 5.2.5. Xử lý click dòng trên lưới phiếu nhập
        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (isEditing)
                {
                    MessageBox.Show("Hệ thống đang ở chế độ Thêm/Sửa! Vui lòng Lưu hoặc Bỏ qua trước khi chọn phiếu khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Thoát chế độ tìm kiếm nếu đang ở chế độ tìm kiếm
                if (txtMaPhieuNhap.Enabled == true)
                {
                    txtMaPhieuNhap.Enabled = false;
                    btnAdd.Enabled = true;
                }

                SelectReceiptRow(e.RowIndex);

                // Mở khóa các ô nhập liệu (giống ToggleInputs(true) ở Category)
                // Theo yêu cầu: Mã Phiếu và Ngày Nhập không được sửa
                txtMaPhieuNhap.Enabled = false;
                txtNguoiDung.Enabled = true;
                txtNguoiDung.ReadOnly = false;
                dtNgayNhap.Enabled = false;
                cboTrangThai.Enabled = true;

                // Bật tắt các nút giống Category
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;

                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }


        private void ResetTab1State()
        {
            isAddingNew = false;
            isEditing = false;
            lblChonRightTitle.Text = "CHI TIẾT SẢN PHẨM ĐÃ CHỌN";
            lblStockInTitle.Text = "MÃ PHIẾU:";
            isSearching = false;

            txtMaPhieuNhap.Text = "";
            dtNgayNhap.Format = DateTimePickerFormat.Custom;
            dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            dtNgayNhap.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            txtNguoiDung.Text = "";

            txtMaPhieuNhap.ReadOnly = true;
            txtMaPhieuNhap.Enabled = false; // T?t ch? d? tìm ki?m
            txtNguoiDung.ReadOnly = true;
            txtNguoiDung.Enabled = false;
            dtNgayNhap.Enabled = false;
            cboTrangThai.Enabled = false;

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            dgvDetails.ClearSelection();
            currentDetails.Clear();
            LoadCurrentDetailsGrid();
        }

        private void SetTab2InputVisibility(bool visible)
        {
            // Mã SP và Tên SP luôn hiển thị nhưng bị khóa vĩnh viễn (không cho chạm vào)
            lblSelMaSP.Visible = true;
            txtSelMaSP.Visible = true;
            txtSelMaSP.Enabled = false;

            lblSelTenSP.Visible = true;
            txtSelTenSP.Visible = true;
            txtSelTenSP.Enabled = false;

            // Số lượng và Giá nhập luôn hiển thị
            lblSelSoLuong.Visible = true;
            txtSelSoLuong.Visible = true;
            lblSelGiaNhap.Visible = true;
            txtSelGiaNhap.Visible = true;
        }

        private void ResetTab2State()
        {
            isAddingDetail = false;
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";

            txtSelMaSP.ReadOnly = true;
            txtSelTenSP.ReadOnly = true;
            txtSelSoLuong.Enabled = false;
            txtSelGiaNhap.Enabled = false;

            SetTab2InputVisibility(true);

            txtProductSearch.Enabled = true; // Luôn mở để gõ tìm kiếm
            txtProductSearch.Text = "";

            btnAddToCart.Enabled = false; // THÊM
            guna2Button4.Enabled = false; // SỬA
            btnRemoveFromCart.Enabled = false; // XÓA
            btnBackToReceipt.Enabled = true; // LƯU CHI TIẾT PHIẾU
            guna2Button3.Enabled = false; // BỎ QUA

            lblProductDetailDesc.Text = "Mã SP: --\nThông tin chi tiết về sản phẩm sẽ được cập nhật ở đây.";
            if (picProductDetail.Image != null)
            {
                picProductDetail.Image.Dispose();
                picProductDetail.Image = null;
            }

            dgvCurrentDetails.ClearSelection();
        }

        // ========================================================
        // TAB 1 EVENTS (PHIẾU NHẬP)
        // ========================================================

        // 5.2.7. Bấm nút THÊM MỚI phiếu nhập
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                isAddingNew = true;
                isEditing = true;

                // Tự sinh mã phiếu tạm thời bằng max mã hiện tại + 1
                string sqlMax = "SELECT MAX(MaPhieuNhap) FROM PhieuNhap";
                string maxStr = DbContext.GetFieldValues(sqlMax);
                int nextId = !string.IsNullOrEmpty(maxStr) ? Convert.ToInt32(maxStr) + 1 : 101;

                txtMaPhieuNhap.Text = "Tự động sinh";
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP MỚI";
                lblStockInTitle.Text = "MÃ PHIẾU: TỰ ĐỘNG SINH";
                txtNguoiDung.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser?.MaNguoiDung ?? "";
                dtNgayNhap.Value = DateTime.Now;
                cboTrangThai.Text = "Chờ xử lý";

                currentDetails.Clear();
                LoadCurrentDetailsGrid();

                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                txtMaPhieuNhap.ReadOnly = true;
                txtMaPhieuNhap.Enabled = false;
                txtNguoiDung.ReadOnly = true;
                txtNguoiDung.Enabled = false;
                dtNgayNhap.Enabled = false;
                cboTrangThai.Enabled = true;

                ResetTab2State();
            }
        }

        // 5.2.8. Bấm nút SỬA thông tin phiếu nhập
        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            // Không cho sửa phiếu nhập đã hoàn thành hoặc đã hủy
            string sqlCheck = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
            string currentStatus = DbContext.GetFieldValues(sqlCheck);

            if (currentStatus == "Đã hoàn thành" || currentStatus == "Đã hủy")
            {
                MessageBox.Show($"Đơn đã {currentStatus.ToLower()}, không thể sửa để bảo toàn toàn vẹn dữ liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNguoiDung.Text))
            {
                MessageBox.Show("Bạn phải nhập Mã nhân viên (Người dùng) để sửa phiếu!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            int userId;
            if (!int.TryParse(txtNguoiDung.Text.Trim(), out userId))
            {
                MessageBox.Show("Mã nhân viên phải là số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            string status = cboTrangThai.Text;
            if (status == "Đã hoàn thành" && currentDetails.Count == 0)
            {
                MessageBox.Show("Không thể lưu phiếu trống ở trạng thái 'Đã hoàn thành'! Vui lòng sang Tab 2 thêm sản phẩm hoặc chọn trạng thái khác.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalAmount = currentDetails.Sum(d => d.ThanhTien);
            string dateFormatted = dtNgayNhap.Value.ToString("yyyy-MM-dd HH:mm:ss");

            // Cập nhật thông tin phiếu chính
            string sqlUpdateMaster = $@"UPDATE PhieuNhap 
                                        SET MaNguoiDung = {userId},
                                            NgayNhap = '{dateFormatted}', 
                                            TrangThai = N'{status}', 
                                            TongTien = {totalAmount} 
                                        WHERE MaPhieuNhap = {receiptId}";
            DbContext.RunSql(sqlUpdateMaster);

            // Xóa chi tiết cũ để chèn lại giỏ hàng mới
            string sqlDelOldDetails = $"DELETE FROM ChiTietNhapHang WHERE MaPhieuNhap = {receiptId}";
            DbContext.RunSql(sqlDelOldDetails);

            // Chèn lại giỏ hàng mới
            foreach (var d in currentDetails)
            {
                string sqlInsertDetail = $@"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                            VALUES ({receiptId}, {d.MaSanPham}, {d.SoLuong}, {d.GiaNhap})";
                DbContext.RunSql(sqlInsertDetail);
            }

            // Xử lý chuyển đổi trạng thái tồn kho
            string invFeedback = "";
            if (currentStatus != "Đã hoàn thành" && status == "Đã hoàn thành")
            {
                // Chuyển từ Chờ xử lý -> Hoàn thành => Cộng tăng kho hàng
                UpdateProductInventoryAndLog(receiptId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Phiếu nhập chuyển sang 'Đã hoàn thành'. Tồn kho sản phẩm được cộng tăng!";
            }
            else if (currentStatus == "Đã hoàn thành" && status == "Đã hủy")
            {
                // Hoàn tác tồn kho: Trừ bớt lại kho hàng do phiếu bị HỦY
                RevertProductInventoryAndLog(receiptId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Phiếu nhập bị HỦY hoàn toàn. Đã trừ hoàn tác lại tồn kho sản phẩm tương ứng!";
            }

            MessageBox.Show("Cập nhật thông tin phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadReceiptsGrid();
            ResetTab1State();
            ResetTab2State();
        }

        // 5.2.9. Bấm nút XÓA phiếu nhập
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            string sqlCheck = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
            string currentStatus = DbContext.GetFieldValues(sqlCheck);

            if (currentStatus == "Đã hủy")
            {
                MessageBox.Show("Phiếu nhập này đã ở trạng thái 'Đã hủy' rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa (chuyển trạng thái sang 'Đã hủy') phiếu nhập #{receiptId} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    string sqlUpdate = $"UPDATE PhieuNhap SET TrangThai = N'Đã hủy' WHERE MaPhieuNhap = {receiptId}";
                    DbContext.RunSql(sqlUpdate);

                    string invFeedback = "";
                    if (currentStatus == "Đã hoàn thành")
                    {
                        // Hoàn tác tồn kho do hủy phiếu đã hoàn thành
                        RevertProductInventoryAndLog(receiptId, currentDetails);
                        invFeedback = "\n[KHO HÀNG] Phiếu nhập bị HỦY. Đã trừ hoàn tác lại tồn kho sản phẩm tương ứng!";
                    }

                    MessageBox.Show("Đã chuyển trạng thái phiếu nhập sang 'Đã hủy'!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadReceiptsGrid();
                    ResetTab1State();
                    ResetTab2State();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 5.2.10. Bấm nút LƯU thay đổi phiếu nhập
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNguoiDung.Text))
            {
                MessageBox.Show("Bạn phải nhập Mã nhân viên (Người dùng) để lập phiếu!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            int userId;
            if (!int.TryParse(txtNguoiDung.Text.Trim(), out userId))
            {
                MessageBox.Show("Mã nhân viên phải là số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            string status = cboTrangThai.Text;
            if (status == "Đã hoàn thành" && currentDetails.Count == 0)
            {
                MessageBox.Show("Không thể lưu phiếu trống ở trạng thái 'Đã hoàn thành'! Vui lòng sang Tab 2 thêm sản phẩm hoặc chọn trạng thái khác.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalAmount = currentDetails.Sum(d => d.ThanhTien);
            string dateFormatted = dtNgayNhap.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string sqlInsertMaster = $@"INSERT INTO PhieuNhap (MaNguoiDung, TongTien, TrangThai, NgayNhap) 
                                        VALUES ({userId}, {totalAmount}, N'{status}', '{dateFormatted}')";
            DbContext.RunSql(sqlInsertMaster);

            string sqlMax = "SELECT MAX(MaPhieuNhap) FROM PhieuNhap";
            string maxStr = DbContext.GetFieldValues(sqlMax);
            int newId = !string.IsNullOrEmpty(maxStr) ? Convert.ToInt32(maxStr) : 101;

            foreach (var d in currentDetails)
            {
                string sqlInsertDetail = $@"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                            VALUES ({newId}, {d.MaSanPham}, {d.SoLuong}, {d.GiaNhap})";
                DbContext.RunSql(sqlInsertDetail);
            }

            string invFeedback = "";
            if (status == "Đã hoàn thành")
            {
                UpdateProductInventoryAndLog(newId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Đã tự động cập nhật cộng tăng số lượng tồn kho của các sản phẩm!";
            }

            MessageBox.Show("Thêm mới phiếu nhập kho thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            isAddingNew = false;

            LoadReceiptsGrid();
            ResetTab1State();
            ResetTab2State();
        }

        // 5.2.10.1. Tăng tồn kho sản phẩm và ghi chép LichSuNhapKho
        private void UpdateProductInventoryAndLog(int receiptId, List<StockInDetailModel> details)
        {
            foreach (var item in details)
            {
                // Lấy tồn kho trước
                string sqlBefore = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {item.MaSanPham}";
                string beforeStr = DbContext.GetFieldValues(sqlBefore);
                int before = !string.IsNullOrEmpty(beforeStr) ? Convert.ToInt32(beforeStr) : 0;
                int after = before + item.SoLuong;

                // Cập nhật tồn mới trong bảng SanPham
                string sqlUpdate = $"UPDATE SanPham SET SoLuongTon = {after} WHERE MaSanPham = {item.MaSanPham}";
                DbContext.RunSql(sqlUpdate);

                // Ghi nhận vào LichSuNhapKho để hiển thị đồng bộ bên Tab Lịch Sử Kho
                string sqlLog = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                   VALUES ({item.MaSanPham}, GETDATE(), {item.SoLuong}, {before}, {after}, N'Nhập kho', {receiptId}, N'Hoàn thành')";
                DbContext.RunSql(sqlLog);
            }
        }

        // 5.2.10.2. Hoàn tác trừ tồn kho sản phẩm và ghi chép LichSuNhapKho do HỦY PHIẾU
        private void RevertProductInventoryAndLog(int receiptId, List<StockInDetailModel> details)
        {
            foreach (var item in details)
            {
                // Lấy tồn kho trước
                string sqlBefore = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {item.MaSanPham}";
                string beforeStr = DbContext.GetFieldValues(sqlBefore);
                int before = !string.IsNullOrEmpty(beforeStr) ? Convert.ToInt32(beforeStr) : 0;
                int after = Math.Max(0, before - item.SoLuong); // Tránh bị âm kho

                // Cập nhật tồn mới trong bảng SanPham
                string sqlUpdate = $"UPDATE SanPham SET SoLuongTon = {after} WHERE MaSanPham = {item.MaSanPham}";
                DbContext.RunSql(sqlUpdate);

                // Ghi nhận vào LichSuNhapKho hoàn tác
                string sqlLog = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                   VALUES ({item.MaSanPham}, GETDATE(), -{item.SoLuong}, {before}, {after}, N'Nhập kho', {receiptId}, N'Hủy bỏ')";
                DbContext.RunSql(sqlLog);
            }
        }

        // 5.2.11. Bấm nút BỎ QUA sửa đổi
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetTab1State(); // Trở về trạng thái ban đầu giống như Làm lại / Load form
        }

        // 5.2.12. Tìm kiếm phiếu nhập theo mã trên Tab 1
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // LẦN 1: Kích hoạt chế độ tìm kiếm
            if (txtMaPhieuNhap.Enabled == false)
            {
                ResetTab1State();
                txtMaPhieuNhap.Enabled = true;
                txtMaPhieuNhap.ReadOnly = false;
                txtMaPhieuNhap.Text = ""; // Xóa text cũ để người dùng nhập

                txtNguoiDung.Enabled = true;
                txtNguoiDung.ReadOnly = false;
                txtNguoiDung.Text = ""; // Xóa text cũ để người dùng nhập
                cboTrangThai.Enabled = true;

                // Mở khóa Ngày Nhập để lọc, chuyển sang CustomFormat rỗng để thể hiện giá trị Null
                dtNgayNhap.Enabled = true;
                dtNgayNhap.Format = DateTimePickerFormat.Custom;
                dtNgayNhap.CustomFormat = " ";

                // Đăng ký sự kiện (xóa trước để tránh trùng lặp nếu ấn nhiều lần)
                dtNgayNhap.ValueChanged -= DtNgayNhap_ValueChanged;
                dtNgayNhap.ValueChanged += DtNgayNhap_ValueChanged;

                // Ẩn/khóa các nút khác
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật! Vui lòng nhập thông tin (Mã, Người dùng...) rồi ấn nút Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhieuNhap.Focus();
                return;
            }

            // LẦN 2: Thực hiện tìm kiếm
            bool hasDateFilter = dtNgayNhap.CustomFormat != " ";
            if (txtMaPhieuNhap.Text == "" && txtNguoiDung.Text == "" && cboTrangThai.Text == "" && !hasDateFilter)
            {
                MessageBox.Show("Hãy nhập ít nhất một điều kiện tìm kiếm!!!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien FROM PhieuNhap p WHERE 1=1";

            if (txtMaPhieuNhap.Text != "" && txtMaPhieuNhap.Text != "Tự động sinh")
                sql += $" AND p.MaPhieuNhap = {txtMaPhieuNhap.Text.Trim()}";

            if (txtNguoiDung.Text != "")
                sql += $" AND p.MaNguoiDung = {txtNguoiDung.Text.Trim()}";

            if (cboTrangThai.Text != "")
                sql += $" AND p.TrangThai = N'{cboTrangThai.Text}'";

            if (hasDateFilter)
            {
                string dateSearch = dtNgayNhap.Value.ToString("yyyy-MM-dd");
                sql += $" AND CAST(p.NgayNhap AS DATE) = '{dateSearch}'";
            }

            DataTable tblSearch = DbContext.GetDataToTable(sql);

            if (tblSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Có {tblSearch.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReceiptsGrid(tblSearch);
                SelectReceiptRow(0);
            }
        }

        private void DtNgayNhap_ValueChanged(object sender, EventArgs e)
        {
            if (dtNgayNhap.CustomFormat == " ")
            {
                dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            }
        }

        // 5.2.13. Làm mới danh sách phiếu nhập
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetTab1State(); // Quay lại giao diện lúc mới vào
            LoadReceiptsGrid();
        }

        // 5.2.14. Bấm nút CHỌN SẢN PHẨM để mở Tab 2
        private void btnChooseProducts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập hoặc ấn Thêm Mới ở Tab 1 trước khi chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tabMain.SelectedTab = tabChonSanPham;

            // Đổi Header Tab 2
            lblChonRightTitle.Text = txtMaPhieuNhap.Text == "Tự động sinh" ? "CHI TIẾT PHIẾU NHẬP MỚI" : $"CHI TIẾT PHIẾU NHẬP {txtMaPhieuNhap.Text}";

            // Xóa form chọn
            btnResetCartForm_Click(this, EventArgs.Empty);

            // Nạp lưới lựa chọn sản phẩm và giỏ hàng hiện tại
            LoadProductsSelectionGrid();
            LoadCurrentDetailsGrid();
        }

        // ========================================================
        // TAB 2 EVENTS (PRODUCT SELECTION)
        // ========================================================

        // 5.2.15. Tải danh sách sản phẩm để chọn (Tab 2 bên trái)
        private void LoadProductsSelectionGrid(DataTable customTable = null)
        {
            DataTable tbl;
            if (customTable != null)
            {
                tbl = customTable;
            }
            else
            {
                string sql = @"SELECT s.MaSanPham, s.TenSanPham, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.TrangThai, s.NgayTao
                               FROM SanPham s 
                               LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                               WHERE s.TrangThai = N'Đang bán' 
                               ORDER BY s.NgayTao DESC";
                tbl = DbContext.GetDataToTable(sql);
            }

            dgvProductsSelection.Columns.Clear();
            dgvProductsSelection.AutoGenerateColumns = true;
            dgvProductsSelection.DataSource = tbl;

            if (dgvProductsSelection.Columns.Count > 0)
            {
                if (dgvProductsSelection.Columns.Contains("MaSanPham")) dgvProductsSelection.Columns["MaSanPham"].HeaderText = "Mã SP";
                if (dgvProductsSelection.Columns.Contains("TenSanPham")) dgvProductsSelection.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                if (dgvProductsSelection.Columns.Contains("TenDanhMuc")) dgvProductsSelection.Columns["TenDanhMuc"].HeaderText = "Danh Mục";
                if (dgvProductsSelection.Columns.Contains("GiaNhap")) { dgvProductsSelection.Columns["GiaNhap"].HeaderText = "Giá Nhập"; dgvProductsSelection.Columns["GiaNhap"].DefaultCellStyle.Format = "N0"; }
                if (dgvProductsSelection.Columns.Contains("GiaBan")) { dgvProductsSelection.Columns["GiaBan"].HeaderText = "Giá Bán"; dgvProductsSelection.Columns["GiaBan"].DefaultCellStyle.Format = "N0"; }
                if (dgvProductsSelection.Columns.Contains("SoLuongTon")) dgvProductsSelection.Columns["SoLuongTon"].HeaderText = "SL Tồn";
                if (dgvProductsSelection.Columns.Contains("TrangThai")) dgvProductsSelection.Columns["TrangThai"].HeaderText = "Trạng Thái";
                if (dgvProductsSelection.Columns.Contains("NgayTao")) { dgvProductsSelection.Columns["NgayTao"].HeaderText = "Ngày Tạo"; dgvProductsSelection.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy"; }
            }

            // Tối ưu giao diện lưới
            dgvProductsSelection.RowTemplate.Height = 35;
            dgvProductsSelection.ColumnHeadersHeight = 35;
        }

        // 5.2.16. Xử lý gõ tìm kiếm nhanh sản phẩm trên Tab 2
        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            string maSP = txtSelMaSP.Text.Trim();
            string tenSP = txtSelTenSP.Text.Trim();
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE s.TrangThai = N'Đang bán'";

            if (!string.IsNullOrEmpty(maSP))
            {
                sql += $" AND s.MaSanPham LIKE '%{maSP}%'";
            }
            if (!string.IsNullOrEmpty(tenSP))
            {
                sql += $" AND (s.TenSanPham LIKE N'%{tenSP}%' OR s.MoTa LIKE N'%{tenSP}%')";
            }
            sql += " ORDER BY s.NgayTao DESC";

            DataTable tblFiltered = DbContext.GetDataToTable(sql);
            LoadProductsSelectionGrid(tblFiltered);
        }

        // 5.2.17. Chọn sản phẩm từ lưới danh mục bên trái
        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (btnRemoveFromCart.Enabled)
                {
                    MessageBox.Show("Bạn đang thao tác Sửa/Xóa 1 sản phẩm trong giỏ hàng. Vui lòng ấn 'Sửa', 'Xóa' hoặc 'Bỏ qua' trước khi chọn sản phẩm mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Lấy Mã Sản Phẩm từ lưới (sau khi AutoGenerateColumns = true, cột MaSanPham có thể không nằm ở index 0 nếu lưới tự tạo, nhưng với DataTable của ta nó đang ở index 0)
                string rawId = dgvProductsSelection.Rows[e.RowIndex].Cells["MaSanPham"].Value?.ToString() ?? "";
                if (int.TryParse(rawId, out int id))
                {
                    // Fetch full product detail from DB because we only loaded 3 columns
                    string sql = $@"SELECT s.MaSanPham, s.TenSanPham, s.GiaNhap, s.Anh, s.MoTa, d.TenDanhMuc 
                                   FROM SanPham s 
                                   LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                                   WHERE s.MaSanPham = {id}";
                    DataTable tbl = DbContext.GetDataToTable(sql);

                    if (tbl.Rows.Count > 0)
                    {
                        SetTab2InputVisibility(true);
                        DataRow r = tbl.Rows[0];
                        txtSelMaSP.Text = r["MaSanPham"].ToString();
                        txtSelTenSP.Text = r["TenSanPham"]?.ToString() ?? "";
                        txtSelGiaNhap.Text = r["GiaNhap"]?.ToString() ?? "0";
                        txtSelSoLuong.Enabled = true;
                        txtSelGiaNhap.Enabled = true;

                        btnAddToCart.Enabled = true; // Bật nút THÊM
                        guna2Button3.Enabled = true; // Bật nút BỎ QUA

                        // Điền mô tả và nạp ảnh an toàn tránh lock file
                        string desc = r["MoTa"]?.ToString() ?? "";
                        string catName = r["TenDanhMuc"]?.ToString() ?? "Không rõ";
                        lblProductDetailDesc.Text = $"Mã sản phẩm: {id}\n" +
                                                    $"Danh mục: {catName}\n" +
                                                    $"Giá nhập đề xuất: {Convert.ToDouble(r["GiaNhap"]):N0} VNĐ\n" +
                                                    $"Mô tả: {desc}";

                        string imagePath = r["Anh"]?.ToString() ?? "";
                        LoadDetailProductImage(imagePath);

                        // Nếu sản phẩm đã được thêm vào giỏ hàng từ trước -> hiện số lượng hiện tại
                        var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                        if (existing != null)
                        {
                            txtSelSoLuong.Text = existing.SoLuong.ToString();
                            txtSelGiaNhap.Text = existing.GiaNhap.ToString();
                        }
                        else
                        {
                            txtSelSoLuong.Text = "1";
                        }

                        // Chuyển sang Tab chi tiết sản phẩm để người dùng xem hình ảnh sản phẩm
                        tabSelectionContainer.SelectedTab = tabProductDetail;

                        if (txtSelSoLuong.Enabled)
                        {
                            txtSelSoLuong.Focus();
                            txtSelSoLuong.SelectAll();
                        }
                    }
                }
            }
        }

        // 5.2.17.1. Thủ tục nạp ảnh sản phẩm bên Tab 2 tránh lock file
        private void LoadDetailProductImage(string imagePath)
        {
            if (picProductDetail.Image != null)
            {
                picProductDetail.Image.Dispose();
                picProductDetail.Image = null;
            }

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(imagePath);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        picProductDetail.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    picProductDetail.Image = null;
                }
            }
        }

        // 5.2.18. Chọn sản phẩm từ lưới GIỎ HÀNG bên phải
        private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (int.TryParse(rawId, out int id))
                {
                    var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                    if (item != null)
                    {
                        // Chuyển sang chế độ Sửa
                        isAddingDetail = false;
                        btnAddToCart.Enabled = false; // Tắt THÊM
                        guna2Button4.Enabled = true; // Bật SỬA
                        btnRemoveFromCart.Enabled = true; // Bật XÓA
                        guna2Button3.Enabled = true; // Bật BỎ QUA
                        btnBackToReceipt.Enabled = true; // LƯU CHI TIẾT PHIẾU luôn bật

                        txtSelSoLuong.Enabled = true;
                        txtSelGiaNhap.Enabled = true;

                        txtSelMaSP.Text = item.MaSanPham.ToString();
                        txtSelTenSP.Text = item.TenSanPham;
                        SetTab2InputVisibility(true);
                        txtSelSoLuong.Text = item.SoLuong.ToString();
                        txtSelGiaNhap.Text = item.GiaNhap.ToString();

                        txtSelSoLuong.Focus();
                        txtSelSoLuong.SelectAll();
                    }
                }
            }
        }

        // 5.2.19. Nạp dữ liệu giỏ hàng hiện tại lên lưới bên phải
        private void LoadCurrentDetailsGrid()
        {
            dgvCurrentDetails.Rows.Clear();
            double total = 0;
            foreach (var item in currentDetails)
            {
                total += item.ThanhTien;
                dgvCurrentDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong.ToString("N0"),
                    item.GiaNhap.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }

            lblTotalAmount.Text = $"TỔNG TIỀN TẠM TÍNH: {total.ToString("N0")} đ";

            // Định dạng lưới giỏ hàng
            dgvCurrentDetails.RowTemplate.Height = 35;
            dgvCurrentDetails.ColumnHeadersHeight = 35;
        }



        // 5.2.20. Bấm nút THÊM (Bắt đầu thêm sản phẩm)
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập hoặc ấn Thêm Mới ở Tab 1 trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu nhập này đã hoàn thành hoặc đã hủy, không thể thay đổi chi tiết sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(rawId) || !int.TryParse(rawId, out int id))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách bên trái trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng nhập kho phải là số nguyên dương lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            if (!double.TryParse(txtSelGiaNhap.Text, out double price) || price < 0)
            {
                MessageBox.Show("Giá nhập hàng kho phải là số thực không âm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong += qty; // Cộng dồn nếu đã có
                existing.GiaNhap = price;
            }
            else
            {
                currentDetails.Add(new StockInDetailModel
                {
                    MaSanPham = id,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    GiaNhap = price
                });
            }

            LoadCurrentDetailsGrid();
            MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetTab2State();
        }

        // 5.2.20.1 Bấm nút SỬA (Sửa ngay sản phẩm đang chọn)
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập hoặc ấn Thêm Mới ở Tab 1 trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu nhập này đã hoàn thành hoặc đã hủy, không thể thay đổi chi tiết sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(rawId) || !int.TryParse(rawId, out int id))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ giỏ hàng bên phải để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng nhập kho phải là số nguyên dương lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            if (!double.TryParse(txtSelGiaNhap.Text, out double price) || price < 0)
            {
                MessageBox.Show("Giá nhập hàng kho phải là số thực không âm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (item != null)
            {
                item.SoLuong = qty;
                item.GiaNhap = price;
                LoadCurrentDetailsGrid();
                MessageBox.Show("Sửa chi tiết sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetTab2State();
            }
        }

        // 5.2.21. Bấm nút XÓA sản phẩm khỏi giỏ hàng
        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập hoặc ấn Thêm Mới ở Tab 1 trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu nhập này đã hoàn thành hoặc đã hủy, không thể thay đổi chi tiết sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(rawId) || !int.TryParse(rawId, out int id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (item != null)
            {
                currentDetails.Remove(item);
                LoadCurrentDetailsGrid();
                MessageBox.Show("Xóa sản phẩm khỏi giỏ hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetTab2State();
            }
        }

        // 5.2.22. Reset ô nhập liệu chi tiết
        private void btnResetCartForm_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }



        // 5.2.23. Bấm nút tìm kiếm sản phẩm trên Tab 2
        private void btnStockInSearch_Click(object sender, EventArgs e)
        {
            // Lần 1: Kích hoạt chế độ tìm kiếm
            if (txtSelMaSP.ReadOnly == true && txtSelTenSP.ReadOnly == true)
            {
                ResetTab2State();
                txtSelMaSP.Enabled = true;
                txtSelTenSP.Enabled = true;
                txtSelMaSP.ReadOnly = false;
                txtSelTenSP.ReadOnly = false;
                txtSelMaSP.Text = ""; // Xóa trắng để nhập
                txtSelTenSP.Text = ""; // Xóa trắng để nhập
                txtSelMaSP.Focus();

                // Ẩn hết các nút đi theo yêu cầu (ngoại trừ nút Tìm kiếm và Làm lại)
                btnAddToCart.Enabled = false;
                guna2Button4.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBackToReceipt.Enabled = false;
                guna2Button3.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật! Vui lòng nhập Mã hoặc Tên SP ở cột phải, rồi ấn Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lần 2: Thực thi tìm kiếm
            FilterProducts();
            MessageBox.Show($"Tìm thấy {dgvProductsSelection.Rows.Count} sản phẩm phù hợp!", "Tìm kiếm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 5.2.24. Làm mới danh sách chọn sản phẩm Tab 2
        private void btnStockInRefresh_Click(object sender, EventArgs e)
        {
            txtProductSearch.Text = "";
            ResetTab2State();
            LoadProductsSelectionGrid();
        }

        private void tabSelectionContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabSelectionContainer.SelectedTab == tabProductDetail)
            {
                // Mặc định không cần nạp thêm
            }
        }

        // 5.2.25. Bấm nút xác nhận CHỌN MẶT HÀNG ở Tab chi tiết ảnh
        private void btnSelectProduct_Click(object sender, EventArgs e)
        {
            string rawId = txtSelMaSP.Text;
            if (!string.IsNullOrEmpty(rawId) && int.TryParse(rawId, out int id))
            {
                tabSelectionContainer.SelectedTab = tabListProducts; // Quay về lưới danh sách
                txtSelSoLuong.Focus();
                txtSelSoLuong.SelectAll();
            }
        }

        // 5.2.26. Bấm nút LƯU CHI TIẾT PHIẾU (Đẩy thẳng lên DB)
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {






            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
            {
                MessageBox.Show("Lỗi không xác định: Không tìm thấy mã phiếu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi trực tiếp hàm lưu phiếu bên Tab 1
            btnSave_Click(sender, e);

            // Nếu lưu thành công, txtMaPhieuNhap.Text có thể sẽ bị reset, ta chuyển về Tab 1
            tabMain.SelectedTab = tabPhieuNhap;
        }

        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabChonSanPham)
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
                {
                    MessageBox.Show("Vui lòng chọn một phiếu nhập hoặc ấn Thêm Mới ở Tab 1 trước khi chuyển sang Tab 2!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        // 5.2.27 Bấm nút BỎ QUA
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }

        private void lblProductDetailDesc_Click(object sender, EventArgs e)
        {


        }
    }
}
