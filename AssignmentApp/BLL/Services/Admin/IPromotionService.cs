using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Admin
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic) liên quan đến Khuyến Mãi.
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IPromotionService
    {
        /// <summary>
        /// Lấy toàn bộ danh sách Khuyến mãi.
        /// </summary>
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();

        /// <summary>
        /// Lấy thông tin chi tiết một Khuyến mãi theo ID.
        /// </summary>
        Task<Promotion> GetPromotionByIdAsync(int maKhuyenMai);

        /// <summary>
        /// Thêm mới một Khuyến mãi, có kiểm tra nghiệp vụ (Validate) trước khi lưu.
        /// Trả về true nếu thành công, false hoặc quăng lỗi (Exception) nếu thất bại.
        /// </summary>
        Task<bool> AddPromotionAsync(Promotion promotion);

        /// <summary>
        /// Cập nhật thông tin Khuyến mãi, có kiểm tra nghiệp vụ.
        /// </summary>
        Task<bool> UpdatePromotionAsync(Promotion promotion);

        /// <summary>
        /// Xóa Khuyến mãi. Trong thực tế, có thể biến đổi logic này thành "Xóa mềm" (chuyển trạng thái).
        /// </summary>
        Task<bool> DeletePromotionAsync(int maKhuyenMai);
    }
}
