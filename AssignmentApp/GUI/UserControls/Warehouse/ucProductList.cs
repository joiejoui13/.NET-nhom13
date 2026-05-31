using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Warehouse;
using AssignmentApp.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        private readonly IProductService _productService;
        private string currentImagePath = "";

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        public ucProductList()
        {
            InitializeComponent();
            _productService = Program.ServiceProvider.GetRequiredService<IProductService>();

            // CẤU HÌNH COMBOBOX: Thêm các tùy chọn cố định (Tách từ Designer)
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang bán", "Ngưng bán" });
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; // Cấm người dùng gõ phím tinh tinh
        }

        private async void ucProductList_Load(object sender, EventArgs e)
        {
            // 1. Nạp dữ liệu vào ComboBox Danh Mục từ CSDL
            await Load_cboDanhMucAsync();

            // 2. Tải toàn bộ danh sách Sản phẩm lên DataGridView
            await LoadProductsGridAsync(null);

            // 3. Khóa các ô nhập liệu và xóa sạch dữ liệu cũ trên form
            ClearInputs();
            ToggleInputs(false);
            
            // 4. Cấu hình trạng thái các nút bấm ban đầu
            btnAdd.Enabled = true;          // Được quyền bấm Thêm mới
            btnEdit.Enabled = false;        // Chưa chọn SP nào nên cấm Sửa
            btnDelete.Enabled = false;      // Cấm Xóa
            btnSave.Enabled = false;        // Đang không thao tác nên cấm Lưu
            btnCancel.Enabled = false;      // Cấm Hủy
            txtMaSanPham.Enabled = false;   // Mã SP do CSDL cấp, cấm gõ tay
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        private async Task Load_cboDanhMucAsync()
        {
            try
            {
                DataTable tblDanhMuc = await _productService.GetCategoriesForComboBoxAsync();
                
                cboDanhMuc.DataSource = tblDanhMuc;
                cboDanhMuc.DisplayMember = "TenDanhMuc"; // Tên hiển thị ra cho người dùng đọc
                cboDanhMuc.ValueMember = "MaDanhMuc";   // Mã chìm bên dưới để lưu CSDL
                cboDanhMuc.SelectedIndex = -1;          // Mặc định không chọn danh mục nào
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductsGridAsync(IEnumerable<Product> customList)
        {
            try
            {
                IEnumerable<Product> listProducts;

                // Nếu người dùng có truyền customList (Ví dụ: dữ liệu sau khi Tìm kiếm) thì dùng nó
                if (customList != null)
                {
                    listProducts = customList;
                }
                else
                {
                    listProducts = await _productService.GetAllProductsAsync();
                }

                // Tắt chức năng tự đẻ cột của DataGridView để không làm hỏng thiết kế
                dgvSanPham.AutoGenerateColumns = false;
                
                var bindingList = new System.ComponentModel.BindingList<Product>(System.Linq.Enumerable.ToList(listProducts));
                dgvSanPham.DataSource = bindingList;

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
                if (dgvSanPham.Columns.Contains("colSoLuongTon")) dgvSanPham.Columns["colSoLuongTon"].DefaultCellStyle.Format = "N0";
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private async Task SelectProductRowAsync(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSanPham.Rows.Count) 
            {
                return;
            }

            dgvSanPham.ClearSelection();
            dgvSanPham.Rows[rowIndex].Selected = true;

            // Ép kiểu an toàn (int.TryParse)
            if (int.TryParse(dgvSanPham.Rows[rowIndex].Cells["colMaSanPham"].Value?.ToString(), out int prodId) == false)
            {
                return;
            }
            
            try
            {
                Product p = await _productService.GetProductByIdAsync(prodId);
                if (p != null)
                {
                    txtMaSanPham.Text = p.MaSanPham.ToString();
                    txtTenSanPham.Text = p.TenSanPham;
                    txtGiaNhap.Text = p.GiaNhap.ToString();
                    txtGiaBan.Text = p.GiaBan.ToString();
                    txtSoLuongTon.Text = p.SoLuongTon.ToString();
                    txtMoTa.Text = p.MoTa ?? "";
                    
                    if (p.MaDanhMuc > 0)
                    {
                        cboDanhMuc.SelectedValue = p.MaDanhMuc;
                    }
                    else
                    {
                        cboDanhMuc.SelectedIndex = -1;
                    }

                    cboTrangThai.Text = !string.IsNullOrEmpty(p.TrangThai) ? p.TrangThai : "Đang bán";
                    currentImagePath = p.Anh ?? "";
                    LoadProductImage(currentImagePath);

                    // Cập nhật Nhãn Tab Chi Tiết Bên Phải
                    lblProductDetailName.Text = p.TenSanPham.ToUpper();
                    lblProductDetailPrice.Text = $"Giá bán: {p.GiaBan.ToString("N0")} VNĐ";
                    lblProductDetailStock.Text = $"Số lượng tồn: {p.SoLuongTon.ToString("N0")}";
                    
                    string tenDanhMuc = !string.IsNullOrEmpty(p.TenDanhMuc) ? p.TenDanhMuc : "Không rõ";
                    
                    lblProductDetailDesc.Text = $"Mã sản phẩm: {p.MaSanPham}\n" +
                                                $"Danh mục: {tenDanhMuc}\n" +
                                                $"Giá nhập: {p.GiaNhap.ToString("N0")} VNĐ\n" +
                                                $"Trạng thái: {p.TrangThai}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private Product GetProductFromInputs()
        {
            double.TryParse(txtGiaNhap.Text, out double importPrice);
            double.TryParse(txtGiaBan.Text, out double salesPrice);
            int.TryParse(txtSoLuongTon.Text, out int stock);
            
            int catId = 0;
            if (cboDanhMuc.SelectedValue != null)
            {
                int.TryParse(cboDanhMuc.SelectedValue.ToString(), out catId);
            }

            return new Product
            {
                TenSanPham = txtTenSanPham.Text.Trim(),
                MaDanhMuc = catId,
                GiaNhap = importPrice,
                GiaBan = salesPrice,
                SoLuongTon = stock,
                MoTa = txtMoTa.Text.Trim(),
                Anh = currentImagePath,
                TrangThai = cboTrangThai.Text
            };
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        private async void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtMaSanPham.Enabled == true)
                {
                    txtMaSanPham.Enabled = false;
                    btnAdd.Enabled = true;
                }

                await SelectProductRowAsync(e.RowIndex);
                
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Product p = GetProductFromInputs();
                bool success = await _productService.AddProductAsync(p);

                if (success)
                {
                    MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ToggleInputs(false);
                    await LoadProductsGridAsync(null);

                    btnCancel.Enabled = false;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaSanPham.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi thêm mới", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.Rows.Count == 0 || string.IsNullOrEmpty(txtMaSanPham.Text) || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách lưới để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Product p = GetProductFromInputs();
                p.MaSanPham = Convert.ToInt32(txtMaSanPham.Text);
                
                bool success = await _productService.UpdateProductAsync(p);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ToggleInputs(false);
                    await LoadProductsGridAsync(null);

                    btnCancel.Enabled = false;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaSanPham.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSanPham.Text) == true || txtMaSanPham.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int prodId = Convert.ToInt32(txtMaSanPham.Text);
            string name = txtTenSanPham.Text;

            DialogResult confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Ngưng bán) sản phẩm '{name}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    bool success = await _productService.SoftDeleteProductAsync(prodId);
                    if (success)
                    {
                        MessageBox.Show("Chuyển trạng thái sản phẩm thành Ngưng bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        await LoadProductsGridAsync(null);
                        ClearInputs();
                        ToggleInputs(false);

                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnCancel.Enabled = false;
                        btnAdd.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            await LoadProductsGridAsync(null);
            ToggleInputs(false);
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaSanPham.Enabled = false;
        }

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

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaSanPham.Enabled == false)
            {
                ClearInputs();
                ToggleInputs(true);
                txtMaSanPham.Enabled = true;

                txtGiaNhap.Text = "";
                txtGiaBan.Text = "";
                txtSoLuongTon.Text = "";
                cboTrangThai.SelectedIndex = -1;

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập thông tin (Tên SP, Giá, Danh mục...) vào các ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaSanPham.Focus();
                return;
            }

            string idTerm = txtMaSanPham.Text.Trim();
            string nameTerm = txtTenSanPham.Text.Trim();
            
            int selectedCatId = -1;
            if (cboDanhMuc.SelectedValue != null)
            {
                int.TryParse(cboDanhMuc.SelectedValue.ToString(), out selectedCatId);
            }

            string statusTerm = cboTrangThai.Text;

            double.TryParse(txtGiaBan.Text, out double priceLimit);
            int.TryParse(txtSoLuongTon.Text, out int stockLimit);

            try
            {
                var listSearch = await _productService.SearchProductsAsync(idTerm, nameTerm, selectedCatId, statusTerm, priceLimit, stockLimit);
                await LoadProductsGridAsync(listSearch);

                if (dgvSanPham.Rows.Count > 0)
                {
                    MessageBox.Show($"Tìm thấy {dgvSanPham.Rows.Count} sản phẩm phù hợp!", "Tìm kiếm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào khớp với thông tin đã nhập!", "Thông báo rỗng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

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
