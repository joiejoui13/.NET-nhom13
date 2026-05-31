using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Admin
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int maKhuyenMai);
        Task<int> AddAsync(Promotion promotion);
        Task<int> UpdateAsync(Promotion promotion);
        Task<int> DeleteAsync(int maKhuyenMai);
    }
}
