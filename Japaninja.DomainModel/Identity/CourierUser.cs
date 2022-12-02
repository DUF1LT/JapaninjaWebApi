using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.DomainModel.Identity;

public class CourierUser : IdentityUser, IHasId
{
    public string FullName { get; set; }
    
    public IReadOnlyCollection<CouriersOrders> Orders { get; set; }
}