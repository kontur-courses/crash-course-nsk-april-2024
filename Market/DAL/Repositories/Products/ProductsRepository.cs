using Market.Enums;
using Market.Exceptions;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories.Products;

internal sealed class ProductsRepository : IProductsRepository
{
    private readonly RepositoryContext _context;

    public ProductsRepository(RepositoryContext repositoryContext)
    {
        _context = repositoryContext;
    }

    public async Task<IReadOnlyCollection<Product>> GetProductsAsync(
        string? name = null, 
        Guid? sellerId = null, 
        ProductCategory? category = null,
        int skip = 0,
        int take = 50)
    {
        IQueryable<Product> query = _context.Products;

        if (name is not null)
            query = query.Where(p => p.Name == name);
        if (sellerId.HasValue)
            query = query.Where(p => p.SellerId == sellerId.Value);
        if (category is not null)
            query = query.Where(p => p.Category == category);

        var products = await query.Skip(skip).Take(take).ToListAsync();

        return products;
    }

    public Task<Product?> GetProductAsync(Guid productId) => 
        _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

    public async Task CreateProductAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }

    public async Task UpdateProductAsync(Guid productId, ProductUpdateInfo updateInfo)
    {
        var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (productToUpdate is null)
            throw ErrorRegistry.NotFound("product", productId);

        if(updateInfo.Name != null)
            productToUpdate.Name = updateInfo.Name;
        if(updateInfo.Description != null)
            productToUpdate.Description = updateInfo.Description;
        if(updateInfo.Category.HasValue)
            productToUpdate.Category = updateInfo.Category.Value;
        if(updateInfo.PriceInRubles.HasValue)
            productToUpdate.PriceInRubles = updateInfo.PriceInRubles.Value;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            throw ErrorRegistry.NotFound("product", productId);

        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }
}