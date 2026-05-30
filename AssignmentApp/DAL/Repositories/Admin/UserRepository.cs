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

        // 1. SỬA TẠI ĐÂY: Chuyển string thành int vì MaNguoiDung trong DB là INT
        public async Task<User> GetByIdAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = maNguoiDung });
        }

        // 2. SỬA TẠI ĐÂY: Hàm Login mới để check bằng Email thay vì dùng chung GetById
        public async Task<User> GetByEmailAsync(string email)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung WHERE Email = @Email";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> AddAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();

            // LƯU Ý: Vì MaNguoiDung là IDENTITY(1,1) nên phải BỎ nó ra khỏi câu lệnh INSERT, SQL sẽ tự sinh.
            string sql = @"INSERT INTO NguoiDung (TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                           VALUES (@TenNguoiDung, @SoDienThoai, @Email, @MatKhau, @VaiTro, @TrangThai, @NgayTao)";
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

        // 3. SỬA TẠI ĐÂY: Chuyển string thành int cho đồng bộ với DB
        public async Task<int> DeleteAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaNguoiDung = maNguoiDung });
        }
    }
}