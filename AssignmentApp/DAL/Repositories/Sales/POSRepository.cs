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
    public class POSRepository
    {
        public async Task<bool> SaveOrderTransactionAsync(Order order, List<OrderDetail> details)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Insert HoaDon
                    string insertOrderSql = @"INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, GiamGia, MaGiaoHang, HinhThucThanhToan, NgayTao) 
                                              VALUES (@MaHoaDon, @MaKhachHang, @MaNguoiDung, @MaKhuyenMai, @TongTien, @GiamGia, @MaGiaoHang, @HinhThucThanhToan, @NgayTao)";
                    await DbContext.Conn.ExecuteAsync(insertOrderSql, order, transaction);

                    // 2. Insert ChiTietHoaDon & update stock
                    foreach (var d in details)
                    {
                        string insertDetailSql = @"INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) 
                                                   VALUES (@MaChiTiet, @MaHoaDon, @MaSanPham, @SoLuong, @DonGia, @ThanhTien)";
                        await DbContext.Conn.ExecuteAsync(insertDetailSql, d, transaction);

                        // Decrement stock
                        string updateStockSql = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaSanPham = @MaSanPham";
                        await DbContext.Conn.ExecuteAsync(updateStockSql, new { SoLuong = d.SoLuong, MaSanPham = d.MaSanPham }, transaction);

                        // Insert inventory history
                        string historyId = "LS" + Guid.NewGuid().ToString().Substring(0, 10).ToUpper();
                        string insertHistorySql = @"INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay) 
                                                    VALUES (@MaLichSu, @MaSanPham, @SoLuong, N'Xuất bán', GETDATE())";
                        await DbContext.Conn.ExecuteAsync(insertHistorySql, new { MaLichSu = historyId, MaSanPham = d.MaSanPham, SoLuong = -d.SoLuong }, transaction);
                    }

                    // 3. Update customer's accumulated points if MaKhachHang is not null/empty
                    if (!string.IsNullOrEmpty(order.MaKhachHang))
                    {
                        // Add 1 point for every 100,000 VND spent (or round down)
                        int earnedPoints = (int)(order.TongTien / 100000);
                        if (earnedPoints > 0)
                        {
                            string updatePointsSql = "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @Points WHERE MaKhachHang = @MaKhachHang";
                            await DbContext.Conn.ExecuteAsync(updatePointsSql, new { Points = earnedPoints, MaKhachHang = order.MaKhachHang }, transaction);
                        }
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
