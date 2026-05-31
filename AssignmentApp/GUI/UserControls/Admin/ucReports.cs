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

            // Extracted from Designer
            cboPeriod.Items.AddRange(new object[] { "Ngày", "Tháng", "Năm" });
            this.Load += ucReports_Load;
        }

        // 5.3.2. Viết thủ tục Form_Load của ucReports
        private void ucReports_Load(object sender, EventArgs e)
        {
            // Bước 1: Thiết lập giá trị mặc định cho bộ lọc
            cboPeriod.SelectedIndex = 1; // Default to "Tháng"
            dtpStartDate.Value = new DateTime(2026, 1, 1);
            dtpEndDate.Value = new DateTime(2026, 12, 31);
            
            // Bước 2: Tự động tải dữ liệu báo cáo lần đầu
            LoadReportData();
        }

        // 5.3.3. Viết thủ tục btnTimkiem_Click (Nút Lọc Báo Cáo)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Tải lại báo cáo dựa trên khoảng thời gian mới
            LoadReportData();
        }

        // Nút làm mới đã bị xóa theo yêu cầu

        // 5.3.5. Viết thủ tục xuất báo cáo Excel (btnSave_Click)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Bước 1: Mở hộp thoại chọn nơi lưu file
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Bước 2: Tạo file và ghi định dạng UTF-8 để không lỗi font tiếng Việt
                        using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            sw.Write('\uFEFF'); // Ký tự BOM giúp Excel nhận diện UTF-8

                            // Bước 3: Ghi dòng tiêu đề cột
                            sw.WriteLine("Mã Hóa Đơn,Khách Hàng,Nhân Viên,Ngày Lập,Tổng Tiền,Thanh Toán");
                            
                            // Bước 4: Duyệt qua từng dòng trong bảng và ghi dữ liệu
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
                                
                                // Ghi dữ liệu, cách nhau bằng dấu phẩy
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

        // 5.3.6. Viết thủ tục LoadReportData (Xử lý biểu đồ và dữ liệu)
        private void LoadReportData()
        {
            try
            {
                // Bước 1: Lấy các mốc thời gian lọc từ giao diện
                var start = dtpStartDate.Value;
                var end = dtpEndDate.Value;
                var period = cboPeriod.SelectedItem?.ToString() ?? "Tháng";

                // Bước 2: Nạp dữ liệu cho các Thẻ Tổng Quan (KPI Cards)
                decimal revenue = _repo.GetRevenue(start, end);
                lblRevenueValue.Text = revenue.ToString("N0") + " ₫";

                int orders = _repo.GetOrderCount(start, end);
                lblOrdersValue.Text = orders.ToString("N0");

                int products = _repo.GetTotalProductsSold(start, end);
                lblProductsValue.Text = products.ToString("N0");

                // Bước 3: Vẽ Biểu đồ đường (Line) và Cột (Bar) - Xu hướng doanh thu và Đơn hàng
                var trend = _repo.GetRevenueTrend(start, end, period);
                var dates = trend.Select(x => x.Period).ToArray();
                var revenues = trend.Select(x => (double)x.Revenue).ToArray();
                var ordersCounts = trend.Select(x => x.OrdersCount).ToArray();

                cartesianChart1.Series = new ISeries[]
                {
                    // Bar Chart (Biểu đồ cột) cho Số lượng đơn hàng
                    new ColumnSeries<int>
                    {
                        Values = ordersCounts,
                        Name = "Số đơn hàng",
                        Fill = new SolidColorPaint(SKColors.DarkOrange),
                        ScalesYAt = 1 // Dùng trục Y thứ 2 (bên phải)
                    },
                    // Line Chart (Biểu đồ đường) cho Doanh thu
                    new LineSeries<double>
                    {
                        Values = revenues,
                        Name = "Doanh thu (₫)",
                        Fill = new SolidColorPaint(SKColors.CornflowerBlue.WithAlpha(50)),
                        Stroke = new SolidColorPaint(SKColors.CornflowerBlue, 3),
                        GeometrySize = 8,
                        GeometryStroke = new SolidColorPaint(SKColors.CornflowerBlue, 3),
                        ScalesYAt = 0 // Dùng trục Y thứ 1 (bên trái)
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
                    new Axis // Trục Y bên trái (Doanh thu)
                    {
                        Labeler = val => val.ToString("N0") + " ₫",
                        SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
                    },
                    new Axis // Trục Y bên phải (Số đơn hàng)
                    {
                        Labeler = val => val.ToString("N0") + " Đơn",
                        Position = LiveChartsCore.Measure.AxisPosition.End,
                        ShowSeparatorLines = false
                    }
                };

                // Bước 4: Vẽ Biểu đồ tròn - Top 5 Sản phẩm bán chạy nhất
                var topProducts = _repo.GetTopProducts(start, end, 5);
                var productSeries = new List<ISeries>();
                foreach (var p in topProducts)
                {
                    productSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { p.SoLuongBan },
                        Name = p.TenSanPham
                    });
                }
                pieChartProducts.Series = productSeries;

                // Bước 5: Vẽ Biểu đồ tròn - Phân bổ Trạng thái đơn hàng
                var orderStatus = _repo.GetOrderStatusDistribution(start, end);
                var statusSeries = new List<ISeries>();
                foreach (var s in orderStatus)
                {
                    statusSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { s.SoLuong },
                        Name = s.TrangThai
                    });
                }
                pieChartStatus.Series = statusSeries;

                // Bước 6: Đổ dữ liệu chi tiết các hóa đơn xuống DataGridView
                var sales = _repo.GetSalesReport(start, end).ToList();
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

        // 5.3.7. Các sự kiện trống để ngăn Designer báo lỗi
        private void btnAdd_Click(object sender, EventArgs e) { }
        private void btnEdit_Click(object sender, EventArgs e) { }
        private void btnDelete_Click(object sender, EventArgs e) { }
        private void btnCancel_Click(object sender, EventArgs e) { }
        private void dgvReports_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
