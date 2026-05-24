using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        // Mock data structures
        public class MockOrder
        {
            public int MaHoaDon { get; set; }
            public int MaKhachHang { get; set; }
            public string? TenKhachHang { get; set; }
            public string? TenNguoiDung { get; set; }
            public double TongTien { get; set; }
            public double GiamGia { get; set; }
            public string? HinhThucThanhToan { get; set; }
            public string? TrangThai { get; set; }
            public string? LoaiHoaDon { get; set; }
            public string? LyDoHuy { get; set; }
            public DateTime NgayTao { get; set; }
            public List<MockOrderDetail> Details { get; set; } = new List<MockOrderDetail>();
        }

        public class MockOrderDetail
        {
            public int MaSanPham { get; set; }
            public string? TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien => SoLuong * DonGia;
        }

        private List<MockOrder> mockOrders = new List<MockOrder>();
        private MockOrder selectedOrder = null;
        private bool isEditing = false;

        public ucOrderManagement()
        {
            InitializeComponent();
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
        }

        private void ucOrderManagement_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            LoadOrdersGrid();
            cboFilterStatus.SelectedIndex = 0; // "Tất cả"
            SetEditState(false);
            
            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
        }

        private void InitializeMockData()
        {
            if (mockOrders.Count > 0) return;

            // Order 1: Completed Sales Order
            var order1 = new MockOrder
            {
                MaHoaDon = 1,
                MaKhachHang = 3,
                TenKhachHang = "Nguyễn Văn Học Sinh",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 694000,
                GiamGia = 0,
                HinhThucThanhToan = "Tiền mặt",
                TrangThai = "Đã hoàn thành",
                LoaiHoaDon = "Đơn bán hàng",
                LyDoHuy = "",
                NgayTao = DateTime.Now.AddDays(-2)
            };
            order1.Details.Add(new MockOrderDetail { MaSanPham = 12, TenSanPham = "Máy tính Casio FX-580VN X", SoLuong = 1, DonGia = 680000 });
            order1.Details.Add(new MockOrderDetail { MaSanPham = 4, TenSanPham = "Vở kẻ ngang Hồng Hà 72 trang", SoLuong = 1, DonGia = 9000 });
            order1.Details.Add(new MockOrderDetail { MaSanPham = 1, TenSanPham = "Bút bi Thiên Long TL-027 Xanh", SoLuong = 1, DonGia = 5000 });

            // Order 2: Processing Order (Purchase Order)
            var order2 = new MockOrder
            {
                MaHoaDon = 2,
                MaKhachHang = 2,
                TenKhachHang = "Công ty CP ABC",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 1360000,
                GiamGia = 240000,
                HinhThucThanhToan = "Chuyển khoản",
                TrangThai = "Chờ xử lý",
                LoaiHoaDon = "Đơn đặt hàng",
                LyDoHuy = "",
                NgayTao = DateTime.Now.AddHours(-4)
            };
            order2.Details.Add(new MockOrderDetail { MaSanPham = 7, TenSanPham = "Giấy in Double A A4 70gsm", SoLuong = 20, DonGia = 80000 });

            // Order 3: Canceled Order
            var order3 = new MockOrder
            {
                MaHoaDon = 3,
                MaKhachHang = 1,
                TenKhachHang = "Trường THPT X",
                TenNguoiDung = "Thu Ngân 1",
                TongTien = 45000,
                GiamGia = 0,
                HinhThucThanhToan = "Tiền mặt",
                TrangThai = "Đã huỷ",
                LoaiHoaDon = "Đơn bán hàng",
                LyDoHuy = "Khách thấy đắt nên không mua nữa",
                NgayTao = DateTime.Now.AddDays(-1)
            };
            order3.Details.Add(new MockOrderDetail { MaSanPham = 3, TenSanPham = "Bút máy Hồng Hà Nét Hoa", SoLuong = 1, DonGia = 45000 });

            mockOrders.Add(order1);
            mockOrders.Add(order2);
            mockOrders.Add(order3);
        }

        private void LoadOrdersGrid(List<MockOrder> dataSource = null)
        {
            dgvOrders.Rows.Clear();
            var list = dataSource ?? mockOrders;
            foreach (var order in list)
            {
                dgvOrders.Rows.Add(
                    order.MaHoaDon,
                    order.TenKhachHang,
                    order.TenNguoiDung,
                    order.TongTien.ToString("N0") + " đ",
                    order.GiamGia.ToString("N0") + " đ",
                    order.HinhThucThanhToan,
                    order.TrangThai,
                    order.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    order.LoaiHoaDon,
                    order.LyDoHuy
                );
            }
        }

        private void SelectOrderRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvOrders.Rows.Count) return;

            dgvOrders.ClearSelection();
            dgvOrders.Rows[rowIndex].Selected = true;

            int orderId = Convert.ToInt32(dgvOrders.Rows[rowIndex].Cells[0].Value);
            selectedOrder = mockOrders.FirstOrDefault(o => o.MaHoaDon == orderId);

            if (selectedOrder != null)
            {
                PopulateOrderDetails(selectedOrder);
            }
        }

        private void PopulateOrderDetails(MockOrder order)
        {
            txtMaHoaDon.Text = order.MaHoaDon.ToString();
            cboLoaiHoaDon.Text = order.LoaiHoaDon;
            txtMaKhachHang.Text = order.TenKhachHang;
            txtTenNguoiDung.Text = order.TenNguoiDung;
            txtTongTien.Text = order.TongTien.ToString("N0") + " đ";
            txtGiamGia.Text = order.GiamGia.ToString("N0") + " đ";
            txtHinhThucThanhToan.Text = order.HinhThucThanhToan;
            txtNgayTao.Text = order.NgayTao.ToString("dd/MM/yyyy HH:mm");
            cboTrangThai.Text = order.TrangThai;
            txtLyDoHuy.Text = order.LyDoHuy;

            // Load Details Grid
            dgvOrderDetails.Rows.Clear();
            foreach (var item in order.Details)
            {
                dgvOrderDetails.Rows.Add(
                    item.MaSanPham,
                    item.TenSanPham,
                    item.SoLuong,
                    item.DonGia.ToString("N0") + " đ",
                    item.ThanhTien.ToString("N0") + " đ"
                );
            }

            // Update Convert to sales button visibility/enabled state
            UpdateConvertToSalesState();
        }

        private void UpdateConvertToSalesState()
        {
            if (selectedOrder != null && !isEditing)
            {
                bool isPreOrder = selectedOrder.LoaiHoaDon == "Đơn đặt hàng";
                bool isCanceledOrCompleted = selectedOrder.TrangThai == "Đã huỷ" || selectedOrder.TrangThai == "Đã hoàn thành";
                
                btnConvertToInvoice.Enabled = isPreOrder && !isCanceledOrCompleted;
                btnConvertToInvoice.Visible = isPreOrder;
            }
            else
            {
                btnConvertToInvoice.Enabled = false;
                btnConvertToInvoice.Visible = isEditing ? false : true;
            }
        }

        private void SetEditState(bool editing)
        {
            isEditing = editing;

            // Enable/disable combo boxes
            cboLoaiHoaDon.Enabled = editing;
            cboTrangThai.Enabled = editing;

            // LyDoHuy is only editable in Edit mode when TrangThai is "Đã huỷ"
            txtLyDoHuy.ReadOnly = !editing || cboTrangThai.Text != "Đã huỷ";

            // Button visibility & states
            btnSave.Visible = editing;
            btnCancel.Visible = editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;

            UpdateConvertToSalesState();
        }

        private void cboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isEditing)
            {
                bool isCanceled = cboTrangThai.Text == "Đã huỷ";
                txtLyDoHuy.ReadOnly = !isCanceled;
                if (!isCanceled)
                {
                    txtLyDoHuy.Text = "";
                }
                else
                {
                    txtLyDoHuy.Focus();
                }
            }
        }

        private void dgvOrders_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isEditing)
            {
                SelectOrderRow(e.RowIndex);
            }
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            string statusFilter = cboFilterStatus.Text;

            var filtered = mockOrders.Where(o =>
            {
                bool matchesKeyword = string.IsNullOrEmpty(keyword) || 
                                     o.MaHoaDon.ToString().Contains(keyword) ||
                                     o.TenKhachHang.ToLower().Contains(keyword) ||
                                     o.TenNguoiDung.ToLower().Contains(keyword);

                bool matchesStatus = statusFilter == "Tất cả" || o.TrangThai == statusFilter;

                return matchesKeyword && matchesStatus;
            }).ToList();

            LoadOrdersGrid(filtered);

            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
            else
            {
                selectedOrder = null;
                ClearInputs();
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboFilterStatus.SelectedIndex = 0;
            LoadOrdersGrid();
            SetEditState(false);
            if (dgvOrders.Rows.Count > 0)
            {
                SelectOrderRow(0);
            }
        }

        private void ClearInputs()
        {
            txtMaHoaDon.Text = "";
            txtMaKhachHang.Text = "";
            txtTenNguoiDung.Text = "";
            txtTongTien.Text = "";
            txtGiamGia.Text = "";
            txtHinhThucThanhToan.Text = "";
            txtNgayTao.Text = "";
            cboLoaiHoaDon.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            txtLyDoHuy.Text = "";
            dgvOrderDetails.Rows.Clear();
            btnConvertToInvoice.Enabled = false;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Chức năng tạo mới đơn bán hàng thực hiện tại màn hình Bán hàng (POS).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã hoàn thành" || selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Không thể chỉnh sửa đơn hàng đã hoàn thành hoặc đã huỷ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetEditState(true);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã huỷ")
            {
                MessageBox.Show("Đơn hàng này đã được hủy trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedOrder.TrangThai == "Đã hoàn thành")
            {
                MessageBox.Show("Không thể hủy đơn hàng đã hoàn thành giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng này không?", "Xác nhận hủy đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                SetEditState(true);
                cboTrangThai.Text = "Đã huỷ";
                txtLyDoHuy.Focus();
                MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng và nhấn 'LƯU THAY ĐỔI' để xác nhận!", "Nhập lý do hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null) return;

            string selectedStatus = cboTrangThai.Text;
            string selectedType = cboLoaiHoaDon.Text;
            string cancelReason = txtLyDoHuy.Text.Trim();

            if (selectedStatus == "Đã huỷ" && string.IsNullOrEmpty(cancelReason))
            {
                MessageBox.Show("Vui lòng nhập lý do hủy đơn hàng!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLyDoHuy.Focus();
                return;
            }

            // Save back to mock data source
            selectedOrder.TrangThai = selectedStatus;
            selectedOrder.LoaiHoaDon = selectedType;
            selectedOrder.LyDoHuy = selectedStatus == "Đã huỷ" ? cancelReason : "";

            // If status is updated, show mock inventory update feedback
            string inventoryMsg = "";
            if (selectedStatus == "Đã huỷ")
            {
                inventoryMsg = "\n[TỒN KHO] Tồn kho của các sản phẩm trong đơn đã được hoàn tác cộng trả lại kho hàng!";
            }
            else if (selectedStatus == "Đã hoàn thành" || selectedStatus == "Đang giao hàng")
            {
                inventoryMsg = "\n[TỒN KHO] Số lượng tồn kho đã được cập nhật tương ứng!";
            }

            MessageBox.Show("Lưu thay đổi thông tin đơn hàng thành công!" + inventoryMsg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetEditState(false);
            LoadOrdersGrid();
            
            // Re-select modified row
            int index = mockOrders.IndexOf(selectedOrder);
            if (index >= 0 && index < dgvOrders.Rows.Count)
            {
                SelectOrderRow(index);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            SetEditState(false);
            if (selectedOrder != null)
            {
                PopulateOrderDetails(selectedOrder);
            }
        }

        private void btnConvertToInvoice_Click(object? sender, EventArgs e)
        {
            if (selectedOrder == null) return;

            if (selectedOrder.LoaiHoaDon != "Đơn đặt hàng")
            {
                MessageBox.Show("Chỉ có thể chuyển đổi Đơn đặt hàng thành Đơn bán hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Xác nhận chuyển Đơn đặt hàng #{selectedOrder.MaHoaDon} thành Đơn bán hàng (Hóa đơn)?\nThao tác này sẽ cập nhật tồn kho và tạo giao dịch bán hàng tương ứng.", "Xác nhận chuyển đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                selectedOrder.LoaiHoaDon = "Đơn bán hàng";
                selectedOrder.TrangThai = "Đã hoàn thành"; // Convert turns into active sales invoice
                
                MessageBox.Show("Đã chuyển đổi Đơn đặt hàng thành Đơn bán hàng thành công!\nTồn kho tương ứng đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadOrdersGrid();
                
                // Re-select row
                int index = mockOrders.IndexOf(selectedOrder);
                if (index >= 0 && index < dgvOrders.Rows.Count)
                {
                    SelectOrderRow(index);
                }
            }
        }
    }
}
