using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public class UserRepository
    {
        public async Task<User> GetUserByMaNguoiDungAsync(string maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = maNguoiDung });
        }

        public async Task<System.Collections.Generic.IEnumerable<User>> GetAllUsersAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao FROM NguoiDung";
            return await DbContext.Conn.QueryAsync<User>(sql);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                           VALUES (@MaNguoiDung, @TenNguoiDung, @SoDienThoai, @Email, @MatKhau, @VaiTro, @TrangThai, @NgayTao)";
            var rows = await DbContext.Conn.ExecuteAsync(sql, user);
            return rows > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"UPDATE NguoiDung SET 
                            TenNguoiDung = @TenNguoiDung, 
                            SoDienThoai = @SoDienThoai, 
                            Email = @Email, 
                            VaiTro = @VaiTro, 
                            TrangThai = @TrangThai 
                           WHERE MaNguoiDung = @MaNguoiDung";
            var rows = await DbContext.Conn.ExecuteAsync(sql, user);
            return rows > 0;
        }

        public async Task<bool> ToggleUserStatusAsync(string maNguoiDung, string newStatus)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            string sql = "UPDATE NguoiDung SET TrangThai = @TrangThai WHERE MaNguoiDung = @MaNguoiDung";
            var rows = await DbContext.Conn.ExecuteAsync(sql, new { TrangThai = newStatus, MaNguoiDung = maNguoiDung });
            return rows > 0;
        }
    }
}
