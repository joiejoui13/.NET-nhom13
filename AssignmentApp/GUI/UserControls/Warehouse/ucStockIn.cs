using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucStockIn : UserControl
    {
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public double GiaNhap { get; set; }
        }

        public class MockStockInDetail
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double GiaNhap { get; set; }
            public double ThanhTien => SoLuong * GiaNhap;
        }

        public class MockStockInReceipt
        {
            public int MaPhieuNhap { get; set; }
            public string NguoiTao { get; set; } = "";
            public DateTime NgayNhap { get; set; }
            public string TrangThai { get; set; } = "Chờ xử lý";
            public List<MockStockInDetail> Details { get; set; } = new List<MockStockInDetail>();
        }

        private List<MockProduct> mockProducts = new List<MockProduct>();
        private List<MockStockInReceipt> mockReceipts = new List<MockStockInReceipt>();
        private MockStockInReceipt? selectedReceipt = null;
        
        // Items currently in the grid during add/edit
        private List<MockStockInDetail> currentDetails = new List<MockStockInDetail>();
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucStockIn()
        {
            InitializeComponent();
        }

        private void ucStockIn_Load(object sender, EventArgs e)
        {
            InitializeProducts();
            InitializeMockReceipts();
            
            // Set up ComboBox binding
            cboSanPham.DisplayMember = "TenSanPham";
            cboSanPham.ValueMember = "MaSanPham";
            cboSanPham.DataSource = mockProducts;

            // Load default receipt
            if (mockReceipts.Count > 0)
            {
                LoadReceipt(mockReceipts[0]);
            }
            
            SetEditState(false);
        }

        private void InitializeProducts()
        {
            mockProducts.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", GiaNhap = 600000 });
            mockProducts.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", GiaNhap = 6000 });
            mockProducts.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", GiaNhap = 3500 });
            mockProducts.Add(new MockProduct { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", GiaNhap = 70000 });
        }

        private void InitializeMockReceipts()
        {
            if (mockReceipts.Count > 0) return;

            var r1 = new MockStockInReceipt
            {
                MaPhieuNhap = 101,
                NguoiTao = "Nguyễn Văn Kho",
                NgayNhap = DateTime.Now.AddDays(-5),
                TrangThai = "Đã hoàn thành"
            };
            r1.Details.Add(new MockStockInDetail { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", SoLuong = 50, GiaNhap = 600000 });
            r1.Details.Add(new MockStockInDetail { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuong = 500, GiaNhap = 6000 });

            var r2 = new MockStockInReceipt
            {
                MaPhieuNhap = 102,
                NguoiTao = "Trần Quản Lý",
                NgayNhap = DateTime.Now.AddDays(-2),
                TrangThai = "Chờ xử lý"
            };
            r2.Details.Add(new MockStockInDetail { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuong = 1000, GiaNhap = 3500 });
            r2.Details.Add(new MockStockInDetail { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", SoLuong = 100, GiaNhap = 70000 });

            mockReceipts.Add(r1);
            mockReceipts.Add(r2);
        }

        private void LoadReceipt(MockStockInReceipt receipt)
        {
            selectedReceipt = receipt;
            txtMaPhieuNhap.Text = receipt.MaPhieuNhap.ToString();
            txtNguoiDung.Text = receipt.NguoiTao;
            dtNgayNhap.Value = receipt.NgayNhap;
            cboTrangThai.Text = receipt.TrangThai;

            // Load details grid
            currentDetails = receipt.Details.Select(d => new MockStockInDetail
            {
                MaSanPham = d.MaSanPham,
                TenSanPham = d.TenSanPham,
                SoLuong = d.SoLuong,
                GiaNhap = d.GiaNhap
            }).ToList();

            PopulateDetailsGrid();
        }

        private void PopulateDetailsGrid()
        {
            dgvDetails.Rows.Clear();
            foreach (var item in currentDetails)
            {
                dgvDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong.ToString("N0"),
                    item.GiaNhap.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Header fields
            txtMaPhieuNhap.ReadOnly = !isAddingNew; // Only editable if we are searching or adding
            dtNgayNhap.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // Product input fields (only enabled in edit mode)
            cboSanPham.Enabled = editing;
            txtSoLuong.ReadOnly = !editing;
            txtGiaNhap.ReadOnly = !editing;

            // Buttons behavior
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            
            if (editing)
            {
                btnAdd.Text = "THÊM MÓN";
                btnDelete.Text = "XÓA MÓN";
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                btnAdd.Text = "TẠO MỚI";
                btnDelete.Text = "XÓA PHIẾU";
                btnAdd.Enabled = true;
                btnDelete.Enabled = selectedReceipt != null;
                btnEdit.Enabled = selectedReceipt != null;
            }
        }

        private void cboSanPham_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem is MockProduct prod)
            {
                txtGiaNhap.Text = prod.GiaNhap.ToString();
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!isEditing)
            {
                // Enter Add Receipt mode
                isAddingNew = true;
                isEditing = true;
                
                txtMaPhieuNhap.Text = (mockReceipts.Count > 0 ? mockReceipts.Max(r => r.MaPhieuNhap) + 1 : 101).ToString();
                txtNguoiDung.Text = "Nguyễn Văn Kho"; // default creator
                dtNgayNhap.Value = DateTime.Now;
                cboTrangThai.Text = "Chờ xử lý";
                
                currentDetails.Clear();
                PopulateDetailsGrid();
                
                SetEditState(true);
                txtSoLuong.Text = "1";
                txtSoLuong.Focus();
            }
            else
            {
                // Add item to active details list
                if (cboSanPham.SelectedItem is MockProduct prod)
                {
                    if (!int.TryParse(txtSoLuong.Text, out int qty) || qty <= 0)
                    {
                        MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtSoLuong.Focus();
                        return;
                    }

                    if (!double.TryParse(txtGiaNhap.Text, out double price) || price < 0)
                    {
                        MessageBox.Show("Giá nhập phải lớn hơn hoặc bằng 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtGiaNhap.Focus();
                        return;
                    }

                    // Check if product already exists in grid
                    var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == prod.MaSanPham);
                    if (existing != null)
                    {
                        existing.SoLuong += qty;
                        existing.GiaNhap = price;
                    }
                    else
                    {
                        currentDetails.Add(new MockStockInDetail
                        {
                            MaSanPham = prod.MaSanPham,
                            TenSanPham = prod.TenSanPham,
                            SoLuong = qty,
                            GiaNhap = price
                        });
                    }

                    PopulateDetailsGrid();
                    txtSoLuong.Text = "1";
                }
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedReceipt == null) return;
            if (selectedReceipt.TrangThai == "Đã hoàn thành" || selectedReceipt.TrangThai == "Đã hủy")
            {
                MessageBox.Show("Không thể chỉnh sửa phiếu nhập đã hoàn thành hoặc đã hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAddingNew = false;
            SetEditState(true);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (isEditing)
            {
                // Remove item from grid
                if (dgvDetails.SelectedRows.Count > 0)
                {
                    int index = dgvDetails.SelectedRows[0].Index;
                    if (index >= 0 && index < currentDetails.Count)
                    {
                        currentDetails.RemoveAt(index);
                        PopulateDetailsGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dòng sản phẩm trong lưới để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Delete receipt
                if (selectedReceipt == null) return;
                
                var confirmResult = MessageBox.Show($"Xác nhận xóa phiếu nhập #{selectedReceipt.MaPhieuNhap}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    mockReceipts.Remove(selectedReceipt);
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (mockReceipts.Count > 0)
                    {
                        LoadReceipt(mockReceipts[0]);
                    }
                    else
                    {
                        selectedReceipt = null;
                        txtMaPhieuNhap.Text = "";
                        txtNguoiDung.Text = "";
                        cboTrangThai.SelectedIndex = -1;
                        currentDetails.Clear();
                        PopulateDetailsGrid();
                    }
                    
                    SetEditState(false);
                }
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (currentDetails.Count == 0)
            {
                MessageBox.Show("Phiếu nhập phải có ít nhất một sản phẩm!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isAddingNew)
            {
                if (!int.TryParse(txtMaPhieuNhap.Text, out int id))
                {
                    MessageBox.Show("Mã phiếu nhập phải là số!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaPhieuNhap.Focus();
                    return;
                }

                if (mockReceipts.Any(r => r.MaPhieuNhap == id))
                {
                    MessageBox.Show("Mã phiếu nhập đã tồn tại!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaPhieuNhap.Focus();
                    return;
                }

                var newReceipt = new MockStockInReceipt
                {
                    MaPhieuNhap = id,
                    NguoiTao = txtNguoiDung.Text,
                    NgayNhap = dtNgayNhap.Value,
                    TrangThai = cboTrangThai.Text,
                    Details = currentDetails.Select(d => new MockStockInDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        GiaNhap = d.GiaNhap
                    }).ToList()
                };

                mockReceipts.Add(newReceipt);
                selectedReceipt = newReceipt;

                // Inventory update feedback simulation
                string invFeedback = "";
                if (newReceipt.TrangThai == "Đã hoàn thành")
                {
                    invFeedback = "\n[TỒN KHO] Số lượng tồn kho của các sản phẩm đã được cộng tăng tương ứng!";
                }

                MessageBox.Show("Thêm mới phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedReceipt != null)
                {
                    string oldStatus = selectedReceipt.TrangThai;
                    selectedReceipt.NgayNhap = dtNgayNhap.Value;
                    selectedReceipt.TrangThai = cboTrangThai.Text;
                    selectedReceipt.Details = currentDetails.Select(d => new MockStockInDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        GiaNhap = d.GiaNhap
                    }).ToList();

                    // Inventory update feedback simulation
                    string invFeedback = "";
                    if (oldStatus != "Đã hoàn thành" && selectedReceipt.TrangThai == "Đã hoàn thành")
                    {
                        invFeedback = "\n[TỒN KHO] Phiếu nhập được chuyển sang 'Đã hoàn thành'. Tồn kho đã được cộng thêm!";
                    }
                    else if (oldStatus == "Đã hoàn thành" && selectedReceipt.TrangThai == "Đã hủy")
                    {
                        invFeedback = "\n[TỒN KHO] Phiếu nhập bị HỦY. Tồn kho đã được hoàn tác trừ lại!";
                    }

                    MessageBox.Show("Cập nhật phiếu nhập thành công!" + invFeedback, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            if (selectedReceipt != null)
            {
                LoadReceipt(selectedReceipt);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedReceipt != null)
            {
                LoadReceipt(selectedReceipt);
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaPhieuNhap.Text, out int searchId))
            {
                MessageBox.Show("Vui lòng nhập Mã phiếu nhập (dạng số) cần tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var receipt = mockReceipts.FirstOrDefault(r => r.MaPhieuNhap == searchId);
            if (receipt != null)
            {
                LoadReceipt(receipt);
                SetEditState(false);
            }
            else
            {
                MessageBox.Show("Không tìm thấy phiếu nhập có mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            if (mockReceipts.Count > 0)
            {
                LoadReceipt(mockReceipts[0]);
            }
            SetEditState(false);
        }
    }
}
