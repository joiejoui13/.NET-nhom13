using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public class PromotionRepository
    {
        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhuyenMai";
            return await DbContext.Conn.QueryAsync<Promotion>(sql);
        }

        public async Task<Promotion> GetByIdAsync(string maKhuyenMai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhuyenMai WHERE MaKhuyenMai = @MaKhuyenMai";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Promotion>(sql, new { MaKhuyenMai = maKhuyenMai });
        }

        public async Task<int> AddAsync(Promotion p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayHetHan, MoTaKhuyenMai, TrangThai) 
                           VALUES (@MaKhuyenMai, @TenKhuyenMai, @PhanTramGiamGia, @NgayBatDau, @NgayHetHan, @MoTaKhuyenMai, @TrangThai)";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> UpdateAsync(Promotion p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE KhuyenMai 
                           SET TenKhuyenMai = @TenKhuyenMai, PhanTramGiamGia = @PhanTramGiamGia, 
                               NgayBatDau = @NgayBatDau, NgayHetHan = @NgayHetHan, 
                               MoTaKhuyenMai = @MoTaKhuyenMai, TrangThai = @TrangThai 
                           WHERE MaKhuyenMai = @MaKhuyenMai";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> DeleteAsync(string maKhuyenMai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM KhuyenMai WHERE MaKhuyenMai = @MaKhuyenMai";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaKhuyenMai = maKhuyenMai });
        }
    }
}
