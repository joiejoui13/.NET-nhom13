using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Main
{
    // Hợp đồng trừu tượng định nghĩa các nghiệp vụ truy xuất dữ liệu Đăng nhập
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface IAuthRepository
    {
        Task<User> GetUserForLoginAsync(string manguoidung);
    }
}
