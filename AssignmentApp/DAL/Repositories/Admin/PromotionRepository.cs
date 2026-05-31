using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Dapper để tối ưu hóa hiệu năng truy vấn.
    /// </summary>
    public class PromotionRepository : IPromotionRepository
    {
        /// <summary>
        /// Truy vấn lấy toàn bộ bảng KhuyenMai.
        /// </summary>
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            // Kiểm tra và mở kết nối tới DB nếu chưa mở
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhuyenMai";
            
            // Dapper tự động map các cột trong CSDL tương ứng với các thuộc tính của DTO Promotion
            return await DbContext.Conn.QueryAsync<Promotion>(sql);
        }

        /// <summary>
        /// Truy vấn một KhuyenMai theo ID. Sử dụng Parameterized Query (@MaKhuyenMai) để chống SQL Injection.
        /// </summary>
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Promotion> GetByIdAsync(int maKhuyenMai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "SELECT * FROM KhuyenMai WHERE MaKhuyenMai = @MaKhuyenMai";
            
            // QuerySingleOrDefaultAsync: Trả về 1 kết quả duy nhất, nếu không tìm thấy thì trả về null
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Promotion>(sql, new { MaKhuyenMai = maKhuyenMai });
        }

        /// <summary>
        /// Thêm mới KhuyenMai. Mã KhuyenMai là khóa chính tự tăng (IDENTITY) nên không cần truyền vào câu lệnh INSERT.
        /// </summary>
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(Promotion p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO KhuyenMai (TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) 
                           VALUES (@TenKhuyenMai, @PhanTramGiamGia, @NgayBatDau, @NgayKetThuc, @MoTaKhuyenMai, @TrangThai)";
            
            // ExecuteAsync dùng để chạy các lệnh thao tác dữ liệu (INSERT, UPDATE, DELETE). Trả về số dòng thành công.
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        /// <summary>
        /// Cập nhật KhuyenMai dựa vào khóa chính @MaKhuyenMai.
        /// </summary>
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<int> UpdateAsync(Promotion p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE KhuyenMai 
                           SET TenKhuyenMai = @TenKhuyenMai, PhanTramGiamGia = @PhanTramGiamGia, 
                               NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, 
                               MoTaKhuyenMai = @MoTaKhuyenMai, TrangThai = @TrangThai 
                           WHERE MaKhuyenMai = @MaKhuyenMai";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        /// <summary>
        /// Xóa vĩnh viễn một KhuyenMai khỏi CSDL.
        /// (Ghi chú: Lớp BLL có thể không bao giờ gọi hàm này nếu muốn Xóa Mềm - Soft Delete).
        /// </summary>
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<int> DeleteAsync(int maKhuyenMai)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == System.Data.ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "DELETE FROM KhuyenMai WHERE MaKhuyenMai = @MaKhuyenMai";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaKhuyenMai = maKhuyenMai });
        }
    }
}
