using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public class UserRepository
    {
        public async Task<User> GetUserByCCCDAsync(string cccd)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // Using Parameterized Query to prevent SQL Injection
            string sql = "SELECT CCCD, Matkhau, Vaitro FROM tblNhanvien WHERE CCCD = @CCCD";
            var parameters = new { CCCD = cccd };
            
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, parameters);
        }
    }
}
