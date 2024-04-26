using Market.Models;

namespace Market.DAL.Repositories.Orders;

public interface IOrdersRepository
{
    public  Task CreateOrderAsync(Order order);

    public  Task ChangeStateForOrder(Guid orderId, OrderState newState);

    public  Task<IReadOnlyCollection<Order>> GetOrdersForSeller(Guid sellerId, bool onlyCreated);
}