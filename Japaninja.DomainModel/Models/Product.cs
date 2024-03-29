﻿using Japaninja.DomainModel.Models.Enums;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class Product : IHasId
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 100;
    public const int MaxWeightLength = 50;


    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Weight { get; set; }

    public float Price { get; set; }

    public ProductType ProductType { get; set; }

    public Spiciness Spiciness { get; set; }

    public string Ingredients { get; set; }

    public string Image { get; set; }

    //public IReadOnlyCollection<ProductsIngredients> ProductsIngredients { get; set; }
}