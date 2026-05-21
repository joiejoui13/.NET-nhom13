using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class DeliveryService
    {
        private readonly DeliveryRepository _deliveryRepo;

        public DeliveryService(DeliveryRepository deliveryRepo)
        {
            _deliveryRepo = deliveryRepo;
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _deliveryRepo.GetAllDeliveriesAsync();
        }

        public async Task<bool> AddDeliveryAsync(Delivery delivery)
        {
            if (string.IsNullOrWhiteSpace(delivery.MaGiaoHang))
            {
                delivery.MaGiaoHang = GenerateDeliveryId();
            }
            
            if (delivery.TrangThaiGiao == "Đã giao" && delivery.NgayGiao == null)
            {
                delivery.NgayGiao = DateTime.Now;
            }

            return await _deliveryRepo.AddDeliveryAsync(delivery);
        }

        public async Task<bool> UpdateDeliveryAsync(Delivery delivery)
        {
            if (delivery.TrangThaiGiao == "Đã giao" && delivery.NgayGiao == null)
            {
                delivery.NgayGiao = DateTime.Now;
            }
            return await _deliveryRepo.UpdateDeliveryAsync(delivery);
        }

        private string GenerateDeliveryId()
        {
            return "GH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
