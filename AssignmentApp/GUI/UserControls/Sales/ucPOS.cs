using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Session;
using Guna.UI2.WinForms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucPOS : UserControl
    {
        private readonly POSRepository _posRepo = new POSRepository();
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly CustomerRepository _customerRepo = new CustomerRepository();
        private readonly PromotionRepository _promoRepo = new PromotionRepository();

        private List<Product> _allProducts = new List<Product>();
        private List<Customer> _allCustomers = new List<Customer>();
        private List<Promotion> _allPromotions = new List<Promotion>();

        private BindingList<CartItem> _cart = new BindingList<CartItem>();

        public ucPOS()
        {
            InitializeComponent();
            SetupCartGrid();
        }

        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            colCartMaSP.DataPropertyName = "MaSanPham";
            colCartTenSP.DataPropertyName = "TenSanPham";
            colCartSoLuong.DataPropertyName = "SoLuong";
            colCartDonGia.DataPropertyName = "DonGia";
            colCartThanhTien.DataPropertyName = "ThanhTien";

            // Make columns format
            colCartDonGia.DefaultCellStyle.Format = "N0";
            colCartThanhTien.DefaultCellStyle.Format = "N0";

            dgvCart.DataSource = _cart;
        }

        private async void ucPOS_Load(object sender, EventArgs e)
        {
            await LoadInitialDataAsync();
            cboHinhThucThanhToan.SelectedIndex = 0; // Default: Tiền mặt
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                // 1. Load Customers
                var customersList = await _customerRepo.GetAllAsync();
                _allCustomers = customersList.ToList();
                var comboCustomers = new List<Customer>
                {
                    new Customer { MaKhachHang = "", TenKhachHang = "Khách vãng lai" }
                };
                comboCustomers.AddRange(_allCustomers);
                cboKhachHang.DataSource = comboCustomers;
                cboKhachHang.DisplayMember = "TenKhachHang";
                cboKhachHang.ValueMember = "MaKhachHang";
                cboKhachHang.SelectedIndex = 0;

                // 2. Load Promotions
                var promoList = await _promoRepo.GetAllAsync();
                _allPromotions = promoList.Where(x => x.TrangThai == "Hoạt động").ToList();
                var comboPromos = new List<Promotion>
                {
                    new Promotion { MaKhuyenMai = "", TenKhuyenMai = "Không áp dụng", PhanTramGiamGia = 0 }
                };
                comboPromos.AddRange(_allPromotions);
                cboKhuyenMai.DataSource = comboPromos;
                cboKhuyenMai.DisplayMember = "TenKhuyenMai";
                cboKhuyenMai.ValueMember = "MaKhuyenMai";
                cboKhuyenMai.SelectedIndex = 0;

                // 3. Load Products
                await RefreshProductListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo màn hình POS: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RefreshProductListAsync()
        {
            try
            {
                var prodList = await _productRepo.GetAllAsync();
                _allProducts = prodList.ToList();
                RenderProductCards(_allProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderProductCards(IEnumerable<Product> products)
        {
            flowProducts.Controls.Clear();
            foreach (var p in products)
            {
                // Card container
                Guna2Panel card = new Guna2Panel
                {
                    Size = new Size(165, 150),
                    BorderRadius = 8,
                    BorderColor = Color.FromArgb(229, 231, 235),
                    BorderThickness = 1,
                    FillColor = Color.White,
                    Margin = new Padding(8),
                    Cursor = Cursors.Hand
                };

                // Product name label
                Label lblName = new Label
                {
                    Text = p.TenSanPham,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(10, 10),
                    Size = new Size(145, 45),
                    TextAlign = ContentAlignment.TopCenter
                };

                // Product price label
                Label lblPrice = new Label
                {
                    Text = p.GiaBan.ToString("N0") + " đ",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(22, 163, 74),
                    Location = new Point(10, 60),
                    Size = new Size(145, 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Product stock quantity label
                Label lblStock = new Label
                {
                    Text = $"Kho: {p.SoLuongTon}",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(10, 90),
                    Size = new Size(145, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                card.Controls.Add(lblName);
                card.Controls.Add(lblPrice);
                card.Controls.Add(lblStock);

                // Setup Click events
                Action<object, EventArgs> addAction = (s, e) =>
                {
                    if (p.SoLuongTon <= 0)
                    {
                        MessageBox.Show($"Sản phẩm '{p.TenSanPham}' đã hết hàng trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    AddToCart(p);
                };

                card.Click += new EventHandler(addAction);
                lblName.Click += new EventHandler(addAction);
                lblPrice.Click += new EventHandler(addAction);
                lblStock.Click += new EventHandler(addAction);

                // Hover transitions
                card.MouseEnter += (s, e) => {
                    card.BorderColor = Color.FromArgb(0, 126, 249);
                    card.FillColor = Color.FromArgb(249, 250, 251);
                };
                card.MouseLeave += (s, e) => {
                    card.BorderColor = Color.FromArgb(229, 231, 235);
                    card.FillColor = Color.White;
                };

                flowProducts.Controls.Add(card);
            }
        }

        private void AddToCart(Product p)
        {
            var existing = _cart.FirstOrDefault(x => x.MaSanPham == p.MaSanPham);
            if (existing != null)
            {
                if (existing.SoLuong >= p.SoLuongTon)
                {
                    MessageBox.Show($"Không thể thêm nhiều hơn! Số lượng tồn kho tối đa là {p.SoLuongTon}.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing.SoLuong++;
                _cart.ResetBindings();
            }
            else
            {
                _cart.Add(new CartItem
                {
                    MaSanPham = p.MaSanPham,
                    TenSanPham = p.TenSanPham,
                    SoLuong = 1,
                    DonGia = p.GiaBan
                });
            }
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            decimal subtotal = _cart.Sum(x => x.ThanhTien);

            Promotion selectedPromo = cboKhuyenMai.SelectedItem as Promotion;
            int discountPercent = selectedPromo?.PhanTramGiamGia ?? 0;
            decimal discount = subtotal * discountPercent / 100;

            decimal total = subtotal - discount;

            lblSubtotal.Text = $"Tạm tính: {subtotal:N0} đ";
            lblDiscount.Text = $"Giảm giá ({discountPercent}%): {discount:N0} đ";
            lblTotal.Text = $"TỔNG CỘNG: {total:N0} đ";
        }

        private void cboKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecalculateTotals();
        }

        private void dgvCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvCart.Columns["colCartSoLuong"].Index)
            {
                var row = dgvCart.Rows[e.RowIndex];
                string maSP = row.Cells["colCartMaSP"].Value?.ToString() ?? string.Empty;
                var item = _cart.FirstOrDefault(x => x.MaSanPham == maSP);
                if (item != null)
                {
                    var product = _allProducts.FirstOrDefault(x => x.MaSanPham == maSP);
                    if (product != null)
                    {
                        if (item.SoLuong > product.SoLuongTon)
                        {
                            MessageBox.Show($"Số lượng yêu cầu vượt quá tồn kho hiện tại ({product.SoLuongTon})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            item.SoLuong = product.SoLuongTon;
                        }
                    }

                    if (item.SoLuong <= 0)
                    {
                        _cart.Remove(item);
                    }
                }
                _cart.ResetBindings();
                RecalculateTotals();
            }
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional double click or specific click deletion
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0) return;

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng hiện tại?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _cart.Clear();
                RecalculateTotals();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _cart.Clear();
            cboKhachHang.SelectedIndex = 0;
            cboKhuyenMai.SelectedIndex = 0;
            cboHinhThucThanhToan.SelectedIndex = 0;
            txtSearch.Text = string.Empty;
            RecalculateTotals();
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống! Vui lòng chọn sản phẩm trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Xác nhận thanh toán đơn hàng này?", "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string orderCode = "HD" + DateTime.Now.ToString("ddMMyyHHmmss");
                string customerCode = cboKhachHang.SelectedValue?.ToString() ?? string.Empty;
                string promoCode = cboKhuyenMai.SelectedValue?.ToString() ?? string.Empty;

                decimal subtotal = _cart.Sum(x => x.ThanhTien);
                Promotion selectedPromo = cboKhuyenMai.SelectedItem as Promotion;
                int discountPercent = selectedPromo?.PhanTramGiamGia ?? 0;
                decimal discount = subtotal * discountPercent / 100;
                decimal total = subtotal - discount;

                Order order = new Order
                {
                    MaHoaDon = orderCode,
                    MaKhachHang = string.IsNullOrEmpty(customerCode) ? null : customerCode,
                    MaNguoiDung = UserSession.CurrentUser?.MaNguoiDung ?? "ADMIN",
                    MaKhuyenMai = string.IsNullOrEmpty(promoCode) ? null : promoCode,
                    TongTien = total,
                    GiamGia = discount,
                    MaGiaoHang = null,
                    HinhThucThanhToan = cboHinhThucThanhToan.SelectedItem?.ToString() ?? "Tiền mặt",
                    NgayTao = DateTime.Now
                };

                List<OrderDetail> details = new List<OrderDetail>();
                foreach (var item in _cart)
                {
                    details.Add(new OrderDetail
                    {
                        MaChiTiet = "CT" + Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
                        MaHoaDon = orderCode,
                        MaSanPham = item.MaSanPham,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia,
                        ThanhTien = item.ThanhTien
                    });
                }

                bool success = await _posRepo.SaveOrderTransactionAsync(order, details);
                if (success)
                {
                    MessageBox.Show($"Thanh toán hóa đơn '{orderCode}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _cart.Clear();
                    RecalculateTotals();
                    await RefreshProductListAsync();
                }
                else
                {
                    MessageBox.Show("Thanh toán đơn hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void PerformSearch()
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                RenderProductCards(_allProducts);
                return;
            }

            var filtered = _allProducts.Where(p => 
                p.MaSanPham.ToLower().Contains(keyword) || 
                p.TenSanPham.ToLower().Contains(keyword)
            ).ToList();

            RenderProductCards(filtered);
        }

        // Inner class for the shopping cart items representation
        private class CartItem
        {
            public string MaSanPham { get; set; } = string.Empty;
            public string TenSanPham { get; set; } = string.Empty;
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien => SoLuong * DonGia;
        }
    }
}
