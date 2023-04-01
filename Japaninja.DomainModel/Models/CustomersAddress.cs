using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class CustomerAddress : IHasId
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public CustomerUser Customer { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string FlatNumber { get; set; }

    public string Entrance { get; set; }

    public string Floor { get; set; }
}