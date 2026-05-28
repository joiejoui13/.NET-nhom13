using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public class UserRepository
    {
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung";
            return await DbContext.Conn.QueryAsync<User>(sql);
        }

        public async Task<User> GetByIdAsync(string maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = maNguoiDung });
        }

        public async Task<int> AddAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                           VALUES (@MaNguoiDung, @TenNguoiDung, @SoDienThoai, @Email, @MatKhau, @VaiTro, @TrangThai, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, u);
        }

        public async Task<int> UpdateAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE NguoiDung 
                           SET TenNguoiDung = @TenNguoiDung, SoDienThoai = @SoDienThoai, Email = @Email, 
                               MatKhau = @MatKhau, VaiTro = @VaiTro, TrangThai = @TrangThai 
                           WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, u);
        }

        public async Task<int> DeleteAsync(string maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaNguoiDung = maNguoiDung });
        }
    }
}
