using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class CustomerRepository
    {
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang";
            return await DbContext.Conn.QueryAsync<Customer>(sql);
        }

        public async Task<Customer> GetByIdAsync(string maKhachHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Customer>(sql, new { MaKhachHang = maKhachHang });
        }

        public async Task<int> AddAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiemTichLuy, NgayTao) 
                           VALUES (@MaKhachHang, @TenKhachHang, @SoDienThoai, @DiemTichLuy, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> UpdateAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE KhachHang 
                           SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, DiemTichLuy = @DiemTichLuy 
                           WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> DeleteAsync(string maKhachHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaKhachHang = maKhachHang });
        }
    }
}
