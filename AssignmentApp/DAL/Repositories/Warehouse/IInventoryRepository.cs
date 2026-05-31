using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryLog>> GetAllAsync();
        Task<InventoryLog> GetByIdAsync(int id);
        
        // Cập nhật giao dịch tồn kho và Tồn kho Sản Phẩm trong cùng 1 Transaction
        Task<bool> AddWithTransactionAsync(InventoryLog log);
        Task<bool> UpdateWithTransactionAsync(InventoryLog newLog, InventoryLog oldLog);
        Task<bool> DeleteWithTransactionAsync(InventoryLog oldLog);

        Task<IEnumerable<InventoryLog>> SearchAsync(string idTerm, string refTerm, string productTerm, string typeTerm, string statusTerm);
        Task<int> GetProductStockAsync(int productId);
        Task<System.Data.DataTable> GetProductsForComboBoxAsync();
    }
}
