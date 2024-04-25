using Market.DAL.Repositories;
using Market.DAL.Repositories.Orders;
using Market.DTO;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("orders")]
public class OrdersControllers : ControllerBase
{
    public OrdersControllers()
    {
        OrdersRepository = new OrdersRepository();
    }

    private OrdersRepository OrdersRepository { get; }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderDto order)
    {
        await OrdersRepository.CreateOrderAsync(new Order
        {
            Id = order.Id,
            State = OrderState.Created,
            CustomerId = order.CustomerId,
            ProductId = order.ProductId,
            SellerId = order.SellerId
        });

        return Ok();
    }

    [HttpPost("{orderId:guid}/set-state")]
    public async Task<IActionResult> SetState([FromRoute] Guid orderId, [FromBody] OrderState state)
    {
        await OrdersRepository.ChangeStateForOrder(orderId, state);
        return Ok();
    }

    [HttpGet("{sellerId:guid}")]
    public async Task<ActionResult<List<OrderDto>>> GetOrders([FromRoute] Guid sellerId, [FromQuery] bool onlyCreated)
    {
        var orders = await OrdersRepository.GetOrdersForSeller(sellerId, onlyCreated);
        return orders.Select(OrderDto.FromModel).ToList();
    }
}