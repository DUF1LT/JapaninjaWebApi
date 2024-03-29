﻿namespace Japaninja.Models.Addresses;

public class CustomerAddressModel
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string FlatNumber { get; set; }

    public string Entrance { get; set; }

    public string Floor { get; set; }
}