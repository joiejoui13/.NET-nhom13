using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    // CÁC LỚP DTO ĐỂ HỨNG DỮ LIỆU TỪ DATABASE
    public class SalesReportRow
    {
        public string MaHoaDon { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string TenNguoiDung { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public decimal TongTien { get; set; }
        public string HinhThucThanhToan { get; set; } = string.Empty;
    }

    public class RevenueTrendRow
    {
        public string Period { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
    }

    public class TopProductRow
    {
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class OrderStatusRow
    {
        public string TrangThai { get; set; } = string.Empty;
        public int SoLuong { get; set; }
    }

    // 5.3.1. Viết lớp ReportRepository (Truy xuất dữ liệu Báo cáo)
    public class ReportRepository
    {
        // 5.3.1.1. Hàm lấy Tổng Doanh Thu
        public decimal GetRevenue(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(TongTien), 0)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End AND TrangThai = N'Đã hoàn thành'";
            return DbContext.Conn.ExecuteScalar<decimal>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        // 5.3.1.2. Hàm lấy Tổng Số Đơn Hàng
        public int GetOrderCount(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COUNT(*)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End AND TrangThai = N'Đã hoàn thành'";
            return DbContext.Conn.ExecuteScalar<int>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        // 5.3.1.3. Hàm lấy Tổng Sản Phẩm Đã Bán
        public int GetTotalProductsSold(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(ct.SoLuong), 0)
                FROM ChiTietHoaDon ct
                INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End AND h.TrangThai = N'Đã hoàn thành'";
            return DbContext.Conn.ExecuteScalar<int>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        // 5.3.1.4. Hàm lấy Bảng kê chi tiết Hóa đơn (Data Grid)
        public IEnumerable<SalesReportRow> GetSalesReport(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT 
                    h.MaHoaDon, 
                    COALESCE(k.TenKhachHang, N'Khách vãng lai') AS TenKhachHang, 
                    COALESCE(n.TenNguoiDung, N'Hệ thống') AS TenNguoiDung, 
                    h.NgayTao, 
                    h.TongTien, 
                    COALESCE(h.PhuongThucThanhToan, N'Tiền mặt') AS HinhThucThanhToan
                FROM HoaDon h
                LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                LEFT JOIN NguoiDung n ON h.MaNguoiDung = n.MaNguoiDung
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End
                ORDER BY h.NgayTao DESC";
            return DbContext.Conn.Query<SalesReportRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        // 5.3.1.5. Hàm lấy Dữ liệu cho Biểu đồ Đường (Xu hướng doanh thu)
        public IEnumerable<RevenueTrendRow> GetRevenueTrend(DateTime start, DateTime end, string period)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            
            string groupQuery = "FORMAT(NgayTao, 'yyyy-MM-dd')"; // Mặc định theo Ngày
            if (period.ToLower() == "tháng" || period.ToLower() == "month")
                groupQuery = "FORMAT(NgayTao, 'yyyy-MM')";
            else if (period.ToLower() == "năm" || period.ToLower() == "year")
                groupQuery = "FORMAT(NgayTao, 'yyyy')";

            string sql = $@"
                SELECT 
                    {groupQuery} AS Period,
                    COALESCE(SUM(TongTien), 0) AS Revenue,
                    COUNT(*) AS OrdersCount
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End AND TrangThai = N'Đã hoàn thành'
                GROUP BY {groupQuery}
                ORDER BY Period ASC";

            return DbContext.Conn.Query<RevenueTrendRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        // 5.3.1.6. Hàm lấy Top 5 Sản Phẩm Bán Chạy Nhất (Biểu đồ Tròn)
        public IEnumerable<TopProductRow> GetTopProducts(DateTime start, DateTime end, int topN = 5)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT TOP (@TopN)
                    ct.MaSanPham,
                    s.TenSanPham,
                    SUM(ct.SoLuong) AS SoLuongBan,
                    SUM(ct.ThanhTien) AS DoanhThu
                FROM ChiTietHoaDon ct
                INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                INNER JOIN SanPham s ON ct.MaSanPham = s.MaSanPham
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End AND h.TrangThai = N'Đã hoàn thành'
                GROUP BY ct.MaSanPham, s.TenSanPham
                ORDER BY SoLuongBan DESC";
            return DbContext.Conn.Query<TopProductRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1), TopN = topN });
        }

        // 5.3.1.7. Hàm lấy Phân bố Trạng thái đơn hàng (Biểu đồ Tròn)
        public IEnumerable<OrderStatusRow> GetOrderStatusDistribution(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT 
                    COALESCE(g.TrangThaiGiao, N'Mua tại quầy') AS TrangThai,
                    COUNT(*) AS SoLuong
                FROM HoaDon h
                LEFT JOIN GiaoHang g ON h.MaHoaDon = g.MaHoaDon
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End
                GROUP BY COALESCE(g.TrangThaiGiao, N'Mua tại quầy')";
            return DbContext.Conn.Query<OrderStatusRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }
    }
}
