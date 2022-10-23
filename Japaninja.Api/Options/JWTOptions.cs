namespace Japaninja.Options;

public class JWTOptions
{
    public const string SectionName = "JWT";


    public string SecretKey { get; set; }

    public int Lifespan { get; set; }
}