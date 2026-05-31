using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung";
            return await DbContext.Conn.QueryAsync<User>(sql);
        }

        public async Task<User> GetByIdAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = maNguoiDung });
        }

        public async Task<int> AddAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
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

        public async Task<int> DeleteAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            // Xóa mềm: Cập nhật TrangThai = 'Khóa'
            string sql = "UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaNguoiDung = maNguoiDung });
        }

        public async Task<IEnumerable<User>> SearchAsync(string idTerm, string nameTerm, string phoneTerm, string emailTerm, string roleTerm, string statusTerm)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            var parameters = new DynamicParameters();
            string sql = "SELECT * FROM NguoiDung WHERE 1=1";

            if (!string.IsNullOrEmpty(idTerm) && int.TryParse(idTerm, out int id))
            {
                sql += " AND MaNguoiDung = @Id";
                parameters.Add("Id", id);
            }
                
            if (!string.IsNullOrEmpty(nameTerm))
            {
                sql += " AND TenNguoiDung LIKE @Name";
                parameters.Add("Name", $"%{nameTerm}%");
            }
                
            if (!string.IsNullOrEmpty(phoneTerm))
            {
                sql += " AND SoDienThoai LIKE @Phone";
                parameters.Add("Phone", $"%{phoneTerm}%");
            }
                
            if (!string.IsNullOrEmpty(emailTerm))
            {
                sql += " AND Email LIKE @Email";
                parameters.Add("Email", $"%{emailTerm}%");
            }
                
            if (!string.IsNullOrEmpty(roleTerm))
            {
                sql += " AND VaiTro = @Role";
                parameters.Add("Role", roleTerm);
            }
                
            if (!string.IsNullOrEmpty(statusTerm))
            {
                sql += " AND TrangThai = @Status";
                parameters.Add("Status", statusTerm);
            }

            return await DbContext.Conn.QueryAsync<User>(sql, parameters);
        }
    }
}
