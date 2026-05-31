using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int maNguoiDung);
        Task<int> AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(int maNguoiDung);
        Task<IEnumerable<User>> SearchAsync(string idTerm, string nameTerm, string phoneTerm, string emailTerm, string roleTerm, string statusTerm);
    }
}
