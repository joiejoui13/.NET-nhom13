using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery?> GetByIdAsync(int maGiaoHang);
        Task<IEnumerable<Delivery>> SearchAsync(int? maGiaoHang, int? maHoaDon, int? maTraHang, string? trangThai);
        Task<int> AddAsync(Delivery d);
        Task<int> UpdateAsync(Delivery d);
        Task<int> UpdateStatusAsync(int maGiaoHang, string trangThai);
    }
}
