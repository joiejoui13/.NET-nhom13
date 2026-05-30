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
        private int activeUserId = 1;      // Mặc định người dùng hệ thống là Admin
        private string activeUserName = "Admin";

        public ucStockIn()
        {
            InitializeComponent();
        }

        // 5.2.1. Sự kiện ucStockIn_Load khi tải Control
        private void ucStockIn_Load(object sender, EventArgs e)
        {
            // Kết nối Database nếu chưa kết nối
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Tải thông tin người dùng đang đăng nhập (hoặc mặc định) từ Database
            LoadActiveUser();

            // Thiết lập ComboBox Trạng thái phiếu nhập
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Chờ xử lý", "Đã hoàn thành", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;

            // Đặt tiêu đề lưới hiển thị danh sách phiếu nhập
            lblGridTitle.Text = "DANH SÁCH PHIẾU NHẬP";
            dgvDetails.Columns[0].HeaderText = "Mã Phiếu Nhập";
            dgvDetails.Columns[1].HeaderText = "Người Nhập";
            dgvDetails.Columns[2].HeaderText = "Ngày Nhập";
            dgvDetails.Columns[3].HeaderText = "Trạng Thái";
            dgvDetails.Columns[4].HeaderText = "Tổng Tiền";

            // Wire up cell click dynamically giống mock code
            dgvDetails.CellClick += dgvDetails_CellClick;

            // Tải lưới danh sách phiếu nhập
            LoadReceiptsGrid();

            // Chọn dòng đầu tiên nếu có dữ liệu
            if (dgvDetails.Rows.Count > 0)
            {
                SelectReceiptRow(0);
            }

            // Đưa các nút và các trường thông tin về trạng thái khóa
            SetEditState(false);
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
                string sql = @"SELECT p.MaPhieuNhap, n.TenNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                               FROM PhieuNhap p 
                               LEFT JOIN NguoiDung n ON p.MaNguoiDung = n.MaNguoiDung
                               ORDER BY p.NgayNhap DESC";
                tbl = DbContext.GetDataToTable(sql);
            }

            // Tải dữ liệu thủ công vào lưới dgvDetails giống hệt cấu trúc của mock code ban đầu
            foreach (DataRow r in tbl.Rows)
            {
                int id = Convert.ToInt32(r["MaPhieuNhap"]);
                string user = r["TenNguoiDung"]?.ToString() ?? "Admin";
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

            dgvDetails.ClearSelection();
            dgvDetails.Rows[rowIndex].Selected = true;

            int receiptId = Convert.ToInt32(dgvDetails.Rows[rowIndex].Cells[0].Value);

            string sql = $@"SELECT p.MaPhieuNhap, n.TenNguoiDung, p.NgayNhap, p.TrangThai 
                           FROM PhieuNhap p 
                           LEFT JOIN NguoiDung n ON p.MaNguoiDung = n.MaNguoiDung 
                           WHERE p.MaPhieuNhap = {receiptId}";
            DataTable tbl = DbContext.GetDataToTable(sql);

            if (tbl.Rows.Count > 0)
            {
                DataRow r = tbl.Rows[0];
                txtMaPhieuNhap.Text = r["MaPhieuNhap"].ToString();
                txtNguoiDung.Text = r["TenNguoiDung"]?.ToString() ?? "Admin";
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
            }
        }

        // 5.2.5. Xử lý click dòng trên lưới phiếu nhập
        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectReceiptRow(e.RowIndex);
            }
        }

        // 5.2.6. Thay đổi trạng thái khóa/mở các ô nhập liệu
        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Header fields
            txtMaPhieuNhap.ReadOnly = !isAddingNew;
            dtNgayNhap.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // Make all buttons visible at all times
            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // Định vị trí các nút bấm tĩnh giống mock code
            btnAdd.Location = new Point(15, 470);
            btnEdit.Location = new Point(115, 470);
            btnDelete.Location = new Point(215, 470);

            btnSave.Location = new Point(15, 515);
            btnSave.Size = new Size(140, 36);
            btnCancel.Location = new Point(165, 515);
            btnCancel.Size = new Size(140, 36);

            // Bật/tắt nút theo trạng thái sửa đổi
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && (dgvDetails.Rows.Count > 0);
            btnDelete.Enabled = !editing && (dgvDetails.Rows.Count > 0);

            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
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
                txtNguoiDung.Text = activeUserName;
                dtNgayNhap.Value = DateTime.Now;
                cboTrangThai.Text = "Chờ xử lý";

                currentDetails.Clear();

                SetEditState(true);

                // Chuyển sang Tab 2 ngay lập tức để chọn sản phẩm vào giỏ hàng
                btnChooseProducts_Click(this, EventArgs.Empty);
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
                MessageBox.Show("Nghiệp vụ kế toán: Không được phép sửa chữa phiếu nhập kho đã 'Đã hoàn thành' hoặc 'Đã hủy'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);

            // Chuyển sang Tab 2 để thay đổi giỏ hàng sản phẩm nhập
            btnChooseProducts_Click(this, EventArgs.Empty);
        }

        // 5.2.9. Bấm nút XÓA phiếu nhập hoàn toàn
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            string sqlCheck = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
            string currentStatus = DbContext.GetFieldValues(sqlCheck);

            if (currentStatus == "Đã hoàn thành")
            {
                MessageBox.Show("Nghiệp vụ kế toán: Không được phép xóa phiếu nhập kho 'Đã hoàn thành'!\nHãy chuyển trạng thái sang 'Đã hủy' để bảo toàn dữ liệu tồn kho.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa hoàn toàn phiếu nhập #{receiptId} khỏi hệ thống?", "Xác nhận xóa phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Xóa chi tiết phiếu trước do ràng buộc khóa ngoại
                    string sqlDelDetails = $"DELETE FROM ChiTietNhapHang WHERE MaPhieuNhap = {receiptId}";
                    DbContext.RunSql(sqlDelDetails);

                    // Xóa phiếu nhập chính
                    string sqlDelMaster = $"DELETE FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
                    DbContext.RunSqlDel(sqlDelMaster);

                    MessageBox.Show("Xóa phiếu nhập khỏi hệ thống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReceiptsGrid();

                    if (dgvDetails.Rows.Count > 0)
                    {
                        SelectReceiptRow(0);
                    }
                    else
                    {
                        txtMaPhieuNhap.Text = "";
                        txtNguoiDung.Text = "";
                        cboTrangThai.SelectedIndex = -1;
                        currentDetails.Clear();
                    }

                    SetEditState(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 5.2.10. Bấm nút LƯU thay đổi phiếu nhập
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentDetails.Count == 0)
            {
                MessageBox.Show("Phiếu nhập kho phải chứa ít nhất 1 sản phẩm! Hãy chọn sản phẩm ở Tab 2.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double totalAmount = currentDetails.Sum(d => d.ThanhTien);
            string status = cboTrangThai.Text;
            string dateFormatted = dtNgayNhap.Value.ToString("yyyy-MM-dd HH:mm:ss");

            if (isAddingNew)
            {
                // Thêm mới phiếu chính (PhieuNhap)
                string sqlInsertMaster = $@"INSERT INTO PhieuNhap (MaNguoiDung, TongTien, TrangThai, NgayNhap) 
                                            VALUES ({activeUserId}, {totalAmount}, N'{status}', '{dateFormatted}')";
                DbContext.RunSql(sqlInsertMaster);

                // Lấy mã phiếu vừa tự sinh ra
                string sqlMax = "SELECT MAX(MaPhieuNhap) FROM PhieuNhap";
                string maxStr = DbContext.GetFieldValues(sqlMax);
                int newId = !string.IsNullOrEmpty(maxStr) ? Convert.ToInt32(maxStr) : 101;

                // Thêm các chi tiết phiếu nhập (ChiTietNhapHang)
                foreach (var d in currentDetails)
                {
                    string sqlInsertDetail = $@"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                                VALUES ({newId}, {d.MaSanPham}, {d.SoLuong}, {d.GiaNhap})";
                    DbContext.RunSql(sqlInsertDetail);
                }

                // Nếu lưu ở trạng thái ĐÃ HOÀN THÀNH -> Tăng số lượng tồn kho sản phẩm + Tạo lịch sử nhập kho
                string invFeedback = "";
                if (status == "Đã hoàn thành")
                {
                    UpdateProductInventoryAndLog(newId, currentDetails);
                    invFeedback = "\n[KHO HÀNG] Đã tự động cập nhật cộng tăng số lượng tồn kho của các sản phẩm!";
                }

                MessageBox.Show("Thêm mới phiếu nhập kho thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Sửa phiếu cũ
                int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

                // Kiểm tra trạng thái cũ trước khi cập nhật
                string sqlOldStatus = $"SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = {receiptId}";
                string oldStatus = DbContext.GetFieldValues(sqlOldStatus);

                // Cập nhật thông tin phiếu chính
                string sqlUpdateMaster = $@"UPDATE PhieuNhap 
                                            SET NgayNhap = '{dateFormatted}', 
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
                if (oldStatus != "Đã hoàn thành" && status == "Đã hoàn thành")
                {
                    // Chuyển từ Chờ xử lý -> Hoàn thành => Cộng tăng kho hàng
                    UpdateProductInventoryAndLog(receiptId, currentDetails);
                    invFeedback = "\n[KHO HÀNG] Phiếu nhập chuyển sang 'Đã hoàn thành'. Tồn kho sản phẩm được cộng tăng!";
                }
                else if (oldStatus == "Đã hoàn thành" && status == "Đã hủy")
                {
                    // Hoàn tác tồn kho: Trừ bớt lại kho hàng do phiếu bị HỦY
                    RevertProductInventoryAndLog(receiptId, currentDetails);
                    invFeedback = "\n[KHO HÀNG] Phiếu nhập bị HỦY hoàn toàn. Đã trừ hoàn tác lại tồn kho sản phẩm tương ứng!";
                }

                MessageBox.Show("Cập nhật thông tin phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            isAddingNew = false;
            SetEditState(false);
            LoadReceiptsGrid();

            // Chọn lại dòng vừa xử lý
            if (txtMaPhieuNhap.Text != "Tự động sinh")
            {
                int targetId = Convert.ToInt32(txtMaPhieuNhap.Text);
                for (int i = 0; i < dgvDetails.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dgvDetails.Rows[i].Cells[0].Value) == targetId)
                    {
                        SelectReceiptRow(i);
                        break;
                    }
                }
            }
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
            isAddingNew = false;
            SetEditState(false);
            if (dgvDetails.SelectedRows.Count > 0)
            {
                SelectReceiptRow(dgvDetails.SelectedRows[0].Index);
            }
            else if (dgvDetails.Rows.Count > 0)
            {
                SelectReceiptRow(0);
            }
        }

        // 5.2.12. Tìm kiếm phiếu nhập theo mã trên Tab 1
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng nhập Mã phiếu nhập (dạng số) vào ô để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string searchVal = txtMaPhieuNhap.Text.Trim();
            string sql = $@"SELECT p.MaPhieuNhap, n.TenNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                           FROM PhieuNhap p 
                           LEFT JOIN NguoiDung n ON p.MaNguoiDung = n.MaNguoiDung
                           WHERE p.MaPhieuNhap = {searchVal}";
            
            DataTable tblSearch = DbContext.GetDataToTable(sql);
            if (tblSearch.Rows.Count > 0)
            {
                LoadReceiptsGrid(tblSearch);
                SelectReceiptRow(0);
            }
            else
            {
                MessageBox.Show("Không tìm thấy phiếu nhập nào có mã vừa nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 5.2.13. Làm mới danh sách phiếu nhập
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReceiptsGrid();
            if (dgvDetails.Rows.Count > 0)
            {
                SelectReceiptRow(0);
            }
            SetEditState(false);
        }

        // 5.2.14. Bấm nút CHỌN SẢN PHẨM để mở Tab 2
        private void btnChooseProducts_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabChonSanPham;

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
            dgvProductsSelection.Rows.Clear();
            DataTable tbl;
            if (customTable != null)
            {
                tbl = customTable;
            }
            else
            {
                string sql = "SELECT MaSanPham, TenSanPham, GiaNhap FROM SanPham WHERE TrangThai = N'Đang bán' ORDER BY TenSanPham ASC";
                tbl = DbContext.GetDataToTable(sql);
            }

            foreach (DataRow r in tbl.Rows)
            {
                int id = Convert.ToInt32(r["MaSanPham"]);
                string name = r["TenSanPham"]?.ToString() ?? "";
                double price = r["GiaNhap"] != DBNull.Value ? Convert.ToDouble(r["GiaNhap"]) : 0;

                dgvProductsSelection.Rows.Add(
                    id,
                    name,
                    price.ToString("N0") + " đ"
                );
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
            string keyword = txtProductSearch.Text.Trim();
            string sql = "SELECT MaSanPham, TenSanPham, GiaNhap FROM SanPham WHERE TrangThai = N'Đang bán'";

            if (!string.IsNullOrEmpty(keyword))
            {
                sql += $" AND (TenSanPham LIKE N'%{keyword}%' OR MoTa LIKE N'%{keyword}%')";
            }
            sql += " ORDER BY TenSanPham ASC";

            DataTable tblFiltered = DbContext.GetDataToTable(sql);
            LoadProductsSelectionGrid(tblFiltered);
        }

        // 5.2.17. Chọn sản phẩm từ lưới danh mục bên trái
        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvProductsSelection.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (int.TryParse(rawId, out int id))
                {
                    string sql = $@"SELECT s.MaSanPham, s.TenSanPham, s.GiaNhap, s.MoTa, s.Anh, d.TenDanhMuc 
                                   FROM SanPham s 
                                   LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                                   WHERE s.MaSanPham = {id}";
                    DataTable tbl = DbContext.GetDataToTable(sql);

                    if (tbl.Rows.Count > 0)
                    {
                        DataRow r = tbl.Rows[0];
                        txtSelMaSP.Text = r["MaSanPham"].ToString();
                        txtSelTenSP.Text = r["TenSanPham"]?.ToString() ?? "";
                        txtSelGiaNhap.Text = r["GiaNhap"]?.ToString() ?? "0";

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

                        txtSelSoLuong.Focus();
                        txtSelSoLuong.SelectAll();
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
                        txtSelMaSP.Text = item.MaSanPham.ToString();
                        txtSelTenSP.Text = item.TenSanPham;
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

        // 5.2.20. Bấm nút THÊM sản phẩm vào giỏ hàng
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút THÊM hoặc SỬA phiếu nhập ở Tab 1 trước khi thêm sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                existing.SoLuong = qty;
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
            btnResetCartForm_Click(this, EventArgs.Empty);
        }

        // 5.2.21. Bấm nút XÓA sản phẩm khỏi giỏ hàng
        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút THÊM hoặc SỬA phiếu nhập ở Tab 1 trước khi sửa giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                btnResetCartForm_Click(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Sản phẩm này hiện tại chưa có trong danh sách phiếu nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 5.2.22. Reset ô nhập liệu chi tiết
        private void btnResetCartForm_Click(object sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelGiaNhap.Text = "";
        }

        // 5.2.23. Bấm nút tìm kiếm sản phẩm trên Tab 2
        private void btnStockInSearch_Click(object sender, EventArgs e)
        {
            FilterProducts();
        }

        // 5.2.24. Làm mới danh sách chọn sản phẩm Tab 2
        private void btnStockInRefresh_Click(object sender, EventArgs e)
        {
            txtProductSearch.Text = "";
            btnResetCartForm_Click(this, EventArgs.Empty);
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

        // 5.2.26. Bấm nút QUAY VỀ PHIẾU NHẬP
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabPhieuNhap;
        }
    }
}
