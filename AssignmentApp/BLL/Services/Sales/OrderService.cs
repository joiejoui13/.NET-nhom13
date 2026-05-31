using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApp.DTO;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;

namespace AssignmentApp.BLL.Services.Sales
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepo.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string keyword)
        {
            return await _orderRepo.SearchAsync(keyword);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(string maHoaDon)
        {
            return await _orderRepo.GetDetailsAsync(maHoaDon);
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> details)
        {
            await _orderRepo.AddAsync(order);
            var savedOrders = await _orderRepo.GetAllAsync();
            string newId = savedOrders.Max(o => int.Parse(o.MaHoaDon)).ToString();

            foreach (var detail in details)
            {
                detail.MaHoaDon = newId;
                await _orderRepo.AddDetailAsync(detail);
                if (int.TryParse(detail.MaSanPham, out int productId))
                {
                    await _productRepo.UpdateStockAsync(productId, -detail.SoLuong);
                }
            }
        }

        public async Task UpdateOrderStatusAsync(Order order, string oldStatus)
        {
            await _orderRepo.UpdateAsync(order);

            // Restore inventory if status changed to "Đã hủy"
            if (oldStatus != "Đã hủy" && order.TrangThai == "Đã hủy")
            {
                var details = await _orderRepo.GetDetailsAsync(order.MaHoaDon.ToString());
                foreach (var d in details)
                {
                    if (int.TryParse(d.MaSanPham, out int pId))
                    {
                        await _productRepo.UpdateStockAsync(pId, d.SoLuong);
                    }
                }
            }
        }

        public async Task UpdateOrderCartAsync(Order order, List<OrderDetail> newDetails)
        {
            var oldDetails = await _orderRepo.GetDetailsAsync(order.MaHoaDon.ToString());
            foreach (var d in oldDetails)
            {
                if (int.TryParse(d.MaSanPham, out int pId))
                {
                    await _productRepo.UpdateStockAsync(pId, d.SoLuong);
                }
            }

            await _orderRepo.DeleteDetailsByOrderIdAsync(order.MaHoaDon.ToString());
            
            decimal total = newDetails.Sum(d => d.ThanhTien);
            order.TongTien = total;
            
            // Note: discount recalculation should be handled in UI or passed in, 
            // but we expect the UI to have already set order.TongTien if it recalculates.
            // Actually, we should just let the UI set order.TongTien before calling this.
            
            foreach (var detail in newDetails)
            {
                detail.MaHoaDon = order.MaHoaDon.ToString();
                await _orderRepo.AddDetailAsync(detail);
                if (int.TryParse(detail.MaSanPham, out int pId))
                {
                    await _productRepo.UpdateStockAsync(pId, -detail.SoLuong);
                }
            }
            
            await _orderRepo.UpdateAsync(order);
        }

        public async Task ConvertToInvoiceAsync(Order order)
        {
            order.LoaiHoaDon = "Đơn bán hàng";
            order.TrangThai = "Đã hoàn thành";
            // await _orderRepo.UpdateAsync(order); // commented out in UI, so leaving commented here or we can enable it
        }
    }
}
