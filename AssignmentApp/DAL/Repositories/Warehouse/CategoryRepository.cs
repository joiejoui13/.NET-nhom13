using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class CategoryRepository
    {
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc";
            return await DbContext.Conn.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetByIdAsync(string maDanhMuc)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM DanhMuc WHERE MaDanhMuc = @MaDanhMuc";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Category>(sql, new { MaDanhMuc = maDanhMuc });
        }

        public async Task<int> AddAsync(Category c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, NgayTao) 
                           VALUES (@MaDanhMuc, @TenDanhMuc, @MoTa, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> UpdateAsync(Category c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE DanhMuc 
                           SET TenDanhMuc = @TenDanhMuc, MoTa = @MoTa 
                           WHERE MaDanhMuc = @MaDanhMuc";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }

        public async Task<int> DeleteAsync(string maDanhMuc)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM DanhMuc WHERE MaDanhMuc = @MaDanhMuc";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaDanhMuc = maDanhMuc });
        }
    }
}
