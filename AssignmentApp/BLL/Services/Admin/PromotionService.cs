using System; // Cung cấp các class cơ bản của C# (như Exception, Console,...)
using System.Collections.Generic; // Cung cấp List<T>, IEnumerable<T> (dùng để chứa danh sách Khuyến mãi)
using System.Linq; // Cung cấp các hàm xử lý mảng/danh sách nhanh (như .ToList(), .Where())
using System.Text; // Dùng khi cần xử lý chuỗi phức tạp (StringBuilder)
// using System.Data; // Trong đồ án của bạn dùng Dapper trả về Object (Promotion) thay vì DataTable, nên không cần System.Data ở đây.

using System.Threading.Tasks; // Cung cấp Task, async/await để chạy bất đồng bộ (không làm đơ giao diện)
using AssignmentApp.DAL.Repositories.Admin; // Gọi đến thư mục chứa "Nhà kho"
using AssignmentApp.DTO; // Gọi đến thư mục chứa cái "Hộp" đựng dữ liệu (Khuon mẫu Promotion)

namespace AssignmentApp.BLL.Services.Admin
{
    public class PromotionService
    {
        private readonly PromotionRepository _promotionRepository;

        public PromotionService()
        {
            _promotionRepository = new PromotionRepository();
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _promotionRepository.GetAllAsync();
        }
    }
}
