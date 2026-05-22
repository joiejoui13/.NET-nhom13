using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;
using Guna.UI2.WinForms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private readonly OrderRepository _orderRepo = new OrderRepository();
        private List<Order> _allOrders = new List<Order>();

        public ucOrderManagement()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvOrders.AutoGenerateColumns = false;
            colMaHoaDon.DataPropertyName = "MaHoaDon";
            colTenKhachHang.DataPropertyName = "TenKhachHang";
            colTenNguoiDung.DataPropertyName = "TenNguoiDung";
            colTongTien.DataPropertyName = "TongTien";
            colGiamGia.DataPropertyName = "GiamGia";
            colHinhThucThanhToan.DataPropertyName = "HinhThucThanhToan";
            colNgayTao.DataPropertyName = "NgayTao";
        }

        private async void ucOrderManagement_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _orderRepo.GetAllAsync();
                _allOrders = list.ToList();
                dgvOrders.DataSource = null;
                dgvOrders.DataSource = _allOrders;

                if (dgvOrders.Rows.Count > 0)
                {
                    dgvOrders.Rows[0].Selected = true;
                    PopulateFields(dgvOrders.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaHoaDon.Text = row.Cells["colMaHoaDon"].Value?.ToString() ?? string.Empty;
            txtMaKhachHang.Text = row.Cells["colTenKhachHang"].Value?.ToString() ?? "Khách vãng lai";
            txtTenNguoiDung.Text = row.Cells["colTenNguoiDung"].Value?.ToString() ?? string.Empty;
            txtHinhThucThanhToan.Text = row.Cells["colHinhThucThanhToan"].Value?.ToString() ?? string.Empty;
            
            if (row.Cells["colNgayTao"].Value is DateTime dt)
            {
                txtNgayTao.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
            {
                txtNgayTao.Text = string.Empty;
            }

            if (row.Cells["colTongTien"].Value is decimal tt)
            {
                txtTongTien.Text = tt.ToString("N0") + " đ";
            }
            else
            {
                txtTongTien.Text = "0 đ";
            }

            if (row.Cells["colGiamGia"].Value is decimal gg)
            {
                txtGiamGia.Text = gg.ToString("N0") + " đ";
            }
            else
            {
                txtGiamGia.Text = "0 đ";
            }
        }

        private void ClearFields()
        {
            txtMaHoaDon.Text = string.Empty;
            txtMaKhachHang.Text = string.Empty;
            txtTenNguoiDung.Text = string.Empty;
            txtHinhThucThanhToan.Text = string.Empty;
            txtNgayTao.Text = string.Empty;
            txtTongTien.Text = string.Empty;
            txtGiamGia.Text = string.Empty;
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOrders.Rows[e.RowIndex] != null)
            {
                PopulateFields(dgvOrders.Rows[e.RowIndex]);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadDataAsync();
                return;
            }

            try
            {
                var list = await _orderRepo.SearchAsync(keyword);
                _allOrders = list.ToList();
                dgvOrders.DataSource = null;
                dgvOrders.DataSource = _allOrders;

                if (dgvOrders.Rows.Count > 0)
                {
                    dgvOrders.Rows[0].Selected = true;
                    PopulateFields(dgvOrders.Rows[0]);
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            await LoadDataAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string maHoaDon = txtMaHoaDon.Text;
            if (string.IsNullOrEmpty(maHoaDon))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn muốn hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn hủy hóa đơn '{maHoaDon}' không?\nThao tác này sẽ hoàn trả số lượng hàng vào kho và xóa các bản ghi liên quan (trả hàng, giao hàng, chi tiết hóa đơn).",
                "Xác nhận hủy hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _orderRepo.DeleteOrderTransactionAsync(maHoaDon);
                    if (ok)
                    {
                        MessageBox.Show("Hủy hóa đơn thành công! Số lượng sản phẩm đã được khôi phục vào kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Hủy hóa đơn thất bại hoặc hóa đơn không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi hủy hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng thực hiện tạo hóa đơn mới tại màn hình POS (Bán Hàng)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hóa đơn đã được thanh toán và lưu trữ. Không thể chỉnh sửa thông tin hóa đơn trực tiếp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                PopulateFields(dgvOrders.SelectedRows[0]);
            }
            else
            {
                ClearFields();
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string maHoaDon = txtMaHoaDon.Text;
            if (string.IsNullOrEmpty(maHoaDon))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var details = await _orderRepo.GetDetailsAsync(maHoaDon);
                var detailList = details.ToList();

                if (detailList.Count == 0)
                {
                    MessageBox.Show("Hóa đơn này không có chi tiết sản phẩm nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Show a dialog form showing the details
                ShowDetailsForm(maHoaDon, detailList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lấy chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDetailsForm(string maHoaDon, List<OrderDetail> details)
        {
            Form detailForm = new Form
            {
                Text = $"Chi Tiết Hóa Đơn - {maHoaDon}",
                Size = new Size(600, 450),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            Guna2Panel cardPanel = new Guna2Panel
            {
                FillColor = Color.White,
                BorderRadius = 10,
                Location = new Point(15, 15),
                Size = new Size(550, 320),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            detailForm.Controls.Add(cardPanel);

            Guna2DataGridView dgvDetails = new Guna2DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                Location = new Point(10, 10),
                Size = new Size(530, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                ThemeStyle = {
                    GridColor = Color.FromArgb(231, 229, 255),
                    HeaderStyle = {
                        BackColor = Color.FromArgb(0, 126, 249),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                    },
                    RowsStyle = {
                        BackColor = Color.White,
                        Height = 28,
                        SelectionBackColor = Color.FromArgb(231, 229, 255),
                        SelectionForeColor = Color.FromArgb(71, 69, 94)
                    }
                }
            };

            DataGridViewTextBoxColumn colSP = new DataGridViewTextBoxColumn
            {
                HeaderText = "Sản Phẩm",
                DataPropertyName = "TenSanPham",
                Width = 220
            };
            DataGridViewTextBoxColumn colSL = new DataGridViewTextBoxColumn
            {
                HeaderText = "Số Lượng",
                DataPropertyName = "SoLuong",
                Width = 80
            };
            DataGridViewTextBoxColumn colGia = new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn Giá",
                DataPropertyName = "DonGia",
                Width = 110
            };
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                HeaderText = "Thành Tiền",
                DataPropertyName = "ThanhTien",
                Width = 120
            };

            dgvDetails.Columns.AddRange(new DataGridViewColumn[] { colSP, colSL, colGia, colTT });
            dgvDetails.AutoGenerateColumns = false;
            
            // Map formatted string values for display or bind list directly
            var displayList = details.Select(d => new
            {
                d.TenSanPham,
                d.SoLuong,
                DonGia = d.DonGia.ToString("N0") + " đ",
                ThanhTien = d.ThanhTien.ToString("N0") + " đ"
            }).ToList();

            dgvDetails.DataSource = displayList;
            cardPanel.Controls.Add(dgvDetails);

            Guna2Button btnClose = new Guna2Button
            {
                Text = "ĐÓNG",
                BorderRadius = 5,
                FillColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(445, 355),
                Size = new Size(120, 36)
            };
            btnClose.Click += (s, e) => { detailForm.Close(); };
            detailForm.Controls.Add(btnClose);

            detailForm.ShowDialog(this);
        }
    }
}
