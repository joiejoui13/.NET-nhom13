using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> AddAsync(Product p);
        Task<int> UpdateAsync(Product p);
        Task<int> SoftDeleteAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit);
        Task<DataTable> GetCategoriesForComboBoxAsync();
    }
}
