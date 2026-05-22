using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Session;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucStockIn : UserControl
    {
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly StockInRepository _stockInRepo = new StockInRepository();
        
        private List<Product> _allProducts = new List<Product>();
        private BindingList<StockInDetail> _detailsList = new BindingList<StockInDetail>();

        public ucStockIn()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvDetails.AutoGenerateColumns = false;
            colMaSanPham.DataPropertyName = "MaSanPham";
            colTenSanPham.DataPropertyName = "TenSanPham";
            colSoLuong.DataPropertyName = "SoLuong";
            colGiaNhap.DataPropertyName = "GiaNhap";
            colThanhTien.DataPropertyName = "ThanhTien";

            // Bind BindingList to grid
            dgvDetails.DataSource = _detailsList;
        }

        private async void ucStockIn_Load(object sender, EventArgs e)
        {
            ResetInvoice();
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var list = await _productRepo.GetAllAsync();
                _allProducts = list.ToList();
                
                cboSanPham.DataSource = null;
                cboSanPham.DataSource = _allProducts;
                cboSanPham.DisplayMember = "TenSanPham";
                cboSanPham.ValueMember = "MaSanPham";

                if (cboSanPham.Items.Count > 0)
                {
                    cboSanPham.SelectedIndex = 0;
                    UpdateProductPrice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProductPrice()
        {
            if (cboSanPham.SelectedValue == null) return;
            string prodId = cboSanPham.SelectedValue.ToString();
            var prod = _allProducts.FirstOrDefault(x => x.MaSanPham == prodId);
            if (prod != null)
            {
                txtGiaNhap.Text = prod.GiaNhap.ToString("0");
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProductPrice();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá nhập phải là số lớn hơn hoặc bằng 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return;
            }

            string prodId = cboSanPham.SelectedValue.ToString();
            var prod = _allProducts.FirstOrDefault(x => x.MaSanPham == prodId);
            if (prod == null) return;

            // Check if product already exists in current details list
            var existing = _detailsList.FirstOrDefault(x => x.MaSanPham == prodId);
            if (existing != null)
            {
                existing.SoLuong += qty;
                existing.GiaNhap = price; // Update to latest price
            }
            else
            {
                var detail = new StockInDetail
                {
                    MaSanPham = prodId,
                    TenSanPham = prod.TenSanPham,
                    SoLuong = qty,
                    GiaNhap = price
                };
                _detailsList.Add(detail);
            }

            // Refresh grid display
            dgvDetails.Refresh();
            _detailsList.ResetBindings();

            UpdateTotal();

            // Clear item input fields for next input
            txtSoLuong.Text = string.Empty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Không hỗ trợ chỉnh sửa phiếu nhập kho đã lập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi danh sách chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có muốn xóa sản phẩm đang chọn khỏi chi tiết không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvDetails.SelectedRows)
                {
                    if (row.DataBoundItem is StockInDetail detail)
                    {
                        _detailsList.Remove(detail);
                    }
                }

                _detailsList.ResetBindings();
                UpdateTotal();
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieuNhap.Text.Trim();
            if (string.IsNullOrWhiteSpace(maPhieu))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaPhieuNhap.Focus();
                return;
            }

            if (_detailsList.Count == 0)
            {
                MessageBox.Show("Chi tiết phiếu nhập trống! Vui lòng thêm ít nhất một sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Check code uniqueness
                var existing = await _stockInRepo.GetByIdAsync(maPhieu);
                if (existing != null)
                {
                    MessageBox.Show("Mã phiếu nhập này đã tồn tại trong hệ thống! Vui lòng đổi mã khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaPhieuNhap.Focus();
                    return;
                }

                var master = new StockIn
                {
                    MaPhieuNhap = maPhieu,
                    MaNguoiDung = UserSession.CurrentUser?.MaNguoiDung,
                    NgayNhap = dtNgayNhap.Value,
                    TongTien = _detailsList.Sum(x => x.ThanhTien)
                };

                int res = await _stockInRepo.AddAsync(master, _detailsList.ToList());
                if (res > 0)
                {
                    MessageBox.Show("Lưu hóa đơn nhập kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInvoice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu hóa đơn nhập kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có thực sự muốn hủy phiếu hiện tại và làm mới không?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                ResetInvoice();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvDetails.DataSource = _detailsList;
            }
            else
            {
                var filtered = _detailsList.Where(x =>
                    x.MaSanPham.ToLower().Contains(keyword) ||
                    x.TenSanPham.ToLower().Contains(keyword)
                ).ToList();
                dgvDetails.DataSource = new BindingList<StockInDetail>(filtered);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            dgvDetails.DataSource = _detailsList;
        }

        private void ResetInvoice()
        {
            txtMaPhieuNhap.Text = "PN_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            dtNgayNhap.Value = DateTime.Now;
            txtSoLuong.Text = string.Empty;
            if (cboSanPham.Items.Count > 0)
            {
                cboSanPham.SelectedIndex = 0;
                UpdateProductPrice();
            }
            _detailsList.Clear();
            _detailsList.ResetBindings();
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _detailsList.Sum(x => x.ThanhTien);
            lblTongTien.Text = total.ToString("N0") + " VNĐ";
        }
    }
}
