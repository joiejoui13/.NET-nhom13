using System;
using System.Data;
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Để sử dụng DbContext kết nối cơ sở dữ liệu

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucInventory : UserControl
    {
        public ucInventory()
        {
            InitializeComponent();
        }

        // 5.2.2. Viết thủ tục Form_Load của ucInventory
        private void ucInventory_Load(object sender, EventArgs e)
        {
            DbContext.Ketnoi();

            // Đăng ký sự kiện thay đổi dữ liệu để tự động tính toán số lượng tồn kho trước/sau
            cboSanPham.SelectedIndexChanged += cboSanPham_SelectedIndexChanged;
            txtSoLuongThayDoi.TextChanged += txtSoLuongThayDoi_TextChanged;

            // Nạp dữ liệu vào ComboBox Sản phẩm
            Load_cboSanPham();
            
            // Nạp dữ liệu lên lưới DataGridView
            Load_DataGridView();

            // Reset các giá trị trên giao diện nhập liệu
            ResetValues();

            // Khóa các trường mã tự sinh và các trường tự động tính toán
            txtMaLichSu.Enabled = false;
            txtSoLuongTruoc.Enabled = false;
            txtSoLuongSau.Enabled = false;

            ToggleInputs(false);

            // Trạng thái nút bấm ban đầu
            btnAdd.Enabled = true;          // Cho phép Thêm mới
            btnEdit.Enabled = false;        // Chưa chọn bản ghi nào thì không cho Sửa
            btnDelete.Enabled = false;      // Chưa chọn bản ghi nào thì không cho Xóa
            btnSave.Enabled = false;        // Chưa bắt đầu thao tác thì khóa Lưu
            btnCancel.Enabled = false;      // Chưa bắt đầu thao tác thì khóa Bỏ qua
        }

        // Tự động tải danh sách sản phẩm từ cơ sở dữ liệu lên ComboBox
        private void Load_cboSanPham()
        {
            string sql = "SELECT MaSanPham, TenSanPham FROM SanPham ORDER BY TenSanPham ASC";
            DataTable tblSP = DbContext.GetDataToTable(sql);
            
            cboSanPham.DataSource = tblSP;
            cboSanPham.ValueMember = "MaSanPham";
            cboSanPham.DisplayMember = "TenSanPham";
            cboSanPham.SelectedIndex = -1;
        }

        // 5.2.3. Viết thủ tục Load_DataGridView
        private void Load_DataGridView(DataTable customTable = null)
        {
            DataTable tblLS;
            if (customTable != null)
            {
                tblLS = customTable;
            }
            else
            {
                string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                               FROM LichSuNhapKho l 
                               LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                               ORDER BY l.Thoigian DESC";
                tblLS = DbContext.GetDataToTable(sql);
            }

            // Tắt tự động tạo cột trên DataGridView
            dgvLichSu.AutoGenerateColumns = false;
            dgvLichSu.DataSource = tblLS;

            // Gán DataPropertyName cho từng cột đã tạo sẵn trong Designer
            if (dgvLichSu.Columns.Contains("colMaLichSu")) dgvLichSu.Columns["colMaLichSu"].DataPropertyName = "MaLichSu";
            if (dgvLichSu.Columns.Contains("colMaSanPham")) dgvLichSu.Columns["colMaSanPham"].DataPropertyName = "MaSanPham";
            if (dgvLichSu.Columns.Contains("colTenSanPham")) dgvLichSu.Columns["colTenSanPham"].DataPropertyName = "TenSanPham";
            if (dgvLichSu.Columns.Contains("colSoLuongThayDoi")) dgvLichSu.Columns["colSoLuongThayDoi"].DataPropertyName = "ThayDoi";
            if (dgvLichSu.Columns.Contains("colSoLuongTruoc")) dgvLichSu.Columns["colSoLuongTruoc"].DataPropertyName = "SoLuongTruoc";
            if (dgvLichSu.Columns.Contains("colSoLuongSau")) dgvLichSu.Columns["colSoLuongSau"].DataPropertyName = "SoLuongSau";
            if (dgvLichSu.Columns.Contains("colLoai")) dgvLichSu.Columns["colLoai"].DataPropertyName = "LoaiGiaoDich";
            if (dgvLichSu.Columns.Contains("colMaThamChieu")) dgvLichSu.Columns["colMaThamChieu"].DataPropertyName = "MaThamChieu";
            if (dgvLichSu.Columns.Contains("colTrangThai")) dgvLichSu.Columns["colTrangThai"].DataPropertyName = "TrangThai";
            if (dgvLichSu.Columns.Contains("colNgay")) dgvLichSu.Columns["colNgay"].DataPropertyName = "Thoigian";

            // Định dạng hiển thị thời gian
            if (dgvLichSu.Columns.Contains("colNgay"))
            {
                dgvLichSu.Columns["colNgay"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            // Định dạng lề, WrapMode và kích thước các cột để không bị mất chữ giống ucPromotion
            dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dgvLichSu.Columns.Contains("colMaLichSu"))
            {
                dgvLichSu.Columns["colMaLichSu"].Width = 95;
                dgvLichSu.Columns["colMaLichSu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colMaSanPham"))
            {
                dgvLichSu.Columns["colMaSanPham"].Width = 70;
                dgvLichSu.Columns["colMaSanPham"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colTenSanPham"))
            {
                dgvLichSu.Columns["colTenSanPham"].MinimumWidth = 180;
                dgvLichSu.Columns["colTenSanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Tự động co giãn theo form
            }
            if (dgvLichSu.Columns.Contains("colSoLuongThayDoi"))
            {
                dgvLichSu.Columns["colSoLuongThayDoi"].Width = 85;
                dgvLichSu.Columns["colSoLuongThayDoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colSoLuongTruoc"))
            {
                dgvLichSu.Columns["colSoLuongTruoc"].Width = 80;
                dgvLichSu.Columns["colSoLuongTruoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colSoLuongSau"))
            {
                dgvLichSu.Columns["colSoLuongSau"].Width = 80;
                dgvLichSu.Columns["colSoLuongSau"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colLoai"))
            {
                dgvLichSu.Columns["colLoai"].Width = 100;
                dgvLichSu.Columns["colLoai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colMaThamChieu"))
            {
                dgvLichSu.Columns["colMaThamChieu"].Width = 100;
                dgvLichSu.Columns["colMaThamChieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colTrangThai"))
            {
                dgvLichSu.Columns["colTrangThai"].Width = 110;
                dgvLichSu.Columns["colTrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvLichSu.Columns.Contains("colNgay"))
            {
                dgvLichSu.Columns["colNgay"].Width = 130;
                dgvLichSu.Columns["colNgay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Đồng bộ giao diện cao cấp và chuyên nghiệp giống ucPromotion
            dgvLichSu.RowTemplate.Height = 40;
            dgvLichSu.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvLichSu.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvLichSu.ColumnHeadersHeight = 40; 
            dgvLichSu.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; // Tắt tự động xuống dòng ở Header gây mất chữ
            dgvLichSu.AllowUserToAddRows = false;
            dgvLichSu.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void ToggleInputs(bool isEnabled)
        {
            cboSanPham.Enabled = isEnabled;
            txtSoLuongThayDoi.Enabled = isEnabled;
            cboLoaiThayDoi.Enabled = isEnabled;
            txtMaThamChieu.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        // 5.2.4. Viết thủ tục ResetValues
        private void ResetValues()
        {
            txtMaLichSu.Text = "";
            txtMaThamChieu.Text = "";
            
            cboSanPham.SelectedIndex = -1;
            txtSoLuongThayDoi.Text = "";
            txtSoLuongTruoc.Text = "";
            txtSoLuongSau.Text = "";
            
            if (cboLoaiThayDoi.Items.Count > 0) cboLoaiThayDoi.SelectedIndex = -1;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = -1;
        }

        // Tự động tính toán số lượng tồn kho trước/sau dựa trên sản phẩm và thay đổi số lượng nhập/xuất
        private void UpdateComputedStock()
        {
            if (cboSanPham.SelectedValue != null && int.TryParse(cboSanPham.SelectedValue.ToString(), out int maSP))
            {
                string sql = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
                string currentStockStr = DbContext.GetFieldValues(sql);
                
                if (!string.IsNullOrEmpty(currentStockStr))
                {
                    txtSoLuongTruoc.Text = currentStockStr;
                    if (int.TryParse(txtSoLuongThayDoi.Text.Trim(), out int change))
                    {
                        txtSoLuongSau.Text = (int.Parse(currentStockStr) + change).ToString();
                    }
                    else
                    {
                        txtSoLuongSau.Text = currentStockStr;
                    }
                }
            }
            else
            {
                txtSoLuongTruoc.Text = "";
                txtSoLuongSau.Text = "";
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComputedStock();
        }

        private void txtSoLuongThayDoi_TextChanged(object sender, EventArgs e)
        {
            UpdateComputedStock();
        }

        // 5.2.5. Viết thủ tục DataGridView_Click
        private void dgvLichSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuongThayDoi.Focus();
                return;
            }
            if (dgvLichSu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvLichSu.CurrentRow == null) return;

            DataGridViewRow row = dgvLichSu.CurrentRow;

            txtMaLichSu.Text = row.Cells["colMaLichSu"].Value?.ToString() ?? "";
            txtMaThamChieu.Text = row.Cells["colMaThamChieu"].Value?.ToString() ?? "";
            
            // Xử lý nạp Sản phẩm vào ComboBox
            if (row.Cells["colMaSanPham"].Value != null)
            {
                cboSanPham.SelectedValue = row.Cells["colMaSanPham"].Value;
            }
            else
            {
                cboSanPham.SelectedIndex = -1;
            }

            // Xử lý lấy số lượng thay đổi làm sạch dấu '+'
            string rawChange = row.Cells["colSoLuongThayDoi"].Value?.ToString() ?? "";
            txtSoLuongThayDoi.Text = rawChange.Replace("+", "");

            txtSoLuongTruoc.Text = row.Cells["colSoLuongTruoc"].Value?.ToString() ?? "";
            txtSoLuongSau.Text = row.Cells["colSoLuongSau"].Value?.ToString() ?? "";
            cboLoaiThayDoi.Text = row.Cells["colLoai"].Value?.ToString() ?? "";
            cboTrangThai.Text = row.Cells["colTrangThai"].Value?.ToString() ?? "";

            ToggleInputs(true);

            // Mở nút Sửa, Xóa, Bỏ qua
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        // 5.2.6. Viết thủ tục btnAdd_Click (Nút Thêm mới)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues();

            btnSave.Enabled = true;       // Cho phép Lưu
            btnCancel.Enabled = true;     // Cho phép Hủy
            btnAdd.Enabled = false;       // Khóa Thêm
            btnEdit.Enabled = false;      // Khóa Sửa
            btnDelete.Enabled = false;    // Khóa Xóa

            txtMaLichSu.Enabled = false;
            ToggleInputs(true);
            cboSanPham.Focus();
        }

        // 5.2.7. Viết thủ tục btnSave_Click (Nút Lưu thay đổi)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (cboSanPham.SelectedValue == null || cboSanPham.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn sản phẩm điều chỉnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSanPham.Focus();
                return;
            }

            if (txtSoLuongThayDoi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập số lượng thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongThayDoi.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuongThayDoi.Text.Trim(), out int thayDoi) || thayDoi == 0)
            {
                MessageBox.Show("Số lượng thay đổi phải là số nguyên khác 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongThayDoi.Focus();
                return;
            }

            if (cboLoaiThayDoi.SelectedIndex == -1 || string.IsNullOrEmpty(cboLoaiThayDoi.Text))
            {
                MessageBox.Show("Bạn phải chọn loại giao dịch thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiThayDoi.Focus();
                return;
            }

            string refIdStr = txtMaThamChieu.Text.Trim();
            if (refIdStr.Length == 0) refIdStr = "0";
            if (!int.TryParse(refIdStr, out int refId))
            {
                MessageBox.Show("Mã tham chiếu phải là số nguyên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThamChieu.Focus();
                return;
            }

            string trangThai = cboTrangThai.Text;
            if (string.IsNullOrEmpty(trangThai)) trangThai = "Đang hoạt động";

            // Kiểm tra và lấy số lượng tồn hiện tại của sản phẩm
            int maSP = Convert.ToInt32(cboSanPham.SelectedValue);
            string sqlCheck = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
            string currentStockStr = DbContext.GetFieldValues(sqlCheck);
            int truoc = string.IsNullOrEmpty(currentStockStr) ? 0 : Convert.ToInt32(currentStockStr);
            int sau = truoc + thayDoi;

            if (sau < 0)
            {
                MessageBox.Show("Tồn kho sau khi điều chỉnh không thể nhỏ hơn 0!", "Lỗi tồn kho âm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongThayDoi.Focus();
                return;
            }

            // Bước 2: Tạo câu lệnh SQL INSERT lưu lịch sử
            string sqlInsert = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                 VALUES ({maSP}, GETDATE(), {thayDoi}, {truoc}, {sau}, N'{cboLoaiThayDoi.Text}', {refId}, N'{trangThai}')";
            DbContext.RunSql(sqlInsert);

            // Bước 3: Cập nhật trực tiếp số tồn mới của sản phẩm trong bảng SanPham
            string sqlUpdateStock = $"UPDATE SanPham SET SoLuongTon = {sau} WHERE MaSanPham = {maSP}";
            DbContext.RunSql(sqlUpdateStock);

            MessageBox.Show("Thêm bản ghi điều chỉnh và cập nhật tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Tải lại Grid và đưa các nút về trạng thái mặc định
            Load_DataGridView();
            ResetValues();

            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        // 5.2.8. Viết thủ tục btnEdit_Click (Nút Sửa bản ghi)
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLichSu.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaLichSu.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra đầu vào hợp lệ
            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Bạn phải chọn sản phẩm điều chỉnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuongThayDoi.Text.Trim(), out int newThayDoi) || newThayDoi == 0)
            {
                MessageBox.Show("Số lượng thay đổi phải là số nguyên khác 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string refIdStr = txtMaThamChieu.Text.Trim();
            if (refIdStr.Length == 0) refIdStr = "0";
            if (!int.TryParse(refIdStr, out int newRefId))
            {
                MessageBox.Show("Mã tham chiếu phải là số nguyên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int currentMaLichSu = Convert.ToInt32(txtMaLichSu.Text);
            int newMaSP = Convert.ToInt32(cboSanPham.SelectedValue);

            // Bước 1: Lấy thông tin bản ghi cũ trong cơ sở dữ liệu để thực hiện nghiệp vụ hoàn trả số lượng (Revert)
            string sqlGetOld = $"SELECT MaSanPham, ThayDoi, TrangThai FROM LichSuNhapKho WHERE MaLichSu = {currentMaLichSu}";
            DataTable tblOld = DbContext.GetDataToTable(sqlGetOld);
            if (tblOld.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bản ghi lịch sử cũ để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldTrangThai = tblOld.Rows[0]["TrangThai"]?.ToString() ?? "";
            if (oldTrangThai == "Đã khóa")
            {
                MessageBox.Show("Bản ghi lịch sử này đã bị khóa hệ thống, không thể chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oldMaSP = Convert.ToInt32(tblOld.Rows[0]["MaSanPham"]);
            int oldThayDoi = Convert.ToInt32(tblOld.Rows[0]["ThayDoi"]);

            // Bước 2: Tính toán tồn kho giả lập để chống âm tồn kho
            // 2.1 Lấy tồn kho thực tế hiện tại của sản phẩm cũ
            string sqlStockOld = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {oldMaSP}";
            int currentStockOld = Convert.ToInt32(DbContext.GetFieldValues(sqlStockOld));
            int stockOldReverted = currentStockOld - oldThayDoi;

            if (stockOldReverted < 0)
            {
                MessageBox.Show("Không thể sửa vì việc thu hồi thay đổi của sản phẩm cũ khiến tồn kho bị âm!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2.2 Lấy tồn kho của sản phẩm mới
            int currentStockNew;
            if (newMaSP == oldMaSP)
            {
                currentStockNew = stockOldReverted; // Nếu cùng sản phẩm, tồn kho ban đầu chính là tồn đã hoàn trả
            }
            else
            {
                string sqlStockNew = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {newMaSP}";
                currentStockNew = Convert.ToInt32(DbContext.GetFieldValues(sqlStockNew));
            }

            int finalStockNew = currentStockNew + newThayDoi;
            if (finalStockNew < 0)
            {
                MessageBox.Show("Không thể sửa vì số lượng thay đổi mới làm tồn kho sản phẩm mới bị âm!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bước 3: Áp dụng nghiệp vụ hoàn trả & lưu số lượng mới vào DB
            // 3.1 Hoàn trả số lượng cũ của sản phẩm cũ
            string sqlRevertOld = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {oldThayDoi} WHERE MaSanPham = {oldMaSP}";
            DbContext.RunSql(sqlRevertOld);

            // 3.2 Lấy tồn kho thực tế mới nhất (sau khi hoàn trả) để tính số lượng trước/sau chính xác
            string sqlLatestStock = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {newMaSP}";
            int latestTruoc = Convert.ToInt32(DbContext.GetFieldValues(sqlLatestStock));
            int latestSau = latestTruoc + newThayDoi;

            // 3.3 Cộng số lượng thay đổi mới cho sản phẩm mới
            string sqlApplyNew = $"UPDATE SanPham SET SoLuongTon = SoLuongTon + {newThayDoi} WHERE MaSanPham = {newMaSP}";
            DbContext.RunSql(sqlApplyNew);

            // 3.4 Cập nhật thông tin chi tiết điều chỉnh kho
            string sqlUpdateLog = $@"UPDATE LichSuNhapKho SET 
                                    MaSanPham = {newMaSP}, 
                                    ThayDoi = {newThayDoi}, 
                                    SoLuongTruoc = {latestTruoc}, 
                                    SoLuongSau = {latestSau}, 
                                    LoaiGiaoDich = N'{cboLoaiThayDoi.Text}', 
                                    MaThamChieu = {newRefId}, 
                                    TrangThai = N'{cboTrangThai.Text}' 
                                    WHERE MaLichSu = {currentMaLichSu}";
            DbContext.RunSql(sqlUpdateLog);

            MessageBox.Show("Cập nhật bản ghi điều chỉnh và đồng bộ tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView();
            ResetValues();

            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        // 5.2.9. Viết thủ tục btnDelete_Click (Nút Xóa bản ghi)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLichSu.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaLichSu.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int currentMaLichSu = Convert.ToInt32(txtMaLichSu.Text);

            // Lấy thông tin bản ghi cần xóa để hoàn trả lại số lượng tồn kho
            string sqlGetOld = $"SELECT MaSanPham, ThayDoi, TrangThai FROM LichSuNhapKho WHERE MaLichSu = {currentMaLichSu}";
            DataTable tblOld = DbContext.GetDataToTable(sqlGetOld);
            if (tblOld.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bản ghi cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldTrangThai = tblOld.Rows[0]["TrangThai"]?.ToString() ?? "";
            if (oldTrangThai == "Đã khóa")
            {
                MessageBox.Show("Bản ghi lịch sử kho này đã bị khóa hệ thống, không thể xóa bỏ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oldMaSP = Convert.ToInt32(tblOld.Rows[0]["MaSanPham"]);
            int oldThayDoi = Convert.ToInt32(tblOld.Rows[0]["ThayDoi"]);

            // Kiểm tra xem nếu thu hồi (revert) thay đổi này có làm tồn kho bị âm không
            string sqlStock = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {oldMaSP}";
            int currentStock = Convert.ToInt32(DbContext.GetFieldValues(sqlStock));
            int stockReverted = currentStock - oldThayDoi;

            if (stockReverted < 0)
            {
                MessageBox.Show("Không thể xóa bản ghi vì việc thu hồi thay đổi của nó làm tồn kho sản phẩm bị âm!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này? Số lượng tồn kho của sản phẩm sẽ được tự động hoàn trả.", "Xác nhận xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Bước 1: Thu hồi số lượng thay đổi trong bảng SanPham
                string sqlRevertStock = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {oldThayDoi} WHERE MaSanPham = {oldMaSP}";
                DbContext.RunSql(sqlRevertStock);

                // Bước 2: Xóa bản ghi lịch sử kho
                string sqlDelete = $"DELETE FROM LichSuNhapKho WHERE MaLichSu = {currentMaLichSu}";
                DbContext.RunSqlDel(sqlDelete);

                MessageBox.Show("Xóa bản ghi điều chỉnh kho và hoàn trả tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Load_DataGridView();
                ResetValues();

                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        // 5.2.10. Viết thủ tục btnCancel_Click (Nút Hủy / Bỏ qua)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetValues();
            
            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            
            txtMaLichSu.Enabled = false;
        }

        // 5.2.11. Viết thủ tục btnSearch_Click (Nút Tìm kiếm)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Bật trường nhập Mã lịch sử để người dùng có thể gõ tìm
            txtMaLichSu.Enabled = true;
            ToggleInputs(true);

            // Kiểm tra các điều kiện tìm kiếm xem có trống không
            if (txtMaLichSu.Text.Trim() == "" && txtMaThamChieu.Text.Trim() == "" && cboSanPham.SelectedIndex == -1 && cboLoaiThayDoi.SelectedIndex == -1 && cboTrangThai.Text == "")
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!! (Ví dụ: Mã LS, Mã tham chiếu, Sản phẩm, Loại, hoặc Trạng thái)", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bắt đầu ghép câu lệnh SQL tìm kiếm lịch sử
            string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                           FROM LichSuNhapKho l 
                           LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                           WHERE 1=1";

            // Nếu nhập Mã lịch sử
            if (txtMaLichSu.Text.Trim() != "")
            {
                sql += $" AND l.MaLichSu = {txtMaLichSu.Text.Trim()}";
            }

            // Nếu nhập Mã tham chiếu
            if (txtMaThamChieu.Text.Trim() != "")
            {
                sql += $" AND l.MaThamChieu = {txtMaThamChieu.Text.Trim()}";
            }

            // Nếu chọn Sản phẩm
            if (cboSanPham.SelectedValue != null && cboSanPham.SelectedIndex != -1)
            {
                sql += $" AND l.MaSanPham = {cboSanPham.SelectedValue}";
            }

            // Nếu chọn Loại giao dịch
            if (cboLoaiThayDoi.SelectedIndex != -1 && !string.IsNullOrEmpty(cboLoaiThayDoi.Text))
            {
                sql += $" AND l.LoaiGiaoDich = N'{cboLoaiThayDoi.Text}'";
            }

            // Nếu chọn Trạng thái
            if (cboTrangThai.SelectedIndex != -1 && !string.IsNullOrEmpty(cboTrangThai.Text))
            {
                sql += $" AND l.TrangThai = N'{cboTrangThai.Text}'";
            }

            sql += " ORDER BY l.Thoigian DESC";

            DataTable tblSearch = DbContext.GetDataToTable(sql);
            if (tblSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Có {tblSearch.Rows.Count} bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Load_DataGridView(tblSearch);
            ResetValues();

            btnCancel.Enabled = true; // Mở nút Bỏ qua để có thể quay về bảng gốc
        }

        // 5.2.12. Viết thủ tục btnRefresh_Click (Nút Làm mới)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Tải lại toàn bộ lưới dữ liệu gốc
            Load_DataGridView();
            ResetValues();

            ToggleInputs(false);

            // Khôi phục trạng thái nút bấm
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            
            txtMaLichSu.Enabled = false;
        }
    }
}