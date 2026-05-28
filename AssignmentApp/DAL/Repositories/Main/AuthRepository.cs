using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Main
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<User> GetUserForLoginAsync(string manguoidung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Fetch user info specifically for authentication. Cho phép đăng nhập bằng Email
            string sql = "SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao FROM NguoiDung WHERE Email = @MaNguoiDung OR TRY_CAST(MaNguoiDung AS NVARCHAR(50)) = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = manguoidung });
        }
    }
}
