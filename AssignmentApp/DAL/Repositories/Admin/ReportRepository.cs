using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO.Models;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class ReportRepository : IReportRepository
    {
        public async Task<decimal> GetRevenueAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(TongTien), 0)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End AND TrangThai = N'Đã hoàn thành'";
            return await DbContext.Conn.ExecuteScalarAsync<decimal>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<int> GetOrderCountAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COUNT(*)
                FROM HoaDon
                WHERE NgayTao >= @Start AND NgayTao <= @End AND TrangThai = N'Đã hoàn thành'";
            return await DbContext.Conn.ExecuteScalarAsync<int>(sql, new { Start = start.Date, End = end.Date.AddDays(1).AddSeconds(-1) });
        }

        public async Task<int> GetTotalProductsSoldAsync(DateTime start, DateTime end)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT COALESCE(SUM(ct.SoLuong), 0)
                FROM ChiTietHoaDon ct
                INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End AND h.TrangThai = N'Đã hoàn thành'";
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
                    COALESCE(h.PhuongThucThanhToan, N'Tiền mặt') AS HinhThucThanhToan
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
                WHERE h.NgayTao >= @Start AND h.NgayTao <= @End AND h.TrangThai = N'Đã hoàn thành'
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
