using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class CouriersOrders: IHasId
{
    public string Id { get; set; }

    public string CourierId { get; set; }

    public CourierUser Courier { get; set; }

    public string OrderId { get; set; }

    public Order Order { get; set; }
}