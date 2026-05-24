using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucCategory : UserControl
    {
        public class MockCategory
        {
            public int MaDanhMuc { get; set; }
            public string TenDanhMuc { get; set; } = "";
            public string MoTa { get; set; } = "";
            public string TrangThai { get; set; } = "Hoạt động";
            public DateTime NgayTao { get; set; }
        }

        private List<MockCategory> mockCategories = new List<MockCategory>();
        private MockCategory? selectedCategory = null;
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucCategory()
        {
            InitializeComponent();
        }

        private void ucCategory_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            LoadCategoriesGrid();
            SetEditState(false);
            if (dgvDanhMuc.Rows.Count > 0)
            {
                SelectCategoryRow(0);
            }
        }

        private void InitializeMockData()
        {
            if (mockCategories.Count > 0) return;

            mockCategories.Add(new MockCategory
            {
                MaDanhMuc = 1,
                TenDanhMuc = "Sách & Vở",
                MoTa = "Các loại sách giáo khoa, sách tham khảo và vở viết",
                TrangThai = "Hoạt động",
                NgayTao = DateTime.Now.AddMonths(-5)
            });

            mockCategories.Add(new MockCategory
            {
                MaDanhMuc = 2,
                TenDanhMuc = "Dụng cụ học tập",
                MoTa = "Bút, thước, tẩy, hộp bút, compa, màu vẽ",
                TrangThai = "Hoạt động",
                NgayTao = DateTime.Now.AddMonths(-4)
            });

            mockCategories.Add(new MockCategory
            {
                MaDanhMuc = 3,
                TenDanhMuc = "Thiết bị văn phòng",
                MoTa = "Máy tính bỏ túi, giấy in, băng keo, dập ghim",
                TrangThai = "Ngưng hoạt động",
                NgayTao = DateTime.Now.AddMonths(-3)
            });
        }

        private void LoadCategoriesGrid(List<MockCategory>? dataSource = null)
        {
            dgvDanhMuc.Rows.Clear();
            var list = dataSource ?? mockCategories;
            foreach (var cat in list)
            {
                dgvDanhMuc.Rows.Add(
                    cat.MaDanhMuc,
                    cat.TenDanhMuc,
                    cat.MoTa,
                    cat.TrangThai,
                    cat.NgayTao.ToString("dd/MM/yyyy")
                );
            }
        }

        private void SelectCategoryRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDanhMuc.Rows.Count) return;

            dgvDanhMuc.ClearSelection();
            dgvDanhMuc.Rows[rowIndex].Selected = true;

            int catId = Convert.ToInt32(dgvDanhMuc.Rows[rowIndex].Cells[0].Value);
            selectedCategory = mockCategories.FirstOrDefault(c => c.MaDanhMuc == catId);

            if (selectedCategory != null)
            {
                PopulateCategoryDetails(selectedCategory);
            }
        }

        private void PopulateCategoryDetails(MockCategory cat)
        {
            txtMaDanhMuc.Text = cat.MaDanhMuc.ToString();
            txtTenDanhMuc.Text = cat.TenDanhMuc;
            txtMoTa.Text = cat.MoTa;
            cboTrangThai.Text = cat.TrangThai;
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Mã DM is identity/auto-generated
            txtMaDanhMuc.ReadOnly = true;

            // Other fields
            txtTenDanhMuc.ReadOnly = !editing;
            txtMoTa.ReadOnly = !editing;
            cboTrangThai.Enabled = editing;

            // Buttons
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
        }

        private void ClearInputs()
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = 0; // default "Hoạt động"
        }

        private void dgvDanhMuc_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectCategoryRow(e.RowIndex);
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            isAddingNew = true;
            ClearInputs();

            int nextId = mockCategories.Count > 0 ? mockCategories.Max(c => c.MaDanhMuc) + 1 : 1;
            txtMaDanhMuc.Text = nextId.ToString();

            SetEditState(true);
            txtTenDanhMuc.Focus();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedCategory == null)
            {
                MessageBox.Show("Vui lòng chọn một danh mục để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAddingNew = false;
            SetEditState(true);
            txtTenDanhMuc.Focus();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedCategory == null)
            {
                MessageBox.Show("Vui lòng chọn một danh mục để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{selectedCategory.TenDanhMuc}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockCategories.Remove(selectedCategory);
                MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoriesGrid();
                if (dgvDanhMuc.Rows.Count > 0)
                {
                    SelectCategoryRow(0);
                }
                else
                {
                    selectedCategory = null;
                    ClearInputs();
                }
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadCategoriesGrid();
            SetEditState(false);
            if (dgvDanhMuc.Rows.Count > 0)
            {
                SelectCategoryRow(0);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedCategory != null)
            {
                PopulateCategoryDetails(selectedCategory);
            }
            else if (dgvDanhMuc.Rows.Count > 0)
            {
                SelectCategoryRow(0);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            // Search by name keyword in txtTenDanhMuc
            string keyword = txtTenDanhMuc.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập Tên danh mục vào ô nhập thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filtered = mockCategories.Where(c => c.TenDanhMuc.ToLower().Contains(keyword)).ToList();
            LoadCategoriesGrid(filtered);

            if (dgvDanhMuc.Rows.Count > 0)
            {
                SelectCategoryRow(0);
            }
            else
            {
                selectedCategory = null;
                ClearInputs();
                MessageBox.Show("Không tìm thấy danh mục phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            string name = txtTenDanhMuc.Text.Trim();
            string desc = txtMoTa.Text.Trim();
            string status = cboTrangThai.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên danh mục không được để trống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDanhMuc.Focus();
                return;
            }

            if (isAddingNew)
            {
                int newId = mockCategories.Count > 0 ? mockCategories.Max(c => c.MaDanhMuc) + 1 : 1;
                var newCat = new MockCategory
                {
                    MaDanhMuc = newId,
                    TenDanhMuc = name,
                    MoTa = desc,
                    TrangThai = status,
                    NgayTao = DateTime.Now
                };
                mockCategories.Add(newCat);
                selectedCategory = newCat;
                MessageBox.Show("Thêm mới danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedCategory != null)
                {
                    selectedCategory.TenDanhMuc = name;
                    selectedCategory.MoTa = desc;
                    selectedCategory.TrangThai = status;
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            LoadCategoriesGrid();

            // Re-select row
            if (selectedCategory != null)
            {
                int index = mockCategories.IndexOf(selectedCategory);
                if (index >= 0 && index < dgvDanhMuc.Rows.Count)
                {
                    SelectCategoryRow(index);
                }
            }
        }
    }
}
