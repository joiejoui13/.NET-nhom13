using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Admin;
using AssignmentApp.DTO.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucReports : UserControl
    {
        // Khởi tạo Service xử lý nghiệp vụ liên quan đến Báo cáo thống kê (thay vì gọi thẳng Repository)
        private readonly IReportService _reportService;

        #region 1. KHỞI TẠO VÀ TẢI FORM (INITIALIZATION & LOAD)

        /// <summary>
        /// Hàm khởi tạo mặc định của UserControl ucReports.
        /// Chạy đầu tiên khi khởi tạo đối tượng, dùng để vẽ giao diện và thiết lập cấu hình ban đầu.
        /// </summary>
        public ucReports()
        {
            InitializeComponent();
            
            // Lấy instance của IReportService thông qua cơ chế Dependency Injection
            _reportService = Program.ServiceProvider.GetRequiredService<IReportService>();

            // CẤU HÌNH COMBOBOX: Danh sách chu kỳ báo cáo (Ngày/Tháng/Năm)
            // Đã được tách từ file Designer để tập trung logic cấu hình tại đây
            cboPeriod.Items.AddRange(new object[] { "Ngày", "Tháng", "Năm" });
            
            // Gắn sự kiện Load form
            this.Load += ucReports_Load;
        }

        /// <summary>
        /// Sự kiện Load: Kích hoạt khi UserControl lần đầu được hiển thị.
        /// Thiết lập các mốc thời gian mặc định và tự động tải dữ liệu báo cáo lần đầu tiên.
        /// </summary>
        private async void ucReports_Load(object sender, EventArgs e)
        {
            // 1. Thiết lập giá trị mặc định cho bộ lọc báo cáo
            cboPeriod.SelectedIndex = 1; // Mặc định chọn chỉ mục 1 (Tháng)
            
            // Đặt khoảng thời gian mặc định là từ đầu năm đến cuối năm 2026
            dtpStartDate.Value = new DateTime(2026, 1, 1);
            dtpEndDate.Value = new DateTime(2026, 12, 31);
            
            // 2. Kích hoạt quy trình tải toàn bộ dữ liệu báo cáo
            await LoadReportDataAsync();
        }

        #endregion

        #region 2. XỬ LÝ BIỂU ĐỒ VÀ DỮ LIỆU BÁO CÁO (CHART & DATA PROCESSING)

        /// <summary>
        /// Hàm cốt lõi đảm nhiệm toàn bộ quy trình tải dữ liệu báo cáo.
        /// Bao gồm: Cập nhật thẻ tổng quan (KPI), vẽ 3 loại biểu đồ (Đường, Cột, Tròn) và đổ dữ liệu vào DataGridView.
        /// </summary>
        private async Task LoadReportDataAsync()
        {
            try
            {
                // 1. THU THẬP THAM SỐ TỪ GIAO DIỆN
                var start = dtpStartDate.Value;
                var end = dtpEndDate.Value;
                // Lấy mốc chu kỳ (Sử dụng lệnh if-else cơ bản thay vì toán tử ??)
                string period = "Tháng"; // Mặc định là Tháng
                if (cboPeriod.SelectedItem != null)
                {
                    period = cboPeriod.SelectedItem.ToString();
                }

                // 2. CẬP NHẬT CÁC THẺ TỔNG QUAN (KPI CARDS)
                // - Lấy và hiển thị Tổng Doanh Thu
                decimal revenue = await _reportService.GetRevenueAsync(start, end);
                lblRevenueValue.Text = revenue.ToString("N0") + " ₫";

                // - Lấy và hiển thị Tổng Số Đơn Hàng
                int orders = await _reportService.GetOrderCountAsync(start, end);
                lblOrdersValue.Text = orders.ToString("N0");

                // - Lấy và hiển thị Tổng Số Sản Phẩm Bán Ra
                int products = await _reportService.GetTotalProductsSoldAsync(start, end);
                lblProductsValue.Text = products.ToString("N0");

                // 3. VẼ BIỂU ĐỒ HỖN HỢP: XU HƯỚNG DOANH THU & ĐƠN HÀNG (LINE & BAR CHART)
                // - Kéo dữ liệu xu hướng từ CSDL
                var trend = await _reportService.GetRevenueTrendAsync(start, end, period);
                var trendList = trend.ToList();
                
                // Phân tách dữ liệu thành các mảng trục X, Y (Sử dụng vòng lặp for cơ bản thay cho LINQ)
                string[] dates = new string[trendList.Count];
                double[] revenues = new double[trendList.Count];
                int[] ordersCounts = new int[trendList.Count];

                for (int i = 0; i < trendList.Count; i++)
                {
                    dates[i] = trendList[i].Period;
                    revenues[i] = (double)trendList[i].Revenue;
                    ordersCounts[i] = trendList[i].OrdersCount;
                }

                // - Khởi tạo các chuỗi dữ liệu (Series) cho biểu đồ LiveCharts
                cartesianChart1.Series = new ISeries[]
                {
                    // Chuỗi 1: Biểu đồ cột (Bar) đại diện cho Số lượng đơn hàng
                    new ColumnSeries<int>
                    {
                        Values = ordersCounts,
                        Name = "Số đơn hàng",
                        Fill = new SolidColorPaint(SKColors.DarkOrange), // Cột màu cam
                        ScalesYAt = 1 // Gắn vào trục Y thứ 2 (Nằm bên phải biểu đồ)
                    },
                    // Chuỗi 2: Biểu đồ đường (Line) đại diện cho Doanh thu
                    new LineSeries<double>
                    {
                        Values = revenues,
                        Name = "Doanh thu (₫)",
                        Fill = new SolidColorPaint(SKColors.CornflowerBlue.WithAlpha(50)), // Tô màu mờ dưới đường (Area chart)
                        Stroke = new SolidColorPaint(SKColors.CornflowerBlue, 3), // Đường viền xanh biển dày 3px
                        GeometrySize = 8, // Kích thước của chấm điểm trên đường
                        GeometryStroke = new SolidColorPaint(SKColors.CornflowerBlue, 3),
                        ScalesYAt = 0 // Gắn vào trục Y thứ 1 (Nằm bên trái biểu đồ)
                    }
                };

                // - Cấu hình trục X (Trục ngang thời gian)
                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = dates,          // Hiển thị ngày/tháng
                        LabelsRotation = 15,     // Xoay nghiêng chữ 15 độ để không bị đè lên nhau
                        SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) // Màu viền ngăn cách (Ghi nhạt)
                    }
                };

                // - Cấu hình 2 trục Y (Trái và Phải)
                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis // Trục Y bên trái: Dùng cho tiền (Doanh thu)
                    {
                        Labeler = FormatMoneyLabel, // Gọi hàm FormatMoneyLabel ở bên dưới thay vì dùng lambda =>
                        SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
                    },
                    new Axis // Trục Y bên phải: Dùng cho số đếm (Đơn hàng)
                    {
                        Labeler = FormatOrderLabel, // Gọi hàm FormatOrderLabel ở bên dưới thay vì dùng lambda =>
                        Position = LiveChartsCore.Measure.AxisPosition.End, // Đặt trục sang phía bên phải (End)
                        ShowSeparatorLines = false // Tắt đường gióng ngang để tránh trùng lặp rối mắt với trục trái
                    }
                };

                // 4. VẼ BIỂU ĐỒ TRÒN 1: TOP 5 SẢN PHẨM BÁN CHẠY
                var topProducts = await _reportService.GetTopProductsAsync(start, end, 5);
                var productSeries = new List<ISeries>();
                foreach (var p in topProducts)
                {
                    // Biểu diễn mỗi sản phẩm là một lát cắt (PieSeries) của biểu đồ tròn
                    productSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { p.SoLuongBan },
                        Name = p.TenSanPham
                    });
                }
                pieChartProducts.Series = productSeries;

                // 5. VẼ BIỂU ĐỒ TRÒN 2: PHÂN BỔ TRẠNG THÁI ĐƠN HÀNG (Hoàn thành, Hủy, Chờ...)
                var orderStatus = await _reportService.GetOrderStatusDistributionAsync(start, end);
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

                // 6. ĐỔ DỮ LIỆU CHI TIẾT HÓA ĐƠN XUỐNG DATAGRIDVIEW
                var sales = await _reportService.GetSalesReportAsync(start, end);
                dgvReports.DataSource = sales.ToList();

                // Tinh chỉnh hiển thị các cột (Chỉ thiết lập nếu cột đã sinh ra)
                if (dgvReports.Columns.Count > 0)
                {
                    dgvReports.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
                    dgvReports.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                    dgvReports.Columns["TenNguoiDung"].HeaderText = "Nhân Viên";
                    
                    dgvReports.Columns["NgayTao"].HeaderText = "Ngày Lập";
                    dgvReports.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    
                    dgvReports.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dgvReports.Columns["TongTien"].DefaultCellStyle.Format = "N0"; // Định dạng phân cách hàng nghìn
                    
                    dgvReports.Columns["HinhThucThanhToan"].HeaderText = "Thanh Toán";
                }
            }
            catch (Exception ex)
            {
                // Bắt và thông báo lỗi trong trường hợp mất kết nối CSDL hoặc dữ liệu bất thường
                MessageBox.Show("Lỗi tải dữ liệu báo cáo: " + ex.Message, "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Các hàm hỗ trợ định dạng nhãn biểu đồ (Dễ hiểu hơn việc dùng cú pháp Lambda =>)
        private string FormatMoneyLabel(double val)
        {
            return val.ToString("N0") + " ₫";
        }

        private string FormatOrderLabel(double val)
        {
            return val.ToString("N0") + " Đơn";
        }

        #endregion

        #region 3. CÁC HÀM XỬ LÝ NÚT BẤM (BUTTON CLICK HANDLERS)

        /// <summary>
        /// Nút Lọc/Tìm kiếm: Kích hoạt tải lại toàn bộ báo cáo dựa trên khoảng thời gian mới vừa chọn.
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        /// <summary>
        /// Nút Xuất Báo Cáo: Trích xuất dữ liệu đang hiển thị trên lưới DataGridView thành file Excel (.CSV).
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Mở hộp thoại chọn nơi lưu file cho người dùng
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                // Chỉ định đuôi file mặc định là CSV
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"; 
                sfd.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd}.csv"; // Tên file tự động sinh theo ngày
                
                // Nếu người dùng ấn Save (Đồng ý lưu)
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 2. Mở luồng stream để tiến hành ghi file, đảm bảo Encoding là UTF8 để xuất không bị lỗi Font Tiếng Việt
                        using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            sw.Write('\uFEFF'); // Mã BOM (Byte Order Mark): Cực kì quan trọng để Excel 2016+ tự động nhận diện UTF-8

                            // 3. Ghi dòng tiêu đề cột cách nhau bằng dấu phẩy
                            sw.WriteLine("Mã Hóa Đơn,Khách Hàng,Nhân Viên,Ngày Lập,Tổng Tiền,Thanh Toán");
                            
                            // 4. Duyệt qua từng dòng dữ liệu trong lưới DataGridView
                            foreach (DataGridViewRow row in dgvReports.Rows)
                            {
                                if (row.IsNewRow) continue; // Bỏ qua dòng trắng cuối cùng của lưới (nếu có)
                                
                                // Lấy giá trị từng ô và xử lý an toàn chống Null bằng cấu trúc if-else cơ bản
                                string ma = "";
                                if (row.Cells["MaHoaDon"].Value != null)
                                {
                                    ma = row.Cells["MaHoaDon"].Value.ToString();
                                }

                                string kh = "";
                                if (row.Cells["TenKhachHang"].Value != null)
                                {
                                    kh = row.Cells["TenKhachHang"].Value.ToString();
                                }

                                string nv = "";
                                if (row.Cells["TenNguoiDung"].Value != null)
                                {
                                    nv = row.Cells["TenNguoiDung"].Value.ToString();
                                }

                                // Lấy và ép kiểu định dạng ngày tháng (sử dụng if-else và GetType để người mới dễ hiểu)
                                string ngay = "";
                                object dateVal = row.Cells["NgayTao"].Value;
                                if (dateVal != null)
                                {
                                    if (dateVal.GetType() == typeof(DateTime)) // Kiểm tra xem kiểu dữ liệu có phải là DateTime không
                                    {
                                        DateTime dt = (DateTime)dateVal; // Ép kiểu (cast) về DateTime
                                        ngay = dt.ToString("dd/MM/yyyy HH:mm");
                                    }
                                    else
                                    {
                                        ngay = dateVal.ToString();
                                    }
                                }

                                // Lấy và ép kiểu tiền tệ
                                string tien = "";
                                object moneyVal = row.Cells["TongTien"].Value;
                                if (moneyVal != null)
                                {
                                    if (moneyVal.GetType() == typeof(decimal))
                                    {
                                        decimal money = (decimal)moneyVal;
                                        tien = money.ToString("F2");
                                    }
                                    else
                                    {
                                        tien = moneyVal.ToString();
                                    }
                                }

                                string tt = "";
                                if (row.Cells["HinhThucThanhToan"].Value != null)
                                {
                                    tt = row.Cells["HinhThucThanhToan"].Value.ToString();
                                }
                                
                                // Ghi dữ liệu dòng xuống file, bọc các giá trị chuỗi trong dấu nháy kép ("") 
                                // để tránh lỗi ngắt cột sai nếu dữ liệu có chứa dấu phẩy nội tại.
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

        #endregion

        #region 4. CÁC SỰ KIỆN TRỐNG (EMPTY HANDLERS)

        // Đây là các sự kiện đã lỡ liên kết trong Designer nhưng hiện tại không dùng tới.
        // Giữ lại các hàm trống này để Visual Studio Designer không bị báo lỗi thiếu hàm.
        private void btnAdd_Click(object sender, EventArgs e) { }
        private void btnEdit_Click(object sender, EventArgs e) { }
        private void btnDelete_Click(object sender, EventArgs e) { }
        private void btnCancel_Click(object sender, EventArgs e) { }
        private void dgvReports_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        #endregion
    }
}
