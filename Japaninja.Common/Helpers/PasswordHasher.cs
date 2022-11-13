using CryptoHelper;

namespace Japaninja.Common.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return Crypto.HashPassword(password);
    }

    public static bool VerifyPassword(string actualPassword, string hashedPassword)
    {
        return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
    }
}