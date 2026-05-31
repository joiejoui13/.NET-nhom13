using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class ReturnRepository : IReturnRepository
    {
        private void EnsureConnection()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }
        }

        public async Task<IEnumerable<Return>> GetAllReturnsAsync()
        {
            EnsureConnection();
            string sql = @"
                SELECT th.*, nd.TenNguoiDung AS NhanVien, kh.TenKhachHang AS KhachHang
                FROM TraHang th
                JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
                JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                ORDER BY th.MaTraHang DESC";
            return await DbContext.Conn.QueryAsync<Return>(sql);
        }

        public async Task<IEnumerable<Return>> SearchReturnsAsync(string maHD, string khach, string nhanVien, string lydo, string trangThai, string loaiGD, decimal? tongTien, DateTime? ngayTra)
        {
            EnsureConnection();
            string sql = @"
                SELECT th.*, nd.TenNguoiDung AS NhanVien, kh.TenKhachHang AS KhachHang
                FROM TraHang th
                JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
                JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE 1=1 ";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(maHD))
            {
                sql += " AND CAST(th.MaHoaDon AS NVARCHAR) LIKE @maHD ";
                parameters.Add("maHD", $"%{maHD}%");
            }
            if (!string.IsNullOrEmpty(khach))
            {
                sql += " AND kh.TenKhachHang LIKE @khach ";
                parameters.Add("khach", $"%{khach}%");
            }
            if (!string.IsNullOrEmpty(nhanVien))
            {
                sql += " AND nd.TenNguoiDung LIKE @nhanVien ";
                parameters.Add("nhanVien", $"%{nhanVien}%");
            }
            if (!string.IsNullOrEmpty(lydo))
            {
                sql += " AND th.LyDo LIKE @lydo ";
                parameters.Add("lydo", $"%{lydo}%");
            }
            if (!string.IsNullOrEmpty(trangThai))
            {
                sql += " AND th.TrangThai = @trangThai ";
                parameters.Add("trangThai", trangThai);
            }
            if (!string.IsNullOrEmpty(loaiGD))
            {
                sql += " AND th.LoaiGiaoDich = @loaiGD ";
                parameters.Add("loaiGD", loaiGD);
            }
            if (tongTien.HasValue)
            {
                sql += " AND th.TongTienHoan = @tongTien ";
                parameters.Add("tongTien", tongTien.Value);
            }
            if (ngayTra.HasValue)
            {
                sql += " AND CAST(th.NgayTra AS DATE) = @ngayTra ";
                parameters.Add("ngayTra", ngayTra.Value.Date);
            }

            sql += " ORDER BY th.MaTraHang DESC";
            return await DbContext.Conn.QueryAsync<Return>(sql, parameters);
        }

        public async Task<IEnumerable<ReturnInvoiceProduct>> GetInvoiceProductsAsync(int maHoaDon)
        {
            EnsureConnection();
            string sql = @"
                SELECT cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong AS SLMua,
                       ISNULL(SUM(ctth.SoLuong), 0) AS DaTra, cthd.DonGia, sp.Anh
                FROM ChiTietHoaDon cthd
                JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                LEFT JOIN ChiTietTraHang ctth ON ctth.MaSanPham = cthd.MaSanPham
                     AND ctth.MaTraHang IN (SELECT MaTraHang FROM TraHang WHERE MaHoaDon = @maHoaDon)
                WHERE cthd.MaHoaDon = @maHoaDon
                GROUP BY cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.Anh";
            return await DbContext.Conn.QueryAsync<ReturnInvoiceProduct>(sql, new { maHoaDon });
        }

        public async Task<IEnumerable<ReturnDetail>> GetReturnDetailsAsync(int maTraHang)
        {
            EnsureConnection();
            string sql = @"
                SELECT ctth.MaSanPham, sp.TenSanPham, ctth.SoLuong, ctth.TienHoan, ctth.TinhTrang
                FROM ChiTietTraHang ctth
                JOIN SanPham sp ON ctth.MaSanPham = sp.MaSanPham
                WHERE ctth.MaTraHang = @maTraHang";
            
            // Note: DonGia can be computed from TienHoan / SoLuong in the UI or mapped directly if fetched
            var details = await DbContext.Conn.QueryAsync<ReturnDetail>(sql, new { maTraHang });
            foreach (var d in details)
            {
                if (d.SoLuong > 0) d.DonGia = d.TienHoan / d.SoLuong;
                else d.DonGia = 0;
            }
            return details;
        }

        public async Task<string?> GetCustomerNameByInvoiceAsync(int maHoaDon)
        {
            EnsureConnection();
            string sql = @"
                SELECT kh.TenKhachHang
                FROM HoaDon hd
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE hd.MaHoaDon = @maHoaDon";
            return await DbContext.Conn.QueryFirstOrDefaultAsync<string>(sql, new { maHoaDon });
        }

        public async Task<int> CreateReturnAsync(Return r)
        {
            EnsureConnection();
            string sql = @"
                INSERT INTO TraHang(MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, TrangThai, NgayTra, LoaiGiaoDich)
                VALUES(@MaHoaDon, @MaNguoiDung, @LyDo, @TongTienHoan, @TrangThai, @NgayTra, @LoaiGiaoDich);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await DbContext.Conn.ExecuteScalarAsync<int>(sql, r);
        }

        public async Task<bool> UpdateReturnAsync(Return r)
        {
            EnsureConnection();
            string sql = @"
                UPDATE TraHang SET
                    LyDo = @LyDo,
                    TrangThai = @TrangThai,
                    LoaiGiaoDich = @LoaiGiaoDich
                WHERE MaTraHang = @MaTraHang";
            var rows = await DbContext.Conn.ExecuteAsync(sql, r);
            return rows > 0;
        }

        public async Task<bool> DeleteReturnTransactionAsync(int maTraHang)
        {
            EnsureConnection();
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Phục hồi số lượng tồn kho
                    string sqlKhoiPhucTon = @"
                        UPDATE SanPham
                        SET SoLuongTon = SoLuongTon - ct.SoLuong
                        FROM SanPham sp
                        JOIN ChiTietTraHang ct ON sp.MaSanPham = ct.MaSanPham
                        WHERE ct.MaTraHang = @maTraHang";
                    await DbContext.Conn.ExecuteAsync(sqlKhoiPhucTon, new { maTraHang }, transaction);

                    // 2. Xóa các chi tiết trả hàng
                    string sqlXoaCT = "DELETE FROM ChiTietTraHang WHERE MaTraHang = @maTraHang";
                    await DbContext.Conn.ExecuteAsync(sqlXoaCT, new { maTraHang }, transaction);

                    // 3. Xóa phiếu trả hàng
                    string sqlXoaPhieu = "DELETE FROM TraHang WHERE MaTraHang = @maTraHang";
                    var rows = await DbContext.Conn.ExecuteAsync(sqlXoaPhieu, new { maTraHang }, transaction);

                    transaction.Commit();
                    return rows > 0;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> SaveReturnDetailsTransactionAsync(int maTraHang, List<ReturnDetail> details, decimal tongTienHoanThucTe, string loaiGiaoDich)
        {
            EnsureConnection();
            using (var transaction = DbContext.Conn.BeginTransaction())
            {
                try
                {
                    // 1. Phục hồi tồn kho của các chi tiết cũ trước khi xóa
                    string sqlKhoiPhucTon = @"
                        UPDATE SanPham
                        SET SoLuongTon = SoLuongTon - ct.SoLuong
                        FROM SanPham sp
                        JOIN ChiTietTraHang ct ON sp.MaSanPham = ct.MaSanPham
                        WHERE ct.MaTraHang = @maTraHang";
                    await DbContext.Conn.ExecuteAsync(sqlKhoiPhucTon, new { maTraHang }, transaction);

                    // 2. Xóa sạch chi tiết trả hàng cũ
                    string sqlXoaCT = "DELETE FROM ChiTietTraHang WHERE MaTraHang = @maTraHang";
                    await DbContext.Conn.ExecuteAsync(sqlXoaCT, new { maTraHang }, transaction);

                    // 3. Chèn lại các chi tiết mới và cập nhật tồn kho mới
                    foreach (var d in details)
                    {
                        string sqlThemCT = @"
                            INSERT INTO ChiTietTraHang(MaTraHang, MaSanPham, SoLuong, TinhTrang, TienHoan)
                            VALUES(@MaTraHang, @MaSanPham, @SoLuong, @TinhTrang, @TienHoan)";
                        await DbContext.Conn.ExecuteAsync(sqlThemCT, new
                        {
                            MaTraHang = maTraHang,
                            MaSanPham = d.MaSanPham,
                            SoLuong = d.SoLuong,
                            TinhTrang = d.TinhTrang,
                            TienHoan = d.TienHoan
                        }, transaction);

                        string sqlCapNhatKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong WHERE MaSanPham = @MaSanPham";
                        await DbContext.Conn.ExecuteAsync(sqlCapNhatKho, new { SoLuong = d.SoLuong, MaSanPham = d.MaSanPham }, transaction);
                    }

                    // 4. Cập nhật lại tổng tiền cho phiếu trả
                    string sqlCapNhatPhieu = @"
                        UPDATE TraHang SET
                            TongTienHoan = @TongTienHoan,
                            NgayTra = GETDATE()
                        WHERE MaTraHang = @maTraHang";
                    await DbContext.Conn.ExecuteAsync(sqlCapNhatPhieu, new { TongTienHoan = tongTienHoanThucTe, maTraHang = maTraHang }, transaction);

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
