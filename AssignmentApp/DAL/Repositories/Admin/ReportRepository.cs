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
    }
}
