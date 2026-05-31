using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Sales
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class CustomerRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang";
            return await DbContext.Conn.QueryAsync<Customer>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Customer> GetByIdAsync(string maKhachHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Customer>(sql, new { MaKhachHang = maKhachHang });
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiemTichLuy, NgayTao) 
                           VALUES (@MaKhachHang, @TenKhachHang, @SoDienThoai, @DiemTichLuy, @NgayTao)";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<int> UpdateAsync(Customer c)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE KhachHang 
                           SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, DiemTichLuy = @DiemTichLuy 
                           WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, c);
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<int> DeleteAsync(string maKhachHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaKhachHang = maKhachHang });
        }
    }
}
