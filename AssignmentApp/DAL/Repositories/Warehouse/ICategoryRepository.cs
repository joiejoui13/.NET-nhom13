using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByNameAsync(string name);
        Task<int> AddAsync(Category category);
        Task<int> UpdateAsync(Category category);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Category>> SearchAsync(string idTerm, string nameTerm, string descTerm, string statusTerm);
    }
}
