using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Japaninja.Common.Options;
using Japaninja.Creators.CourierUserCreator;
using Japaninja.Creators.ProductCreator;
using Japaninja.Extensions;
using Japaninja.JWT;
using Japaninja.Logging;
using Japaninja.Repositories;
using Japaninja.Repositories.DatabaseInitializer;
using Japaninja.Repositories.UnitOfWork;
using Japaninja.Services.Auth;
using Japaninja.Services.Product;
using Japaninja.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
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
builder.Services.Configure<ManagerCredentialsOptions>(configuration.GetSection(ManagerCredentialsOptions.SectionName));

builder.Services.AddDbContext<JapaninjaDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        o => o.MigrationsAssembly("Japaninja.Repositories"))
    );

builder.Services.AddSingleton<IDatabaseInitializer<JapaninjaDbContext>, DatabaseInitializer>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        var allowedOrigins = configuration.GetSection("ClientOrigin").Get<string[]>();
        b.WithOrigins(allowedOrigins).AllowAnyMethod().AllowCredentials().AllowAnyHeader();
    });
});

builder.Services.AddJapaninjaIdentity();

var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[$"{JWTOptions.SectionName}:SecretKey"]));
builder.Services.AddJapaninjaAuthentication(jwtKey);
builder.Services.AddJapaninjaAuthorization();

builder.Services.AddTransient<IUnitOfWorkFactory<UnitOfWork>, UnitOfWorkFactory>();
builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddTransient<ICustomersService, CustomersService>();
builder.Services.AddTransient<ICouriersService, CouriersService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IProductCreator, ProductCreator>();
builder.Services.AddSingleton<ICourierUserCreator, CourierUserCreator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;

var app = builder.Build();

InitializeContexts(app.Services);
InitializeDatabase(app.Services);

app.UseCors();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

static ILogger GetLogger(IServiceProvider serviceProvider)
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("MainLogger");

    return logger;
}

static void InitializeContexts(IServiceProvider serviceProvider)
{
    LoggerContext.Current = serviceProvider.GetRequiredService<ILogger>();
}

static void InitializeDatabase(IServiceProvider serviceProvider)
{
    LoggerContext.Current.LogInformation("Start initializing database");
    var databaseInitializer = serviceProvider.GetRequiredService<IDatabaseInitializer<JapaninjaDbContext>>();
    var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<JapaninjaDbContext>>();
    using (var dbContext = new JapaninjaDbContext(dbContextOptions))
    {
        databaseInitializer.Initialize(dbContext);
    }
    LoggerContext.Current.LogInformation("End initializing database");
}