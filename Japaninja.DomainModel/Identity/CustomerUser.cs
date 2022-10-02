using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.DomainModel.Identity;

public class CustomerUser : IdentityUser, IHasId
{
    public IReadOnlyCollection<CustomersOrders> Orders { get; set; }
}