using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Admin
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
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
