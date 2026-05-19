using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Main
{
    // Hợp đồng trừu tượng định nghĩa nghiệp vụ logic xử lý Đăng nhập
    public interface IAuthService
    {
        Task<User> LoginAsync(string manguoidung, string matkhau);
    }
}
