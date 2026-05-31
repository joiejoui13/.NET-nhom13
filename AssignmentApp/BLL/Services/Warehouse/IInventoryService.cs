using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryLog>> GetAllLogsAsync();
        Task<InventoryLog> GetLogByIdAsync(int id);
        
        Task<bool> AddLogAsync(InventoryLog log);
        Task<bool> UpdateLogAsync(InventoryLog newLog);
        Task<bool> DeleteLogAsync(int id);

        Task<IEnumerable<InventoryLog>> SearchLogsAsync(string idTerm, string refTerm, string productTerm, string typeTerm, string statusTerm);
        Task<int> GetProductStockAsync(int productId);
        Task<System.Data.DataTable> GetProductsForComboBoxAsync();
    }
}
