using System.Threading.Tasks;
using System.Collections.Generic;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public class CustomerRepository
    {
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaKhachHang, TenKhachHang, SoDienThoai, DiemTichLuy, NgayTao FROM KhachHang";
            return await DbContext.Conn.QueryAsync<Customer>(sql);
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiemTichLuy, NgayTao) 
                           VALUES (@MaKhachHang, @TenKhachHang, @SoDienThoai, @DiemTichLuy, @NgayTao)";
            var rows = await DbContext.Conn.ExecuteAsync(sql, customer);
            return rows > 0;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"UPDATE KhachHang SET 
                            TenKhachHang = @TenKhachHang, 
                            SoDienThoai = @SoDienThoai, 
                            DiemTichLuy = @DiemTichLuy 
                           WHERE MaKhachHang = @MaKhachHang";
            var rows = await DbContext.Conn.ExecuteAsync(sql, customer);
            return rows > 0;
        }
    }
}
