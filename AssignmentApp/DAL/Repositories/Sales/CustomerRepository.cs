using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang";
            return await DbContext.Conn.QueryAsync<Customer>(sql);
        }

        public async Task<Customer?> GetByIdAsync(int maKhachHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Customer>(sql, new { MaKhachHang = maKhachHang });
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string tenKhachHang, string soDienThoai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            string sql = "SELECT * FROM KhachHang WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(tenKhachHang))
            {
                sql += " AND TenKhachHang LIKE @TenKhachHang";
                parameters.Add("TenKhachHang", "%" + tenKhachHang + "%");
            }
            if (!string.IsNullOrEmpty(soDienThoai))
            {
                sql += " AND SoDienThoai LIKE @SoDienThoai";
                parameters.Add("SoDienThoai", "%" + soDienThoai + "%");
            }

            return await DbContext.Conn.QueryAsync<Customer>(sql, parameters);
        }

        public async Task<int> AddAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO KhachHang (TenKhachHang, SoDienThoai, Email, DiaChi, NgayTao, TrangThai) 
                           VALUES (@TenKhachHang, @SoDienThoai, @Email, @DiaChi, @NgayTao, @TrangThai)";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> UpdateAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE KhachHang 
                           SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, Email = @Email, 
                               DiaChi = @DiaChi, TrangThai = @TrangThai 
                           WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> UpdateStatusAsync(int maKhachHang, string trangThai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE KhachHang SET TrangThai = @TrangThai WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaKhachHang = maKhachHang, TrangThai = trangThai });
        }
    }
}
