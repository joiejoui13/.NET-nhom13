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
    public class StockInRepository
    {
        public async Task<StockIn> GetByIdAsync(string maPhieuNhap)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<StockIn>(sql, new { MaPhieuNhap = maPhieuNhap });
        }

        public async Task<int> AddAsync(StockIn master, List<StockInDetail> details)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Insert header (PhieuNhap)
                    string sqlMaster = @"
                        INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, NgayNhap, TongTien)
                        VALUES (@MaPhieuNhap, @MaNguoiDung, @NgayNhap, @TongTien)";
                    
                    int resMaster = await DbContext.Conn.ExecuteAsync(sqlMaster, master, transaction);

                    // 2. Insert details and update inventory
                    foreach (var detail in details)
                    {
                        // Ensure ID is generated for detail
                        if (string.IsNullOrEmpty(detail.MaChiTietPhieuNhap))
                        {
                            detail.MaChiTietPhieuNhap = "CTPN_" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
                        }
                        detail.MaPhieuNhap = master.MaPhieuNhap;

                        // Insert detail (ChiTietPhieuNhap)
                        string sqlDetail = @"
                            INSERT INTO ChiTietPhieuNhap (MaChiTietPhieuNhap, MaPhieuNhap, MaSanPham, SoLuong, GiaNhap)
                            VALUES (@MaChiTietPhieuNhap, @MaPhieuNhap, @MaSanPham, @SoLuong, @GiaNhap)";
                        
                        await DbContext.Conn.ExecuteAsync(sqlDetail, detail, transaction);

                        // Update product stock (SanPham.SoLuongTon) and cost (SanPham.GiaNhap = detail.GiaNhap)
                        string sqlUpdateProduct = @"
                            UPDATE SanPham
                            SET SoLuongTon = SoLuongTon + @SoLuong,
                                GiaNhap = @GiaNhap
                            WHERE MaSanPham = @MaSanPham";
                        
                        await DbContext.Conn.ExecuteAsync(sqlUpdateProduct, new { 
                            SoLuong = detail.SoLuong, 
                            GiaNhap = detail.GiaNhap, 
                            MaSanPham = detail.MaSanPham 
                        }, transaction);

                        // Insert inventory log (LichSuTonKho) for traceability
                        string maLichSu = "LS_" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
                        string sqlInventoryLog = @"
                            INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay)
                            VALUES (@MaLichSu, @MaSanPham, @SoLuongThayDoi, @Loai, @Ngay)";
                        
                        await DbContext.Conn.ExecuteAsync(sqlInventoryLog, new {
                            MaLichSu = maLichSu,
                            MaSanPham = detail.MaSanPham,
                            SoLuongThayDoi = detail.SoLuong,
                            Loai = "Nhập kho",
                            Ngay = master.NgayNhap
                        }, transaction);
                    }

                    transaction.Commit();
                    return 1;
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
