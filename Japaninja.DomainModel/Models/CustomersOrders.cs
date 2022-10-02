using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class CustomersOrders: IHasId
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public CustomerUser Customer { get; set; }

    public string OrderId { get; set; }

    public Order Order { get; set; }
}