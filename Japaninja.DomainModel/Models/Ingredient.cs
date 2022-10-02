﻿using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class Ingredient : IHasId
{
    public const int MaxNameLength = 50;


    public string Id { get; set; }

    public string Name { get; set; }
}