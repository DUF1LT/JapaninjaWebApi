using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class CustomerAddress : IHasId
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public CustomerUser Customer { get; set; }

    public string Address { get; set; }
}