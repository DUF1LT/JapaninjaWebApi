using Japaninja.DomainModel.Models;
using Japaninja.Models.Addresses;

namespace Japaninja.Models.Order;

public class OrderConfiguration
{
    public float DeliveryPrice { get; set; }

    public float MinDeliveryFreePrice { get; set; }

    public IReadOnlyCollection<Cutlery> Cutlery { get; set; }

    public IReadOnlyCollection<CustomerAddressModel> Addressess { get; set; }

    public Restaurant SelfPickupRestaurant { get; set; }
}