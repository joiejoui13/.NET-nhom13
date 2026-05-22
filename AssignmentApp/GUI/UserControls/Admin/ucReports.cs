using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Admin;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucReports : UserControl
    {
        private readonly ReportRepository _reportRepo = new ReportRepository();
        private List<SalesReportRow> _allReports = new List<SalesReportRow>();

        public ucReports()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvReports.AutoGenerateColumns = false;
            dgvReports.Columns.Clear();

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Mã Hóa Đơn", 
                DataPropertyName = "MaHoaDon", 
                Name = "colMaHoaDon",
                FillWeight = 100
            });
            dgvReports.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Khách Hàng", 
                DataPropertyName = "TenKhachHang", 
                Name = "colTenKhachHang",
                FillWeight = 150
            });
            dgvReports.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Nhân Viên Lập", 
                DataPropertyName = "TenNguoiDung", 
                Name = "colTenNguoiDung",
                FillWeight = 150
            });
            dgvReports.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Ngày Tạo", 
                DataPropertyName = "NgayTao", 
                Name = "colNgayTao",
                FillWeight = 120
            });
            
            var colTongTien = new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Tổng Tiền", 
                DataPropertyName = "TongTien", 
                Name = "colTongTien",
                FillWeight = 100
            };
            colTongTien.DefaultCellStyle.Format = "N0";
            colTongTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReports.Columns.Add(colTongTien);
            
            dgvReports.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Thanh Toán", 
                DataPropertyName = "HinhThucThanhToan", 
                Name = "colHinhThucThanhToan",
                FillWeight = 100
            });
        }

        private async void ucReports_Load(object sender, EventArgs e)
        {
            // Default: last 30 days
            dtpStartDate.Value = DateTime.Today.AddDays(-30);
            dtpEndDate.Value = DateTime.Today;

            await RefreshReportAsync();
        }

        private async Task RefreshReportAsync()
        {
            DateTime start = dtpStartDate.Value;
            DateTime end = dtpEndDate.Value;

            if (start.Date > end.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Fetch metrics
                decimal totalRevenue = await _reportRepo.GetRevenueAsync(start, end);
                int totalOrders = await _reportRepo.GetOrderCountAsync(start, end);

                // 2. Display metrics
                lblRevenueValue.Text = totalRevenue.ToString("N0") + " ₫";
                lblOrdersValue.Text = totalOrders.ToString("N0");

                // 3. Fetch and display grid records
                var records = await _reportRepo.GetSalesReportAsync(start, end);
                _allReports = records.ToList();
                FilterLocalReports();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterLocalReports()
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvReports.DataSource = null;
                dgvReports.DataSource = _allReports;
            }
            else
            {
                var filtered = _allReports.Where(x =>
                    x.MaHoaDon.ToLower().Contains(keyword) ||
                    x.TenKhachHang.ToLower().Contains(keyword) ||
                    x.TenNguoiDung.ToLower().Contains(keyword) ||
                    x.HinhThucThanhToan.ToLower().Contains(keyword)
                ).ToList();
                dgvReports.DataSource = null;
                dgvReports.DataSource = filtered;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm không khả dụng cho báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng sửa không khả dụng cho báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng xóa không khả dụng cho báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng hủy bỏ không khả dụng cho báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterLocalReports();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            dtpStartDate.Value = DateTime.Today.AddDays(-30);
            dtpEndDate.Value = DateTime.Today;
            await RefreshReportAsync();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"BaoCaoDoanhThu_{dtpStartDate.Value:yyyyMMdd}_den_{dtpEndDate.Value:yyyyMMdd}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        // Write UTF-8 BOM for Excel Vietnamese language display support
                        sb.WriteBom();

                        // CSV Headers
                        sb.AppendLine("Mã Hóa Đơn,Khách Hàng,Nhân Viên Lập,Ngày Tạo,Tổng Tiền,Thanh Toán");
                        
                        foreach (DataGridViewRow row in dgvReports.Rows)
                        {
                            var maHD = row.Cells["colMaHoaDon"].Value?.ToString() ?? "";
                            var khachHang = row.Cells["colTenKhachHang"].Value?.ToString() ?? "";
                            var nhanVien = row.Cells["colTenNguoiDung"].Value?.ToString() ?? "";
                            var ngayTao = row.Cells["colNgayTao"].Value?.ToString() ?? "";
                            var tongTien = row.Cells["colTongTien"].Value?.ToString() ?? "0";
                            var thanhToan = row.Cells["colHinhThucThanhToan"].Value?.ToString() ?? "";

                            // Escape CSV fields
                            khachHang = $"\"{khachHang.Replace("\"", "\"\"")}\"";
                            nhanVien = $"\"{nhanVien.Replace("\"", "\"\"")}\"";
                            thanhToan = $"\"{thanhToan.Replace("\"", "\"\"")}\"";

                            sb.AppendLine($"{maHD},{khachHang},{nhanVien},{ngayTao},{tongTien},{thanhToan}");
                        }

                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Xuất file báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    public static class StringBuilderExtensions
    {
        public static void WriteBom(this StringBuilder sb)
        {
            // Injecting standard UTF-8 BOM chars
            sb.Append((char)0xFEFF);
        }
    }
}
