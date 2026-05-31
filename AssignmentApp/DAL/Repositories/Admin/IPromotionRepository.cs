using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL cho bảng KhuyenMai.
    /// Bất kỳ class nào thực thi interface này đều phải tuân thủ việc viết code cho các hàm bên dưới.
    /// </summary>
    public interface IPromotionRepository
    {
        /// <summary>
        /// Lấy toàn bộ danh sách Khuyến mãi từ Database.
        /// </summary>
        Task<IEnumerable<Promotion>> GetAllAsync();

        /// <summary>
        /// Lấy thông tin của một Khuyến mãi cụ thể thông qua mã ID.
        /// </summary>
        Task<Promotion> GetByIdAsync(int maKhuyenMai);

        /// <summary>
        /// Thêm mới một Khuyến mãi vào Database.
        /// Trả về số dòng bị ảnh hưởng (số dòng được Insert thành công).
        /// </summary>
        Task<int> AddAsync(Promotion promotion);

        /// <summary>
        /// Cập nhật thông tin một Khuyến mãi hiện có.
        /// Trả về số dòng bị ảnh hưởng (số dòng được Update thành công).
        /// </summary>
        Task<int> UpdateAsync(Promotion promotion);

        /// <summary>
        /// Xóa một Khuyến mãi khỏi Database dựa vào mã ID.
        /// Trả về số dòng bị ảnh hưởng (số dòng được Delete thành công).
        /// </summary>
        Task<int> DeleteAsync(int maKhuyenMai);
    }
}
