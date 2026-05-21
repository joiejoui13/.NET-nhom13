using System.Threading.Tasks;
using System.Collections.Generic;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class OrderDetailRepository
    {
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string maHoaDon)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
            return await DbContext.Conn.QueryAsync<OrderDetail>(sql, new { MaHoaDon = maHoaDon });
        }

        public async Task<bool> AddOrderDetailAsync(OrderDetail detail)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) 
                           VALUES (@MaChiTiet, @MaHoaDon, @MaSanPham, @SoLuong, @DonGia, @ThanhTien)";
            var rows = await DbContext.Conn.ExecuteAsync(sql, detail);
            return rows > 0;
        }

        public async Task<bool> UpdateOrderDetailAsync(OrderDetail detail)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"UPDATE ChiTietHoaDon SET 
                            MaSanPham = @MaSanPham,
                            SoLuong = @SoLuong,
                            DonGia = @DonGia,
                            ThanhTien = @ThanhTien
                           WHERE MaChiTiet = @MaChiTiet";
            var rows = await DbContext.Conn.ExecuteAsync(sql, detail);
            return rows > 0;
        }

        public async Task<bool> DeleteOrderDetailAsync(string maChiTiet)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "DELETE FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
            var rows = await DbContext.Conn.ExecuteAsync(sql, new { MaChiTiet = maChiTiet });
            return rows > 0;
        }
    }
}
