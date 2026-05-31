using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public interface IOrderRepository
    {
        Task<int> AddAsync(Order o);
        Task<int> UpdateAsync(Order o);
        Task<int> AddDetailAsync(OrderDetail d);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> SearchAsync(string keyword);
        Task<IEnumerable<OrderDetail>> GetDetailsAsync(string maHoaDon);
        Task<bool> DeleteOrderTransactionAsync(string maHoaDon);
        Task<int> DeleteDetailsByOrderIdAsync(string maHoaDon);
    }
}
