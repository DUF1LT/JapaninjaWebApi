using Japaninja.Creators.ProductCreator;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Product;
using Japaninja.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductCreator _productCreator;

    public ProductsController(
        IProductService productService,
        IProductCreator productCreator)
    {
        _productService = productService;
        _productCreator = productCreator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateProduct createProduct)
    {
        var product = _productCreator.CreateFrom(createProduct);
        await _productService.AddNewProduct(product);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(ProductType? type)
    {
        var products = await _productService.GetProducts(type);

        return Ok(products);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(string id, [FromBody]CreateProduct createProduct)
    {
        var product = _productCreator.CreateFrom(createProduct);
        var result = await _productService.EditProduct(id, product);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _productService.DeleteProduct(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}