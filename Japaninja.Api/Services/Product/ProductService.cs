using System.ComponentModel;
using System.Linq.Expressions;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Logging;
using Japaninja.Models.Sorting;
using Japaninja.Repositories.Repositories.Product;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Japaninja.Services.Product;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
    {
        _unitOfWork = unitOfWorkFactory.Create();
    }

    public async Task<IReadOnlyCollection<DomainModel.Models.Product>> GetProducts(ProductType? type, SortByField? sortByField, SortByDirection? sortByDirection)
    {
        var productRepo = _unitOfWork.GetRepository<DomainModel.Models.Product, ProductRepository>();

        var baseQuery = productRepo.GetQuery();
        if (type is not null)
        {
            baseQuery = baseQuery.Where(t => t.ProductType == type);
        }

        if (sortByField is not null && sortByDirection is not null)
        {
            var predicate =  GetPredicateBySortByField(sortByField.Value);
            baseQuery = sortByDirection switch
            {
                SortByDirection.Ascending => baseQuery.OrderBy(predicate),
                SortByDirection.Descending => baseQuery.OrderByDescending(predicate),
                _ => baseQuery
            };
        }

        var productsOfType = await baseQuery.ToListAsync();

        return productsOfType;
    }

    public async Task<bool> EditProduct(string id, DomainModel.Models.Product product)
    {
        var productRepo = _unitOfWork.GetRepository<DomainModel.Models.Product, ProductRepository>();

        var editProduct = await productRepo.GetByIdAsync(id);
        if (editProduct is null)
        {
            LoggerContext.Current.LogError("Failed to edit product {Id}, because product doesn't exists", id);

            return false;
        }

        editProduct.Name = product.Name;
        editProduct.Description = product.Description;
        editProduct.Price = product.Price;
        editProduct.Ingredients = product.Ingredients;
        editProduct.Spiciness = product.Spiciness;
        editProduct.Weight = product.Weight;
        editProduct.ProductType = product.ProductType;
        editProduct.Image = product.Image;

        productRepo.Update(editProduct);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var productRepo = _unitOfWork.GetRepository<DomainModel.Models.Product, ProductRepository>();

        var product = await productRepo.GetByIdAsync(id);
        if (product is null)
        {
            LoggerContext.Current.LogError("Failed to delete product {Id}, because product doesn't exists", id);
            return false;
        }

        productRepo.Delete(product);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task AddNewProduct(DomainModel.Models.Product product)
    {
        var productRepo = _unitOfWork.GetRepository<DomainModel.Models.Product, ProductRepository>();

        productRepo.Add(product);

        await _unitOfWork.SaveChangesAsync();
    }


    private Expression<Func<DomainModel.Models.Product, object>> GetPredicateBySortByField(SortByField sortByField)
    {
        switch (sortByField)
        {
            case SortByField.Name:
                return p => p.Name;
            case SortByField.Price:
                return p => p.Price;
            default:
                throw new InvalidEnumArgumentException("Invalid sorting column was provided");
        }
    }
}