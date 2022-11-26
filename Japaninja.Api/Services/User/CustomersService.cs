using Japaninja.Common.Helpers;
using Japaninja.DomainModel.Identity;
using Japaninja.Logging;
using Japaninja.Repositories.Constants;
using Japaninja.Repositories.Repositories.User.Customers;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.User;

public class CustomersService : ICustomersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CustomersService(
        IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWorkFactory.Create();
    }


    public async Task<CustomerUser> GetCustomerByIdAsync(string id)
    {
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomersRepository>();

        return await customerRepository.GetByIdAsync(id);
    }

    public async Task<CustomerUser> GetCustomerByEmailAsync(string email)
    {
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomersRepository>();

        return await customerRepository.GetByEmailAsync(email);
    }

    public async Task<string> AddCustomerAsync(string email, string password)
    {
        var customerId = Guid.NewGuid().ToString();
        var customer = new CustomerUser
        {
            Id = customerId,
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(customer, password);
        if (!result.Succeeded)
        {
            LoggerContext.Current.LogError($"Failed to create customer {customer.Email} - {result.Errors.Select(e => e.Code)}");
            throw new InvalidOperationException($"Failed to create user {customer.Email}");
        }

        await _userManager.AddToRoleAsync(customer, Roles.Customer);

        return customerId;
    }
}