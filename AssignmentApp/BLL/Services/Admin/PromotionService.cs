using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Admin
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer) của hệ thống Khuyến mãi.
    /// Nó đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// </summary>
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        /// <summary>
        /// Constructor Injection - Kỹ thuật Dependency Injection (DI)
        /// Nhận một object Repository từ DI container (đã cấu hình ở Program.cs) thay vì tự khởi tạo bằng từ khóa "new".
        /// Việc này giúp mã nguồn lỏng lẻo (loose coupling) và dễ dàng viết Unit Test sau này.
        /// </summary>
        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            // Tầng này có thể thêm logic phân quyền, ví dụ: Nếu là nhân viên thường thì không cho xem danh sách này.
            return await _promotionRepository.GetAllAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Promotion> GetPromotionByIdAsync(int maKhuyenMai)
        {
            if (maKhuyenMai <= 0) return null;
            return await _promotionRepository.GetByIdAsync(maKhuyenMai);
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<bool> AddPromotionAsync(Promotion promotion)
        {
            if (promotion == null) throw new ArgumentNullException(nameof(promotion));
            
            // ================== BẮT ĐẦU VÙNG KIỂM DUYỆT NGHIỆP VỤ (VALIDATION) ==================
            // Mã khuyến mãi tự động tăng (INT IDENTITY) nên không cần kiểm tra khi Thêm mới
            
            if (promotion.NgayKetThuc < promotion.NgayBatDau)
                throw new Exception("Lỗi nghiệp vụ: Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");

            if (promotion.PhanTramGiamGia < 0 || promotion.PhanTramGiamGia > 100)
                throw new Exception("Lỗi nghiệp vụ: Phần trăm giảm giá phải hợp lệ (nằm trong khoảng từ 0 đến 100%)!");
            // ====================================================================================

            // Sau khi mọi dữ liệu đã hợp lệ, mới gọi xuống Repository (Tầng DAL) để tương tác CSDL
            int rowsAffected = await _promotionRepository.AddAsync(promotion);
            return rowsAffected > 0;
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<bool> UpdatePromotionAsync(Promotion promotion)
        {
            if (promotion == null) throw new ArgumentNullException(nameof(promotion));
            
            // ================== KIỂM DUYỆT NGHIỆP VỤ ==================
            if (promotion.MaKhuyenMai <= 0)
                throw new Exception("Mã khuyến mãi không hợp lệ!");
            
            if (promotion.NgayKetThuc < promotion.NgayBatDau)
                throw new Exception("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");

            if (promotion.PhanTramGiamGia < 0 || promotion.PhanTramGiamGia > 100)
                throw new Exception("Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100!");
            // ==========================================================

            int rowsAffected = await _promotionRepository.UpdateAsync(promotion);
            return rowsAffected > 0;
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<bool> DeletePromotionAsync(int maKhuyenMai)
        {
            if (maKhuyenMai <= 0) throw new ArgumentException("Mã khuyến mãi không hợp lệ!");
            
            // Xóa cứng từ DB (Tuỳ theo quy định nghiệp vụ công ty, có nơi sẽ yêu cầu Xóa mềm bằng cách update trạng thái thành 'Không hoạt động')
            // Trong đề bài Assignment này, Repository viết hàm DELETE FROM KhuyenMai.
            int rowsAffected = await _promotionRepository.DeleteAsync(maKhuyenMai);
            return rowsAffected > 0;
        }
    }
}
