using System;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using AssignmentApp.BLL.Services.Warehouse;
using AssignmentApp.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    /// <summary>
    /// Giao diện người dùng (Tầng GUI - Presentation Layer).
    /// Chịu trách nhiệm hiển thị dữ liệu và tiếp nhận thao tác từ người dùng.
    /// Thiết kế chuẩn 3-Tier: Hoàn toàn không chứa câu lệnh SQL. Mọi thao tác xử lý nghiệp vụ đều gọi thông qua các Service (BLL) bằng Dependency Injection.
    /// Ứng dụng triệt để cơ chế xử lý bất đồng bộ (async/await) để tránh làm đơ (freeze) giao diện khi tải dữ liệu.
    /// </summary>
    public partial class ucInventory : UserControl
    {
        private readonly IInventoryService _inventoryService;

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        public ucInventory()
        {
            InitializeComponent();
            
            // Dependency Injection
            _inventoryService = Program.ServiceProvider.GetRequiredService<IInventoryService>();

            // CẤU HÌNH COMBOBOX
            cboLoaiThayDoi.Items.AddRange(new object[] { "Nhập kho", "Xuất kho bán", "Xuất hủy" });
            cboTrangThai.Items.AddRange(new object[] { "Đang hoạt động", "Đã khóa" });
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Kích hoạt khi giao diện vừa được tải lên. Khởi tạo cấu hình và gọi BLL để lấy dữ liệu đổ vào Grid.
        /// </summary>
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-') { e.Handled = true; } }

        private async void ucInventory_Load(object sender, EventArgs e)
        {
            cboSanPham.SelectedIndexChanged += cboSanPham_SelectedIndexChanged;
            if (txtSoLuongThayDoi != null) txtSoLuongThayDoi.KeyPress += NumericOnly_KeyPress; if (txtMaThamChieu != null) txtMaThamChieu.KeyPress += NumericOnly_KeyPress;
            txtSoLuongThayDoi.TextChanged += txtSoLuongThayDoi_TextChanged;

            await Load_cboSanPhamAsync();
            await Load_DataGridViewAsync();

            ResetValues();

            txtMaLichSu.Enabled = false;   
            txtSoLuongTruoc.Enabled = false; 
            txtSoLuongSau.Enabled = false;   

            ToggleInputs(false);

            btnAdd.Enabled = true;          
            btnEdit.Enabled = false;        
            btnDelete.Enabled = false;      
            btnSave.Enabled = false;        
            btnCancel.Enabled = false;      
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        private async Task Load_cboSanPhamAsync()
        {
            DataTable tblSP = await _inventoryService.GetProductsForComboBoxAsync();
            
            // Tạm thời gỡ event để tránh kích hoạt tính toán khi đang load
            cboSanPham.SelectedIndexChanged -= cboSanPham_SelectedIndexChanged;

            cboSanPham.DataSource = tblSP;
            cboSanPham.ValueMember = "MaSanPham";   
            cboSanPham.DisplayMember = "TenSanPham"; 
            cboSanPham.SelectedIndex = -1; 

            cboSanPham.SelectedIndexChanged += cboSanPham_SelectedIndexChanged;
        }

        private async Task Load_DataGridViewAsync(IEnumerable<InventoryLog> customData = null)
        {
            try
            {
                var data = customData ?? await _inventoryService.GetAllLogsAsync();
                var dataList = data != null ? new System.Collections.Generic.List<InventoryLog>(data) : new System.Collections.Generic.List<InventoryLog>();

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

                dgvLichSu.AutoGenerateColumns = false;
                dgvLichSu.DataSource = dataList;

                if (dgvLichSu.Columns.Contains("colNgay"))
                    dgvLichSu.Columns["colNgay"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lưới: " + ex.Message);
            }

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
                dgvLichSu.Columns["colTenSanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
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

            dgvLichSu.RowTemplate.Height = 40;
            dgvLichSu.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvLichSu.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvLichSu.ColumnHeadersHeight = 40; 
            dgvLichSu.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; 
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

        private async void UpdateComputedStockAsync()
        {
            if (cboSanPham.SelectedValue != null && int.TryParse(cboSanPham.SelectedValue.ToString(), out int maSP))
            {
                try
                {
                    int currentStock = await _inventoryService.GetProductStockAsync(maSP);
                    
                    txtSoLuongTruoc.Text = currentStock.ToString();
                    
                    if (int.TryParse(txtSoLuongThayDoi.Text.Trim(), out int change))
                    {
                        txtSoLuongSau.Text = (currentStock + change).ToString();
                    }
                    else
                    {
                        txtSoLuongSau.Text = currentStock.ToString();
                    }
                }
                catch
                {
                    // Ignore errors during live computation
                }
            }
            else
            {
                txtSoLuongTruoc.Text = "";
                txtSoLuongSau.Text = "";
            }
        }

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

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComputedStockAsync();
        }

        private void txtSoLuongThayDoi_TextChanged(object sender, EventArgs e)
        {
            UpdateComputedStockAsync();
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private void dgvLichSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtMaLichSu.Enabled == true)
                {
                    txtMaLichSu.Enabled = false;
                    btnAdd.Enabled = true;
                }

                DataGridViewRow row = dgvLichSu.Rows[e.RowIndex];

                if (row.Cells["colMaLichSu"].Value != null)
                    txtMaLichSu.Text = row.Cells["colMaLichSu"].Value.ToString();

                if (row.Cells["colMaThamChieu"].Value != null)
                    txtMaThamChieu.Text = row.Cells["colMaThamChieu"].Value.ToString();
                
                // Tắt event tạm thời để không bị gọi api get stock liên tục khi gán
                cboSanPham.SelectedIndexChanged -= cboSanPham_SelectedIndexChanged;
                if (row.Cells["colMaSanPham"].Value != null)
                    cboSanPham.SelectedValue = row.Cells["colMaSanPham"].Value;
                cboSanPham.SelectedIndexChanged += cboSanPham_SelectedIndexChanged;
                
                if (row.Cells["colSoLuongThayDoi"].Value != null)
                    txtSoLuongThayDoi.Text = row.Cells["colSoLuongThayDoi"].Value.ToString();

                if (row.Cells["colSoLuongTruoc"].Value != null)
                    txtSoLuongTruoc.Text = row.Cells["colSoLuongTruoc"].Value.ToString();

                if (row.Cells["colSoLuongSau"].Value != null)
                    txtSoLuongSau.Text = row.Cells["colSoLuongSau"].Value.ToString();

                if (row.Cells["colLoai"].Value != null)
                    cboLoaiThayDoi.Text = row.Cells["colLoai"].Value.ToString();

                if (row.Cells["colTrangThai"].Value != null)
                    cboTrangThai.Text = row.Cells["colTrangThai"].Value.ToString();

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
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
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
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInventoryInputs(out int thayDoi, out int refId))
                return;

            string trangThai = cboTrangThai.Text;
            if (string.IsNullOrEmpty(trangThai)) trangThai = "Đang hoạt động";

            try
            {
                var newLog = new InventoryLog
                {
                    MaSanPham = Convert.ToInt32(cboSanPham.SelectedValue),
                    ThayDoi = thayDoi,
                    LoaiGiaoDich = cboLoaiThayDoi.Text,
                    MaThamChieu = refId,
                    TrangThai = trangThai
                };

                bool success = await _inventoryService.AddLogAsync(newLog);

                if (success)
                {
                    MessageBox.Show("Thêm bản ghi điều chỉnh và cập nhật tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);

                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaLichSu.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Message.Contains("Tồn kho sau"))
                    txtSoLuongThayDoi.Focus();
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLichSu.Rows.Count == 0 || txtMaLichSu.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInventoryInputs(out int newThayDoi, out int newRefId))
                return;

            try
            {
                var updateLog = new InventoryLog
                {
                    MaLichSu = Convert.ToInt32(txtMaLichSu.Text),
                    MaSanPham = Convert.ToInt32(cboSanPham.SelectedValue),
                    ThayDoi = newThayDoi,
                    LoaiGiaoDich = cboLoaiThayDoi.Text,
                    MaThamChieu = newRefId,
                    TrangThai = cboTrangThai.Text
                };

                bool success = await _inventoryService.UpdateLogAsync(updateLog);

                if (success)
                {
                    MessageBox.Show("Cập nhật bản ghi điều chỉnh và tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);

                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAdd.Enabled = true;
                    txtMaLichSu.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLichSu.Rows.Count == 0 || txtMaLichSu.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int currentMaLichSu = Convert.ToInt32(txtMaLichSu.Text);

            if (MessageBox.Show("Bạn có chắc chắn muốn hủy bản ghi này? Số lượng tồn kho của sản phẩm sẽ được HỆ THỐNG TỰ ĐỘNG THU HỒI.", "Xác nhận hủy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    bool success = await _inventoryService.DeleteLogAsync(currentMaLichSu);

                    if (success)
                    {
                        MessageBox.Show("Hủy bản ghi điều chỉnh kho và hoàn trả tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        await Load_DataGridViewAsync();
                        ResetValues();
                        ToggleInputs(false);

                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnCancel.Enabled = false;
                        btnAdd.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
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
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaLichSu.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaLichSu.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã bật!\nVui lòng nhập thông tin cần lọc vào các ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaLichSu.Focus();
                return;
            }

            string idTerm = txtMaLichSu.Text.Trim();
            string refTerm = txtMaThamChieu.Text.Trim();
            string productTerm = (cboSanPham.SelectedValue != null && cboSanPham.SelectedIndex != -1) ? cboSanPham.SelectedValue.ToString() : "";
            string typeTerm = (cboLoaiThayDoi.SelectedIndex != -1) ? cboLoaiThayDoi.Text : "";
            string statusTerm = (cboTrangThai.SelectedIndex != -1) ? cboTrangThai.Text : "";

            if (string.IsNullOrEmpty(idTerm) && string.IsNullOrEmpty(refTerm) && string.IsNullOrEmpty(productTerm) &&
                string.IsNullOrEmpty(typeTerm) && string.IsNullOrEmpty(statusTerm))
            {
                MessageBox.Show("Vui lòng điền ít nhất một tiêu chí tìm kiếm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var searchResults = await _inventoryService.SearchLogsAsync(idTerm, refTerm, productTerm, typeTerm, statusTerm);
                
                var resultsList = new System.Collections.Generic.List<InventoryLog>(searchResults);
                
                if (resultsList.Count > 0)
                {
                    ResetValues();
                    MessageBox.Show($"Tìm thấy {resultsList.Count} bản ghi thỏa mãn yêu cầu!!!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ResetValues();
                    MessageBox.Show("Không tìm thấy dữ liệu nào khớp với thông tin đã nhập!", "Không có kết quả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                await Load_DataGridViewAsync(resultsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await Load_DataGridViewAsync();
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

    }
}
