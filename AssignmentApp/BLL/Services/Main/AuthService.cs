using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Main;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;
using AssignmentApp.DAL.Core;

namespace AssignmentApp.BLL.Services.Main
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer).
    /// Đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// Kỹ thuật Dependency Injection (DI) được áp dụng qua Constructor.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User> LoginAsync(string manguoidung, string matkhau)
        {
            // Gọi DAL để lấy user theo MaNguoiDung
            var user = await _authRepository.GetUserForLoginAsync(manguoidung);
            
            if (user != null)
            {
                // Kiểm tra mật khẩu (Sử dụng PasswordHasher)
                if (PasswordHasher.VerifyPassword(matkhau, user.MatKhau))
                {
                    return user;
                }
            }
            
            return null; // Đăng nhập thất bại
        }

        public bool CheckDatabaseConnection()
        {
            return DbContext.Ketnoi();
        }
    }
}
