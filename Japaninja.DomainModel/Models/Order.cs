using System.ComponentModel.DataAnnotations.Schema;
using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class Order : IHasId
{
    public const int MaxCommentLength = 500;

    public string Id { get; set; }

    public int NumberId { get; set; }

    public string CustomerId { get; set; }

    public CustomerUser Customer { get; set; }

    public string CourierId { get; set; }

    public CourierUser Courier { get; set; }

    public string RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; }

    public string CustomerAddressId { get; set; }

    public CustomerAddress CustomerAddress { get; set; }

    public IReadOnlyCollection<OrdersProducts> Products { get; set; }

    public IReadOnlyCollection<OrdersCutlery> Cutlery { get; set; }

    public DateTime DeliveryTime { get; set; }

    public DateTime DeliveryFactTime { get; set; }

    public OrderStatus Status { get; set; }

    public string Comment { get; set; }
}