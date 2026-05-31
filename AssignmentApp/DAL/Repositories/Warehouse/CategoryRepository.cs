using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc";
            return await DbContext.Conn.QueryAsync<Category>(sql);
        }

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

        public async Task<int> AddAsync(Category category)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO DanhMuc (TenDanhMuc, MoTa, TrangThai, NgayTao) 
                           VALUES (@TenDanhMuc, @MoTa, @TrangThai, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, category);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE DanhMuc SET 
                           TenDanhMuc = @TenDanhMuc, MoTa = @MoTa, 
                           TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat 
                           WHERE MaDanhMuc = @MaDanhMuc";
            return await DbContext.Conn.ExecuteAsync(sql, category);
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            // Xóa mềm: Chuyển trạng thái sang 'Đã hủy'
            string sql = "UPDATE DanhMuc SET TrangThai = N'Đã hủy' WHERE MaDanhMuc = @Id";
            return await DbContext.Conn.ExecuteAsync(sql, new { Id = id });
        }

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
