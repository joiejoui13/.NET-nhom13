using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    public partial class ucCategory : UserControl
    {
        private readonly ICategoryService _categoryService;

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        public ucCategory()
        {
            InitializeComponent();

            _categoryService = Program.ServiceProvider.GetRequiredService<ICategoryService>();

            cboTrangThai.Items.Clear(); 
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Đã hủy" });
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; 
            
            this.Load += ucCategory_Load;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Kích hoạt khi giao diện vừa được tải lên. Khởi tạo cấu hình và gọi BLL để lấy dữ liệu đổ vào Grid.
        /// </summary>
        private async void ucCategory_Load(object sender, EventArgs e)
        {
            await Load_DataGridViewAsync();

            ResetValues();
            txtMaDanhMuc.Enabled = false; 
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        private async Task Load_DataGridViewAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (dgvDanhMuc.Columns.Contains("colMaDanhMuc")) dgvDanhMuc.Columns["colMaDanhMuc"].DataPropertyName = "MaDanhMuc";
                if (dgvDanhMuc.Columns.Contains("colTenDanhMuc")) dgvDanhMuc.Columns["colTenDanhMuc"].DataPropertyName = "TenDanhMuc";
                if (dgvDanhMuc.Columns.Contains("colMoTa")) dgvDanhMuc.Columns["colMoTa"].DataPropertyName = "MoTa";
                if (dgvDanhMuc.Columns.Contains("colTrangThai")) dgvDanhMuc.Columns["colTrangThai"].DataPropertyName = "TrangThai";
                if (dgvDanhMuc.Columns.Contains("colNgayTao")) dgvDanhMuc.Columns["colNgayTao"].DataPropertyName = "NgayTao";
                if (dgvDanhMuc.Columns.Contains("colNgayCapNhat")) dgvDanhMuc.Columns["colNgayCapNhat"].DataPropertyName = "NgayCapNhat";

                dgvDanhMuc.AutoGenerateColumns = false; 
                dgvDanhMuc.DataSource = categories.ToList();
                
                if (dgvDanhMuc.Columns.Contains("colNgayTao"))
                    dgvDanhMuc.Columns["colNgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                
                if (dgvDanhMuc.Columns.Contains("colNgayCapNhat"))
                    dgvDanhMuc.Columns["colNgayCapNhat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                dgvDanhMuc.AllowUserToAddRows = false;
                dgvDanhMuc.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetValues()
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = -1; 
        }

        private void ToggleInputs(bool isEnabled)
        {
            txtTenDanhMuc.Enabled = isEnabled;
            txtMoTa.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        private Category GetCategoryFromInputs()
        {
            var category = new Category
            {
                TenDanhMuc = txtTenDanhMuc.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.Text.Trim()
            };

            if (int.TryParse(txtMaDanhMuc.Text, out int id))
            {
                category.MaDanhMuc = id;
            }

            return category;
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtMaDanhMuc.Enabled == true)
                {
                    txtMaDanhMuc.Enabled = false;
                    btnAdd.Enabled = true;
                }

                DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
                
                txtMaDanhMuc.Text = row.Cells[0].Value?.ToString();
                txtTenDanhMuc.Text = row.Cells[1].Value?.ToString();
                txtMoTa.Text = row.Cells[2].Value?.ToString();
                cboTrangThai.Text = row.Cells[3].Value?.ToString();
                
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
            
            txtMaDanhMuc.Enabled = false;
            txtMaDanhMuc.Text = "(Tự động sinh)";
            cboTrangThai.Text = "Hoạt động"; 
            
            ToggleInputs(true);
            txtTenDanhMuc.Focus(); 

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var category = GetCategoryFromInputs();
                
                bool success = await _categoryService.AddCategoryAsync(category);

                if (success)
                {
                    MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);

                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaDanhMuc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count == 0 || string.IsNullOrEmpty(txtMaDanhMuc.Text) || txtMaDanhMuc.Text == "(Tự động sinh)")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var category = GetCategoryFromInputs();
                
                bool success = await _categoryService.UpdateCategoryAsync(category);

                if (success)
                {
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);

                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAdd.Enabled = true;
                    txtMaDanhMuc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count == 0 || string.IsNullOrEmpty(txtMaDanhMuc.Text) || txtMaDanhMuc.Text == "(Tự động sinh)")
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Đã hủy) danh mục này không?", "Cảnh báo xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    int id = int.Parse(txtMaDanhMuc.Text);
                    bool success = await _categoryService.DeleteCategoryAsync(id);

                    if (success)
                    {
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show(ex.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            txtMaDanhMuc.Enabled = false;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaDanhMuc.Enabled == false)
            {
                ResetValues();
                txtMaDanhMuc.Enabled = true; 
                ToggleInputs(true);

                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập thông tin (Mã, Tên, Mô tả...) vào ô trống rồi ấn nút Tìm kiếm lần nữa.", "Thông báo Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaDanhMuc.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaDanhMuc.Text) && string.IsNullOrWhiteSpace(txtTenDanhMuc.Text) && string.IsNullOrWhiteSpace(txtMoTa.Text) && string.IsNullOrWhiteSpace(cboTrangThai.Text))
            {
                MessageBox.Show("Hãy nhập ít nhất một điều kiện tìm kiếm!!!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            try
            {
                string idTerm = txtMaDanhMuc.Text.Trim();
                string nameTerm = txtTenDanhMuc.Text.Trim();
                string descTerm = txtMoTa.Text.Trim();
                string statusTerm = cboTrangThai.Text.Trim();

                var categories = await _categoryService.SearchCategoriesAsync(idTerm, nameTerm, descTerm, statusTerm);
                var categoryList = categories.ToList();

                if (categoryList.Count == 0)
                {
                    MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Tìm thấy {categoryList.Count} bản ghi thỏa mãn điều kiện!!!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
                dgvDanhMuc.DataSource = categoryList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtMaDanhMuc.Enabled = false;
        }

        #endregion

        #region 5. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvDanhMuc_CellClick(sender, e);
        }

        #endregion
    }
}
