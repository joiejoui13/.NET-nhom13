using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class InventoryRepository : IInventoryRepository
    {
        public async Task<IEnumerable<InventoryLog>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, 
                                  l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                           FROM LichSuNhapKho l 
                           LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                           ORDER BY l.Thoigian DESC";
            return await DbContext.Conn.QueryAsync<InventoryLog>(sql);
        }

        public async Task<InventoryLog> GetByIdAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, 
                                  l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                           FROM LichSuNhapKho l 
                           LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                           WHERE l.MaLichSu = @Id";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<InventoryLog>(sql, new { Id = id });
        }

        public async Task<int> GetProductStockAsync(int productId)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @Id";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<int>(sql, new { Id = productId });
        }

        public async Task<bool> AddWithTransactionAsync(InventoryLog log)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Insert log
                    string sqlInsert = @"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                         VALUES (@MaSanPham, @Thoigian, @ThayDoi, @SoLuongTruoc, @SoLuongSau, @LoaiGiaoDich, @MaThamChieu, @TrangThai)";
                    await DbContext.Conn.ExecuteAsync(sqlInsert, log, transaction);

                    // 2. Update stock
                    string sqlUpdateStock = "UPDATE SanPham SET SoLuongTon = @SoLuongSau WHERE MaSanPham = @MaSanPham";
                    await DbContext.Conn.ExecuteAsync(sqlUpdateStock, new { SoLuongSau = log.SoLuongSau, MaSanPham = log.MaSanPham }, transaction);

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

        public async Task<bool> UpdateWithTransactionAsync(InventoryLog newLog, InventoryLog oldLog)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Revert old
                    string sqlRevert = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @ThayDoi WHERE MaSanPham = @MaSanPham";
                    await DbContext.Conn.ExecuteAsync(sqlRevert, new { ThayDoi = oldLog.ThayDoi, MaSanPham = oldLog.MaSanPham }, transaction);

                    // 2. Apply new
                    string sqlApply = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @ThayDoi WHERE MaSanPham = @MaSanPham";
                    await DbContext.Conn.ExecuteAsync(sqlApply, new { ThayDoi = newLog.ThayDoi, MaSanPham = newLog.MaSanPham }, transaction);

                    // 3. Update log
                    string sqlUpdateLog = @"UPDATE LichSuNhapKho SET 
                                            MaSanPham = @MaSanPham, 
                                            ThayDoi = @ThayDoi, 
                                            SoLuongTruoc = @SoLuongTruoc, 
                                            SoLuongSau = @SoLuongSau, 
                                            LoaiGiaoDich = @LoaiGiaoDich, 
                                            MaThamChieu = @MaThamChieu, 
                                            TrangThai = @TrangThai 
                                            WHERE MaLichSu = @MaLichSu";
                    await DbContext.Conn.ExecuteAsync(sqlUpdateLog, newLog, transaction);

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

        public async Task<bool> DeleteWithTransactionAsync(InventoryLog oldLog)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Revert stock
                    string sqlRevert = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @ThayDoi WHERE MaSanPham = @MaSanPham";
                    await DbContext.Conn.ExecuteAsync(sqlRevert, new { ThayDoi = oldLog.ThayDoi, MaSanPham = oldLog.MaSanPham }, transaction);

                    // 2. Soft delete log
                    string sqlDeleteLog = "UPDATE LichSuNhapKho SET TrangThai = N'Đã hủy' WHERE MaLichSu = @MaLichSu";
                    await DbContext.Conn.ExecuteAsync(sqlDeleteLog, new { MaLichSu = oldLog.MaLichSu }, transaction);

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

        public async Task<IEnumerable<InventoryLog>> SearchAsync(string idTerm, string refTerm, string productTerm, string typeTerm, string statusTerm)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            var parameters = new DynamicParameters();
            string sql = @"SELECT l.MaLichSu, l.MaSanPham, s.TenSanPham, l.ThayDoi, l.SoLuongTruoc, l.SoLuongSau, 
                                  l.LoaiGiaoDich, l.MaThamChieu, l.TrangThai, l.Thoigian 
                           FROM LichSuNhapKho l 
                           LEFT JOIN SanPham s ON l.MaSanPham = s.MaSanPham
                           WHERE 1=1";

            if (!string.IsNullOrEmpty(idTerm) && int.TryParse(idTerm, out int id))
            {
                sql += " AND l.MaLichSu = @Id";
                parameters.Add("Id", id);
            }
                
            if (!string.IsNullOrEmpty(refTerm) && int.TryParse(refTerm, out int refId))
            {
                sql += " AND l.MaThamChieu = @Ref";
                parameters.Add("Ref", refId);
            }

            if (!string.IsNullOrEmpty(productTerm) && int.TryParse(productTerm, out int productId))
            {
                sql += " AND l.MaSanPham = @Product";
                parameters.Add("Product", productId);
            }

            if (!string.IsNullOrEmpty(typeTerm))
            {
                sql += " AND l.LoaiGiaoDich = @Type";
                parameters.Add("Type", typeTerm);
            }

            if (!string.IsNullOrEmpty(statusTerm))
            {
                sql += " AND l.TrangThai = @Status";
                parameters.Add("Status", statusTerm);
            }

            sql += " ORDER BY l.Thoigian DESC";

            return await DbContext.Conn.QueryAsync<InventoryLog>(sql, parameters);
        }

        public async Task<System.Data.DataTable> GetProductsForComboBoxAsync()
        {
            return await Task.Run(() => 
            {
                string sql = "SELECT MaSanPham, TenSanPham FROM SanPham ORDER BY TenSanPham ASC";
                return DbContext.GetDataToTable(sql);
            });
        }
    }
}
