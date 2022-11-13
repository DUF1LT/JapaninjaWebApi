using Japaninja.Common.Helpers;
using Japaninja.DomainModel.Identity;
using Japaninja.Repositories.Constants;
using Japaninja.Repositories.Repositories.User.Customer;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.User;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CustomerService(
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
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomerRepository>();

        return await customerRepository.GetByIdAsync(id);
    }

    public async Task<CustomerUser> GetCustomerByEmailAsync(string email)
    {
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomerRepository>();

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

        await _userManager.CreateAsync(customer, password);
        await _userManager.AddToRoleAsync(customer, Roles.Customer);

        return customerId;
    }
}