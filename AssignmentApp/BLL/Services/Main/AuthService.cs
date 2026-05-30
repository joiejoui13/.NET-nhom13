using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Main;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;

namespace AssignmentApp.BLL.Services.Main
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        // Đổi tên tham số từ manguoidung thành email cho đúng logic 3 lớp
        public async Task<User> LoginAsync(string email, string matkhau)
        {
            // Gọi DAL để lấy user theo Email
            var user = await _authRepository.GetUserForLoginAsync(email);

            if (user != null)
            {
                // SỬA TẠI ĐÂY: Vì DB đang lưu chuỗi thô '123' nên so sánh trực tiếp, dùng thêm .Trim() cho chắc chắn
                if (user.MatKhau.Trim() == matkhau.Trim())
                {
                    return user;
                }
            }

            return null; // Đăng nhập thất bại
        }
    }
}