using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class OrdersCutlery : IHasId
{
    public string Id { get; set; }

    public string OrderId { get; set; }

    public Order Order { get; set; }

    public string CutleryId { get; set; }

    public Cutlery Cutlery { get; set; }

    public int Amount { get; set; }
}