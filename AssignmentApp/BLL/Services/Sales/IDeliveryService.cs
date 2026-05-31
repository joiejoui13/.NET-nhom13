using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public interface IDeliveryService
    {
        Task<IEnumerable<Delivery>> GetAllDeliveriesAsync();
        Task<Delivery?> GetDeliveryByIdAsync(int maGiaoHang);
        Task<IEnumerable<Delivery>> SearchDeliveriesAsync(int? maGiaoHang, int? maHoaDon, int? maTraHang, string? trangThai);
        Task<int> AddDeliveryAsync(Delivery delivery);
        Task<int> UpdateDeliveryAsync(Delivery delivery);
        Task<int> SoftDeleteDeliveryAsync(int maGiaoHang);
    }
}
