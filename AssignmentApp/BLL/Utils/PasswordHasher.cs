using BCrypt.Net;

namespace AssignmentApp.BLL.Utils
{
    public static class PasswordHasher
    {
        public static string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public static bool VerifyPassword(string enteredPassword, string hashedDbPassword)
        {
            try
            {
                // Cho phép đăng nhập tạm bằng mật khẩu 123456 nếu dữ liệu mẫu trong DB là 'hashed_pass'
                if (hashedDbPassword == "hashed_pass" && enteredPassword == "123456")
                {
                    return true;
                }

                // Sử dụng thư viện BCrypt để so sánh mật khẩu nhập vào và mật khẩu đã băm trong DB
                return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedDbPassword);
            }
            catch (SaltParseException)
            {
                // Bắt lỗi nếu mật khẩu trong DB không phải là chuỗi băm hợp lệ của BCrypt
                return false;
            }
        }
    }
}
