using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Main
{
    // Hợp đồng trừu tượng định nghĩa các nghiệp vụ truy xuất dữ liệu Đăng nhập
    public interface IAuthRepository
    {
        Task<User> GetUserForLoginAsync(string manguoidung);
    }
}
