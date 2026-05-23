using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class ProductRepository
    {
        // 1. Lấy danh sách tất cả sản phẩm
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT MaSanPham AS MaHang, TenSanPham AS TenHang, MaDanhMuc AS MaLoai, GiaNhap AS DonGiaNhap, GiaBan AS DonGiaBan, SoLuongTon AS SoLuong, MoTa AS GhiChu, Anh FROM SanPham";
            return await DbContext.Conn.QueryAsync<Product>(sql);
        }

        // 2. Lấy sản phẩm theo mã
        public async Task<Product> GetByIdAsync(string maHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT MaSanPham AS MaHang, TenSanPham AS TenHang, MaDanhMuc AS MaLoai, GiaNhap AS DonGiaNhap, GiaBan AS DonGiaBan, SoLuongTon AS SoLuong, MoTa AS GhiChu, Anh FROM SanPham WHERE MaSanPham = @MaHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Product>(sql, new { MaHang = maHang });
        }

        // 3. Thêm sản phẩm mới (Dùng Parameters để chống SQL Injection)
        public async Task<int> AddAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, SoLuongTon, GiaNhap, GiaBan, MoTa, Anh) 
                           VALUES (@MaHang, @TenHang, @MaLoai, @SoLuong, @DonGiaNhap, @DonGiaBan, @GhiChu, @Anh)";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        // 4. Xóa sản phẩm
        public async Task<int> DeleteAsync(string maHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM SanPham WHERE MaSanPham = @MaHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaHang = maHang });
        }

        // 5. Cập nhật ảnh sản phẩm
        public async Task<int> UpdateImageAsync(string maHang, string anh)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE SanPham SET Anh = @Anh WHERE MaSanPham = @MaHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaHang = maHang, Anh = anh });
        }
    }
}
