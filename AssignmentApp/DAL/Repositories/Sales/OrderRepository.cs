using System.Threading.Tasks;
using System.Collections.Generic;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class OrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, GiamGia, MaGiaoHang, HinhThucThanhToan, NgayTao FROM HoaDon ORDER BY NgayTao DESC";
            return await DbContext.Conn.QueryAsync<Order>(sql);
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, GiamGia, MaGiaoHang, HinhThucThanhToan, NgayTao) 
                           VALUES (@MaHoaDon, @MaKhachHang, @MaNguoiDung, @MaKhuyenMai, @TongTien, @GiamGia, @MaGiaoHang, @HinhThucThanhToan, @NgayTao)";
            var rows = await DbContext.Conn.ExecuteAsync(sql, order);
            return rows > 0;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"UPDATE HoaDon SET 
                            MaKhachHang = @MaKhachHang,
                            MaNguoiDung = @MaNguoiDung,
                            MaKhuyenMai = @MaKhuyenMai,
                            GiamGia = @GiamGia,
                            MaGiaoHang = @MaGiaoHang,
                            HinhThucThanhToan = @HinhThucThanhToan
                           WHERE MaHoaDon = @MaHoaDon";
            var rows = await DbContext.Conn.ExecuteAsync(sql, order);
            return rows > 0;
        }

        public async Task<bool> UpdateOrderTotalAsync(string maHoaDon, decimal tongTien)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "UPDATE HoaDon SET TongTien = @TongTien WHERE MaHoaDon = @MaHoaDon";
            var rows = await DbContext.Conn.ExecuteAsync(sql, new { TongTien = tongTien, MaHoaDon = maHoaDon });
            return rows > 0;
        }

        public async Task<bool> DeleteOrderAsync(string maHoaDon)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            // Note: Cascade delete might need to delete ChiTietHoaDon first, but we handle that in the Service or here
            string sqlDetails = "DELETE FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
            await DbContext.Conn.ExecuteAsync(sqlDetails, new { MaHoaDon = maHoaDon });

            string sql = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            var rows = await DbContext.Conn.ExecuteAsync(sql, new { MaHoaDon = maHoaDon });
            return rows > 0;
        }
    }
}
