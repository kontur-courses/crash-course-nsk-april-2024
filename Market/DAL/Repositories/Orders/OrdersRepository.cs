using Market.Exceptions;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories.Orders;

internal class OrdersRepository
{
    private readonly RepositoryContext _context;

    public OrdersRepository()
    {
        _context = new RepositoryContext();
    }

    public async Task CreateOrderAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }

    public async Task ChangeStateForOrder(Guid orderId, OrderState newState)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            throw ErrorRegistry.NotFound("order", orderId);

        order.State = newState;

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

    public async Task<IReadOnlyCollection<Order>> GetOrdersForSeller(Guid sellerId, bool onlyCreated)
    {
        var query = _context.Orders.Where(o => o.SellerId == sellerId);
        
        if (onlyCreated) 
            query = query.Where(o => o.State == OrderState.Created);

        return await query.ToListAsync();
    }
}