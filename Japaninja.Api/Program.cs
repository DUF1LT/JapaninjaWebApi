using System.Reflection;
using System.Text;
using FluentValidation;
using Japaninja.Extensions;
using Japaninja.JWT;
using Japaninja.Logging;
using Japaninja.Options;
using Japaninja.Repositories;
using Japaninja.Repositories.UnitOfWork;
using Japaninja.Services.Auth;
using Japaninja.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Host
    .ConfigureLogging((ctx, loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
    })
    .UseSerilog((ctx, lc) => lc.WriteTo.Console());

var configuration = builder.Configuration;

builder.Services.AddSingleton(GetLogger);

builder.Services.Configure<JWTOptions>(configuration.GetSection(JWTOptions.SectionName));

builder.Services.AddDbContext<JapaninjaDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        o => o.MigrationsAssembly("Japaninja.Repositories"))
    );

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("*");
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserManager<UserManager<IdentityUser>>()
    .AddEntityFrameworkStores<JapaninjaDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[$"{JWTOptions.SectionName}:SecretKey"]));
builder.Services.AddJapaninjaAuthentication(jwtKey);

builder.Services.AddSingleton<IUnitOfWorkFactory<UnitOfWork>, UnitOfWorkFactory>();
builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

var app = builder.Build();
InitializeContexts(app.Services);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static Microsoft.Extensions.Logging.ILogger GetLogger(IServiceProvider serviceProvider)
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("MainLogger");

    return logger;
}

static void InitializeContexts(IServiceProvider serviceProvider)
{
    LoggerContext.Current = serviceProvider.GetRequiredService<ILogger>();
}