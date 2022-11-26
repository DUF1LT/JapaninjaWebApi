using Japaninja.Authorization;
using Japaninja.Creators.ProductCreator;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Product;
using Japaninja.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsManager)]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductCreator _productCreator;

    public ManagerController(
        IProductService productService,
        IProductCreator productCreator)
    {
        _productService = productService;
        _productCreator = productCreator;
    }


}