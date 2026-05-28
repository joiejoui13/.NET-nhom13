using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;
using AssignmentApp.DAL.Core;

namespace AssignmentApp.GUI.Forms
{
    public partial class frmPOS : Form
    {
        private ProductRepository _productRepo;
        private POSRepository _posRepo;
        private List<Product> _products;
        private List<OrderDetail> _cart = new List<OrderDetail>();

        public frmPOS()
        {
            InitializeComponent();
            _productRepo = new ProductRepository();
            _posRepo = new POSRepository();
        }

        private async void frmPOS_Load(object sender, EventArgs e)
        {
            // Set text for create order button
            btnBackToReceipt.Text = "TẠO HÓA ĐƠN";
            guna2Button3.Text = "ĐÓNG";
            
            // hide unused buttons
            guna2Button4.Visible = false; // "Sửa"

            await LoadProductsAsync();
        }

        private async System.Threading.Tasks.Task LoadProductsAsync(string keyword = "")
        {
            var products = await _productRepo.GetAllAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.TenSanPham.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 || p.MaSanPham.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            _products = products.ToList();

            dgvProductsSelection.Rows.Clear();
            foreach (var p in _products)
            {
                dgvProductsSelection.Rows.Add(p.MaSanPham, p.TenSanPham, p.GiaBan.ToString("N0"));
            }
        }

        private void dgvProductsSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvProductsSelection.Rows[e.RowIndex];
                txtSelMaSP.Text = row.Cells[0].Value.ToString();
                txtSelTenSP.Text = row.Cells[1].Value.ToString();
                txtSelGiaNhap.Text = row.Cells[2].Value.ToString().Replace(",", ""); // actually GiaBan
                txtSelSoLuong.Text = "1";
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSelMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSelSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maSp = txtSelMaSP.Text;
            decimal price = decimal.Parse(txtSelGiaNhap.Text);

            var existing = _cart.FirstOrDefault(c => c.MaSanPham == maSp);
            if (existing != null)
            {
                existing.SoLuong += qty;
                existing.ThanhTien = existing.SoLuong * existing.DonGia;
            }
            else
            {
                _cart.Add(new OrderDetail
                {
                    MaSanPham = maSp,
                    TenSanPham = txtSelTenSP.Text,
                    SoLuong = qty,
                    DonGia = price,
                    ThanhTien = qty * price
                });
            }

            UpdateCartUI();
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCurrentDetails.SelectedRows.Count > 0)
            {
                var row = dgvCurrentDetails.SelectedRows[0];
                string maSp = row.Cells[0].Value.ToString();
                var item = _cart.FirstOrDefault(c => c.MaSanPham == maSp);
                if (item != null)
                {
                    _cart.Remove(item);
                    UpdateCartUI();
                }
            }
        }

        private void UpdateCartUI()
        {
            dgvCurrentDetails.Rows.Clear();
            decimal total = 0;
            foreach (var item in _cart)
            {
                dgvCurrentDetails.Rows.Add(item.MaSanPham, item.TenSanPham, item.SoLuong, item.DonGia.ToString("N0"), item.ThanhTien.ToString("N0"));
                total += item.ThanhTien;
            }
            lblTotalAmount.Text = $"Tổng tiền: {total:N0} VNĐ";
        }

        private async void btnBackToReceipt_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mặc định tạo đơn hàng cho khách lẻ (MaKhachHang = null) hoặc có thể mở popup để chọn khách
            var order = new Order
            {
                MaHoaDon = "", // Identity column in DB
                MaKhachHang = null, 
                MaNguoiDung = "1", // Hardcoded for now
                TongTien = (decimal)_cart.Sum(c => c.ThanhTien),
                GiamGia = 0,
                HinhThucThanhToan = "Tiền mặt",
                NgayTao = DateTime.Now
            };

            try
            {
                bool success = await _posRepo.SaveOrderTransactionAsync(order, _cart);
                if (success)
                {
                    MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnPOSSearch_Click(object sender, EventArgs e)
        {
            await LoadProductsAsync(txtProductSearch.Text);
        }

        private async void btnPOSRefresh_Click(object sender, EventArgs e)
        {
            txtProductSearch.Clear();
            await LoadProductsAsync();
        }
    }
}

