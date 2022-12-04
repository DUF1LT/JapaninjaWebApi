using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class Restaurant : IHasId
{
    public const int MaxAddressLength = 200;


    public string Id { get; set; }

    public string Address { get; set; }
}