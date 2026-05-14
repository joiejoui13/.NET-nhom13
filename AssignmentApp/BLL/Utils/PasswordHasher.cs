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
                // For this assignment, if DB uses plaintext passwords, compare directly
                return enteredPassword == hashedDbPassword;
            }
            catch (SaltParseException)
            {
                return false;
            }
        }
    }
}
