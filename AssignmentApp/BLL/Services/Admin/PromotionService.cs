using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Admin
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        // Constructor Injection - Nhận repository từ DI container
        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _promotionRepository.GetAllAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int maKhuyenMai)
        {
            if (maKhuyenMai <= 0) return null;
            return await _promotionRepository.GetByIdAsync(maKhuyenMai);
        }

        public async Task<bool> AddPromotionAsync(Promotion promotion)
        {
            if (promotion == null) throw new ArgumentNullException(nameof(promotion));
            
            // Validate nghiệp vụ cơ bản
            // Mã khuyến mãi tự động tăng (INT IDENTITY) nên không cần kiểm tra khi Thêm mới
            
            if (promotion.NgayKetThuc < promotion.NgayBatDau)
                throw new Exception("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");

            if (promotion.PhanTramGiamGia < 0 || promotion.PhanTramGiamGia > 100)
                throw new Exception("Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100!");

            int rowsAffected = await _promotionRepository.AddAsync(promotion);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdatePromotionAsync(Promotion promotion)
        {
            if (promotion == null) throw new ArgumentNullException(nameof(promotion));
            
            if (promotion.MaKhuyenMai <= 0)
                throw new Exception("Mã khuyến mãi không hợp lệ!");
            
            if (promotion.NgayKetThuc < promotion.NgayBatDau)
                throw new Exception("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");

            if (promotion.PhanTramGiamGia < 0 || promotion.PhanTramGiamGia > 100)
                throw new Exception("Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100!");

            int rowsAffected = await _promotionRepository.UpdateAsync(promotion);
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePromotionAsync(int maKhuyenMai)
        {
            if (maKhuyenMai <= 0) throw new ArgumentException("Mã khuyến mãi không hợp lệ!");
            int rowsAffected = await _promotionRepository.DeleteAsync(maKhuyenMai);
            return rowsAffected > 0;
        }
    }
}
