using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
        public class MockProduct
        {
            public string MaSanPham { get; set; } = "";
            public string TenSanPham { get; set; } = "";
            public int SoLuongMua { get; set; }
            public int DaTra { get; set; }
            public double DonGia { get; set; }
        }

        public class MockReturnDetail
        {
            public string MaSanPham { get; set; } = "";
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public string TinhTrang { get; set; } = "";
            public double ThanhTien => SoLuong * DonGia;
        }

        public class MockReturnSlip
        {
            public int MaTraHang { get; set; }
            public string MaHoaDon { get; set; } = "";
            public string NhanVien { get; set; } = "";
            public string KhachHang { get; set; } = "";
            public DateTime NgayTra { get; set; }
            public string LyDo { get; set; } = "";
            public double TongTienHoan { get; set; }
            public string TrangThai { get; set; } = "Hoàn thành";
            public string LoaiGiaoDich { get; set; } = "Trả hàng";
            public List<MockReturnDetail> Details { get; set; } = new List<MockReturnDetail>();
        }

        private List<MockProduct> invoiceProducts = new List<MockProduct>();
        private List<MockReturnSlip> mockReturns = new List<MockReturnSlip>();
        private MockReturnSlip? selectedReturn = null;

        // Giỏ hàng trả hiện tại trong Tab 2
        private List<MockReturnDetail> currentDetails = new List<MockReturnDetail>();
        private bool isEditing = false;
        private bool isAddingNew = false;

        public ucReturn()
        {
            InitializeComponent();
            cboLoaiGiaoDich.SelectedIndexChanged += cboLoaiGiaoDich_SelectedIndexChanged;
        }

        private void cboLoaiGiaoDich_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isEditing)
            {
                LoadCurrentDetailsGrid();
            }
        }

        private void ucReturn_Load(object sender, EventArgs e)
        {
            InitializeMockReturns();
            
            // Mặc định nạp phiếu trả hàng đầu tiên
            if (mockReturns.Count > 0)
            {
                LoadReturn(mockReturns[0]);
            }

            SetEditState(false);
        }

        private void InitializeMockReturns()
        {
            if (mockReturns.Count > 0) return;

            var r1 = new MockReturnSlip
            {
                MaTraHang = 1,
                MaHoaDon = "1",
                NhanVien = "Thu Ngân 1",
                KhachHang = "Nguyễn Văn A",
                NgayTra = DateTime.Now.AddDays(-2),
                LyDo = "Máy tính phím bấm bị kẹt",
                TongTienHoan = 680000,
                TrangThai = "Hoàn thành",
                LoaiGiaoDich = "Trả hàng"
            };
            r1.Details.Add(new MockReturnDetail { MaSanPham = "12", TenSanPham = "Máy tính Casio FX-580VN X", SoLuong = 1, DonGia = 680000, TinhTrang = "Lỗi kẹt phím số 5" });

            var r2 = new MockReturnSlip
            {
                MaTraHang = 2,
                MaHoaDon = "2",
                NhanVien = "Thu Ngân 1",
                KhachHang = "Công ty CP ABC",
                NgayTra = DateTime.Now.AddDays(-1),
                LyDo = "Đổi rách bìa vở Campus",
                TongTienHoan = 0,
                TrangThai = "Hoàn thành",
                LoaiGiaoDich = "Đổi hàng (1:1)"
            };
            r2.Details.Add(new MockReturnDetail { MaSanPham = "1", TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuong = 10, DonGia = 5000, TinhTrang = "Khách đổi sang bút cùng giá" });

            mockReturns.Add(r1);
            mockReturns.Add(r2);
        }

        private void LoadReturn(MockReturnSlip r)
        {
            selectedReturn = r;
            txtMaHoaDon.Text = r.MaHoaDon;
            txtLyDo.Text = r.LyDo;
            txtTongTienHoan.Text = r.TongTienHoan.ToString("N0") + " đ";
            dtpNgayTra.Value = r.NgayTra;
            cboTrangThai.Text = r.TrangThai;
            cboLoaiGiaoDich.Text = r.LoaiGiaoDich;
            lblKhachHang.Text = $"Khách hàng: {r.KhachHang}";
            lblNhanVien.Text = $"Nhân viên: {r.NhanVien}";

            currentDetails = r.Details.Select(d => new MockReturnDetail
            {
                MaSanPham = d.MaSanPham,
                TenSanPham = d.TenSanPham,
                SoLuong = d.SoLuong,
                DonGia = d.DonGia,
                TinhTrang = d.TinhTrang
            }).ToList();

            PopulateDetailsGrid();
            LoadReturnListGrid();
        }

        private void PopulateDetailsGrid()
        {
            dgvCurrentDetails.Rows.Clear();
            foreach (var item in currentDetails)
            {
                dgvCurrentDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong.ToString("N0"),
                    item.DonGia.ToString("N0") + " đ",
                    item.TinhTrang,
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
        }

        private void LoadReturnListGrid()
        {
            dgvReturns.Rows.Clear();
            foreach (var r in mockReturns)
            {
                dgvReturns.Rows.Add(
                    r.MaTraHang.ToString(),
                    r.MaHoaDon,
                    r.TrangThai,
                    r.LoaiGiaoDich,
                    r.TongTienHoan.ToString("N0") + " đ",
                    r.NhanVien,
                    r.NgayTra.ToString("dd/MM/yyyy HH:mm"),
                    r.LyDo
                );
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            txtMaHoaDon.ReadOnly = !isAddingNew;
            txtLyDo.ReadOnly = !editing;
            dtpNgayTra.Enabled = editing;
            cboTrangThai.Enabled = editing;
            cboLoaiGiaoDich.Enabled = editing;

            // Chế độ chỉnh sửa: ẩn Add/Delete, hiện Save/Cancel/ChooseProducts
            btnAdd.Visible = !editing;
            btnDelete.Visible = !editing;
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnChooseProducts.Visible = editing;

            if (!editing)
            {
                btnAdd.Enabled = true;
                btnDelete.Enabled = selectedReturn != null;
            }
        }

        // ==========================================
        // SỰ KIỆN TAB 1
        // ==========================================

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!isEditing)
            {
                isAddingNew = true;
                isEditing = true;

                txtMaHoaDon.Text = "";
                txtLyDo.Text = "";
                txtTongTienHoan.Text = "0";
                dtpNgayTra.Value = DateTime.Now;
                cboTrangThai.SelectedIndex = 0;
                cboLoaiGiaoDich.SelectedIndex = 0;
                lblKhachHang.Text = "Khách hàng: (Chờ nhập hóa đơn...)";
                lblNhanVien.Text = "Nhân viên: Thu Ngân 1";

                currentDetails.Clear();
                PopulateDetailsGrid();

                SetEditState(true);
                txtMaHoaDon.Focus();
            }
        }



        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedReturn == null) return;
            var confirmResult = MessageBox.Show($"Xác nhận xóa phiếu trả hàng #{selectedReturn.MaTraHang}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                mockReturns.Remove(selectedReturn);
                MessageBox.Show("Xóa phiếu trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if (mockReturns.Count > 0)
                {
                    LoadReturn(mockReturns[0]);
                }
                else
                {
                    selectedReturn = null;
                    txtMaHoaDon.Text = "";
                    txtLyDo.Text = "";
                    txtTongTienHoan.Text = "0";
                    lblKhachHang.Text = "Khách hàng: (Trống)";
                    lblNhanVien.Text = "Nhân viên: (Trống)";
                    currentDetails.Clear();
                    PopulateDetailsGrid();
                    LoadReturnListGrid();
                }
                SetEditState(false);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn gốc!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            if (currentDetails.Count == 0)
            {
                MessageBox.Show("Phiếu trả phải có ít nhất một sản phẩm! Hãy chọn sản phẩm ở Tab 2.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalAmt = cboLoaiGiaoDich.Text == "Đổi hàng (1:1)" ? 0 : currentDetails.Sum(d => d.ThanhTien);

            if (isAddingNew)
            {
                int newId = mockReturns.Count > 0 ? mockReturns.Max(r => r.MaTraHang) + 1 : 1;
                var newSlip = new MockReturnSlip
                {
                    MaTraHang = newId,
                    MaHoaDon = txtMaHoaDon.Text,
                    NhanVien = "Thu Ngân 1",
                    KhachHang = txtMaHoaDon.Text == "2" ? "Công ty CP ABC" : "Nguyễn Văn A",
                    NgayTra = dtpNgayTra.Value,
                    LyDo = txtLyDo.Text,
                    TongTienHoan = totalAmt,
                    TrangThai = cboTrangThai.Text,
                    LoaiGiaoDich = cboLoaiGiaoDich.Text,
                    Details = currentDetails.Select(d => new MockReturnDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        DonGia = d.DonGia,
                        TinhTrang = d.TinhTrang
                    }).ToList()
                };

                mockReturns.Add(newSlip);
                selectedReturn = newSlip;
                string msg = cboLoaiGiaoDich.Text == "Đổi hàng (1:1)"
                    ? "Thêm mới phiếu đổi hàng 1:1 thành công!\n[TỒN KHO] Đã ghi nhận đổi hàng (Tiền hoàn = 0 đ)."
                    : "Thêm mới phiếu trả hàng thành công!\n[TỒN KHO] Tồn kho của sản phẩm trả đã được cộng lại!";
                MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selectedReturn != null)
                {
                    selectedReturn.LyDo = txtLyDo.Text;
                    selectedReturn.NgayTra = dtpNgayTra.Value;
                    selectedReturn.TrangThai = cboTrangThai.Text;
                    selectedReturn.LoaiGiaoDich = cboLoaiGiaoDich.Text;
                    selectedReturn.TongTienHoan = totalAmt;
                    selectedReturn.Details = currentDetails.Select(d => new MockReturnDetail
                    {
                        MaSanPham = d.MaSanPham,
                        TenSanPham = d.TenSanPham,
                        SoLuong = d.SoLuong,
                        DonGia = d.DonGia,
                        TinhTrang = d.TinhTrang
                    }).ToList();

                    string msg = cboLoaiGiaoDich.Text == "Đổi hàng (1:1)"
                        ? "Cập nhật phiếu đổi hàng thành công!"
                        : "Cập nhật phiếu trả hàng thành công!";
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            isAddingNew = false;
            SetEditState(false);
            if (selectedReturn != null)
            {
                LoadReturn(selectedReturn);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            isAddingNew = false;
            SetEditState(false);
            if (selectedReturn != null)
            {
                LoadReturn(selectedReturn);
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            string kw = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(kw))
            {
                LoadReturnListGrid();
                return;
            }

            var filtered = mockReturns.Where(r => r.MaTraHang.ToString() == kw || r.MaHoaDon.Contains(kw) || r.KhachHang.Contains(kw)).ToList();
            dgvReturns.Rows.Clear();
            foreach (var r in filtered)
            {
                dgvReturns.Rows.Add(
                    r.MaTraHang.ToString(),
                    r.MaHoaDon,
                    r.TrangThai,
                    r.LoaiGiaoDich,
                    r.TongTienHoan.ToString("N0") + " đ",
                    r.NhanVien,
                    r.NgayTra.ToString("dd/MM/yyyy HH:mm"),
                    r.LyDo
                );
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadReturnListGrid();
        }

        private void dgvReturns_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string idStr = dgvReturns.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                if (int.TryParse(idStr, out int id))
                {
                    var r = mockReturns.FirstOrDefault(x => x.MaTraHang == id);
                    if (r != null)
                    {
                        LoadReturn(r);
                        SetEditState(false);
                    }
                }
            }
        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            FetchInvoiceDetailsStub();
        }

        private void FetchInvoiceDetailsStub()
        {
            string maHoaDon = txtMaHoaDon.Text.Trim();
            if (string.IsNullOrEmpty(maHoaDon)) return;

            invoiceProducts.Clear();
            if (maHoaDon == "2")
            {
                lblKhachHang.Text = "Khách hàng: Công ty CP ABC | Địa chỉ giao: Tòa nhà văn phòng Cầu Giấy";
                invoiceProducts.Add(new MockProduct { MaSanPham = "7", TenSanPham = "Giấy in Double A A4 70gsm", SoLuongMua = 20, DaTra = 0, DonGia = 80000 });
                invoiceProducts.Add(new MockProduct { MaSanPham = "1", TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuongMua = 100, DaTra = 5, DonGia = 5000 });
            }
            else
            {
                lblKhachHang.Text = "Khách hàng: Nguyễn Văn A (Mua tại quầy)";
                invoiceProducts.Add(new MockProduct { MaSanPham = "12", TenSanPham = "Máy tính Casio FX-580VN X", SoLuongMua = 1, DaTra = 0, DonGia = 680000 });
                invoiceProducts.Add(new MockProduct { MaSanPham = "4", TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuongMua = 10, DaTra = 1, DonGia = 9000 });
            }
        }

        private void btnChooseProducts_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng điền mã hóa đơn gốc ở Tab 1 trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FetchInvoiceDetailsStub();
            tabMain.SelectedTab = tabChonSanPham;
            btnResetCartForm_Click(this, EventArgs.Empty);
            LoadProductsSelectionGrid();
            LoadCurrentDetailsGrid();
        }

        // ==========================================
        // SỰ KIỆN TAB 2
        // ==========================================

        private void LoadProductsSelectionGrid()
        {
            dgvProductsSelection.Rows.Clear();
            foreach (var p in invoiceProducts)
            {
                dgvProductsSelection.Rows.Add(
                    p.MaSanPham,
                    p.TenSanPham,
                    p.SoLuongMua,
                    p.DaTra,
                    p.DonGia.ToString("N0") + " đ"
                );
            }
        }

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
                    item.DonGia.ToString("N0") + " đ",
                    item.TinhTrang,
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }
            bool isDoiHang = cboLoaiGiaoDich.Text == "Đổi hàng (1:1)";
            double displayAmt = isDoiHang ? 0 : total;

            lblTotalAmount.Text = isDoiHang
                ? $"TỔNG GIÁ TRỊ ĐỔI TRẢ (1:1): {total.ToString("N0")} đ | HOÀN TIỀN: 0 đ"
                : $"TỔNG TIỀN HOÀN TRẢ TẠM TÍNH: {total.ToString("N0")} đ";

            txtTongTienHoan.Text = displayAmt.ToString("N0") + " đ";
        }

        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dgvProductsSelection.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var p = invoiceProducts.FirstOrDefault(prod => prod.MaSanPham == id);
                if (p != null)
                {
                    txtSelMaSP.Text = p.MaSanPham;
                    txtSelTenSP.Text = p.TenSanPham;
                    txtSelDonGia.Text = p.DonGia.ToString();

                    var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                    if (existing != null)
                    {
                        txtSelSoLuong.Text = existing.SoLuong.ToString();
                        txtSelTinhTrang.Text = existing.TinhTrang;
                    }
                    else
                    {
                        txtSelSoLuong.Text = "1";
                        txtSelTinhTrang.Text = "";
                    }

                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
                }
            }
        }

        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dgvCurrentDetails.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
                if (item != null)
                {
                    txtSelMaSP.Text = item.MaSanPham;
                    txtSelTenSP.Text = item.TenSanPham;
                    txtSelSoLuong.Text = item.SoLuong.ToString();
                    txtSelDonGia.Text = item.DonGia.ToString();
                    txtSelTinhTrang.Text = item.TinhTrang;

                    txtSelSoLuong.Focus();
                    txtSelSoLuong.SelectAll();
                }
            }
        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần trả từ lưới hóa đơn gốc bên trái!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSelSoLuong.Focus();
                return;
            }

            var prod = invoiceProducts.FirstOrDefault(p => p.MaSanPham == id);
            if (prod != null)
            {
                int maxQty = prod.SoLuongMua - prod.DaTra;
                if (qty > maxQty)
                {
                    MessageBox.Show($"Số lượng trả vượt quá giới hạn! Tối đa có thể trả thêm: {maxQty}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSelSoLuong.Text = maxQty.ToString();
                    return;
                }
            }

            double.TryParse(txtSelDonGia.Text, out double price);

            var existing = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (existing != null)
            {
                existing.SoLuong = qty;
                existing.TinhTrang = txtSelTinhTrang.Text;
            }
            else
            {
                currentDetails.Add(new MockReturnDetail
                {
                    MaSanPham = id,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    DonGia = price,
                    TinhTrang = txtSelTinhTrang.Text
                });
            }

            LoadCurrentDetailsGrid();
            btnResetCartForm_Click(this, EventArgs.Empty);
        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            string id = txtSelMaSP.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần xóa khỏi phiếu trả!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = currentDetails.FirstOrDefault(d => d.MaSanPham == id);
            if (item != null)
            {
                currentDetails.Remove(item);
                LoadCurrentDetailsGrid();
                btnResetCartForm_Click(this, EventArgs.Empty);
            }
        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
        }

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            tabMain.SelectedTab = tabPhieuTra;
            PopulateDetailsGrid();
        }
    }
}
