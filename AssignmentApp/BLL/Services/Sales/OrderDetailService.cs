using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepo;
        private readonly OrderRepository _orderRepo;

        public OrderDetailService(OrderDetailRepository orderDetailRepo, OrderRepository orderRepo)
        {
            _orderDetailRepo = orderDetailRepo;
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string maHoaDon)
        {
            return await _orderDetailRepo.GetOrderDetailsByOrderIdAsync(maHoaDon);
        }

        public async Task<bool> AddOrderDetailAsync(OrderDetail detail)
        {
            if (string.IsNullOrEmpty(detail.MaChiTiet))
            {
                detail.MaChiTiet = "CT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
            
            detail.ThanhTien = detail.SoLuong * detail.DonGia;
            
            bool isSuccess = await _orderDetailRepo.AddOrderDetailAsync(detail);
            if (isSuccess)
            {
                await UpdateOrderTotalAsync(detail.MaHoaDon);
            }
            return isSuccess;
        }

        public async Task<bool> UpdateOrderDetailAsync(OrderDetail detail)
        {
            detail.ThanhTien = detail.SoLuong * detail.DonGia;
            
            bool isSuccess = await _orderDetailRepo.UpdateOrderDetailAsync(detail);
            if (isSuccess)
            {
                await UpdateOrderTotalAsync(detail.MaHoaDon);
            }
            return isSuccess;
        }

        public async Task<bool> DeleteOrderDetailAsync(string maChiTiet, string maHoaDon)
        {
            bool isSuccess = await _orderDetailRepo.DeleteOrderDetailAsync(maChiTiet);
            if (isSuccess)
            {
                await UpdateOrderTotalAsync(maHoaDon);
            }
            return isSuccess;
        }

        private async Task UpdateOrderTotalAsync(string maHoaDon)
        {
            var details = await _orderDetailRepo.GetOrderDetailsByOrderIdAsync(maHoaDon);
            decimal total = details.Sum(d => d.ThanhTien);
            
            // We should ideally fetch the order and apply GiamGia, but for simplicity here we just set TongTien
            await _orderRepo.UpdateOrderTotalAsync(maHoaDon, total);
        }
    }
}
