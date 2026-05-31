using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Admin
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user, bool updatePassword);
        Task<bool> LockUserAsync(int id);
        Task<IEnumerable<User>> SearchUsersAsync(string idTerm, string nameTerm, string phoneTerm, string emailTerm, string roleTerm, string statusTerm);
    }
}
