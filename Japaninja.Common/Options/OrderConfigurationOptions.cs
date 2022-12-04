namespace Japaninja.Common.Options;

public class OrderConfigurationOptions
{
    public static string SectionName = "OrderConfiguration";


    public float DeliveryPrice { get; set; }

    public float MinDeliveryFreePrice { get; set; }
}