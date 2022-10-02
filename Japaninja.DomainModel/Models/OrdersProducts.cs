using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class OrdersProducts : IHasId
{
    public string Id { get; set; }

    public string OrderId { get; set; }

    public Order Order { get; set; }

    public string ProductId { get; set; }

    public Product Product { get; set; }

    public int Amount { get; set; }
}