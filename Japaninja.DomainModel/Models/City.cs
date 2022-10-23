using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class City : IHasId
{
    public const int MaxNameLength = 50;


    public string Id { get; set; }

    public string Name { get; set; }

    public IReadOnlyCollection<Restaurant> Restaurants { get; set; }
}