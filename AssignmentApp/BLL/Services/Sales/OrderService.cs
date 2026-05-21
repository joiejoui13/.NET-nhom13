using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;

        public OrderService(OrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepo.GetAllOrdersAsync();
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.MaHoaDon))
            {
                order.MaHoaDon = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            order.NgayTao = DateTime.Now;
            return await _orderRepo.AddOrderAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            return await _orderRepo.UpdateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderTotalAsync(string maHoaDon, decimal tongTien)
        {
            return await _orderRepo.UpdateOrderTotalAsync(maHoaDon, tongTien);
        }

        public async Task<bool> DeleteOrderAsync(string maHoaDon)
        {
            return await _orderRepo.DeleteOrderAsync(maHoaDon);
        }
    }
}
