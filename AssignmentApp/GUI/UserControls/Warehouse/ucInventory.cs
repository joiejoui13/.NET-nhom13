using System;
using System.Data; // Thư viện để thao tác với bảng dữ liệu
using System.Windows.Forms;
using AssignmentApp.DAL.Core; // Thư viện tương tác CSDL

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucInventory : UserControl
    {
        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo UserControl Quản lý Tồn kho.
        /// Thực thi đầu tiên để vẽ giao diện và cấu hình các danh sách lựa chọn tĩnh.
        /// </summary>
        public ucInventory()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Gắn danh sách thả xuống cứng (Tách từ Designer sang đây để dễ quản lý)
            cboLoaiThayDoi.Items.AddRange(new object[] { "Nhập kho", "Xuất kho bán", "Xuất hủy" });
            cboTrangThai.Items.AddRange(new object[] { "Đang hoạt động", "Đã khóa" });
        }

        /// <summary>
        /// Sự kiện Load Form: Chạy một lần duy nhất khi màn hình Lịch sử kho hiển thị.
        /// </summary>
        private void ucInventory_Load(object sender, EventArgs e)
        {
            DbContext.Ketnoi();

            // 1. Đăng ký sự kiện thay đổi dữ liệu để TỰ ĐỘNG TÍNH TOÁN tồn kho
            cboSanPham.SelectedIndexChanged += cboSanPham_SelectedIndexChanged;
            txtSoLuongThayDoi.TextChanged += txtSoLuongThayDoi_TextChanged;

            // 2. Tải danh sách Sản phẩm động từ CSDL lên ComboBox
            Load_cboSanPham();
            
            // 3. Tải lịch sử nhập xuất lên DataGridView
            Load_DataGridView(null);

            // 4. Xóa trắng các ô nhập liệu và thiết lập trạng thái nghỉ (Read-only)
            ResetValues();

            txtMaLichSu.Enabled = false;   // Cột tự tăng Identity
            txtSoLuongTruoc.Enabled = false; // Tự động tính, không cho tự nhập tay
            txtSoLuongSau.Enabled = false;   // Tự động tính, không cho tự nhập tay

            ToggleInputs(false);

            // 5. Cấu hình trạng thái nút bấm ban đầu
            btnAdd.Enabled = true;          
            btnEdit.Enabled = false;        
            btnDelete.Enabled = false;      
            btnSave.Enabled = false;        
            btnCancel.Enabled = false;      
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Lấy toàn bộ sản phẩm đang có trong CSDL để đổ vào ComboBox cho người dùng chọn.
        /// </summary>
        private void Load_cboSanPham()
        {
            string sql = "SELECT MaSanPham, TenSanPham FROM SanPham ORDER BY TenSanPham ASC";
            DataTable tblSP = DbContext.GetDataToTable(sql);
            
            cboSanPham.DataSource = tblSP;
            cboSanPham.ValueMember = "MaSanPham";   // Mã chìm ẩn phía dưới
            cboSanPham.DisplayMember = "TenSanPham"; // Tên hiển thị lên mặt ComboBox
            cboSanPham.SelectedIndex = -1; // Mặc định không chọn cái nào
        }

        /// <summary>
        /// Kéo dữ liệu Lịch sử Kho từ CSDL và đổ vào lưới hiển thị (DataGridView).
        /// Hỗ trợ cả 2 chế độ: Nạp toàn bộ hoặc Nạp dữ liệu tìm kiếm (customTable).
        /// </summary>
        private void Load_DataGridView(DataTable customTable)
        {
            DataTable tblLS;
            if (customTable != null)
            {
                tblLS = customTable; // Dùng dữ liệu được truyền vào (khi lọc/tìm kiếm)
            }
            else
            {
                // Lấy tất cả dữ liệu lịch sử kèm theo Tên sản phẩm từ bảng SanPham
                string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                               FROM LichSuNhapKho l 
                               LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                               ORDER BY l.Thoigian DESC";
                tblLS = DbContext.GetDataToTable(sql);
            }

            // BINDING DỮ LIỆU: Ánh xạ dữ liệu vào đúng vị trí cột trên giao diện
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

            // Tắt chế độ lưới tự sinh thêm cột
            dgvLichSu.AutoGenerateColumns = false;
            dgvLichSu.DataSource = tblLS;

            // ĐỊNH DẠNG HIỂN THỊ
            if (dgvLichSu.Columns.Contains("colNgay"))
            {
                dgvLichSu.Columns["colNgay"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            // ĐỊNH DẠNG LỀ VÀ KÍCH THƯỚC CỘT (Tắt AutoSize toàn cục để tránh cột ép nhau)
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
                dgvLichSu.Columns["colTenSanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Tên SP giãn lấp đầy bảng
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

            // Giao diện Row & Header
            dgvLichSu.RowTemplate.Height = 40;
            dgvLichSu.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvLichSu.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvLichSu.ColumnHeadersHeight = 40; 
            dgvLichSu.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; 
            dgvLichSu.AllowUserToAddRows = false;
            dgvLichSu.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        /// <summary>
        /// Mở hoặc khóa các ô TextBox và ComboBox.
        /// </summary>
        private void ToggleInputs(bool isEnabled)
        {
            cboSanPham.Enabled = isEnabled;
            txtSoLuongThayDoi.Enabled = isEnabled;
            cboLoaiThayDoi.Enabled = isEnabled;
            txtMaThamChieu.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        /// <summary>
        /// Đưa toàn bộ ô nhập liệu về rỗng hoặc trạng thái trống.
        /// </summary>
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

        /// <summary>
        /// HÀM TÍNH TOÁN LOGIC NGHIỆP VỤ (Vô cùng quan trọng).
        /// Lấy Số lượng tồn thực tế của Sản Phẩm từ DB, sau đó cộng/trừ ảo để hiển thị cho người dùng thấy trước Số lượng trước/sau.
        /// </summary>
        private void UpdateComputedStock()
        {
            // Kiểm tra xem đã chọn Sản phẩm nào chưa
            if (cboSanPham.SelectedValue != null)
            {
                // Ép kiểu an toàn (int.TryParse) lấy mã SP
                int maSP = 0;
                if (int.TryParse(cboSanPham.SelectedValue.ToString(), out maSP) == true)
                {
                    // Truy vấn lấy tồn kho hiện tại
                    string sql = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
                    string currentStockStr = DbContext.GetFieldValues(sql);
                    
                    if (string.IsNullOrEmpty(currentStockStr) == false)
                    {
                        // 1. Gán số lượng trước
                        txtSoLuongTruoc.Text = currentStockStr;
                        
                        // 2. Tính số lượng sau
                        int change = 0;
                        if (int.TryParse(txtSoLuongThayDoi.Text.Trim(), out change) == true)
                        {
                            int currentStock = int.Parse(currentStockStr);
                            int computedAfter = currentStock + change;
                            txtSoLuongSau.Text = computedAfter.ToString();
                        }
                        else
                        {
                            txtSoLuongSau.Text = currentStockStr; // Không nhập số thay đổi thì trước sau như một
                        }
                    }
                }
            }
            else
            {
                txtSoLuongTruoc.Text = "";
                txtSoLuongSau.Text = "";
            }
        }

        /// <summary>
        /// Hàm tập trung kiểm duyệt thông tin nhập liệu đầu vào. Giúp tối giản code cho nút Lưu/Sửa.
        /// </summary>
        private bool ValidateInventoryInputs(out int thayDoi, out int refId)
        {
            thayDoi = 0;
            refId = 0;

            if (cboSanPham.SelectedValue == null || cboSanPham.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn sản phẩm điều chỉnh!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSanPham.Focus();
                return false;
            }

            if (txtSoLuongThayDoi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập số lượng thay đổi!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongThayDoi.Focus();
                return false;
            }

            if (int.TryParse(txtSoLuongThayDoi.Text.Trim(), out thayDoi) == false || thayDoi == 0)
            {
                MessageBox.Show("Số lượng thay đổi phải là một số nguyên khác 0!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongThayDoi.Focus();
                return false;
            }

            if (cboLoaiThayDoi.SelectedIndex == -1 || string.IsNullOrEmpty(cboLoaiThayDoi.Text) == true)
            {
                MessageBox.Show("Bạn phải chọn loại giao dịch thay đổi!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiThayDoi.Focus();
                return false;
            }

            string refIdStr = txtMaThamChieu.Text.Trim();
            if (refIdStr.Length == 0) refIdStr = "0";
            if (int.TryParse(refIdStr, out refId) == false)
            {
                MessageBox.Show("Mã tham chiếu bắt buộc phải là số nguyên!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThamChieu.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Sự kiện kích hoạt khi người dùng vừa đổi lựa chọn sản phẩm trong ComboBox.
        /// Gây ra việc gọi hàm UpdateComputedStock để tính lại tồn kho trước/sau.
        /// </summary>
        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComputedStock();
        }

        /// <summary>
        /// Sự kiện kích hoạt mỗi khi gõ phím vào ô Số lượng thay đổi.
        /// Tính toán Real-time (thời gian thực) cho người dùng thấy tồn kho sẽ ra sao.
        /// </summary>
        private void txtSoLuongThayDoi_TextChanged(object sender, EventArgs e)
        {
            UpdateComputedStock();
        }

        /// <summary>
        /// Sự kiện nhấn chuột vào DataGridView.
        /// Đồng bộ ngược dữ liệu từ dòng được chọn lên trên các TextBox.
        /// </summary>
        private void dgvLichSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra click hợp lệ vào dòng dữ liệu
            if (e.RowIndex >= 0)
            {
                // Thoát chế độ tìm kiếm nếu đang bật
                if (txtMaLichSu.Enabled == true)
                {
                    txtMaLichSu.Enabled = false;
                    btnAdd.Enabled = true;
                }

                DataGridViewRow row = dgvLichSu.Rows[e.RowIndex];

                // Gán dữ liệu dùng cấu trúc if-else rất tường minh, không dùng "??"
                if (row.Cells["colMaLichSu"].Value != null)
                {
                    txtMaLichSu.Text = row.Cells["colMaLichSu"].Value.ToString();
                }

                if (row.Cells["colMaThamChieu"].Value != null)
                {
                    txtMaThamChieu.Text = row.Cells["colMaThamChieu"].Value.ToString();
                }
                
                // Đồng bộ sản phẩm vào ComboBox
                if (row.Cells["colMaSanPham"].Value != null)
                {
                    cboSanPham.SelectedValue = row.Cells["colMaSanPham"].Value;
                }
                
                if (row.Cells["colSoLuongThayDoi"].Value != null)
                {
                    txtSoLuongThayDoi.Text = row.Cells["colSoLuongThayDoi"].Value.ToString();
                }

                if (row.Cells["colSoLuongTruoc"].Value != null)
                {
                    txtSoLuongTruoc.Text = row.Cells["colSoLuongTruoc"].Value.ToString();
                }

                if (row.Cells["colSoLuongSau"].Value != null)
                {
                    txtSoLuongSau.Text = row.Cells["colSoLuongSau"].Value.ToString();
                }

                if (row.Cells["colLoai"].Value != null)
                {
                    cboLoaiThayDoi.Text = row.Cells["colLoai"].Value.ToString();
                }

                if (row.Cells["colTrangThai"].Value != null)
                {
                    cboTrangThai.Text = row.Cells["colTrangThai"].Value.ToString();
                }

                // Mở giao diện cho phép thao tác Sửa/Xóa
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
        /// Nút Thêm mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetValues();
            
            txtMaLichSu.Enabled = false;
            txtMaLichSu.Text = "Tự động sinh";
            cboTrangThai.Text = "Đang hoạt động";
            
            ToggleInputs(true);
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            cboSanPham.Focus();
        }

        /// <summary>
        /// Nút Lưu (INSERT): Thực hiện lưu Lịch sử Điều chỉnh Kho mới, VÀ đồng thời cập nhật Số lượng Tồn vào bảng Sản Phẩm.
        /// Nghiệp vụ rất khắt khe: Không cho phép tồn kho âm.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm duyệt đầu vào chung
            int thayDoi = 0;
            int refId = 0;
            if (ValidateInventoryInputs(out thayDoi, out refId) == false)
            {
                return; // Có lỗi thì ngừng lưu
            }

            // Gán trạng thái mặc định nếu người dùng để trống
            string trangThai = cboTrangThai.Text;
            if (string.IsNullOrEmpty(trangThai) == true) 
            {
                trangThai = "Đang hoạt động";
            }

            // 2. NGHIỆP VỤ TỒN KHO: Lấy tồn hiện tại trong CSDL và tính toán xem Tồn Sau Cùng có bị âm không?
            int maSP = Convert.ToInt32(cboSanPham.SelectedValue);
            string sqlCheck = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
            string currentStockStr = DbContext.GetFieldValues(sqlCheck);
            
            int truoc = 0;
            if (string.IsNullOrEmpty(currentStockStr) == false)
            {
                truoc = Convert.ToInt32(currentStockStr);
            }
            int sau = truoc + thayDoi;

            // NẾU TỒN KHO BỊ ÂM -> Cấm không cho lưu
            if (sau < 0)
            {
                MessageBox.Show("Tồn kho sau khi điều chỉnh không thể nhỏ hơn 0! Vui lòng xem lại số lượng thay đổi.", "Lỗi tồn kho âm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongThayDoi.Focus();
                return;
            }

            // 3. TẠO LỊCH SỬ KHO (INSERT)
            string sqlInsert = $@"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                  VALUES ({maSP}, GETDATE(), {thayDoi}, {truoc}, {sau}, N'{cboLoaiThayDoi.Text}', {refId}, N'{trangThai}')";
            DbContext.RunSql(sqlInsert);

            // 4. CẬP NHẬT TỒN KHO THỰC TẾ VÀO BẢNG SẢN PHẨM (UPDATE)
            string sqlUpdateStock = $"UPDATE SanPham SET SoLuongTon = {sau} WHERE MaSanPham = {maSP}";
            DbContext.RunSql(sqlUpdateStock);

            MessageBox.Show("Thêm bản ghi điều chỉnh và cập nhật tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 5. Kết thúc quy trình, tải lại form
            Load_DataGridView(null);
            ResetValues();
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaLichSu.Enabled = false;
        }

        /// <summary>
        /// Nút Sửa (UPDATE): Cực kỳ phức tạp.
        /// Quy trình: Rút lại (Revert) thay đổi cũ -> Tính số lượng mới -> Áp dụng lại (Apply).
        /// Cần phải ngăn ngừa trường hợp Revert làm Tồn kho âm, hoặc Apply mới làm Tồn kho âm.
        /// </summary>
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

            // 1. Kiểm duyệt đầu vào chung
            int newThayDoi = 0;
            int newRefId = 0;
            if (ValidateInventoryInputs(out newThayDoi, out newRefId) == false)
            {
                return; 
            }

            int currentMaLichSu = Convert.ToInt32(txtMaLichSu.Text);
            int newMaSP = Convert.ToInt32(cboSanPham.SelectedValue);

            // 2. LẤY LỊCH SỬ CŨ TỪ CSDL ĐỂ REVERT
            string sqlGetOld = $"SELECT MaSanPham, ThayDoi, TrangThai FROM LichSuNhapKho WHERE MaLichSu = {currentMaLichSu}";
            DataTable tblOld = DbContext.GetDataToTable(sqlGetOld);
            if (tblOld.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bản ghi lịch sử cũ để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Không cho phép sửa nếu bản ghi đã bị Đã Khóa / Đã hủy
            string oldTrangThai = tblOld.Rows[0]["TrangThai"].ToString();
            if (oldTrangThai == "Đã khóa" || oldTrangThai == "Đã hủy")
            {
                MessageBox.Show("Bản ghi lịch sử này đã bị chốt (khóa hoặc hủy), không thể chỉnh sửa!", "Bảo vệ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oldMaSP = Convert.ToInt32(tblOld.Rows[0]["MaSanPham"]);
            int oldThayDoi = Convert.ToInt32(tblOld.Rows[0]["ThayDoi"]);

            // 3. TÍNH TOÁN GIẢ LẬP ĐỂ CHỐNG TỒN KHO ÂM (QUAN TRỌNG)
            // Lấy tồn hiện tại của sản phẩm cũ
            string sqlStockOld = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {oldMaSP}";
            int currentStockOld = Convert.ToInt32(DbContext.GetFieldValues(sqlStockOld));
            
            // Số lượng tồn sau khi RÚT LẠI (Revert) thay đổi cũ
            int stockOldReverted = currentStockOld - oldThayDoi;

            if (stockOldReverted < 0)
            {
                MessageBox.Show("Không thể sửa! Nếu hủy lệnh cũ thì sản phẩm bị âm tồn kho (có thể đã xuất bán).", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Số lượng tồn hiện tại của sản phẩm mới (Nếu sửa khác mặt hàng)
            int currentStockNew = 0;
            if (newMaSP == oldMaSP)
            {
                currentStockNew = stockOldReverted; // Nếu cùng 1 sản phẩm thì kế thừa luôn
            }
            else
            {
                string sqlStockNew = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {newMaSP}";
                currentStockNew = Convert.ToInt32(DbContext.GetFieldValues(sqlStockNew));
            }

            // Tính tồn mới sau cùng
            int finalStockNew = currentStockNew + newThayDoi;
            if (finalStockNew < 0)
            {
                MessageBox.Show("Không thể sửa vì thay đổi mới làm tồn kho của sản phẩm bị âm!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. ÁP DỤNG NGHIỆP VỤ VÀO DB (KHI ĐÃ VƯỢT QUA CÁC BÀI KIỂM TRA CHỐNG ÂM KHO)
            
            // Bước 4.1: Revert cũ
            string sqlRevertOld = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {oldThayDoi} WHERE MaSanPham = {oldMaSP}";
            DbContext.RunSql(sqlRevertOld);

            // Bước 4.2: Tính lại thông tin trước/sau một lần nữa cho chính xác chuẩn nhất
            string sqlLatestStock = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {newMaSP}";
            int latestTruoc = Convert.ToInt32(DbContext.GetFieldValues(sqlLatestStock));
            int latestSau = latestTruoc + newThayDoi;

            // Bước 4.3: Apply mới
            string sqlApplyNew = $"UPDATE SanPham SET SoLuongTon = SoLuongTon + {newThayDoi} WHERE MaSanPham = {newMaSP}";
            DbContext.RunSql(sqlApplyNew);

            // Bước 4.4: Update lịch sử
            string sqlUpdateHistory = $@"UPDATE LichSuNhapKho SET 
                                    MaSanPham = {newMaSP}, 
                                    ThayDoi = {newThayDoi}, 
                                    SoLuongTruoc = {latestTruoc}, 
                                    SoLuongSau = {latestSau}, 
                                    LoaiGiaoDich = N'{cboLoaiThayDoi.Text}', 
                                    MaThamChieu = {newRefId}, 
                                    TrangThai = N'{cboTrangThai.Text}' 
                                    WHERE MaLichSu = {currentMaLichSu}";
            DbContext.RunSql(sqlUpdateHistory);

            MessageBox.Show("Cập nhật bản ghi điều chỉnh và tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView(null);
            ResetValues();
            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtMaLichSu.Enabled = false;
        }

        /// <summary>
        /// Nút Xóa (DELETE): Cập nhật trạng thái 'Đã hủy' (Xóa mềm).
        /// Đồng thời sẽ TỰ ĐỘNG THU HỒI lại Số lượng vào bảng Sản Phẩm.
        /// </summary>
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

            string oldTrangThai = tblOld.Rows[0]["TrangThai"].ToString();
            if (oldTrangThai == "Đã khóa" || oldTrangThai == "Đã hủy")
            {
                MessageBox.Show("Bản ghi này đã bị khóa hệ thống hoặc bị hủy từ trước, không thể tác động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oldMaSP = Convert.ToInt32(tblOld.Rows[0]["MaSanPham"]);
            int oldThayDoi = Convert.ToInt32(tblOld.Rows[0]["ThayDoi"]);

            // Tính tồn kho giả lập sau khi thu hồi để tránh làm Tồn Kho bị âm
            string sqlStock = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {oldMaSP}";
            int currentStock = Convert.ToInt32(DbContext.GetFieldValues(sqlStock));
            int stockReverted = currentStock - oldThayDoi;

            if (stockReverted < 0)
            {
                MessageBox.Show("Không thể xóa bản ghi vì việc thu hồi sẽ làm Tồn kho của sản phẩm rớt xuống dưới 0 (Âm kho)!", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hỏi xác nhận lần cuối
            if (MessageBox.Show("Bạn có chắc chắn muốn hủy bản ghi này? Số lượng tồn kho của sản phẩm sẽ được HỆ THỐNG TỰ ĐỘNG THU HỒI.", "Xác nhận hủy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Thu hồi trong bảng SanPham
                string sqlRevertStock = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {oldThayDoi} WHERE MaSanPham = {oldMaSP}";
                DbContext.RunSql(sqlRevertStock);

                // Xóa mềm Lịch sử
                string sqlDelete = $"UPDATE LichSuNhapKho SET TrangThai = N'Đã hủy' WHERE MaLichSu = {currentMaLichSu}";
                DbContext.RunSql(sqlDelete);

                MessageBox.Show("Hủy bản ghi điều chỉnh kho và hoàn trả tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Load_DataGridView(null);
                ResetValues();
                ToggleInputs(false);

                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        /// <summary>
        /// Nút Hủy thao tác nhập liệu.
        /// </summary>
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

        /// <summary>
        /// Nút Tìm Kiếm (Hoạt động 2 pha).
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // PHASE 1: Bật các ô cho phép nhập điều kiện tìm kiếm
            if (txtMaLichSu.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaLichSu.Enabled = true; // Cho phép tìm theo ID Lịch Sử

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật!\nVui lòng nhập thông tin cần lọc vào các ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaLichSu.Focus();
                return;
            }

            // PHASE 2: Thực thi truy vấn
            string idTerm = txtMaLichSu.Text.Trim();
            string refTerm = txtMaThamChieu.Text.Trim();
            
            string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                           FROM LichSuNhapKho l 
                           LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                           WHERE 1=1"; // Công thức 'WHERE 1=1' giúp việc nối chuỗi 'AND ...' phía sau cực kì dễ dàng

            if (string.IsNullOrEmpty(idTerm) == false)
                sql += $" AND l.MaLichSu = {idTerm}";

            if (string.IsNullOrEmpty(refTerm) == false)
                sql += $" AND l.MaThamChieu = {refTerm}";

            if (cboSanPham.SelectedValue != null && cboSanPham.SelectedIndex != -1)
                sql += $" AND l.MaSanPham = {cboSanPham.SelectedValue}";

            if (cboLoaiThayDoi.SelectedIndex != -1 && string.IsNullOrEmpty(cboLoaiThayDoi.Text) == false)
                sql += $" AND l.LoaiGiaoDich = N'{cboLoaiThayDoi.Text}'";

            if (cboTrangThai.SelectedIndex != -1 && string.IsNullOrEmpty(cboTrangThai.Text) == false)
                sql += $" AND l.TrangThai = N'{cboTrangThai.Text}'";

            sql += " ORDER BY l.Thoigian DESC";

            DataTable tblSearch = DbContext.GetDataToTable(sql);

            if (tblSearch.Rows.Count > 0)
            {
                ResetValues();
                MessageBox.Show($"Tìm thấy {tblSearch.Rows.Count} bản ghi thỏa mãn yêu cầu!!!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResetValues();
                MessageBox.Show("Không tìm thấy dữ liệu nào khớp với thông tin đã nhập!", "Không có kết quả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Gửi dữ liệu tìm được vào DataGridView (Sử dụng tham số customTable)
            Load_DataGridView(tblSearch);
        }

        /// <summary>
        /// Nút Làm mới (Tải lại).
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Tải lại dữ liệu rỗng (Không truyền tham số customTable thì nó tự load SQL gốc)
            Load_DataGridView(null);
            ResetValues();
            ToggleInputs(false);

            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            
            txtMaLichSu.Enabled = false;
        }

        #endregion

        #region 5. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        // Nếu bạn lỡ click nhầm ở Designer sinh ra hàm này, không sao cả, để nguyên đây không xóa để chống lỗi Designer.
        #endregion
    }
}