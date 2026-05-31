using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class DeliveryRepository : IDeliveryRepository
    {
        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang";
            return await DbContext.Conn.QueryAsync<Delivery>(sql);
        }

        public async Task<Delivery?> GetByIdAsync(int maGiaoHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Delivery>(sql, new { MaGiaoHang = maGiaoHang });
        }

        public async Task<IEnumerable<Delivery>> SearchAsync(int? maGiaoHang, int? maHoaDon, int? maTraHang, string? trangThai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            string sql = "SELECT * FROM GiaoHang WHERE 1=1";
            var parameters = new DynamicParameters();

            if (maGiaoHang.HasValue)
            {
                sql += " AND MaGiaoHang = @MaGiaoHang";
                parameters.Add("MaGiaoHang", maGiaoHang.Value);
            }
            if (maHoaDon.HasValue)
            {
                sql += " AND MaHoaDon = @MaHoaDon";
                parameters.Add("MaHoaDon", maHoaDon.Value);
            }
            if (maTraHang.HasValue)
            {
                sql += " AND MaTraHang = @MaTraHang";
                parameters.Add("MaTraHang", maTraHang.Value);
            }
            if (!string.IsNullOrEmpty(trangThai))
            {
                sql += " AND TrangThaiGiao = @TrangThaiGiao";
                parameters.Add("TrangThaiGiao", trangThai);
            }

            return await DbContext.Conn.QueryAsync<Delivery>(sql, parameters);
        }

        public async Task<int> AddAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO GiaoHang (MaHoaDon, MaTraHang, DiaChiGiao, TrangThaiGiao, NgayGiao) 
                           VALUES (@MaHoaDon, @MaTraHang, @DiaChiGiao, @TrangThaiGiao, @NgayGiao)";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }

        public async Task<int> UpdateAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE GiaoHang 
                           SET MaHoaDon = @MaHoaDon, MaTraHang = @MaTraHang, DiaChiGiao = @DiaChiGiao, 
                               TrangThaiGiao = @TrangThaiGiao, NgayGiao = @NgayGiao 
                           WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }

        public async Task<int> UpdateStatusAsync(int maGiaoHang, string trangThai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE GiaoHang SET TrangThaiGiao = @TrangThaiGiao WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaGiaoHang = maGiaoHang, TrangThaiGiao = trangThai });
        }
    }
}
