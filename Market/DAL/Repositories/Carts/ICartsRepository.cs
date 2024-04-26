using Market.Models;

namespace Market.DAL.Repositories.Carts;

public interface ICartsRepository
{
    public Task<Cart?> GetCartAsync(Guid customerId);


     public Task AddProductToCartAsync(Guid customerId, Guid productId);

    public Task RemoveProductFromCartAsync(Guid customerId, Guid productId);

    public Task ClearAll(Guid customerId);
}