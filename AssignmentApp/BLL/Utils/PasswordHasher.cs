using BCrypt.Net;

namespace AssignmentApp.BLL.Security
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
