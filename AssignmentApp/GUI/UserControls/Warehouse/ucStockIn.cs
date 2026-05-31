using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Import DbContext để dùng trực tiếp Database

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucStockIn : UserControl
    {
        #region A. CODE DÙNG CHUNG (SHARED CODE)

        // Class mô hình chứa dữ liệu 1 dòng chi tiết sản phẩm (Nằm trong giỏ hàng)
        public class StockInDetailModel
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double GiaNhap { get; set; }
            public double ThanhTien 
            {
                get 
                {
                    return SoLuong * GiaNhap;
                }
            }
        }

        // Biến toàn cục dùng chung cho cả 2 Tab
        private List<StockInDetailModel> currentDetails = new List<StockInDetailModel>();
        private bool isEditing = false;
        private bool isAddingNew = false;
        private bool isAddingDetail = false;
        private bool isSearching = false;
        
        // Mặc định người dùng hệ thống là Admin
        private int activeUserId = 1;      
        private string activeUserName = "Admin";

        /// <summary>
        /// Hàm khởi tạo UserControl.
        /// Chạy đầu tiên để khởi tạo các giao diện tĩnh.
        /// </summary>
        public ucStockIn()
        {
            InitializeComponent();

            // Cấu hình ComboBox trạng thái (chuyển từ Designer sang đây để dễ kiểm soát)
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Chờ xử lý", "Đã hoàn thành", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Sự kiện Load: Chạy khi mở trang Quản lý Nhập Kho.
        /// </summary>
        private void ucStockIn_Load(object sender, EventArgs e)
        {
            // Kết nối Database nếu chưa kết nối
            DbContext.Ketnoi();

            // Đăng ký sự kiện Click cho nút Tìm kiếm và Làm lại ở Tab 2
            guna2Button1.Click += btnStockInSearch_Click;
            guna2Button2.Click += btnStockInRefresh_Click;
            
            // Bắt sự kiện chuyển Tab
            tabMain.Selecting += tabMain_Selecting;

            // Đăng ký sự kiện click chuột vào các lưới
            dgvDetails.CellClick += dgvDetails_CellClick;
            dgvProductsSelection.CellClick += dgvProductsSelection_CellClick;

            // Tải lưới chọn sản phẩm
            LoadProductsSelectionGrid(null);

            // Tải thông tin người dùng đang hoạt động
            LoadActiveUser();

            // Đặt tiêu đề lưới hiển thị danh sách phiếu nhập
            lblGridTitle.Text = "DANH SÁCH PHIẾU NHẬP";
            
            if (dgvDetails.Columns.Count >= 5)
            {
                dgvDetails.Columns[0].HeaderText = "Mã Phiếu Nhập";
                dgvDetails.Columns[1].HeaderText = "Mã NV";
                dgvDetails.Columns[2].HeaderText = "Ngày Nhập";
                dgvDetails.Columns[3].HeaderText = "Trạng Thái";
                dgvDetails.Columns[4].HeaderText = "Tổng Tiền";
            }

            // Tải lưới phiếu nhập
            LoadReceiptsGrid(null);

            // Đưa tất cả các ô về trạng thái nghỉ (khóa và làm sạch)
            ResetTab1State();
            ResetTab2State();
        }

        /// <summary>
        /// Lấy thông tin tài khoản người dùng đang hoạt động.
        /// (Dùng cú pháp if-else cơ bản)
        /// </summary>
        private void LoadActiveUser()
        {
            try
            {
                string sqlUser = "SELECT TOP 1 MaNguoiDung, TenNguoiDung FROM NguoiDung";
                DataTable tblUser = DbContext.GetDataToTable(sqlUser);
                
                if (tblUser.Rows.Count > 0)
                {
                    activeUserId = Convert.ToInt32(tblUser.Rows[0]["MaNguoiDung"]);
                    
                    if (tblUser.Rows[0]["TenNguoiDung"] != DBNull.Value)
                    {
                        activeUserName = tblUser.Rows[0]["TenNguoiDung"].ToString();
                    }
                    else
                    {
                        activeUserName = "Admin";
                    }
                }
            }
            catch
            {
                activeUserId = 1;
                activeUserName = "Admin";
            }
        }

        /// <summary>
        /// Ngăn chặn chuyển qua Tab 2 nếu người dùng chưa chọn phiếu hoặc chưa bấm Thêm mới.
        /// </summary>
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabChonSanPham)
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
                {
                    MessageBox.Show("Vui lòng chọn một phiếu nhập ở Tab 1, hoặc ấn nút Thêm Mới, trước khi chuyển sang Tab 2!", "Cảnh báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true; // Khóa không cho chuyển Tab
                }
            }
        }

        /// <summary>
        /// NGHIỆP VỤ (CHUNG): Cộng thêm vào kho + Lưu vào lịch sử (LichSuNhapKho).
        /// Được gọi khi Phiếu nhập chuyển sang trạng thái "Đã hoàn thành".
        /// </summary>
        private void UpdateProductInventoryAndLog(int receiptId, List<StockInDetailModel> details)
        {
            foreach (StockInDetailModel item in details)
            {
                string sqlBefore = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {item.MaSanPham}";
                string beforeStr = DbContext.GetFieldValues(sqlBefore);
                
                int before = 0;
                if (string.IsNullOrEmpty(beforeStr) == false)
                {
                    before = Convert.ToInt32(beforeStr);
                }
                
                int after = before + item.SoLuong; // Cộng dồn số cũ vào số lượng nhập

                // 1. Ghi vào bảng Sản phẩm
                string sqlUpdate = $"UPDATE SanPham SET SoLuongTon = {after} WHERE MaSanPham = {item.MaSanPham}";
                DbContext.RunSql(sqlUpdate);

                // 2. Ghi vào Sổ theo dõi Kho
                string sqlLog = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                   VALUES ({item.MaSanPham}, GETDATE(), {item.SoLuong}, {before}, {after}, N'Nhập kho', {receiptId}, N'Hoàn thành')";
                DbContext.RunSql(sqlLog);
            }
        }

        /// <summary>
        /// NGHIỆP VỤ ĐẢO NGƯỢC (CHUNG): Trừ lại số tồn trong kho (Khi xóa/hủy Phiếu)
        /// Nguy hiểm: Phải chắc chắn việc trừ không làm cho số tồn bị Âm (<0)
        /// </summary>
        private void RevertProductInventoryAndLog(int receiptId, List<StockInDetailModel> details)
        {
            foreach (StockInDetailModel item in details)
            {
                string sqlBefore = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {item.MaSanPham}";
                string beforeStr = DbContext.GetFieldValues(sqlBefore);
                
                int before = 0;
                if (string.IsNullOrEmpty(beforeStr) == false)
                {
                    before = Convert.ToInt32(beforeStr);
                }

                int after = before - item.SoLuong; // Lệnh hủy phiếu -> Rút lại kho
                if (after < 0) after = 0; // Kỹ thuật an toàn: Không bao giờ để kho bị âm

                string sqlUpdate = $"UPDATE SanPham SET SoLuongTon = {after} WHERE MaSanPham = {item.MaSanPham}";
                DbContext.RunSql(sqlUpdate);

                string sqlLog = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                   VALUES ({item.MaSanPham}, GETDATE(), -{item.SoLuong}, {before}, {after}, N'Nhập kho', {receiptId}, N'Hủy bỏ')";
                DbContext.RunSql(sqlLog);
            }
        }

        #endregion

        #region B. CODE DÀNH RIÊNG CHO TAB 1 (QUẢN LÝ PHIẾU NHẬP)

        /// <summary>
        /// Xóa trắng và khóa các ô nhập liệu ở Tab 1 (Phiếu Nhập)
        /// </summary>
        private void ResetTab1State()
        {
            isAddingNew = false;
            isEditing = false;
            isSearching = false;
            
            lblChonRightTitle.Text = "CHI TIẾT SẢN PHẨM ĐÃ CHỌN";
            lblStockInTitle.Text = "MÃ PHIẾU:";
            
            txtMaPhieuNhap.Text = "";
            dtNgayNhap.Format = DateTimePickerFormat.Custom;
            dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            dtNgayNhap.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            txtNguoiDung.Text = "";

            txtMaPhieuNhap.ReadOnly = true;
            txtMaPhieuNhap.Enabled = false; 
            
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

        /// <summary>
        /// Tải danh sách Phiếu Nhập vào lưới ở Tab 1.
        /// </summary>
        private void LoadReceiptsGrid(DataTable customTable)
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

            foreach (DataRow r in tbl.Rows)
            {
                int id = Convert.ToInt32(r["MaPhieuNhap"]);
                
                string user = "";
                if (r["MaNguoiDung"] != DBNull.Value) user = r["MaNguoiDung"].ToString();

                DateTime date = DateTime.Now;
                if (r["NgayNhap"] != DBNull.Value) date = Convert.ToDateTime(r["NgayNhap"]);

                string status = "Chờ xử lý";
                if (r["TrangThai"] != DBNull.Value) status = r["TrangThai"].ToString();

                double total = 0;
                if (r["TongTien"] != DBNull.Value) total = Convert.ToDouble(r["TongTien"]);

                dgvDetails.Rows.Add(
                    id,
                    user,
                    date.ToString("dd/MM/yyyy HH:mm"),
                    status,
                    total.ToString("N0") + " đ"
                );
            }

            dgvDetails.RowTemplate.Height = 40;
            dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDetails.ColumnHeadersHeight = 40;
            dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }

        /// <summary>
        /// Xử lý chọn 1 phiếu nhập từ danh sách Tab 1, sau đó kéo chi tiết (giỏ hàng) của phiếu đó.
        /// </summary>
        private void SelectReceiptRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDetails.Rows.Count) return;

            string codeVal = "";
            if (dgvDetails.Rows[rowIndex].Cells[0].Value != null)
            {
                codeVal = dgvDetails.Rows[rowIndex].Cells[0].Value.ToString();
            }

            lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP " + codeVal;
            lblStockInTitle.Text = "MÃ PHIẾU: " + codeVal;

            dgvDetails.ClearSelection();
            dgvDetails.Rows[rowIndex].Selected = true;

            int receiptId = 0;
            if (int.TryParse(codeVal, out receiptId) == false) return;

            string sql = $@"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai 
                           FROM PhieuNhap p 
                           WHERE p.MaPhieuNhap = {receiptId}";
            DataTable tbl = DbContext.GetDataToTable(sql);

            if (tbl.Rows.Count > 0)
            {
                DataRow r = tbl.Rows[0];
                txtMaPhieuNhap.Text = r["MaPhieuNhap"].ToString();
                
                if (r["MaNguoiDung"] != DBNull.Value) txtNguoiDung.Text = r["MaNguoiDung"].ToString();
                else txtNguoiDung.Text = "";

                if (r["NgayNhap"] != DBNull.Value) dtNgayNhap.Value = Convert.ToDateTime(r["NgayNhap"]);
                else dtNgayNhap.Value = DateTime.Now;

                if (r["TrangThai"] != DBNull.Value) cboTrangThai.Text = r["TrangThai"].ToString();
                else cboTrangThai.Text = "Chờ xử lý";

                // Kéo giỏ hàng (Chi tiết) từ Database vào biến `currentDetails` chung
                string sqlDet = $@"SELECT c.MaSanPham, s.TenSanPham, c.SoLuong, c.DonGia 
                                   FROM ChiTietNhapHang c 
                                   LEFT JOIN SanPham s ON c.MaSanPham = s.MaSanPham 
                                   WHERE c.MaPhieuNhap = {receiptId}";
                DataTable tblDet = DbContext.GetDataToTable(sqlDet);

                currentDetails.Clear(); // Dọn giỏ cũ
                
                foreach (DataRow rDet in tblDet.Rows)
                {
                    StockInDetailModel detail = new StockInDetailModel();
                    detail.MaSanPham = Convert.ToInt32(rDet["MaSanPham"]);
                    
                    if (rDet["TenSanPham"] != DBNull.Value) detail.TenSanPham = rDet["TenSanPham"].ToString();
                    else detail.TenSanPham = "Sản phẩm ẩn";

                    detail.SoLuong = Convert.ToInt32(rDet["SoLuong"]);
                    detail.GiaNhap = Convert.ToDouble(rDet["DonGia"]);

                    currentDetails.Add(detail);
                }
                
                // Tải giỏ hàng lên lưới con bên phải
                LoadCurrentDetailsGrid();
            }
        }

        /// <summary>
        /// Kích hoạt khi bấm vào 1 dòng Phiếu Nhập trên lưới.
        /// </summary>
        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (isEditing == true)
                {
                    MessageBox.Show("Hệ thống đang ở chế độ Thêm/Sửa! Vui lòng Lưu hoặc Bỏ qua thao tác trước khi chọn phiếu khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtMaPhieuNhap.Enabled == true)
                {
                    txtMaPhieuNhap.Enabled = false;
                    btnAdd.Enabled = true;
                }

                SelectReceiptRow(e.RowIndex);

                txtMaPhieuNhap.Enabled = false; 
                txtNguoiDung.Enabled = true;
                txtNguoiDung.ReadOnly = false;
                dtNgayNhap.Enabled = false;    
                cboTrangThai.Enabled = true;

                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;

                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        /// <summary>
        /// Cấu hình định dạng ngày tháng khi kích hoạt lịch chọn.
        /// </summary>
        private void DtNgayNhap_ValueChanged(object sender, EventArgs e)
        {
            if (dtNgayNhap.CustomFormat == " ")
            {
                dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            }
        }

        /// <summary>
        /// Nút Thêm mới phiếu nhập.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing == false)
            {
                isAddingNew = true;
                isEditing = true;

                // Sinh mã giả lập cho người dùng dễ nhìn
                string sqlMax = "SELECT MAX(MaPhieuNhap) FROM PhieuNhap";
                string maxStr = DbContext.GetFieldValues(sqlMax);
                int nextId = 101;
                if (string.IsNullOrEmpty(maxStr) == false)
                {
                    nextId = Convert.ToInt32(maxStr) + 1;
                }

                txtMaPhieuNhap.Text = "Tự động sinh";
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP MỚI";
                lblStockInTitle.Text = "MÃ PHIẾU: TỰ ĐỘNG SINH";
                
                // Mặc định lấy user đang đăng nhập
                if (AssignmentApp.BLL.Session.UserSession.CurrentUser != null)
                {
                    txtNguoiDung.Text = AssignmentApp.BLL.Session.UserSession.CurrentUser.MaNguoiDung.ToString();
                }
                else
                {
                    txtNguoiDung.Text = activeUserId.ToString();
                }

                dtNgayNhap.Value = DateTime.Now;
                cboTrangThai.Text = "Chờ xử lý";

                currentDetails.Clear(); // Dọn giỏ hàng để chuẩn bị nhập đồ mới
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

        /// <summary>
        /// Nút Sửa phiếu nhập. (Code cực khủng: vừa lưu phiếu, vừa cập nhật tồn kho).
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            string sqlCheck = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
            string currentStatus = DbContext.GetFieldValues(sqlCheck);

            if (currentStatus == "Đã hoàn thành" || currentStatus == "Đã hủy")
            {
                MessageBox.Show($"Đơn đã {currentStatus.ToLower()}, hệ thống đã khóa lại, không thể sửa đổi để bảo toàn toàn vẹn dữ liệu!", "Cảnh báo an ninh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtNguoiDung.Text.Trim()) == true)
            {
                MessageBox.Show("Bạn phải nhập Mã nhân viên (Người dùng) lập phiếu!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            int userId = 0;
            if (int.TryParse(txtNguoiDung.Text.Trim(), out userId) == false)
            {
                MessageBox.Show("Mã nhân viên bắt buộc phải là con số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            string status = cboTrangThai.Text;
            if (status == "Đã hoàn thành" && currentDetails.Count == 0)
            {
                MessageBox.Show("Bạn không thể lưu phiếu RỖNG (không có sản phẩm) ở trạng thái 'Đã hoàn thành'! Vui lòng sang Tab 2 thêm sản phẩm, hoặc đổi trạng thái thành Chờ xử lý.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalAmount = 0;
            foreach (StockInDetailModel d in currentDetails)
            {
                totalAmount += d.ThanhTien;
            }

            string dateFormatted = dtNgayNhap.Value.ToString("yyyy-MM-dd HH:mm:ss");

            // 1. Cập nhật thông tin phiếu tổng quan (Master)
            string sqlUpdateMaster = $@"UPDATE PhieuNhap 
                                        SET MaNguoiDung = {userId},
                                            NgayNhap = '{dateFormatted}', 
                                            TrangThai = N'{status}', 
                                            TongTien = {totalAmount} 
                                        WHERE MaPhieuNhap = {receiptId}";
            DbContext.RunSql(sqlUpdateMaster);

            // 2. Cập nhật Giỏ hàng (Xóa cũ, chèn mới)
            string sqlDelOldDetails = $"DELETE FROM ChiTietNhapHang WHERE MaPhieuNhap = {receiptId}";
            DbContext.RunSql(sqlDelOldDetails);

            foreach (StockInDetailModel d in currentDetails)
            {
                string sqlInsertDetail = $@"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                            VALUES ({receiptId}, {d.MaSanPham}, {d.SoLuong}, {d.GiaNhap})";
                DbContext.RunSql(sqlInsertDetail);
            }

            // 3. Nghiệp vụ: Chuyển đổi trạng thái tồn kho thực tế.
            string invFeedback = "";
            if (currentStatus != "Đã hoàn thành" && status == "Đã hoàn thành")
            {
                UpdateProductInventoryAndLog(receiptId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Phiếu nhập chuyển sang 'Đã hoàn thành'. Hàng hóa đã được cất kho thành công!";
            }
            else if (currentStatus == "Đã hoàn thành" && status == "Đã hủy")
            {
                RevertProductInventoryAndLog(receiptId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Phiếu nhập vừa bị HỦY GẤP. Đã trừ ngược lại số lượng tồn kho tương ứng!";
            }

            MessageBox.Show("Cập nhật thông tin phiếu nhập thành công!" + invFeedback, "Hoàn tất nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadReceiptsGrid(null);
            ResetTab1State();
            ResetTab2State();
        }

        /// <summary>
        /// Nút Xóa phiếu nhập (Cập nhật thành 'Đã hủy' - Soft Delete).
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            string sqlCheck = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
            string currentStatus = DbContext.GetFieldValues(sqlCheck);

            if (currentStatus == "Đã hủy")
            {
                MessageBox.Show("Phiếu nhập này vốn dĩ đã bị Hủy rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Bạn có thực sự muốn xóa (hủy bỏ) phiếu nhập mã #{receiptId} không?", "Xác nhận khẩn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    string sqlUpdate = $"UPDATE PhieuNhap SET TrangThai = N'Đã hủy' WHERE MaPhieuNhap = {receiptId}";
                    DbContext.RunSql(sqlUpdate);

                    string invFeedback = "";
                    if (currentStatus == "Đã hoàn thành")
                    {
                        RevertProductInventoryAndLog(receiptId, currentDetails);
                        invFeedback = "\n[KHO HÀNG] Phiếu nhập bị HỦY. Đã tự động thu hồi lại tồn kho sản phẩm trong kho!";
                    }

                    MessageBox.Show("Đã hủy bỏ phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadReceiptsGrid(null);
                    ResetTab1State();
                    ResetTab2State();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi kỹ thuật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Nút Lưu mới toàn bộ 1 phiếu.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNguoiDung.Text.Trim()) == true)
            {
                MessageBox.Show("Bạn phải nhập Mã nhân viên lập phiếu!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            int userId = 0;
            if (int.TryParse(txtNguoiDung.Text.Trim(), out userId) == false)
            {
                MessageBox.Show("Mã nhân viên phải là kiểu số nguyên!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            string status = cboTrangThai.Text;
            if (status == "Đã hoàn thành" && currentDetails.Count == 0)
            {
                MessageBox.Show("Không thể lưu một phiếu 'Trống rỗng' mà ở trạng thái 'Đã hoàn thành'! Xin hãy qua Tab 2 thêm hàng.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalAmount = 0;
            foreach (StockInDetailModel d in currentDetails)
            {
                totalAmount += d.ThanhTien;
            }
            
            string dateFormatted = dtNgayNhap.Value.ToString("yyyy-MM-dd HH:mm:ss");

            // 1. Thêm phiếu mới
            string sqlInsertMaster = $@"INSERT INTO PhieuNhap (MaNguoiDung, TongTien, TrangThai, NgayNhap) 
                                        VALUES ({userId}, {totalAmount}, N'{status}', '{dateFormatted}')";
            DbContext.RunSql(sqlInsertMaster);

            // Tìm Mã Phiếu Vừa Sinh Ra
            string sqlMax = "SELECT MAX(MaPhieuNhap) FROM PhieuNhap";
            string maxStr = DbContext.GetFieldValues(sqlMax);
            int newId = 101;
            if (string.IsNullOrEmpty(maxStr) == false)
            {
                newId = Convert.ToInt32(maxStr);
            }

            // 2. Chèn giỏ hàng vào Database
            foreach (StockInDetailModel d in currentDetails)
            {
                string sqlInsertDetail = $@"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                            VALUES ({newId}, {d.MaSanPham}, {d.SoLuong}, {d.GiaNhap})";
                DbContext.RunSql(sqlInsertDetail);
            }

            // 3. Nếu là trạng thái Hoàn Thành -> Cộng thẳng vào Kho luôn
            string invFeedback = "";
            if (status == "Đã hoàn thành")
            {
                UpdateProductInventoryAndLog(newId, currentDetails);
                invFeedback = "\n[KHO HÀNG] Số lượng hàng đã được nhập thẳng vào Kho thành công!";
            }

            MessageBox.Show("Lập phiếu nhập kho mới thành công!" + invFeedback, "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);

            isAddingNew = false;
            LoadReceiptsGrid(null);
            ResetTab1State();
            ResetTab2State();
        }

        /// <summary>
        /// Nút Bỏ qua thay đổi.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetTab1State(); 
        }

        /// <summary>
        /// Nút Tìm kiếm phiếu (Có 2 Giai đoạn: Bật form -> Chạy Code).
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaPhieuNhap.Enabled == false)
            {
                ResetTab1State();
                
                txtMaPhieuNhap.Enabled = true;
                txtMaPhieuNhap.ReadOnly = false;
                txtMaPhieuNhap.Text = ""; 

                txtNguoiDung.Enabled = true;
                txtNguoiDung.ReadOnly = false;
                txtNguoiDung.Text = "";
                
                cboTrangThai.Enabled = true;
                
                dtNgayNhap.Enabled = true;
                dtNgayNhap.Format = DateTimePickerFormat.Custom;
                dtNgayNhap.CustomFormat = " ";

                dtNgayNhap.ValueChanged -= DtNgayNhap_ValueChanged;
                dtNgayNhap.ValueChanged += DtNgayNhap_ValueChanged;

                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                MessageBox.Show("Chế độ Lọc Dữ Liệu Đã Bật!\nHãy nhập thông tin cần tìm (Mã Phiếu, Ngày, NV...) rồi nhấn 'Tìm kiếm' một lần nữa.", "Trợ lý Ảo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhieuNhap.Focus();
                return;
            }

            bool hasDateFilter = false;
            if (dtNgayNhap.CustomFormat != " ")
            {
                hasDateFilter = true;
            }

            if (txtMaPhieuNhap.Text == "" && txtNguoiDung.Text == "" && cboTrangThai.Text == "" && hasDateFilter == false)
            {
                MessageBox.Show("Cần tối thiểu 1 thông tin để bộ lọc có thể hoạt động!!!", "Từ chối thực thi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien FROM PhieuNhap p WHERE 1=1";

            if (txtMaPhieuNhap.Text != "" && txtMaPhieuNhap.Text != "Tự động sinh")
                sql += $" AND p.MaPhieuNhap = {txtMaPhieuNhap.Text.Trim()}";

            if (txtNguoiDung.Text != "")
                sql += $" AND p.MaNguoiDung = {txtNguoiDung.Text.Trim()}";

            if (cboTrangThai.Text != "")
                sql += $" AND p.TrangThai = N'{cboTrangThai.Text}'";

            if (hasDateFilter == true)
            {
                string dateSearch = dtNgayNhap.Value.ToString("yyyy-MM-dd");
                sql += $" AND CAST(p.NgayNhap AS DATE) = '{dateSearch}'";
            }

            DataTable tblSearch = DbContext.GetDataToTable(sql);

            if (tblSearch.Rows.Count == 0)
            {
                MessageBox.Show("Tìm đỏ cả mắt mà không thấy phiếu nào giống yêu cầu!", "Lỗi rỗng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Tuyệt vời! Lọc ra được {tblSearch.Rows.Count} phiếu thỏa mãn.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReceiptsGrid(tblSearch);
                SelectReceiptRow(0); // Chỉ định nhảy thẳng vào phiếu đầu tiên tìm được
            }
        }

        /// <summary>
        /// Nút Tải lại (F5) toàn trang.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetTab1State(); 
            LoadReceiptsGrid(null);
        }

        /// <summary>
        /// Nút Chuyển nhanh qua Tab Chọn Hàng (Tab 2)
        /// </summary>
        private void btnChooseProducts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
            {
                MessageBox.Show("Muốn chọn hàng vào phiếu thì phải có cái Phiếu trước đã. Bấm 'Thêm mới' nha!", "Nhắc nhở nhẹ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tabMain.SelectedTab = tabChonSanPham; // Chuyển Tab cái rụp

            if (txtMaPhieuNhap.Text == "Tự động sinh")
            {
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP MỚI";
            }
            else
            {
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP " + txtMaPhieuNhap.Text;
            }

            btnResetCartForm_Click(this, EventArgs.Empty);

            LoadProductsSelectionGrid(null);
            LoadCurrentDetailsGrid();
        }

        #endregion

        #region C. CODE DÀNH RIÊNG CHO TAB 2 (CHỌN HÀNG & GIỎ HÀNG)

        /// <summary>
        /// Thay đổi trạng thái Ẩn/Hiện của các khung nhập chi tiết sản phẩm ở Tab 2.
        /// </summary>
        private void SetTab2InputVisibility(bool visible)
        {
            lblSelMaSP.Visible = visible;
            txtSelMaSP.Visible = visible;
            txtSelMaSP.Enabled = false; // Mã SP luôn luôn không cho tự gõ tay

            lblSelTenSP.Visible = visible;
            txtSelTenSP.Visible = visible;
            txtSelTenSP.Enabled = false; // Tên SP ăn theo CSDL, cấm gõ tay

            lblSelSoLuong.Visible = visible;
            txtSelSoLuong.Visible = visible;
            lblSelGiaNhap.Visible = visible;
            txtSelGiaNhap.Visible = visible;
        }

        /// <summary>
        /// Xóa trắng và khóa các ô nhập liệu ở Tab 2 (Chọn Sản Phẩm)
        /// </summary>
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

            txtProductSearch.Enabled = true; // Luôn mở ô gõ tìm kiếm nhanh
            txtProductSearch.Text = "";

            btnAddToCart.Enabled = false;       // THÊM
            guna2Button4.Enabled = false;       // SỬA
            btnRemoveFromCart.Enabled = false;  // XÓA
            btnBackToReceipt.Enabled = true;    // LƯU VÀO PHIẾU
            guna2Button3.Enabled = false;       // BỎ QUA

            lblProductDetailDesc.Text = "Mã SP: --\nThông tin chi tiết về sản phẩm sẽ được cập nhật ở đây.";
            
            // Xóa ảnh cũ
            if (picProductDetail.Image != null)
            {
                picProductDetail.Image.Dispose();
                picProductDetail.Image = null;
            }

            dgvCurrentDetails.ClearSelection();
        }

        /// <summary>
        /// Tải toàn bộ danh sách sản phẩm lên bảng bên trái của Tab 2 để dễ quẹt mã/chọn thủ công.
        /// </summary>
        private void LoadProductsSelectionGrid(DataTable customTable)
        {
            DataTable tbl;
            if (customTable != null)
            {
                tbl = customTable;
            }
            else
            {
                // Chỉ hiển thị các mặt hàng Đang bán
                string sql = @"SELECT s.MaSanPham, s.TenSanPham, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.TrangThai, s.NgayTao
                               FROM SanPham s 
                               LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                               WHERE s.TrangThai = N'Đang bán' 
                               ORDER BY s.NgayTao DESC";
                tbl = DbContext.GetDataToTable(sql);
            }

            dgvProductsSelection.Columns.Clear();
            dgvProductsSelection.AutoGenerateColumns = true; // Dùng Auto vì đây là bảng phụ tìm kiếm nhanh
            dgvProductsSelection.DataSource = tbl;

            // Đổi Header bằng tiếng Việt
            if (dgvProductsSelection.Columns.Count > 0)
            {
                if (dgvProductsSelection.Columns.Contains("MaSanPham")) dgvProductsSelection.Columns["MaSanPham"].HeaderText = "Mã SP";
                if (dgvProductsSelection.Columns.Contains("TenSanPham")) dgvProductsSelection.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                if (dgvProductsSelection.Columns.Contains("TenDanhMuc")) dgvProductsSelection.Columns["TenDanhMuc"].HeaderText = "Danh Mục";
                
                if (dgvProductsSelection.Columns.Contains("GiaNhap")) 
                { 
                    dgvProductsSelection.Columns["GiaNhap"].HeaderText = "Giá Nhập"; 
                    dgvProductsSelection.Columns["GiaNhap"].DefaultCellStyle.Format = "N0"; 
                }
                
                if (dgvProductsSelection.Columns.Contains("GiaBan")) 
                { 
                    dgvProductsSelection.Columns["GiaBan"].HeaderText = "Giá Bán"; 
                    dgvProductsSelection.Columns["GiaBan"].DefaultCellStyle.Format = "N0"; 
                }
                
                if (dgvProductsSelection.Columns.Contains("SoLuongTon")) dgvProductsSelection.Columns["SoLuongTon"].HeaderText = "SL Tồn";
                if (dgvProductsSelection.Columns.Contains("TrangThai")) dgvProductsSelection.Columns["TrangThai"].HeaderText = "Trạng Thái";
                
                if (dgvProductsSelection.Columns.Contains("NgayTao")) 
                { 
                    dgvProductsSelection.Columns["NgayTao"].HeaderText = "Ngày Tạo"; 
                    dgvProductsSelection.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy"; 
                }
            }

            dgvProductsSelection.RowTemplate.Height = 35;
            dgvProductsSelection.ColumnHeadersHeight = 35;
        }

        /// <summary>
        /// Tải Danh sách Giỏ hàng bằng việc bốc dữ liệu từ biến RAM (`currentDetails`) nhả xuống DataGridView.
        /// </summary>
        private void LoadCurrentDetailsGrid()
        {
            dgvCurrentDetails.Rows.Clear();
            double total = 0;
            
            foreach (StockInDetailModel item in currentDetails)
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

            lblTotalAmount.Text = $"TỔNG TIỀN PHIẾU NÀY: {total.ToString("N0")} đ";

            dgvCurrentDetails.RowTemplate.Height = 35;
            dgvCurrentDetails.ColumnHeadersHeight = 35;
        }

        /// <summary>
        /// Hàm lọc sản phẩm (Được gọi liên tục mỗi khi nhấn phím).
        /// </summary>
        private void FilterProducts()
        {
            string maSP = txtSelMaSP.Text.Trim();
            string tenSP = txtSelTenSP.Text.Trim();
            
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, d.TenDanhMuc, s.GiaNhap, s.GiaBan, s.SoLuongTon, s.TrangThai, s.NgayTao 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE s.TrangThai = N'Đang bán'";

            if (string.IsNullOrEmpty(maSP) == false)
            {
                sql += $" AND s.MaSanPham LIKE '%{maSP}%'";
            }
            if (string.IsNullOrEmpty(tenSP) == false)
            {
                sql += $" AND (s.TenSanPham LIKE N'%{tenSP}%' OR s.MoTa LIKE N'%{tenSP}%')";
            }
            sql += " ORDER BY s.NgayTao DESC";

            DataTable tblFiltered = DbContext.GetDataToTable(sql);
            LoadProductsSelectionGrid(tblFiltered);
        }

        /// <summary>
        /// Hàm tải và hiển thị ảnh an toàn (tránh treo hoặc khóa file hệ thống).
        /// </summary>
        private void LoadDetailProductImage(string imagePath)
        {
            if (picProductDetail.Image != null)
            {
                picProductDetail.Image.Dispose();
                picProductDetail.Image = null;
            }

            if (string.IsNullOrEmpty(imagePath) == false && File.Exists(imagePath) == true)
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(imagePath);
                    MemoryStream ms = new MemoryStream(bytes);
                    picProductDetail.Image = Image.FromStream(ms);
                }
                catch
                {
                    picProductDetail.Image = null;
                }
            }
        }

        /// <summary>
        /// Bấm chuột vào sản phẩm bên cột Danh sách.
        /// </summary>
        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Nếu đang chỉnh sửa rở dang bên kia thì không cho chọn tiếp
                if (btnRemoveFromCart.Enabled == true)
                {
                    MessageBox.Show("Hãy hoàn tất việc thêm/sửa sản phẩm đang thao tác trước khi chuyển sang chọn sản phẩm khác!", "Lời nhắc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                string rawId = "";
                if (dgvProductsSelection.Rows[e.RowIndex].Cells["MaSanPham"].Value != null)
                {
                    rawId = dgvProductsSelection.Rows[e.RowIndex].Cells["MaSanPham"].Value.ToString();
                }

                int id = 0;
                if (int.TryParse(rawId, out id) == true)
                {
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
                        
                        if (r["TenSanPham"] != DBNull.Value) txtSelTenSP.Text = r["TenSanPham"].ToString();
                        else txtSelTenSP.Text = "";

                        if (r["GiaNhap"] != DBNull.Value) txtSelGiaNhap.Text = r["GiaNhap"].ToString();
                        else txtSelGiaNhap.Text = "0";
                        
                        txtSelSoLuong.Enabled = true;
                        txtSelGiaNhap.Enabled = true;

                        btnAddToCart.Enabled = true; // Bật nút THÊM VÀO GIỎ
                        guna2Button3.Enabled = true; // Bật BỎ QUA

                        // Gán vào bảng nhãn phụ
                        string desc = "";
                        if (r["MoTa"] != DBNull.Value) desc = r["MoTa"].ToString();

                        string catName = "Không rõ";
                        if (r["TenDanhMuc"] != DBNull.Value) catName = r["TenDanhMuc"].ToString();
                        
                        lblProductDetailDesc.Text = $"Mã sản phẩm: {id}\n" +
                                                    $"Danh mục: {catName}\n" +
                                                    $"Giá nhập đề xuất: {Convert.ToDouble(r["GiaNhap"]).ToString("N0")} VNĐ\n" +
                                                    $"Mô tả chi tiết: {desc}";

                        string imagePath = "";
                        if (r["Anh"] != DBNull.Value) imagePath = r["Anh"].ToString();
                        LoadDetailProductImage(imagePath);

                        // Tìm xem trong List Giỏ Hàng hiện tại có chứa món đồ này chưa (Thay thế cú pháp LINQ)
                        StockInDetailModel existing = null;
                        foreach (StockInDetailModel model in currentDetails)
                        {
                            if (model.MaSanPham == id)
                            {
                                existing = model;
                                break;
                            }
                        }

                        if (existing != null)
                        {
                            // Nếu đã có trong giỏ -> Lấy số lượng đó thả lại vào ô textbox để người dùng tăng/giảm tiếp
                            txtSelSoLuong.Text = existing.SoLuong.ToString();
                            txtSelGiaNhap.Text = existing.GiaNhap.ToString();
                        }
                        else
                        {
                            txtSelSoLuong.Text = "1"; // Mặc định mỗi cú click chuột là số 1
                        }

                        // Mở tab ảnh
                        tabSelectionContainer.SelectedTab = tabProductDetail;

                        if (txtSelSoLuong.Enabled == true)
                        {
                            txtSelSoLuong.Focus();
                            txtSelSoLuong.SelectAll();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bấm chọn dòng sản phẩm trên Giỏ hàng (Để sửa số lượng hoặc Xóa).
        /// </summary>
        private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = "";
                if (dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    rawId = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value.ToString();
                }

                int id = 0;
                if (int.TryParse(rawId, out id) == true)
                {
                    // Lọc tay (không dùng Linq)
                    StockInDetailModel item = null;
                    foreach (StockInDetailModel model in currentDetails)
                    {
                        if (model.MaSanPham == id)
                        {
                            item = model;
                            break;
                        }
                    }

                    if (item != null)
                    {
                        isAddingDetail = false;
                        
                        btnAddToCart.Enabled = false; 
                        guna2Button4.Enabled = true; // Hiện Nút Sửa Nhỏ
                        btnRemoveFromCart.Enabled = true; // Hiện Nút Xóa Thùng Rác
                        guna2Button3.Enabled = true; 
                        btnBackToReceipt.Enabled = true; 

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

        /// <summary>
        /// Tự động tìm kiếm mỗi khi người dùng gõ phím vào ô. (Real-time).
        /// </summary>
        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        /// <summary>
        /// Thêm đồ vào Giỏ hàng
        /// </summary>
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
            {
                MessageBox.Show("Bạn chưa có Phiếu Nhập! Hãy tạo mới ở Tab 1 trước.", "Lỗi rỗng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy")
            {
                MessageBox.Show("Phiếu đã chốt rồi thì miễn thêm bớt nữa nhé!", "An ninh hàng hóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawId = txtSelMaSP.Text;
            int id = 0;
            if (string.IsNullOrEmpty(rawId) == true || int.TryParse(rawId, out id) == false)
            {
                MessageBox.Show("Bạn chưa chọn món nào bên trái cả!", "Ế khách", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qty = 0;
            if (int.TryParse(txtSelSoLuong.Text, out qty) == false || qty <= 0)
            {
                MessageBox.Show("Phải nhập số lượng dương để người ta còn xếp kho!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            double price = 0;
            if (double.TryParse(txtSelGiaNhap.Text, out price) == false || price < 0)
            {
                MessageBox.Show("Giá tiền tệ quá! Bạn nhập số đoàng hoàng đi.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            // Quét danh sách
            StockInDetailModel existing = null;
            foreach (StockInDetailModel model in currentDetails)
            {
                if (model.MaSanPham == id)
                {
                    existing = model;
                    break;
                }
            }

            if (existing != null)
            {
                existing.SoLuong += qty; // Cộng dồn thẳng vào số cũ
                existing.GiaNhap = price; // Cập nhật luôn giá mới
            }
            else
            {
                StockInDetailModel newModel = new StockInDetailModel();
                newModel.MaSanPham = id;
                newModel.TenSanPham = txtSelTenSP.Text;
                newModel.SoLuong = qty;
                newModel.GiaNhap = price;
                
                currentDetails.Add(newModel);
            }

            LoadCurrentDetailsGrid(); // Lên mâm
            MessageBox.Show("Đã cho vào rọ (Giỏ hàng)!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            ResetTab2State(); // Lau sạch bảng
        }

        /// <summary>
        /// Nút SỬA dòng trong giỏ hàng.
        /// </summary>
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true) return;
            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy") return;

            string rawId = txtSelMaSP.Text;
            int id = 0;
            if (string.IsNullOrEmpty(rawId) == true || int.TryParse(rawId, out id) == false) return;

            int qty = 0;
            if (int.TryParse(txtSelSoLuong.Text, out qty) == false || qty <= 0)
            {
                MessageBox.Show("Số lượng phải chuẩn chứ!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            double price = 0;
            if (double.TryParse(txtSelGiaNhap.Text, out price) == false || price < 0)
            {
                MessageBox.Show("Giá tiền phải chuẩn chứ!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            StockInDetailModel item = null;
            foreach (StockInDetailModel model in currentDetails)
            {
                if (model.MaSanPham == id)
                {
                    item = model;
                    break;
                }
            }

            if (item != null)
            {
                item.SoLuong = qty;
                item.GiaNhap = price;
                LoadCurrentDetailsGrid();
                MessageBox.Show("Đã cập nhật lại thông số!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetTab2State();
            }
        }

        /// <summary>
        /// Nút XÓA sản phẩm ra khỏi giỏ
        /// </summary>
        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true) return;
            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy") return;

            string rawId = txtSelMaSP.Text;
            int id = 0;
            if (string.IsNullOrEmpty(rawId) == true || int.TryParse(rawId, out id) == false) return;

            StockInDetailModel item = null;
            foreach (StockInDetailModel model in currentDetails)
            {
                if (model.MaSanPham == id)
                {
                    item = model;
                    break;
                }
            }

            if (item != null)
            {
                currentDetails.Remove(item); // Ném ra khỏi list
                LoadCurrentDetailsGrid();
                MessageBox.Show("Đã xóa khỏi giỏ!", "Sạch sẽ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetTab2State();
            }
        }

        /// <summary>
        /// Nút Bỏ Qua (Bên Tab 2)
        /// </summary>
        private void btnResetCartForm_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }

        /// <summary>
        /// Nút tìm kiếm (Tab 2)
        /// </summary>
        private void btnStockInSearch_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.ReadOnly == true && txtSelTenSP.ReadOnly == true)
            {
                ResetTab2State();
                txtSelMaSP.Enabled = true;
                txtSelTenSP.Enabled = true;
                txtSelMaSP.ReadOnly = false;
                txtSelTenSP.ReadOnly = false;
                
                txtSelMaSP.Text = ""; 
                txtSelTenSP.Text = ""; 
                txtSelMaSP.Focus();

                btnAddToCart.Enabled = false;
                guna2Button4.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                btnBackToReceipt.Enabled = false;
                guna2Button3.Enabled = false;

                MessageBox.Show("Bật Tìm Kiếm Nhanh: Bạn điền vào ô Tên hoặc Mã bên trên rồi bấm 'Lọc' tiếp nhé.", "Gợi ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FilterProducts();
            MessageBox.Show($"Tôi đã tìm thấy {dgvProductsSelection.Rows.Count} sản phẩm khớp với bạn rồi đó!", "OK nhe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Làm tươi (F5) bảng chọn hàng bên trái
        /// </summary>
        private void btnStockInRefresh_Click(object sender, EventArgs e)
        {
            txtProductSearch.Text = "";
            ResetTab2State();
            LoadProductsSelectionGrid(null);
        }

        /// <summary>
        /// Xác nhận đã xem ảnh xong, quay lại giao diện nhập số lượng.
        /// </summary>
        private void btnSelectProduct_Click(object sender, EventArgs e)
        {
            string rawId = txtSelMaSP.Text;
            int id = 0;
            if (string.IsNullOrEmpty(rawId) == false && int.TryParse(rawId, out id) == true)
            {
                tabSelectionContainer.SelectedTab = tabListProducts; 
                txtSelSoLuong.Focus();
                txtSelSoLuong.SelectAll();
            }
        }

        /// <summary>
        /// Bấm Nút 'Lưu Vào Phiếu' màu đỏ to đùng bên Tab 2.
        /// Bản chất nó gọi lại chính hàm `btnSave_Click` của Tab 1.
        /// </summary>
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
            {
                MessageBox.Show("Không có phiếu nào cả, không thể lưu giỏ hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnSave_Click(sender, e); // Gọi ké 
            
            // Xong xuôi thì kéo người dùng về lại Tab Phiếu chính cho dễ nhìn.
            tabMain.SelectedTab = tabPhieuNhap;
        }

        /// <summary>
        /// Nút bỏ qua (hủy) phụ
        /// </summary>
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }

        #endregion

        #region D. CÁC SỰ KIỆN TRỐNG (TRÁNH LỖI GIAO DIỆN)

        private void tabSelectionContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Trống
        }

        private void lblProductDetailDesc_Click(object sender, EventArgs e)
        {
            // Trống
        }

        #endregion
    }
}
