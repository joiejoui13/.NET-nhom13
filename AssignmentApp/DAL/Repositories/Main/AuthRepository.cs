using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Main
{
    public class AuthRepository : IAuthRepository
    {
        // Đổi tên tham số từ manguoidung thành email cho rõ nghĩa logic
        public async Task<User> GetUserForLoginAsync(string email)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            // SỬA TẠI ĐÂY: Thay đổi điều kiện WHERE từ MaNguoiDung sang Email = @Email
            string sql = @"SELECT MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao 
                           FROM NguoiDung 
                           WHERE Email = @Email";

            // Truyền tham số Email vào câu lệnh Query của Dapper
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}