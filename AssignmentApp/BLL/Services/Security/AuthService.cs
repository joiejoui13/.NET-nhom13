using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Security;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Security;

namespace AssignmentApp.BLL.Services.Security
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;

        public AuthService()
        {
            _authRepository = new AuthRepository();
        }

        public async Task<User> LoginAsync(string cccd, string password)
        {
            // Gọi DAL để lấy user theo CCCD
            var user = await _authRepository.GetUserForLoginAsync(cccd);
            
            if (user != null)
            {
                // Kiểm tra mật khẩu (Sử dụng PasswordHasher)
                if (PasswordHasher.VerifyPassword(password, user.MatKhau))
                {
                    return user;
                }
            }
            
            return null; // Đăng nhập thất bại
        }
    }
}
