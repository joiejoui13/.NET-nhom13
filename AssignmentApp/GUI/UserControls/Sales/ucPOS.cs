using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucPOS : UserControl
    {
        public class MockProduct
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public string DanhMuc { get; set; } = "";
            public double DonGia { get; set; }
            public int TonKho { get; set; }
            public string TrangThai { get; set; } = "Đang bán";
            public string MoTa { get; set; } = "";
        }

        public class MockCustomer
        {
            public int MaKhachHang { get; set; }
            public string TenKhachHang { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
        }

        public class MockPromotion
        {
            public int MaKhuyenMai { get; set; }
            public string TenKhuyenMai { get; set; } = "";
            public double PhanTramGiam { get; set; }
        }

        public class CartItem
        {
            public int MaSanPham { get; set; }
            public string TenSanPham { get; set; } = "";
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien => SoLuong * DonGia;
        }

        private List<MockProduct> products = new List<MockProduct>();
        private List<MockCustomer> customers = new List<MockCustomer>();
        private List<MockPromotion> promotions = new List<MockPromotion>();
        private List<CartItem> cart = new List<CartItem>();
        private MockProduct? selectedProduct = null;

        public ucPOS()
        {
            InitializeComponent();
        }

        private void ucPOS_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            BindDropdowns();
            LoadProductsGrid();
            
            // Set Default Selected Index for LoaiGiaoDich
            cboLoaiGiaoDich.SelectedIndex = 0; // "Đơn bán hàng (Hóa đơn)"
            cboHinhThucThanhToan.SelectedIndex = 0; // "Tiền mặt"
            cboKhachHang.SelectedIndex = 0;
            cboKhuyenMai.SelectedIndex = 0;

            ResetCart();
        }

        private void InitializeMockData()
        {
            // Seed Products
            products.Add(new MockProduct { MaSanPham = 1, TenSanPham = "Máy tính Casio FX-580VN X", DanhMuc = "Thiết bị văn phòng", DonGia = 680000, TonKho = 120, MoTa = "Máy tính khoa học thông minh thế hệ mới cho học sinh." });
            products.Add(new MockProduct { MaSanPham = 2, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", DanhMuc = "Sách & Vở", DonGia = 9000, TonKho = 850, MoTa = "Giấy viết chất lượng cao, định lượng 70gsm chống nhòe." });
            products.Add(new MockProduct { MaSanPham = 3, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", DanhMuc = "Dụng cụ học tập", DonGia = 5000, TonKho = 1500, MoTa = "Bút bi ngòi 0.5mm êm trơn, mực đậm màu xanh thông dụng." });
            products.Add(new MockProduct { MaSanPham = 4, TenSanPham = "Giấy in Double A A4 70gsm", DanhMuc = "Thiết bị văn phòng", DonGia = 80000, TonKho = 200, MoTa = "Giấy in văn phòng cao cấp, độ trắng sáng vượt trội." });

            // Seed Customers
            customers.Add(new MockCustomer { MaKhachHang = 1, TenKhachHang = "Khách vãng lai", SoDienThoai = "" });
            customers.Add(new MockCustomer { MaKhachHang = 2, TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0987654321" });
            customers.Add(new MockCustomer { MaKhachHang = 3, TenKhachHang = "Trần Thị B", SoDienThoai = "0912345678" });

            // Seed Promotions
            promotions.Add(new MockPromotion { MaKhuyenMai = 0, TenKhuyenMai = "Không áp dụng", PhanTramGiam = 0 });
            promotions.Add(new MockPromotion { MaKhuyenMai = 1, TenKhuyenMai = "Giảm giá khai trương - 10%", PhanTramGiam = 10 });
            promotions.Add(new MockPromotion { MaKhuyenMai = 2, TenKhuyenMai = "Chào hè năng động - 5%", PhanTramGiam = 5 });
        }

        // Helper to map promotion name property for bindings since property TenPromotion isn't in definition
        private class PromotionDisplayHelper
        {
            public int MaKhuyenMai { get; set; }
            public string TenKhuyenMai { get; set; } = "";
            public double PhanTramGiam { get; set; }
        }

        private void BindDropdowns()
        {
            cboKhachHang.DataSource = customers.Select(c => new {
                Id = c.MaKhachHang,
                DisplayText = string.IsNullOrEmpty(c.SoDienThoai) ? c.TenKhachHang : $"{c.TenKhachHang} ({c.SoDienThoai})"
            }).ToList();
            cboKhachHang.DisplayMember = "DisplayText";
            cboKhachHang.ValueMember = "Id";

            cboKhuyenMai.DataSource = promotions.Select(p => new {
                Id = p.MaKhuyenMai,
                DisplayText = p.PhanTramGiam > 0 ? $"{p.TenKhuyenMai} ({p.PhanTramGiam}%)" : p.TenKhuyenMai
            }).ToList();
            cboKhuyenMai.DisplayMember = "DisplayText";
            cboKhuyenMai.ValueMember = "Id";
        }

        private void LoadProductsGrid(List<MockProduct>? dataSource = null)
        {
            dgvProducts.Rows.Clear();
            var list = dataSource ?? products;
            foreach (var prod in list)
            {
                dgvProducts.Rows.Add(
                    prod.MaSanPham,
                    prod.TenSanPham,
                    prod.DanhMuc,
                    prod.DonGia.ToString("N0") + " đ",
                    prod.TonKho.ToString("N0"),
                    prod.TrangThai
                );
            }
        }

        private void cboLoaiGiaoDich_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isSalesOrder = cboLoaiGiaoDich.SelectedIndex == 0;
            if (isSalesOrder)
            {
                btnPay.Text = "THANH TOÁN";
                btnPay.FillColor = Color.FromArgb(22, 163, 74); // Green for Sales Pay
                lblTotal.ForeColor = Color.FromArgb(22, 163, 74);
                
                lblKhuyenMai.Enabled = true;
                cboKhuyenMai.Enabled = true;
                lblHinhThucThanhToan.Enabled = true;
                cboHinhThucThanhToan.Enabled = true;
            }
            else
            {
                btnPay.Text = "XÁC NHẬN TRẢ HÀNG";
                btnPay.FillColor = Color.FromArgb(244, 67, 54); // Red for Return Refund
                lblTotal.ForeColor = Color.FromArgb(244, 67, 54);
                
                // Disable Promotions/Payments since returns refund money back directly
                lblKhuyenMai.Enabled = false;
                cboKhuyenMai.SelectedIndex = 0;
                cboKhuyenMai.Enabled = false;
                lblHinhThucThanhToan.Enabled = false;
                cboHinhThucThanhToan.Enabled = false;
            }

            RecalculateTotals();
        }

        private void cboKhuyenMai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            RecalculateTotals();
        }

        private void LoadProductDetail(MockProduct prod)
        {
            selectedProduct = prod;
            lblProductDetailName.Text = prod.TenSanPham.ToUpper();
            lblProductDetailPrice.Text = $"Giá bán: {prod.DonGia.ToString("N0")} đ";
            lblProductDetailStock.Text = $"Tình trạng: Còn hàng (Tồn kho: {prod.TonKho})";
            lblProductDetailDesc.Text = $"Mã SP: {prod.MaSanPham} | Danh mục: {prod.DanhMuc}\n{prod.MoTa}";
            tabPOSContainer.SelectedIndex = 1; // Switch to detail tab
        }

        private void dgvProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int prodId = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells[0].Value);
                var prod = products.FirstOrDefault(p => p.MaSanPham == prodId);
                if (prod != null)
                {
                    LoadProductDetail(prod);
                }
            }
        }

        private void btnDetailAddToCart_Click(object? sender, EventArgs e)
        {
            if (selectedProduct == null) return;

            if (selectedProduct.TonKho <= 0 && cboLoaiGiaoDich.SelectedIndex == 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng trong kho, không thể thêm vào đơn bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if already in cart
            var item = cart.FirstOrDefault(i => i.MaSanPham == selectedProduct.MaSanPham);
            if (item != null)
            {
                item.SoLuong++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    MaSanPham = selectedProduct.MaSanPham,
                    TenSanPham = selectedProduct.TenSanPham,
                    SoLuong = 1,
                    DonGia = selectedProduct.DonGia
                });
            }

            tabPOSContainer.SelectedIndex = 0; // switch back to list
            LoadCartGrid();
            RecalculateTotals();
        }

        private void LoadCartGrid()
        {
            dgvCart.Rows.Clear();
            foreach (var item in cart)
            {
                dgvCart.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ",
                    "X"
                );
            }
        }

        private void dgvCart_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2) // Qty column changed
            {
                int prodId = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells[0].Value);
                var item = cart.FirstOrDefault(i => i.MaSanPham == prodId);
                if (item != null)
                {
                    string valStr = dgvCart.Rows[e.RowIndex].Cells[2].Value.ToString() ?? "1";
                    if (int.TryParse(valStr, out int qty) && qty > 0)
                    {
                        var prod = products.FirstOrDefault(p => p.MaSanPham == prodId);
                        if (prod != null && qty > prod.TonKho && cboLoaiGiaoDich.SelectedIndex == 0)
                        {
                            MessageBox.Show($"Số lượng yêu cầu vượt quá tồn kho hiện tại ({prod.TonKho})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            qty = prod.TonKho;
                        }
                        item.SoLuong = qty;
                    }
                    else
                    {
                        item.SoLuong = 1;
                    }
                }
                LoadCartGrid();
                RecalculateTotals();
            }
        }

        private void dgvCart_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 5) // Xóa button column
            {
                int prodId = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells[0].Value);
                var item = cart.FirstOrDefault(i => i.MaSanPham == prodId);
                if (item != null)
                {
                    cart.Remove(item);
                    LoadCartGrid();
                    RecalculateTotals();
                }
            }
        }

        private void RecalculateTotals()
        {
            double subtotal = cart.Sum(i => i.ThanhTien);
            double discount = 0;

            if (cboLoaiGiaoDich.SelectedIndex == 0) // Only apply discount for Sales
            {
                int promoId = Convert.ToInt32(cboKhuyenMai.SelectedValue);
                var promo = promotions.FirstOrDefault(p => p.MaKhuyenMai == promoId);
                if (promo != null)
                {
                    discount = subtotal * (promo.PhanTramGiam / 100.0);
                }
            }

            double total = subtotal - discount;

            lblSubtotal.Text = $"Tạm tính: {subtotal.ToString("N0")} đ";
            lblDiscount.Text = $"Giảm giá: {discount.ToString("N0")} đ";
            
            if (cboLoaiGiaoDich.SelectedIndex == 0)
            {
                lblTotal.Text = $"TỔNG: {total.ToString("N0")} đ";
            }
            else
            {
                lblTotal.Text = $"TIỀN HOÀN: {subtotal.ToString("N0")} đ";
            }
        }

        private void ResetCart()
        {
            cart.Clear();
            LoadCartGrid();
            RecalculateTotals();
            cboKhachHang.SelectedIndex = 0;
            cboKhuyenMai.SelectedIndex = 0;
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            ResetCart();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có muốn hủy bỏ toàn bộ giỏ hàng hiện tại?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                ResetCart();
            }
        }

        private void btnPay_Click(object? sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng rỗng! Vui lòng chọn sản phẩm trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isSales = cboLoaiGiaoDich.SelectedIndex == 0;
            string customerName = cboKhachHang.Text;

            if (isSales)
            {
                double total = cart.Sum(i => i.ThanhTien) - (cart.Sum(i => i.ThanhTien) * (promotions.FirstOrDefault(p => p.MaKhuyenMai == Convert.ToInt32(cboKhuyenMai.SelectedValue))?.PhanTramGiam ?? 0) / 100.0);
                
                // Deduct stock
                foreach (var item in cart)
                {
                    var prod = products.FirstOrDefault(p => p.MaSanPham == item.MaSanPham);
                    if (prod != null)
                    {
                        prod.TonKho -= item.SoLuong;
                    }
                }

                MessageBox.Show($"Thanh toán hóa đơn thành công!\n" +
                                $"Khách hàng: {customerName}\n" +
                                $"Tổng tiền: {total.ToString("N0")} đ\n" +
                                $"[TỒN KHO] Đã cập nhật trừ số lượng tương ứng trong kho hàng!", 
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                double refund = cart.Sum(i => i.ThanhTien);

                // Add back stock
                foreach (var item in cart)
                {
                    var prod = products.FirstOrDefault(p => p.MaSanPham == item.MaSanPham);
                    if (prod != null)
                    {
                        prod.TonKho += item.SoLuong;
                    }
                }

                MessageBox.Show($"Tạo phiếu trả hàng & Hoàn tiền thành công!\n" +
                                $"Khách hàng trả: {customerName}\n" +
                                $"Tổng tiền hoàn trả: {refund.ToString("N0")} đ\n" +
                                $"[TỒN KHO] Đã cộng trả lại tồn kho của các mặt hàng!", 
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ResetCart();
            LoadProductsGrid();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadProductsGrid();
                return;
            }

            var filtered = products.Where(p => 
                p.MaSanPham.ToString().Contains(keyword) || 
                p.TenSanPham.ToLower().Contains(keyword) || 
                p.DanhMuc.ToLower().Contains(keyword)
            ).ToList();

            LoadProductsGrid(filtered);
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
