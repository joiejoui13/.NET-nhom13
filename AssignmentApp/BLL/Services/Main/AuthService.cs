using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Main;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;

namespace AssignmentApp.BLL.Services.Main
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;

        public AuthService()
        {
            _authRepository = new AuthRepository();
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
    }
}
