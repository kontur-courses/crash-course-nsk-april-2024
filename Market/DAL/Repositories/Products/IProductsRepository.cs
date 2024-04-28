using Market.Enums;
using Market.Models;

namespace Market.DAL.Repositories.Products;

public interface IProductsRepository
{
    public  Task<IReadOnlyCollection<Product>> GetProductsAsync(
        string? name = null,
        Guid? sellerId = null,
        ProductCategory? category = null,
        int skip = 0,
        int take = 50);

    public Task<Product?> GetProductAsync(Guid productId);

    public  Task CreateProductAsync(Product product);

    public  Task UpdateProductAsync(Guid productId, ProductUpdateInfo updateInfo);

    public  Task DeleteProductAsync(Guid productId);
}