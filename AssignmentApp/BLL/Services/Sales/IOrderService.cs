using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> SearchOrdersAsync(string keyword);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(string maHoaDon);
        Task CreateOrderAsync(Order order, List<OrderDetail> details);
        Task UpdateOrderStatusAsync(Order order, string oldStatus);
        Task UpdateOrderCartAsync(Order order, List<OrderDetail> newDetails);
        Task ConvertToInvoiceAsync(Order order);
    }
}
