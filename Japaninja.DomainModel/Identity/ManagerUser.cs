using Japaninja.DomainModel.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.DomainModel.Identity;

public class ManagerUser : IdentityUser, IHasId
{
}