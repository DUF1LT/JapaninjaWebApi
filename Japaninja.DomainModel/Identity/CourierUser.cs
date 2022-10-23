using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.DomainModel.Identity;

public class CourierUser : IdentityUser, IHasId
{
    public IReadOnlyCollection<CouriersOrders> Orders { get; set; }
}