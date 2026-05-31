using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DAL.Repositories.Sales.IDeliveryRepository _deliveryRepository;

        public DeliveryService(DAL.Repositories.Sales.IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _deliveryRepository.GetAllAsync();
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int maGiaoHang)
        {
            return await _deliveryRepository.GetByIdAsync(maGiaoHang);
        }

        public async Task<IEnumerable<Delivery>> SearchDeliveriesAsync(int? maGiaoHang, int? maHoaDon, int? maTraHang, string? trangThai)
        {
            return await _deliveryRepository.SearchAsync(maGiaoHang, maHoaDon, maTraHang, trangThai);
        }

        public async Task<int> AddDeliveryAsync(Delivery delivery)
        {
            if (string.IsNullOrWhiteSpace(delivery.DiaChiGiao))
                throw new System.ArgumentException("Địa chỉ giao hàng không được để trống!");

            if (!delivery.MaHoaDon.HasValue && !delivery.MaTraHang.HasValue)
                throw new System.ArgumentException("Giao hàng phải gắn với một Mã hóa đơn hoặc Mã phiếu trả hàng!");

            if (!delivery.NgayGiao.HasValue)
                delivery.NgayGiao = System.DateTime.Now;
                
            if (string.IsNullOrEmpty(delivery.TrangThaiGiao))
                delivery.TrangThaiGiao = "Chờ giao";

            return await _deliveryRepository.AddAsync(delivery);
        }

        public async Task<int> UpdateDeliveryAsync(Delivery delivery)
        {
            if (string.IsNullOrWhiteSpace(delivery.DiaChiGiao))
                throw new System.ArgumentException("Địa chỉ giao hàng không được để trống!");

            if (!delivery.MaHoaDon.HasValue && !delivery.MaTraHang.HasValue)
                throw new System.ArgumentException("Giao hàng phải gắn với một Mã hóa đơn hoặc Mã phiếu trả hàng!");

            return await _deliveryRepository.UpdateAsync(delivery);
        }

        public async Task<int> SoftDeleteDeliveryAsync(int maGiaoHang)
        {
            return await _deliveryRepository.UpdateStatusAsync(maGiaoHang, "Đã hủy");
        }
    }
}
