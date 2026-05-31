using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Admin; // Dùng Service BLL
using AssignmentApp.DTO;                // Dùng DTO
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucPromotion : UserControl
    {
        // Khai báo giao diện Service (BLL) để gọi xử lý nghiệp vụ thay vì gọi thẳng Database
        private readonly IPromotionService _promotionService;

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo mặc định của UserControl.
        /// Chạy đầu tiên khi khởi tạo đối tượng, dùng để vẽ giao diện và thiết lập các cấu hình tĩnh.
        /// </summary>
        public ucPromotion()
        {
            InitializeComponent();

            // CẤU HÌNH COMBOBOX: Danh sách tùy chọn trạng thái. 
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Không hoạt động", "Chưa diễn ra" });

            // DEPENDENCY INJECTION: Tự động lấy class xử lý BLL từ hệ thống đã đăng ký ở Program.cs
            if (Program.ServiceProvider != null)
            {
                _promotionService = Program.ServiceProvider.GetRequiredService<IPromotionService>();
            }
        }

        /// <summary>
        /// Sự kiện Load: Kích hoạt khi UserControl lần đầu được nạp lên giao diện.
        /// Sử dụng async/await để tránh làm đơ giao diện khi tải dữ liệu từ CSDL.
        /// </summary>
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Kích hoạt khi giao diện vừa được tải lên. Khởi tạo cấu hình và gọi BLL để lấy dữ liệu đổ vào Grid.
        /// </summary>
        private async void ucPromotion_Load(object sender, EventArgs e)
        {
            // 1. Tải toàn bộ dữ liệu danh sách khuyến mãi lên DataGridView thông qua BLL
            await Load_DataGridViewAsync();
            
            // 2. Thiết lập giao diện ban đầu (Trạng thái nghỉ)
            ResetValues(); 
            txtMaKhuyenMai.Enabled = false; // Khóa trường mã vì mã do hệ thống quản lý
            ToggleInputs(false);            // Khóa toàn bộ các ô nhập liệu vì chưa vào chế độ Thêm/Sửa
            
            // 3. Thiết lập trạng thái các nút chức năng
            btnAdd.Enabled = true;          // Bật nút Thêm mới
            btnEdit.Enabled = false;        // Tắt Sửa (Vì chưa chọn dòng nào)
            btnDelete.Enabled = false;      // Tắt Xóa
            btnSave.Enabled = false;        // Tắt Lưu
            btnCancel.Enabled = false;      // Tắt Hủy
        }

        #endregion

        #region 2. CÁC HÀM HỖ TRỢ GIAO DIỆN VÀ DỮ LIỆU (HELPER METHODS)

        /// <summary>
        /// Truy vấn dữ liệu từ BLL và đổ lên giao diện DataGridView.
        /// </summary>
        private async Task Load_DataGridViewAsync()
        {
            if (_promotionService == null) return;

            // Gọi BLL lấy danh sách DTO (Chuyển thành List)
            var promotions = await _promotionService.GetAllPromotionsAsync();
            
            // Tắt tự sinh cột tự động
            dgvPromotion.AutoGenerateColumns = false;
            
            // Gán dữ liệu vào lưới
            dgvPromotion.DataSource = promotions.ToList();

            // ÁNH XẠ DỮ LIỆU: Nối các Property của DTO vào đúng cột
            if (dgvPromotion.Columns.Count >= 6)
            {
                dgvPromotion.Columns[0].DataPropertyName = "MaKhuyenMai";
                dgvPromotion.Columns[1].DataPropertyName = "TenKhuyenMai";
                dgvPromotion.Columns[2].DataPropertyName = "PhanTramGiamGia";
                dgvPromotion.Columns[3].DataPropertyName = "NgayBatDau";
                dgvPromotion.Columns[4].DataPropertyName = "NgayKetThuc"; // Đã đổi tên khớp với DTO mới
                dgvPromotion.Columns[5].DataPropertyName = "TrangThai";

                dgvPromotion.Columns[0].HeaderText = "Mã KM";
                dgvPromotion.Columns[1].HeaderText = "Tên Khuyến Mãi";
                dgvPromotion.Columns[2].HeaderText = "% Giảm";
                dgvPromotion.Columns[3].HeaderText = "Ngày Bắt Đầu";
                dgvPromotion.Columns[4].HeaderText = "Ngày Kết Thúc";
                dgvPromotion.Columns[5].HeaderText = "Trạng Thái";
            }

            // CĂN CHỈNH CHIỀU RỘNG VÀ HIỂN THỊ CỘT
            dgvPromotion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dgvPromotion.Columns.Count >= 6)
            {
                dgvPromotion.Columns[0].Width = 80;
                dgvPromotion.Columns[0].MinimumWidth = 80;
                dgvPromotion.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgvPromotion.Columns[1].MinimumWidth = 200;
                dgvPromotion.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                
                dgvPromotion.Columns[2].Width = 90;
                dgvPromotion.Columns[2].MinimumWidth = 90;
                dgvPromotion.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgvPromotion.Columns[3].Width = 150;
                dgvPromotion.Columns[3].MinimumWidth = 150;
                dgvPromotion.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgvPromotion.Columns[4].Width = 150;
                dgvPromotion.Columns[4].MinimumWidth = 150;
                dgvPromotion.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgvPromotion.Columns[5].Width = 120;
                dgvPromotion.Columns[5].MinimumWidth = 120;
                dgvPromotion.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // ĐỊNH DẠNG CHUNG CỦA LƯỚI
            dgvPromotion.RowTemplate.Height = 40; 
            dgvPromotion.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvPromotion.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvPromotion.ColumnHeadersHeight = 40; 
            dgvPromotion.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvPromotion.AllowUserToAddRows = false; 
            dgvPromotion.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        /// <summary>
        /// Đưa toàn bộ các ô nhập liệu về trạng thái trống hoặc giá trị mặc định.
        /// </summary>
        private void ResetValues()
        {
            txtMaKhuyenMai.Text = "";
            txtTenKhuyenMai.Text = "";
            txtPhanTramGiamGia.Text = "";
            txtMoTaKhuyenMai.Text = "";
            dtNgayBatDau.Value = DateTime.Now;
            dtNgayBatDau.Checked = true;
            dtNgayHetHan.Value = DateTime.Now;
            dtNgayHetHan.Checked = true;
            cboTrangThai.SelectedIndex = -1; 
        }

        /// <summary>
        /// Bật/Tắt khả năng chỉnh sửa của các ô nhập liệu trên form.
        /// </summary>
        private void ToggleInputs(bool isEnabled)
        {
            txtTenKhuyenMai.Enabled = isEnabled;
            txtPhanTramGiamGia.Enabled = isEnabled;
            txtMoTaKhuyenMai.Enabled = isEnabled;
            dtNgayBatDau.Enabled = isEnabled;
            dtNgayHetHan.Enabled = isEnabled;
            cboTrangThai.Enabled = isEnabled;
        }

        /// <summary>
        /// Hàm gom dữ liệu trên Giao diện vào DTO để tiện truyền xuống BLL.
        /// </summary>
        private Promotion GetPromotionFromInputs()
        {
            int percent = 0;
            if (int.TryParse(txtPhanTramGiamGia.Text.Trim(), out int val))
            {
                percent = val;
            }

            int id = 0;
            if (int.TryParse(txtMaKhuyenMai.Text.Trim(), out int parsedId))
            {
                id = parsedId;
            }

            return new Promotion
            {
                MaKhuyenMai = id,
                TenKhuyenMai = txtTenKhuyenMai.Text.Trim(),
                PhanTramGiamGia = percent,
                MoTaKhuyenMai = txtMoTaKhuyenMai.Text.Trim(),
                NgayBatDau = dtNgayBatDau.Value,
                NgayKetThuc = dtNgayHetHan.Value, // Map Giao diện sang DTO NgayKetThuc
                TrangThai = cboTrangThai.Text
            };
        }

        #endregion

        #region 3. CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN (EVENTS)

        /// <summary>
        /// Sự kiện Click vào một ô bất kỳ trong lưới danh sách Khuyến Mãi.
        /// </summary>
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng chọn một dòng trên bảng (DataGridView). Dữ liệu sẽ được trích xuất và hiển thị ngược lên các ô nhập liệu.
        /// </summary>
        private async void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtMaKhuyenMai.Enabled == true)
                {
                    txtMaKhuyenMai.Enabled = false;
                    btnAdd.Enabled = true;
                }

                // 1. LẤY MÃ TỪ GRID VÀ GỌI BLL ĐỂ LẤY CHI TIẾT
                int id = 0;
                if (dgvPromotion.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    if (int.TryParse(dgvPromotion.Rows[e.RowIndex].Cells[0].Value.ToString(), out int parsedId))
                    {
                        id = parsedId;
                    }
                }

                if (id > 0)
                {
                    var p = await _promotionService.GetPromotionByIdAsync(id);
                    if (p != null)
                    {
                        txtMaKhuyenMai.Text = p.MaKhuyenMai.ToString();
                        txtTenKhuyenMai.Text = p.TenKhuyenMai;
                        txtPhanTramGiamGia.Text = p.PhanTramGiamGia.ToString();
                        dtNgayBatDau.Value = p.NgayBatDau;
                        dtNgayHetHan.Value = p.NgayKetThuc;
                        cboTrangThai.Text = p.TrangThai;
                        txtMoTaKhuyenMai.Text = p.MoTaKhuyenMai;
                    }
                }
                
                // 2. CHUYỂN ĐỔI TRẠNG THÁI GIAO DIỆN
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
            
            txtMaKhuyenMai.Enabled = false;
            // CSDL sẽ tự sinh mã tự tăng (INT IDENTITY)
            txtMaKhuyenMai.Text = "Tự động sinh";
            cboTrangThai.Text = "Hoạt động"; 
            
            ToggleInputs(true); 
            
            btnSave.Enabled = true;     
            btnCancel.Enabled = true;   
            
            btnAdd.Enabled = false;     
            btnEdit.Enabled = false;    
            btnDelete.Enabled = false;  

            txtTenKhuyenMai.Focus(); 
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newPromo = GetPromotionFromInputs();

                // Lớp BLL sẽ tự lo phần Validate, nếu sai quy tắc nó sẽ throw Exception
                bool success = await _promotionService.AddPromotionAsync(newPromo);

                if (success)
                {
                    MessageBox.Show("Thêm mới khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await Load_DataGridViewAsync();    
                    ResetValues();          
                    ToggleInputs(false);    
                    
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    txtMaKhuyenMai.Enabled = false;
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
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text) || txtMaKhuyenMai.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một khuyến mãi hợp lệ trong danh sách để chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var editPromo = GetPromotionFromInputs();
                bool success = await _promotionService.UpdatePromotionAsync(editPromo);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await Load_DataGridViewAsync();
                    ResetValues();
                    ToggleInputs(false);
                    
                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAdd.Enabled = true;
                    txtMaKhuyenMai.Enabled = false;
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
            if (dgvPromotion.Rows.Count == 0 || string.IsNullOrEmpty(txtMaKhuyenMai.Text))
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Không hoạt động) khuyến mãi này không?", "Xác nhận hành động", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    int id = int.Parse(txtMaKhuyenMai.Text);
                    // Lớp BLL có thể tự quyết định là Delete hẳn trong CSDL hay Update thành "Không hoạt động"
                    // Ở đây gọi hàm Xóa, và Repository sẽ xử lý triệt để
                    bool success = await _promotionService.DeletePromotionAsync(id);
                    
                    if (success)
                    {
                        MessageBox.Show("Đã xóa bản ghi (chuyển trạng thái) thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtMaKhuyenMai.Enabled = false;
        }
/// <summary>
        /// [SỰ KIỆN GIAO DIỆN] Xử lý khi người dùng nhấn nút. Giao diện (GUI) sẽ thu thập dữ liệu và chuyển xuống tầng BLL để xử lý.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaKhuyenMai.Enabled == false && btnAdd.Enabled == true)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaKhuyenMai.Enabled = true; 
                
                dtNgayBatDau.Checked = false;
                dtNgayHetHan.Checked = false;

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập các tiêu chí cần lọc vào ô nhập liệu rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhuyenMai.Focus();
                return;
            }

            // Thực hiện Lọc bằng cách kéo danh sách từ BLL và lọc qua LINQ trực tiếp trên Giao diện
            var allPromos = await _promotionService.GetAllPromotionsAsync();
            var filtered = allPromos.AsEnumerable();

            string idTerm = txtMaKhuyenMai.Text.Trim();
            string nameTerm = txtTenKhuyenMai.Text.Trim();
            string statusTerm = cboTrangThai.Text;

            if (string.IsNullOrEmpty(idTerm) && string.IsNullOrEmpty(nameTerm) && string.IsNullOrEmpty(statusTerm) && !dtNgayBatDau.Checked && !dtNgayHetHan.Checked)
            {
                MessageBox.Show("Vui lòng điền ít nhất một tiêu chí tìm kiếm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(idTerm))
                filtered = filtered.Where(p => p.MaKhuyenMai.ToString().Contains(idTerm));
                
            if (!string.IsNullOrEmpty(nameTerm))
                filtered = filtered.Where(p => p.TenKhuyenMai.ToLower().Contains(nameTerm.ToLower()));
                
            if (!string.IsNullOrEmpty(statusTerm))
                filtered = filtered.Where(p => p.TrangThai == statusTerm);

            if (dtNgayBatDau.Checked)
                filtered = filtered.Where(p => p.NgayBatDau.Date == dtNgayBatDau.Value.Date);
                
            if (dtNgayHetHan.Checked)
                filtered = filtered.Where(p => p.NgayKetThuc.Date == dtNgayHetHan.Value.Date);

            var resultList = filtered.ToList();
            dgvPromotion.DataSource = resultList;

            if (resultList.Count > 0)
            {
                ResetValues();
                MessageBox.Show($"Hoàn tất! Tìm thấy {resultList.Count} bản ghi khớp yêu cầu.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ResetValues();
                MessageBox.Show("Rất tiếc, không tìm thấy khuyến mãi nào khớp với các tiêu chí tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            btnCancel.Enabled = false; 
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
            txtMaKhuyenMai.Enabled = false;
        }

        #endregion
    }
}
