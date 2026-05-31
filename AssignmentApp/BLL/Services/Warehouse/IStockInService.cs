using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.BLL.Services.Warehouse
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IStockInService
    {
        Task<IEnumerable<StockInReceipt>> GetAllReceiptsAsync();
        Task<StockInReceipt> GetReceiptByIdAsync(int id);
        Task<IEnumerable<StockInDetailModel>> GetReceiptDetailsAsync(int receiptId);
        Task<IEnumerable<StockInReceipt>> SearchReceiptsAsync(string receiptIdStr, string userIdStr, string status, string dateStr);
        
        Task<int> SaveReceiptAsync(StockInReceipt receipt, List<StockInDetailModel> details, bool isAddingNew);
        Task<bool> CancelReceiptAsync(int receiptId);
    }
}
