using Japaninja.DomainModel.Models;

namespace Japaninja.Models.Order;

public class OrderConfiguration
{
    public float DeliveryPrice { get; set; }

    public float MinDeliveryFreePrice { get; set; }

    public IReadOnlyCollection<Cutlery> Cutlery { get; set; }

    public IReadOnlyCollection<CustomerAddress> Addressess { get; set; }

    public Restaurant SelfPickupRestaurant { get; set; }
}