using Japaninja.DomainModel.Models;

namespace Japaninja.Models.Order;

public class CreateOrder
{
    public string CustomerId { get; set; }

    public IReadOnlyCollection<OrderProduct> Products { get; set; }

    public IReadOnlyCollection<OrderCutlery> Cutlery { get; set; }

    public Restaurant Restaurant { get; set; }

    public DateTime? DeliveryTime { get; set; }

    public OrderAddressInfo Address { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string AdditionalInfo { get; set; }
}