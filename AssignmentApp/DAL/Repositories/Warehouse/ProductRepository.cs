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
            string sql = "SELECT * FROM tblHangHoa";
            return await DbContext.Conn.QueryAsync<Product>(sql);
        }

        // 2. Lấy sản phẩm theo mã
        public async Task<Product> GetByIdAsync(string maHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM tblHangHoa WHERE MaHang = @MaHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Product>(sql, new { MaHang = maHang });
        }

        // 3. Thêm sản phẩm mới (Dùng Parameters để chống SQL Injection)
        public async Task<int> AddAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO tblHangHoa (MaHang, TenHang, MaLoai, SoLuong, DonGiaBan) 
                           VALUES (@MaHang, @TenHang, @MaLoai, @SoLuong, @DonGiaBan)";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        // 4. Xóa sản phẩm
        public async Task<int> DeleteAsync(string maHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM tblHangHoa WHERE MaHang = @MaHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaHang = maHang });
        }
    }
}
