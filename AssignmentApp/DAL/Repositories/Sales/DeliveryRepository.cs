using System.Threading.Tasks;
using System.Collections.Generic;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class DeliveryRepository
    {
        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao FROM GiaoHang";
            return await DbContext.Conn.QueryAsync<Delivery>(sql);
        }

        public async Task<bool> AddDeliveryAsync(Delivery delivery)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) 
                           VALUES (@MaGiaoHang, @MaHoaDon, @DiaChiGiao, @TrangThaiGiao, @NgayGiao)";
            var rows = await DbContext.Conn.ExecuteAsync(sql, delivery);
            return rows > 0;
        }

        public async Task<bool> UpdateDeliveryAsync(Delivery delivery)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"UPDATE GiaoHang SET 
                            MaHoaDon = @MaHoaDon, 
                            DiaChiGiao = @DiaChiGiao, 
                            TrangThaiGiao = @TrangThaiGiao,
                            NgayGiao = @NgayGiao
                           WHERE MaGiaoHang = @MaGiaoHang";
            var rows = await DbContext.Conn.ExecuteAsync(sql, delivery);
            return rows > 0;
        }
    }
}
