using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Admin;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucReports : UserControl
    {
        private readonly ReportRepository _repo = new ReportRepository();

        public ucReports()
        {
            InitializeComponent();
            this.Load += ucReports_Load;
        }

        private async void ucReports_Load(object sender, EventArgs e)
        {
            cboPeriod.SelectedIndex = 1; // Default to "Tháng"
            dtpStartDate.Value = new DateTime(2026, 1, 1);
            dtpEndDate.Value = new DateTime(2026, 12, 31);
            
            await LoadReportDataAsync();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // BOM for UTF-8 compatibility with Excel
                            sw.Write('\uFEFF');

                            // Write headers
                            sw.WriteLine("Mã Hóa Đơn,Khách Hàng,Nhân Viên,Ngày Lập,Tổng Tiền,Thanh Toán");
                            
                            foreach (DataGridViewRow row in dgvReports.Rows)
                            {
                                if (row.IsNewRow) continue;
                                var ma = row.Cells["MaHoaDon"].Value?.ToString() ?? "";
                                var kh = row.Cells["TenKhachHang"].Value?.ToString() ?? "";
                                var nv = row.Cells["TenNguoiDung"].Value?.ToString() ?? "";
                                
                                var dateVal = row.Cells["NgayTao"].Value;
                                var ngay = dateVal is DateTime dt ? dt.ToString("dd/MM/yyyy HH:mm") : (dateVal?.ToString() ?? "");
                                
                                var moneyVal = row.Cells["TongTien"].Value;
                                var tien = moneyVal is decimal money ? money.ToString("F2") : (moneyVal?.ToString() ?? "");
                                
                                var tt = row.Cells["HinhThucThanhToan"].Value?.ToString() ?? "";
                                sw.WriteLine($"\"{ma}\",\"{kh}\",\"{nv}\",\"{ngay}\",\"{tien}\",\"{tt}\"");
                            }
                        }
                        MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task LoadReportDataAsync()
        {
            try
            {
                var start = dtpStartDate.Value;
                var end = dtpEndDate.Value;
                var period = cboPeriod.SelectedItem?.ToString() ?? "Tháng";

                // 1. KPI Cards
                decimal revenue = await _repo.GetRevenueAsync(start, end);
                lblRevenueValue.Text = revenue.ToString("N0") + " ₫";

                int orders = await _repo.GetOrderCountAsync(start, end);
                lblOrdersValue.Text = orders.ToString("N0");

                int products = await _repo.GetTotalProductsSoldAsync(start, end);
                lblProductsValue.Text = products.ToString("N0");

                // 2. Cartesian Chart - Revenue Trend
                var trend = await _repo.GetRevenueTrendAsync(start, end, period);
                var dates = trend.Select(x => x.Period).ToArray();
                var revenues = trend.Select(x => (double)x.Revenue).ToArray();

                cartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = revenues,
                        Name = "Doanh thu (₫)",
                        Fill = new SolidColorPaint(SKColors.CornflowerBlue.WithAlpha(50)),
                        Stroke = new SolidColorPaint(SKColors.CornflowerBlue, 3),
                        GeometrySize = 8,
                        GeometryStroke = new SolidColorPaint(SKColors.CornflowerBlue, 3)
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = dates,
                        LabelsRotation = 15,
                        SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
                    }
                };

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Labeler = val => val.ToString("N0") + " ₫",
                        SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
                    }
                };

                // 3. Pie Chart - Top 5 Selling Products
                var topProducts = await _repo.GetTopProductsAsync(start, end, 5);
                var productSeries = new List<ISeries>();
                foreach (var p in topProducts)
                {
                    productSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { p.SoLuongBan },
                        Name = p.TenSanPham,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue} SP"
                    });
                }
                pieChartProducts.Series = productSeries;

                // 4. Pie Chart - Order Statuses / Delivery Statuses
                var orderStatus = await _repo.GetOrderStatusDistributionAsync(start, end);
                var statusSeries = new List<ISeries>();
                foreach (var s in orderStatus)
                {
                    statusSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { s.SoLuong },
                        Name = s.TrangThai,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue} Đơn"
                    });
                }
                pieChartStatus.Series = statusSeries;

                // 5. Data Grid Detail
                var sales = (await _repo.GetSalesReportAsync(start, end)).ToList();
                dgvReports.DataSource = sales;

                if (dgvReports.Columns.Count > 0)
                {
                    dgvReports.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
                    dgvReports.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                    dgvReports.Columns["TenNguoiDung"].HeaderText = "Nhân Viên";
                    dgvReports.Columns["NgayTao"].HeaderText = "Ngày Lập";
                    dgvReports.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvReports.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dgvReports.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvReports.Columns["HinhThucThanhToan"].HeaderText = "Thanh Toán";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Stubs for CRUD event handlers to satisfy the Designer
        private void btnAdd_Click(object sender, EventArgs e) { }
        private void btnEdit_Click(object sender, EventArgs e) { }
        private void btnDelete_Click(object sender, EventArgs e) { }
        private void btnCancel_Click(object sender, EventArgs e) { }
    }
}
