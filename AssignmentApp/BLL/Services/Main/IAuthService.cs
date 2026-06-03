using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Main
{
    // Hợp đồng trừu tượng định nghĩa nghiệp vụ logic xử lý Đăng nhập
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IAuthService
    {
        Task<User> LoginAsync(string manguoidung, string matkhau);
        bool CheckDatabaseConnection();
    }
}
