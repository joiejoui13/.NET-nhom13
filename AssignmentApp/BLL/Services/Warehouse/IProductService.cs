using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product p);
        Task<bool> UpdateProductAsync(Product p);
        Task<bool> SoftDeleteProductAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit);
        Task<DataTable> GetCategoriesForComboBoxAsync();
    }
}
