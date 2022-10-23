using Japaninja.Models.Auth;

namespace Japaninja.Services.Auth;

public interface IAuthService
{
    AuthData GetAuthData(string id);

    string HashPassword(string password);

    bool VerifyPassword(string actualPassword, string hashedPassword);
}