using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class ProductRepository
    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM SanPham";
            return await DbContext.Conn.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(string maSanPham)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM SanPham WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Product>(sql, new { MaSanPham = maSanPham });
        }

        public async Task<int> AddAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaBan, GiaNhap, SoLuongTon, MoTa, TrangThai, NgayTao) 
                           VALUES (@MaSanPham, @TenSanPham, @MaDanhMuc, @GiaBan, @GiaNhap, @SoLuongTon, @MoTa, @TrangThai, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> UpdateAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE SanPham 
                           SET TenSanPham = @TenSanPham, MaDanhMuc = @MaDanhMuc, GiaBan = @GiaBan, 
                               GiaNhap = @GiaNhap, SoLuongTon = @SoLuongTon, MoTa = @MoTa, 
                               TrangThai = @TrangThai 
                           WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> DeleteAsync(string maSanPham)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM SanPham WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaSanPham = maSanPham });
        }
    }
}
