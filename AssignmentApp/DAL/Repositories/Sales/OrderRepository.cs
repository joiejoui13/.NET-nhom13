using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class OrderRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT h.*, k.TenKhachHang, n.TenNguoiDung 
                           FROM HoaDon h
                           LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                           LEFT JOIN NguoiDung n ON h.MaNguoiDung = n.MaNguoiDung
                           ORDER BY h.NgayTao DESC";
            return await DbContext.Conn.QueryAsync<Order>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<Order>> SearchAsync(string keyword)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT h.*, k.TenKhachHang, n.TenNguoiDung 
                           FROM HoaDon h
                           LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                           LEFT JOIN NguoiDung n ON h.MaNguoiDung = n.MaNguoiDung
                           WHERE h.MaHoaDon LIKE @Keyword 
                              OR h.MaKhachHang LIKE @Keyword 
                              OR k.TenKhachHang LIKE @Keyword
                              OR n.TenNguoiDung LIKE @Keyword
                           ORDER BY h.NgayTao DESC";
            return await DbContext.Conn.QueryAsync<Order>(sql, new { Keyword = $"%{keyword}%" });
        }

        public async Task<IEnumerable<OrderDetail>> GetDetailsAsync(string maHoaDon)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT d.*, p.TenSanPham 
                           FROM ChiTietHoaDon d
                           JOIN SanPham p ON d.MaSanPham = p.MaSanPham
                           WHERE d.MaHoaDon = @MaHoaDon";
            return await DbContext.Conn.QueryAsync<OrderDetail>(sql, new { MaHoaDon = maHoaDon });
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<bool> DeleteOrderTransactionAsync(string maHoaDon)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Get details to restore stock
                    string detailsSql = "SELECT * FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
                    var details = await DbContext.Conn.QueryAsync<OrderDetail>(detailsSql, new { MaHoaDon = maHoaDon }, transaction);

                    foreach (var d in details)
                    {
                        // Restore product stock
                        string updateStockSql = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong WHERE MaSanPham = @MaSanPham";
                        await DbContext.Conn.ExecuteAsync(updateStockSql, new { SoLuong = d.SoLuong, MaSanPham = d.MaSanPham }, transaction);

                        // Insert stock history log
                        string historyId = "LS" + Guid.NewGuid().ToString().Substring(0, 10).ToUpper();
                        string insertHistorySql = @"INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay) 
                                                    VALUES (@MaLichSu, @MaSanPham, @SoLuong, N'Hủy hóa đơn', GETDATE())";
                        await DbContext.Conn.ExecuteAsync(insertHistorySql, new { MaLichSu = historyId, MaSanPham = d.MaSanPham, SoLuong = d.SoLuong }, transaction);
                    }

                    // 2. Delete dependent records first:
                    // Delete details in return
                    string deleteReturnDetailsSql = @"DELETE d FROM ChiTietTraHang d 
                                                     JOIN TraHang t ON d.MaTraHang = t.MaTraHang 
                                                     WHERE t.MaHoaDon = @MaHoaDon";
                    await DbContext.Conn.ExecuteAsync(deleteReturnDetailsSql, new { MaHoaDon = maHoaDon }, transaction);

                    string deleteReturnSql = "DELETE FROM TraHang WHERE MaHoaDon = @MaHoaDon";
                    await DbContext.Conn.ExecuteAsync(deleteReturnSql, new { MaHoaDon = maHoaDon }, transaction);

                    // Delete delivery
                    string deleteDeliverySql = "DELETE FROM GiaoHang WHERE MaHoaDon = @MaHoaDon";
                    await DbContext.Conn.ExecuteAsync(deleteDeliverySql, new { MaHoaDon = maHoaDon }, transaction);

                    // Delete order details
                    string deleteOrderDetailsSql = "DELETE FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
                    await DbContext.Conn.ExecuteAsync(deleteOrderDetailsSql, new { MaHoaDon = maHoaDon }, transaction);

                    // 3. Delete order
                    string deleteOrderSql = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
                    int affected = await DbContext.Conn.ExecuteAsync(deleteOrderSql, new { MaHoaDon = maHoaDon }, transaction);

                    transaction.Commit();
                    return affected > 0;
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
