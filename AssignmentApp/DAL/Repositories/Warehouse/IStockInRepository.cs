using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public interface IStockInRepository
    {
        Task<IEnumerable<StockInReceipt>> GetAllReceiptsAsync();
        Task<StockInReceipt> GetReceiptByIdAsync(int id);
        Task<IEnumerable<StockInDetailModel>> GetReceiptDetailsAsync(int receiptId);
        Task<IEnumerable<StockInReceipt>> SearchReceiptsAsync(int? receiptId, int? userId, string status, string date);
        
        // Transaction methods
        Task<int> SaveReceiptWithTransactionAsync(StockInReceipt receipt, List<StockInDetailModel> details, bool isAddingNew);
        Task<bool> CancelReceiptWithTransactionAsync(int receiptId);
    }
}
