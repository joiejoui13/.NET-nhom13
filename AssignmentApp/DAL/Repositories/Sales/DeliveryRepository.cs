using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class DeliveryRepository
    {
        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang";
            return await DbContext.Conn.QueryAsync<Delivery>(sql);
        }

        public async Task<Delivery> GetByIdAsync(string maGiaoHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Delivery>(sql, new { MaGiaoHang = maGiaoHang });
        }

        public async Task<int> AddAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) 
                           VALUES (@MaGiaoHang, @MaHoaDon, @DiaChiGiao, @TrangThaiGiao, @NgayGiao)";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }

        public async Task<int> UpdateAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE GiaoHang 
                           SET MaHoaDon = @MaHoaDon, DiaChiGiao = @DiaChiGiao, 
                               TrangThaiGiao = @TrangThaiGiao, NgayGiao = @NgayGiao 
                           WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }

        public async Task<int> DeleteAsync(string maGiaoHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM GiaoHang WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaGiaoHang = maGiaoHang });
        }
    }
}
