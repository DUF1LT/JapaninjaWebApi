namespace Japaninja.Models.Order;

public class OrderProductModel
{
    public DomainModel.Models.Product Product { get; set; }

    public int Amount { get; set; }
}