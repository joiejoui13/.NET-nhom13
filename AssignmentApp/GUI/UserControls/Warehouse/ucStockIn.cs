using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Warehouse;
using AssignmentApp.DTO.Models;
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
    public partial class ucStockIn : UserControl
    {
        private readonly IStockInService _stockInService;
        private readonly IProductService _productService;

        #region A. BIẾN TOÀN CỤC VÀ KHỞI TẠO

        // Biến toàn cục dùng chung cho cả 2 Tab
        private List<StockInDetailModel> currentDetails = new List<StockInDetailModel>();
        private bool isEditing = false;
        private bool isAddingNew = false;
        private bool isAddingDetail = false;
        private bool isSearching = false;
        
        // Mặc định người dùng hệ thống là Admin
        private int activeUserId = 1;      
        private string activeUserName = "Admin";

        public ucStockIn()
        {
            InitializeComponent();
            
            _stockInService = Program.ServiceProvider.GetRequiredService<IStockInService>();
            _productService = Program.ServiceProvider.GetRequiredService<IProductService>();

            // Cấu hình ComboBox trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Chờ xử lý", "Đã hoàn thành", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Kích hoạt khi giao diện vừa được tải lên. Khởi tạo cấu hình và gọi BLL để lấy dữ liệu đổ vào Grid.
        /// </summary>
        private async void ucStockIn_Load(object sender, EventArgs e)
        {
            guna2Button1.Click += btnStockInSearch_Click;
            guna2Button2.Click += btnStockInRefresh_Click;
            
            tabMain.Selecting += tabMain_Selecting;

            dgvDetails.CellClick += dgvDetails_CellClick;
            dgvProductsSelection.CellClick += dgvProductsSelection_CellClick;
            dgvCurrentDetails.CellClick += dgvCurrentDetails_CellClick;

            await LoadProductsSelectionGridAsync(null);
            LoadActiveUser();

            lblGridTitle.Text = "DANH SÁCH PHIẾU NHẬP";
            
            if (dgvDetails.Columns.Count >= 5)
            {
                dgvDetails.Columns[0].HeaderText = "Mã Phiếu Nhập";
                dgvDetails.Columns[1].HeaderText = "Mã NV";
                dgvDetails.Columns[2].HeaderText = "Ngày Nhập";
                dgvDetails.Columns[3].HeaderText = "Trạng Thái";
                dgvDetails.Columns[4].HeaderText = "Tổng Tiền";
            }

            await LoadReceiptsGridAsync(null);

            ResetTab1State();
            ResetTab2State();
        }

        private void LoadActiveUser()
        {
            if (AssignmentApp.BLL.Session.UserSession.CurrentUser != null)
            {
                activeUserId = AssignmentApp.BLL.Session.UserSession.CurrentUser.MaNguoiDung;
                activeUserName = AssignmentApp.BLL.Session.UserSession.CurrentUser.TenNguoiDung;
            }
            else
            {
                activeUserId = 1;
                activeUserName = "Admin";
            }
        }

        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabChonSanPham)
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
                {
                    MessageBox.Show("Vui lòng chọn một phiếu nhập ở Tab 1, hoặc ấn nút Thêm Mới, trước khi chuyển sang Tab 2!", "Cảnh báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region B. TAB 1 - QUẢN LÝ PHIẾU NHẬP

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

        private async Task LoadReceiptsGridAsync(IEnumerable<StockInReceipt> customList)
        {
            try
            {
                IEnumerable<StockInReceipt> list;
                if (customList != null) list = customList;
                else list = await _stockInService.GetAllReceiptsAsync();

                dgvDetails.Rows.Clear();
                foreach (var r in list)
                {
                    dgvDetails.Rows.Add(
                        r.MaPhieuNhap,
                        r.MaNguoiDung,
                        r.NgayNhap.ToString("dd/MM/yyyy HH:mm"),
                        r.TrangThai,
                        r.TongTien.ToString("N0") + " đ"
                    );
                }

                dgvDetails.RowTemplate.Height = 40;
                dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
                dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgvDetails.ColumnHeadersHeight = 40;
                dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SelectReceiptRowAsync(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDetails.Rows.Count) return;

            string codeVal = dgvDetails.Rows[rowIndex].Cells[0].Value?.ToString() ?? "";

            lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP " + codeVal;
            lblStockInTitle.Text = "MÃ PHIẾU: " + codeVal;

            dgvDetails.ClearSelection();
            dgvDetails.Rows[rowIndex].Selected = true;

            if (!int.TryParse(codeVal, out int receiptId)) return;

            try
            {
                var receipt = await _stockInService.GetReceiptByIdAsync(receiptId);
                if (receipt != null)
                {
                    txtMaPhieuNhap.Text = receipt.MaPhieuNhap.ToString();
                    txtNguoiDung.Text = receipt.MaNguoiDung.ToString();
                    dtNgayNhap.Value = receipt.NgayNhap;
                    cboTrangThai.Text = receipt.TrangThai ?? "Chờ xử lý";

                    var details = await _stockInService.GetReceiptDetailsAsync(receiptId);
                    currentDetails.Clear();
                    foreach (var d in details)
                    {
                        currentDetails.Add(new StockInDetailModel
                        {
                            MaSanPham = d.MaSanPham,
                            TenSanPham = string.IsNullOrEmpty(d.TenSanPham) ? "Sản phẩm ẩn" : d.TenSanPham,
                            SoLuong = d.SoLuong,
                            GiaNhap = d.GiaNhap
                        });
                    }
                    
                    LoadCurrentDetailsGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private async void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
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

                await SelectReceiptRowAsync(e.RowIndex);

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

        private void DtNgayNhap_ValueChanged(object sender, EventArgs e)
        {
            if (dtNgayNhap.CustomFormat == " ")
            {
                dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing == false)
            {
                isAddingNew = true;
                isEditing = true;

                txtMaPhieuNhap.Text = "Tự động sinh";
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP MỚI";
                lblStockInTitle.Text = "MÃ PHIẾU: TỰ ĐỘNG SINH";
                
                txtNguoiDung.Text = activeUserId.ToString();
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
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh") return;
            if (!int.TryParse(txtNguoiDung.Text.Trim(), out int userId))
            {
                MessageBox.Show("Mã nhân viên bắt buộc phải là con số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            try
            {
                StockInReceipt receipt = new StockInReceipt
                {
                    MaPhieuNhap = Convert.ToInt32(txtMaPhieuNhap.Text),
                    MaNguoiDung = userId,
                    NgayNhap = dtNgayNhap.Value,
                    TrangThai = cboTrangThai.Text
                };

                await _stockInService.SaveReceiptAsync(receipt, currentDetails, false);
                MessageBox.Show("Cập nhật thông tin phiếu nhập thành công!", "Hoàn tất nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadReceiptsGridAsync(null);
                ResetTab1State();
                ResetTab2State();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) || txtMaPhieuNhap.Text == "Tự động sinh") return;

            int receiptId = Convert.ToInt32(txtMaPhieuNhap.Text);

            DialogResult confirmResult = MessageBox.Show($"Bạn có thực sự muốn xóa (hủy bỏ) phiếu nhập mã #{receiptId} không?", "Xác nhận khẩn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    bool success = await _stockInService.CancelReceiptAsync(receiptId);
                    if (success)
                    {
                        MessageBox.Show("Đã hủy bỏ phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadReceiptsGridAsync(null);
                        ResetTab1State();
                        ResetTab2State();
                    }
                    else
                    {
                        MessageBox.Show("Phiếu nhập này vốn dĩ đã bị Hủy rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtNguoiDung.Text.Trim(), out int userId))
            {
                MessageBox.Show("Mã nhân viên phải là kiểu số nguyên!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNguoiDung.Focus();
                return;
            }

            try
            {
                StockInReceipt receipt = new StockInReceipt
                {
                    MaNguoiDung = userId,
                    NgayNhap = dtNgayNhap.Value,
                    TrangThai = cboTrangThai.Text
                };

                await _stockInService.SaveReceiptAsync(receipt, currentDetails, true);
                
                MessageBox.Show("Lập phiếu nhập kho mới thành công!", "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);

                isAddingNew = false;
                await LoadReceiptsGridAsync(null);
                ResetTab1State();
                ResetTab2State();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetTab1State(); 
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
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

                MessageBox.Show("Chế độ Lọc Dữ Liệu Đã Bật!\nHãy nhập thông tin cần tìm rồi nhấn 'Tìm kiếm' một lần nữa.", "Trợ lý Ảo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhieuNhap.Focus();
                return;
            }

            try
            {
                string dateSearch = dtNgayNhap.CustomFormat == " " ? "" : dtNgayNhap.Value.ToString("yyyy-MM-dd");
                var listSearch = await _stockInService.SearchReceiptsAsync(txtMaPhieuNhap.Text.Trim(), txtNguoiDung.Text.Trim(), cboTrangThai.Text, dateSearch);
                
                await LoadReceiptsGridAsync(listSearch);
                MessageBox.Show($"Tuyệt vời! Lọc ra được {dgvDetails.Rows.Count} phiếu thỏa mãn.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dgvDetails.Rows.Count > 0)
                {
                    await SelectReceiptRowAsync(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetTab1State(); 
            await LoadReceiptsGridAsync(null);
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnChooseProducts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
            {
                MessageBox.Show("Muốn chọn hàng vào phiếu thì phải có cái Phiếu trước đã. Bấm 'Thêm mới' nha!", "Nhắc nhở nhẹ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tabMain.SelectedTab = tabChonSanPham; 

            if (txtMaPhieuNhap.Text == "Tự động sinh")
            {
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP MỚI";
            }
            else
            {
                lblChonRightTitle.Text = "CHI TIẾT PHIẾU NHẬP " + txtMaPhieuNhap.Text;
            }

            btnResetCartForm_Click(this, EventArgs.Empty);
        }

        #endregion

        #region C. TAB 2 - CHỌN SẢN PHẨM & GIỎ HÀNG

        private void SetTab2InputVisibility(bool visible)
        {
            lblSelMaSP.Visible = visible;
            txtSelMaSP.Visible = visible;
            txtSelMaSP.Enabled = false;

            lblSelTenSP.Visible = visible;
            txtSelTenSP.Visible = visible;
            txtSelTenSP.Enabled = false;

            lblSelSoLuong.Visible = visible;
            txtSelSoLuong.Visible = visible;
            lblSelGiaNhap.Visible = visible;
            txtSelGiaNhap.Visible = visible;
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

            txtProductSearch.Enabled = true; 
            txtProductSearch.Text = "";

            btnAddToCart.Enabled = false;       
            guna2Button4.Enabled = false;       
            btnRemoveFromCart.Enabled = false;  
            btnBackToReceipt.Enabled = true;    
            guna2Button3.Enabled = false;       

            lblProductDetailDesc.Text = "Mã SP: --\nThông tin chi tiết về sản phẩm sẽ được cập nhật ở đây.";
            
            if (picProductDetail.Image != null)
            {
                picProductDetail.Image.Dispose();
                picProductDetail.Image = null;
            }

            dgvCurrentDetails.ClearSelection();
        }

        private async Task LoadProductsSelectionGridAsync(IEnumerable<Product> customList)
        {
            try
            {
                IEnumerable<Product> list;
                if (customList != null) list = customList;
                else list = await _productService.SearchProductsAsync("", "", -1, "Đang bán", 0, 0); // Only selling items

                dgvProductsSelection.Columns.Clear();
                dgvProductsSelection.AutoGenerateColumns = true; 
                dgvProductsSelection.DataSource = list;

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lưới sản phẩm chọn: " + ex.Message);
            }
        }

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

        private async Task FilterProductsAsync()
        {
            string maSP = txtSelMaSP.Text.Trim();
            string tenSP = txtSelTenSP.Text.Trim();
            
            try
            {
                var list = await _productService.SearchProductsAsync(maSP, tenSP, -1, "Đang bán", 0, 0);
                await LoadProductsSelectionGridAsync(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

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
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private async void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (btnRemoveFromCart.Enabled == true)
                {
                    MessageBox.Show("Hãy hoàn tất việc thêm/sửa sản phẩm đang thao tác trước khi chuyển sang chọn sản phẩm khác!", "Lời nhắc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                string rawId = dgvProductsSelection.Rows[e.RowIndex].Cells["MaSanPham"].Value?.ToString() ?? "";

                if (int.TryParse(rawId, out int id))
                {
                    try
                    {
                        var p = await _productService.GetProductByIdAsync(id);
                        if (p != null)
                        {
                            SetTab2InputVisibility(true);
                            
                            txtSelMaSP.Text = p.MaSanPham.ToString();
                            txtSelTenSP.Text = p.TenSanPham ?? "";
                            txtSelGiaNhap.Text = p.GiaNhap.ToString();
                            
                            txtSelSoLuong.Enabled = true;
                            txtSelGiaNhap.Enabled = true;

                            btnAddToCart.Enabled = true; 
                            guna2Button3.Enabled = true; 

                            string catName = string.IsNullOrEmpty(p.TenDanhMuc) ? "Không rõ" : p.TenDanhMuc;
                            
                            lblProductDetailDesc.Text = $"Mã sản phẩm: {id}\n" +
                                                        $"Danh mục: {catName}\n" +
                                                        $"Giá nhập đề xuất: {p.GiaNhap.ToString("N0")} VNĐ\n" +
                                                        $"Mô tả chi tiết: {(p.MoTa ?? "")}";

                            LoadDetailProductImage(p.Anh ?? "");

                            StockInDetailModel existing = currentDetails.Find(x => x.MaSanPham == id);

                            if (existing != null)
                            {
                                txtSelSoLuong.Text = existing.SoLuong.ToString();
                                txtSelGiaNhap.Text = existing.GiaNhap.ToString();
                            }
                            else
                            {
                                txtSelSoLuong.Text = "1"; 
                            }

                            tabSelectionContainer.SelectedTab = tabProductDetail;

                            if (txtSelSoLuong.Enabled == true)
                            {
                                txtSelSoLuong.Focus();
                                txtSelSoLuong.SelectAll();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rawId = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";

                if (int.TryParse(rawId, out int id))
                {
                    StockInDetailModel item = currentDetails.Find(x => x.MaSanPham == id);

                    if (item != null)
                    {
                        isAddingDetail = false;
                        
                        btnAddToCart.Enabled = false; 
                        guna2Button4.Enabled = true; 
                        btnRemoveFromCart.Enabled = true; 
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

        private async void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtProductSearch.Text.Trim();
            try
            {
                var list = await _productService.SearchProductsAsync(keyword, keyword, -1, "Đang bán", 0, 0);
                await LoadProductsSelectionGridAsync(list);
            }
            catch (Exception)
            {
                // Silent catch on rapid typing
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
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

            if (!int.TryParse(txtSelMaSP.Text, out int id))
            {
                MessageBox.Show("Bạn chưa chọn món nào bên trái cả!", "Ế khách", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Phải nhập số lượng dương để người ta còn xếp kho!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            if (!double.TryParse(txtSelGiaNhap.Text, out double price) || price < 0)
            {
                MessageBox.Show("Giá tiền tệ quá! Bạn nhập số đoàng hoàng đi.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            StockInDetailModel existing = currentDetails.Find(x => x.MaSanPham == id);

            if (existing != null)
            {
                existing.SoLuong += qty; 
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
            MessageBox.Show("Đã cho vào rọ (Giỏ hàng)!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            ResetTab2State(); 
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true) return;
            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy") return;

            if (!int.TryParse(txtSelMaSP.Text, out int id)) return;

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng phải chuẩn chứ!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            if (!double.TryParse(txtSelGiaNhap.Text, out double price) || price < 0)
            {
                MessageBox.Show("Giá tiền phải chuẩn chứ!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelGiaNhap.Focus();
                return;
            }

            StockInDetailModel item = currentDetails.Find(x => x.MaSanPham == id);

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
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true) return;
            if (cboTrangThai.Text == "Đã hoàn thành" || cboTrangThai.Text == "Đã hủy") return;

            if (!int.TryParse(txtSelMaSP.Text, out int id)) return;

            StockInDetailModel item = currentDetails.Find(x => x.MaSanPham == id);

            if (item != null)
            {
                currentDetails.Remove(item); 
                LoadCurrentDetailsGrid();
                MessageBox.Show("Đã xóa khỏi giỏ!", "Sạch sẽ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetTab2State();
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnResetCartForm_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnStockInSearch_Click(object sender, EventArgs e)
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

            await FilterProductsAsync();
            MessageBox.Show($"Tôi đã tìm thấy {dgvProductsSelection.Rows.Count} sản phẩm khớp với bạn rồi đó!", "OK nhe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnStockInRefresh_Click(object sender, EventArgs e)
        {
            txtProductSearch.Text = "";
            ResetTab2State();
            await LoadProductsSelectionGridAsync(null);
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnSelectProduct_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelMaSP.Text) && int.TryParse(txtSelMaSP.Text, out _))
            {
                tabSelectionContainer.SelectedTab = tabListProducts; 
                txtSelSoLuong.Focus();
                txtSelSoLuong.SelectAll();
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhieuNhap.Text) == true)
            {
                MessageBox.Show("Không có phiếu nào cả, không thể lưu giỏ hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isAddingNew) btnSave_Click(sender, e);
            else btnEdit_Click(sender, e);
            
            tabMain.SelectedTab = tabPhieuNhap;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ResetTab2State();
        }

        #endregion

        #region D. CÁC SỰ KIỆN TRỐNG

        private void tabSelectionContainer_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblProductDetailDesc_Click(object sender, EventArgs e) { }

        #endregion
    }
}
