using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class OrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            if (AssignmentApp.DAL.Core.DbContext.Conn == null || AssignmentApp.DAL.Core.DbContext.Conn.State == System.Data.ConnectionState.Closed) AssignmentApp.DAL.Core.DbContext.Ketnoi();
            string sql = "SELECT MaHoaDon AS MaHD, MaNguoiDung AS MaNV, MaKhachHang AS MaKH, NgayTao AS NgayBan, TongTien, TrangThai FROM HoaDon";
            return await Dapper.SqlMapper.QueryAsync<Order>(AssignmentApp.DAL.Core.DbContext.Conn, sql);
        }

        public async Task<Order> GetByIdAsync(string maHD)
        {
            if (AssignmentApp.DAL.Core.DbContext.Conn == null || AssignmentApp.DAL.Core.DbContext.Conn.State == System.Data.ConnectionState.Closed) AssignmentApp.DAL.Core.DbContext.Ketnoi();
            string sql = "SELECT MaHoaDon AS MaHD, MaNguoiDung AS MaNV, MaKhachHang AS MaKH, NgayTao AS NgayBan, TongTien, TrangThai FROM HoaDon WHERE MaHoaDon = @MaHD";
            return await Dapper.SqlMapper.QuerySingleOrDefaultAsync<Order>(AssignmentApp.DAL.Core.DbContext.Conn, sql, new { MaHD = maHD });
        }

        public async Task<int> AddAsync(Order o)
        {
            if (AssignmentApp.DAL.Core.DbContext.Conn == null || AssignmentApp.DAL.Core.DbContext.Conn.State == System.Data.ConnectionState.Closed) AssignmentApp.DAL.Core.DbContext.Ketnoi();
            string sql = @"INSERT INTO HoaDon (MaHoaDon, MaNguoiDung, MaKhachHang, NgayTao, TongTien, TrangThai) 
                           VALUES (@MaHD, @MaNV, @MaKH, @NgayBan, @TongTien, @TrangThai)";
            return await Dapper.SqlMapper.ExecuteAsync(AssignmentApp.DAL.Core.DbContext.Conn, sql, o);
        }
    }
}
