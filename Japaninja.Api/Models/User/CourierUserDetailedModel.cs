namespace Japaninja.Models.User;

public class CourierUserDetailsModel : CourierUserModel
{
    public int OrdersAmount { get; set; }

    public int AverageRating { get; set; }
}