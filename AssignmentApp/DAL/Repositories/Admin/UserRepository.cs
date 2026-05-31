using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class UserRepository : IUserRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung";
            return await DbContext.Conn.QueryAsync<User>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<User> GetByIdAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<User>(sql, new { MaNguoiDung = maNguoiDung });
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO NguoiDung (TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) 
                           VALUES (@TenNguoiDung, @SoDienThoai, @Email, @MatKhau, @VaiTro, @TrangThai, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, u);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<int> UpdateAsync(User u)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE NguoiDung 
                           SET TenNguoiDung = @TenNguoiDung, SoDienThoai = @SoDienThoai, Email = @Email, 
                               MatKhau = @MatKhau, VaiTro = @VaiTro, TrangThai = @TrangThai 
                           WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, u);
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<int> DeleteAsync(int maNguoiDung)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            // Xóa mềm: Cập nhật TrangThai = 'Khóa'
            string sql = "UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE MaNguoiDung = @MaNguoiDung";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaNguoiDung = maNguoiDung });
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
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
