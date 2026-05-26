using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
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

    public class ReportRepository
    {
        public async Task<decimal> GetRevenueAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(TongTien), 0)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End";
            return await DbContext.Conn.ExecuteScalarAsync<decimal>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<int> GetOrderCountAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COUNT(*)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End";
            return await DbContext.Conn.ExecuteScalarAsync<int>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<int> GetTotalProductsSoldAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(ct.SoLuong), 0)
                FROM ChiTietHoaDon ct
                INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End";
            return await DbContext.Conn.ExecuteScalarAsync<int>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<IEnumerable<SalesReportRow>> GetSalesReportAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT 
                    h.MaHoaDon, 
                    COALESCE(k.TenKhachHang, N'Khách vãng lai') AS TenKhachHang, 
                    COALESCE(n.TenNguoiDung, N'Hệ thống') AS TenNguoiDung, 
                    h.NgayTao, 
                    h.TongTien, 
                    COALESCE(h.HinhThucThanhToan, N'Tiền mặt') AS HinhThucThanhToan
                FROM HoaDon h
                LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                LEFT JOIN NguoiDung n ON h.MaNguoiDung = n.MaNguoiDung
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End
                ORDER BY h.NgayTao DESC";
            return await DbContext.Conn.QueryAsync<SalesReportRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<IEnumerable<RevenueTrendRow>> GetRevenueTrendAsync(DateTime start, DateTime end, string period)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            
            int length = 10;
            if (period.ToLower() == "tháng" || period.ToLower() == "month")
            {
                length = 7;
            }
            else if (period.ToLower() == "năm" || period.ToLower() == "year")
            {
                length = 4;
            }

            string sql = $@"
                SELECT 
                    CONVERT(VARCHAR({length}), NgayTao, 120) AS Period,
                    COALESCE(SUM(TongTien), 0) AS Revenue,
                    COUNT(*) AS OrdersCount
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End
                GROUP BY CONVERT(VARCHAR({length}), NgayTao, 120)
                ORDER BY Period ASC";

            return await DbContext.Conn.QueryAsync<RevenueTrendRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<IEnumerable<TopProductRow>> GetTopProductsAsync(DateTime start, DateTime end, int topN = 5)
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
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End
                GROUP BY ct.MaSanPham, s.TenSanPham
                ORDER BY SoLuongBan DESC";
            return await DbContext.Conn.QueryAsync<TopProductRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1), TopN = topN });
        }

        public async Task<IEnumerable<OrderStatusRow>> GetOrderStatusDistributionAsync(DateTime start, DateTime end)
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
            return await DbContext.Conn.QueryAsync<OrderStatusRow>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }
    }
}
