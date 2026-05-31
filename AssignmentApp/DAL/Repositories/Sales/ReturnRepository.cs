using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class ReturnRepository
    {
        public async Task<IEnumerable<Return>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM TraHang ORDER BY NgayTra DESC";
            return await DbContext.Conn.QueryAsync<Return>(sql);
        }

        public async Task<IEnumerable<ReturnDetail>> GetDetailsAsync(string maTraHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT d.*, p.TenSanPham 
                           FROM ChiTietTraHang d 
                           JOIN SanPham p ON d.MaSanPham = p.MaSanPham 
                           WHERE d.MaTraHang = @MaTraHang";
            return await DbContext.Conn.QueryAsync<ReturnDetail>(sql, new { MaTraHang = maTraHang });
        }

        public async Task<bool> SaveReturnTransactionAsync(Return r, List<ReturnDetail> details)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Insert TraHang Master
                    string insertMasterSql = @"INSERT INTO TraHang (MaTraHang, MaHoaDon, NgayTra, LyDo, TongTienHoan, MaNguoiDung) 
                                               VALUES (@MaTraHang, @MaHoaDon, @NgayTra, @LyDo, @TongTienHoan, @MaNguoiDung)";
                    await DbContext.Conn.ExecuteAsync(insertMasterSql, r, transaction);

                    // 2. Insert Details, Update Stock, Insert Stock History
                    foreach (var d in details)
                    {
                        // a. Insert ChiTietTraHang
                        string insertDetailSql = @"INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan) 
                                                   VALUES (@MaChiTietTra, @MaTraHang, @MaSanPham, @SoLuong, @TienHoan)";
                        await DbContext.Conn.ExecuteAsync(insertDetailSql, d, transaction);

                        // b. Increment SanPham.SoLuongTon
                        string updateStockSql = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong WHERE MaSanPham = @MaSanPham";
                        await DbContext.Conn.ExecuteAsync(updateStockSql, new { SoLuong = d.SoLuong, MaSanPham = d.MaSanPham }, transaction);

                        // c. Insert LichSuTonKho
                        string historyId = "LS" + Guid.NewGuid().ToString().Substring(0, 10).ToUpper();
                        string insertHistorySql = @"INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay) 
                                                    VALUES (@MaLichSu, @MaSanPham, @SoLuong, N'Nhập trả hàng', GETDATE())";
                        await DbContext.Conn.ExecuteAsync(insertHistorySql, new { MaLichSu = historyId, MaSanPham = d.MaSanPham, SoLuong = d.SoLuong }, transaction);
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
