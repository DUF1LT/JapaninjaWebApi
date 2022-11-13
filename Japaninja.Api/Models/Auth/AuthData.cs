namespace Japaninja.Models.Auth;

public class AuthData
{
    public string Id { get; set; }

    public string Token { get; set; }

    public long TokenExpirationTime { get; set; }

    public string Role { get; set; }
}