using BC = BCrypt.Net.BCrypt;

namespace Vesta.Helpers
{
    public static class BCryptHelper
    {
        public static string HashPassword(string password)
        {
            return BC.HashPassword(password, workFactor:  10);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
}