using Japaninja.DomainModel.Identity;
using Japaninja.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<JapaninjaDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*");
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserManager<UserManager<IdentityUser>>()
    .AddEntityFrameworkStores<JapaninjaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSpaStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
