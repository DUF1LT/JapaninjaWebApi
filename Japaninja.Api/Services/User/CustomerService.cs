using Japaninja.DomainModel.Identity;
using Japaninja.Repositories.Repositories.User.Customer;
using Japaninja.Repositories.UnitOfWork;
using Japaninja.Services.Auth;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.User;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public CustomerService(
        IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory,
        IAuthService authService)
    {
        _unitOfWork = unitOfWorkFactory.Create();
        _authService = authService;
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
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomerRepository>();

        var customerId = Guid.NewGuid().ToString();
        var customer = new CustomerUser
        {
            Id = customerId,
            Email = email,
            PasswordHash = _authService.HashPassword(password),
        };

        customerRepository.Add(customer);

        await _unitOfWork.SaveChangesAsync();

        return customerId;
    }
}