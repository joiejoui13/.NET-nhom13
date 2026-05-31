using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO.Models;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class StockInRepository : IStockInRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<StockInReceipt>> GetAllReceiptsAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                           FROM PhieuNhap p 
                           ORDER BY p.NgayNhap DESC";
            return await DbContext.Conn.QueryAsync<StockInReceipt>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<StockInReceipt> GetReceiptByIdAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                           FROM PhieuNhap p 
                           WHERE p.MaPhieuNhap = @MaPhieuNhap";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<StockInReceipt>(sql, new { MaPhieuNhap = id });
        }

        public async Task<IEnumerable<StockInDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT c.MaSanPham, s.TenSanPham, c.SoLuong, c.DonGia AS GiaNhap 
                           FROM ChiTietNhapHang c 
                           LEFT JOIN SanPham s ON c.MaSanPham = s.MaSanPham 
                           WHERE c.MaPhieuNhap = @MaPhieuNhap";
            return await DbContext.Conn.QueryAsync<StockInDetailModel>(sql, new { MaPhieuNhap = receiptId });
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<StockInReceipt>> SearchReceiptsAsync(int? receiptId, int? userId, string status, string date)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            
            string sql = @"SELECT p.MaPhieuNhap, p.MaNguoiDung, p.NgayNhap, p.TrangThai, p.TongTien 
                           FROM PhieuNhap p 
                           WHERE 1=1";
            var parameters = new DynamicParameters();

            if (receiptId.HasValue)
            {
                sql += " AND p.MaPhieuNhap = @MaPhieuNhap";
                parameters.Add("MaPhieuNhap", receiptId.Value);
            }
            if (userId.HasValue)
            {
                sql += " AND p.MaNguoiDung = @MaNguoiDung";
                parameters.Add("MaNguoiDung", userId.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND p.TrangThai = @TrangThai";
                parameters.Add("TrangThai", status);
            }
            if (!string.IsNullOrEmpty(date))
            {
                sql += " AND CAST(p.NgayNhap AS DATE) = @NgayNhap";
                parameters.Add("NgayNhap", date);
            }

            sql += " ORDER BY p.NgayNhap DESC";

            return await DbContext.Conn.QueryAsync<StockInReceipt>(sql, parameters);
        }

        public async Task<int> SaveReceiptWithTransactionAsync(StockInReceipt receipt, List<StockInDetailModel> details, bool isAddingNew)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    int receiptId = receipt.MaPhieuNhap;

                    if (isAddingNew)
                    {
                        string insertMasterSql = @"INSERT INTO PhieuNhap (MaNguoiDung, TongTien, TrangThai, NgayNhap) 
                                                   OUTPUT INSERTED.MaPhieuNhap 
                                                   VALUES (@MaNguoiDung, @TongTien, @TrangThai, @NgayNhap)";
                        receiptId = await DbContext.Conn.QuerySingleAsync<int>(insertMasterSql, receipt, transaction);
                    }
                    else
                    {
                        string updateMasterSql = @"UPDATE PhieuNhap 
                                                   SET MaNguoiDung = @MaNguoiDung, NgayNhap = @NgayNhap, 
                                                       TrangThai = @TrangThai, TongTien = @TongTien 
                                                   WHERE MaPhieuNhap = @MaPhieuNhap";
                        await DbContext.Conn.ExecuteAsync(updateMasterSql, receipt, transaction);

                        string delOldDetailsSql = "DELETE FROM ChiTietNhapHang WHERE MaPhieuNhap = @MaPhieuNhap";
                        await DbContext.Conn.ExecuteAsync(delOldDetailsSql, new { MaPhieuNhap = receiptId }, transaction);
                    }

                    // Insert details
                    foreach (var d in details)
                    {
                        string insertDetailSql = @"INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSanPham, SoLuong, DonGia) 
                                                   VALUES (@MaPhieuNhap, @MaSanPham, @SoLuong, @GiaNhap)";
                        await DbContext.Conn.ExecuteAsync(insertDetailSql, 
                            new { MaPhieuNhap = receiptId, MaSanPham = d.MaSanPham, SoLuong = d.SoLuong, GiaNhap = d.GiaNhap }, 
                            transaction);
                    }

                    // If status is "Đã hoàn thành", update inventory
                    if (receipt.TrangThai == "Đã hoàn thành")
                    {
                        foreach (var item in details)
                        {
                            string getStockSql = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @MaSanPham";
                            int before = await DbContext.Conn.QuerySingleOrDefaultAsync<int>(getStockSql, new { MaSanPham = item.MaSanPham }, transaction);
                            int after = before + item.SoLuong;

                            string updateStockSql = "UPDATE SanPham SET SoLuongTon = @After WHERE MaSanPham = @MaSanPham";
                            await DbContext.Conn.ExecuteAsync(updateStockSql, new { After = after, MaSanPham = item.MaSanPham }, transaction);

                            string insertLogSql = @"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                                    VALUES (@MaSanPham, GETDATE(), @ThayDoi, @SoLuongTruoc, @SoLuongSau, N'Nhập kho', @MaThamChieu, N'Hoàn thành')";
                            await DbContext.Conn.ExecuteAsync(insertLogSql, new 
                            { 
                                MaSanPham = item.MaSanPham, 
                                ThayDoi = item.SoLuong, 
                                SoLuongTruoc = before, 
                                SoLuongSau = after, 
                                MaThamChieu = receiptId 
                            }, transaction);
                        }
                    }

                    transaction.Commit();
                    return receiptId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> CancelReceiptWithTransactionAsync(int receiptId)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            // First check the current status to know if we need to revert inventory
            string checkStatusSql = "SELECT TrangThai FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";
            string currentStatus = await DbContext.Conn.QuerySingleOrDefaultAsync<string>(checkStatusSql, new { MaPhieuNhap = receiptId });

            if (currentStatus == "Đã hủy") return false;

            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    string updateStatusSql = "UPDATE PhieuNhap SET TrangThai = N'Đã hủy' WHERE MaPhieuNhap = @MaPhieuNhap";
                    await DbContext.Conn.ExecuteAsync(updateStatusSql, new { MaPhieuNhap = receiptId }, transaction);

                    if (currentStatus == "Đã hoàn thành")
                    {
                        // Get details to revert
                        string getDetailsSql = "SELECT MaSanPham, SoLuong FROM ChiTietNhapHang WHERE MaPhieuNhap = @MaPhieuNhap";
                        var details = await DbContext.Conn.QueryAsync<StockInDetailModel>(getDetailsSql, new { MaPhieuNhap = receiptId }, transaction);

                        foreach (var item in details)
                        {
                            string getStockSql = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @MaSanPham";
                            int before = await DbContext.Conn.QuerySingleOrDefaultAsync<int>(getStockSql, new { MaSanPham = item.MaSanPham }, transaction);
                            int after = before - item.SoLuong;
                            if (after < 0) throw new Exception($"Hủy phiếu sẽ làm âm kho sản phẩm mã {item.MaSanPham}. Tồn kho hiện tại: {before}."); // Ngăn chặn âm kho

                            string updateStockSql = "UPDATE SanPham SET SoLuongTon = @After WHERE MaSanPham = @MaSanPham";
                            await DbContext.Conn.ExecuteAsync(updateStockSql, new { After = after, MaSanPham = item.MaSanPham }, transaction);

                            string insertLogSql = @"INSERT INTO LichSuNhapKho (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai) 
                                                    VALUES (@MaSanPham, GETDATE(), @ThayDoi, @SoLuongTruoc, @SoLuongSau, N'Nhập kho', @MaThamChieu, N'Hủy bỏ')";
                            await DbContext.Conn.ExecuteAsync(insertLogSql, new 
                            { 
                                MaSanPham = item.MaSanPham, 
                                ThayDoi = -item.SoLuong, 
                                SoLuongTruoc = before, 
                                SoLuongSau = after, 
                                MaThamChieu = receiptId 
                            }, transaction);
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
