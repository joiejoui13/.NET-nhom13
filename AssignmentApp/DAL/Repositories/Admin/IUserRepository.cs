using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Admin
{
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
