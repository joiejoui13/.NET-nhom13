using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Security
{
    public class AuthRepository
    {
        public async Task<User> GetUserForLoginAsync(string cccd)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Fetch user info specifically for authentication
            string sql = "SELECT MaNV, TenNV, CCCD, Matkhau, Vaitro FROM tblNhanvien WHERE CCCD = @CCCD";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { CCCD = cccd });
        }
    }
}
