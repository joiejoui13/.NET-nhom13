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
    public class DeliveryRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang";
            return await DbContext.Conn.QueryAsync<Delivery>(sql);
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Delivery> GetByIdAsync(string maGiaoHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM GiaoHang WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Delivery>(sql, new { MaGiaoHang = maGiaoHang });
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) 
                           VALUES (@MaGiaoHang, @MaHoaDon, @DiaChiGiao, @TrangThaiGiao, @NgayGiao)";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<int> UpdateAsync(Delivery d)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE GiaoHang 
                           SET MaHoaDon = @MaHoaDon, DiaChiGiao = @DiaChiGiao, 
                               TrangThaiGiao = @TrangThaiGiao, NgayGiao = @NgayGiao 
                           WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, d);
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<int> DeleteAsync(string maGiaoHang)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM GiaoHang WHERE MaGiaoHang = @MaGiaoHang";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaGiaoHang = maGiaoHang });
        }
    }
}
