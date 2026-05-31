using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc";
            return await DbContext.Conn.QueryAsync<Category>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Category> GetByIdAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc WHERE MaDanhMuc = @Id";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc WHERE TenDanhMuc = @Name";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Category>(sql, new { Name = name });
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(Category category)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO DanhMuc (TenDanhMuc, MoTa, TrangThai, NgayTao) 
                           VALUES (@TenDanhMuc, @MoTa, @TrangThai, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, category);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<int> UpdateAsync(Category category)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE DanhMuc SET 
                           TenDanhMuc = @TenDanhMuc, MoTa = @MoTa, 
                           TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat 
                           WHERE MaDanhMuc = @MaDanhMuc";
            return await DbContext.Conn.ExecuteAsync(sql, category);
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<int> DeleteAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            // Xóa mềm: Chuyển trạng thái sang 'Đã hủy'
            string sql = "UPDATE DanhMuc SET TrangThai = N'Đã hủy' WHERE MaDanhMuc = @Id";
            return await DbContext.Conn.ExecuteAsync(sql, new { Id = id });
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<Category>> SearchAsync(string idTerm, string nameTerm, string descTerm, string statusTerm)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            
            var parameters = new DynamicParameters();
            string sql = "SELECT * FROM DanhMuc WHERE 1=1";

            if (!string.IsNullOrEmpty(idTerm) && int.TryParse(idTerm, out int id))
            {
                sql += " AND MaDanhMuc = @Id";
                parameters.Add("Id", id);
            }
                
            if (!string.IsNullOrEmpty(nameTerm))
            {
                sql += " AND TenDanhMuc LIKE @Name";
                parameters.Add("Name", $"%{nameTerm}%");
            }
                
            if (!string.IsNullOrEmpty(descTerm))
            {
                sql += " AND MoTa LIKE @Desc";
                parameters.Add("Desc", $"%{descTerm}%");
            }
                
            if (!string.IsNullOrEmpty(statusTerm))
            {
                sql += " AND TrangThai = @Status";
                parameters.Add("Status", statusTerm);
            }

            return await DbContext.Conn.QueryAsync<Category>(sql, parameters);
        }
    }
}
