namespace Japaninja.Models.User;

public class CourierUserDetailsModel : CourierUserModel
{
    public string Image { get; set; }

    public int OrdersAmount { get; set; }

    public int AverageRating { get; set; }
}