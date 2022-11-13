using Japaninja.Models.Auth;

namespace Japaninja.Services.Auth;

public interface IAuthService
{
    Task<AuthData> GetAuthDataAsync(string id);
}