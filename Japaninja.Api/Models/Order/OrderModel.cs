using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Addresses;

namespace Japaninja.Models.Order;

public class OrderModel
{
    public string Id { get; set; }

    public int NumberId { get; set; }

    public string CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhoneNumber { get; set; }

    public string CourierId { get; set; }

    public string RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; }

    public string CustomerAddressId { get; set; }

    public CustomerAddressModel CustomerAddress { get; set; }

    public IReadOnlyCollection<OrderProductModel> Products { get; set; }

    public float Price { get; set; }

    public IReadOnlyCollection<OrderCutleryModel> Cutlery { get; set; }

    public DateTime? DeliveryTime { get; set; }

    public DateTime? DeliveryFactTime { get; set; }

    public OrderStatus Status { get; set; }

    public string Comment { get; set; }
}