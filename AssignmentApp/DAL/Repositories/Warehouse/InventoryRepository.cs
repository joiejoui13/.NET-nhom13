using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class InventoryRepository
    {
        public async Task<IEnumerable<InventoryLog>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT l.MaLichSu, l.MaSanPham, l.SoLuongThayDoi, l.Loai, l.Ngay, s.TenSanPham 
                FROM LichSuTonKho l
                LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                ORDER BY l.Ngay DESC";
            return await DbContext.Conn.QueryAsync<InventoryLog>(sql);
        }

        public async Task<InventoryLog> GetByIdAsync(string maLichSu)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"
                SELECT l.MaLichSu, l.MaSanPham, l.SoLuongThayDoi, l.Loai, l.Ngay, s.TenSanPham 
                FROM LichSuTonKho l
                LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                WHERE l.MaLichSu = @MaLichSu";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<InventoryLog>(sql, new { MaLichSu = maLichSu });
        }

        public async Task<int> AddAsync(InventoryLog log)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Insert into LichSuTonKho
                    string sqlInsert = @"
                        INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay) 
                        VALUES (@MaLichSu, @MaSanPham, @SoLuongThayDoi, @Loai, @Ngay)";
                    int res1 = await DbContext.Conn.ExecuteAsync(sqlInsert, log, transaction);

                    // 2. Update SanPham SoLuongTon
                    string sqlUpdate = @"
                        UPDATE SanPham 
                        SET SoLuongTon = SoLuongTon + @SoLuongThayDoi 
                        WHERE MaSanPham = @MaSanPham";
                    int res2 = await DbContext.Conn.ExecuteAsync(sqlUpdate, new { SoLuongThayDoi = log.SoLuongThayDoi, MaSanPham = log.MaSanPham }, transaction);

                    transaction.Commit();
                    return res1 > 0 && res2 > 0 ? 1 : 0;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> DeleteAsync(string maLichSu)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            
            // Standard historical delete
            string sql = "DELETE FROM LichSuTonKho WHERE MaLichSu = @MaLichSu";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaLichSu = maLichSu });
        }
    }
}
