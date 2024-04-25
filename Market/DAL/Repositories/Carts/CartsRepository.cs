using Market.Exceptions;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories.Carts;

internal sealed class CartsRepository
{
    private readonly RepositoryContext _context;

    public CartsRepository()
    {
        _context = new RepositoryContext();
    }

    public Task<Cart?> GetCartAsync(Guid customerId) =>
        _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));


    public async Task AddProductToCartAsync(Guid customerId, Guid productId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
        if (cart == null)
            throw ErrorRegistry.NotFound("order of customer", customerId);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            throw ErrorRegistry.NotFound("product", productId);

        cart.ProductIds = new List<Guid>(cart.ProductIds) { productId };

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

    public async Task RemoveProductFromCartAsync(Guid customerId, Guid productId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
        if (cart == null)
            throw ErrorRegistry.NotFound("order of customer", customerId);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            throw ErrorRegistry.NotFound("product", productId);

        cart.ProductIds = cart.ProductIds.Where(p => p != productId).ToList();

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

    public async Task ClearAll(Guid customerId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        if (cart == null)
            throw ErrorRegistry.NotFound("customer", customerId);

        try
        {
            cart.ProductIds = new List<Guid>();

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }
}