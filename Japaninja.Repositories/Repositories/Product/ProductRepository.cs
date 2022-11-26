using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.Product;

public class ProductRepository : Repository<DomainModel.Models.Product>, IProductRepository
{
    public ProductRepository(DbContext dbContext)
        : base(dbContext)
    { }
}