using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Security
{
    public class AuthRepository
    {
        public async Task<User> GetUserForLoginAsync(string username)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Fetch user info specifically for authentication
            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao FROM NguoiDung WHERE MaNguoiDung = @Username OR Email = @Username";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }
    }
}
