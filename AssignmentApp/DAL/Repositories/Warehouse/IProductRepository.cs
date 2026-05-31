using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> AddAsync(Product p);
        Task<int> UpdateAsync(Product p);
        Task<int> SoftDeleteAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit);
        Task<IEnumerable<Product>> SearchByTextAsync(string keyword, string catIdText, string catNameText, string status);
        Task<int> UpdateStockAsync(int productId, int quantityChange);
        Task<DataTable> GetCategoriesForComboBoxAsync();
    }
}


