using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class CustomerAddress : IHasId
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public CustomerUser Customer { get; set; }

    public string Street { get; set; }

    public string House { get; set; }

    public string Housing { get; set; }

    public string Flat { get; set; }

    public string Entrance { get; set; }

    public string Floor { get; set; }
}