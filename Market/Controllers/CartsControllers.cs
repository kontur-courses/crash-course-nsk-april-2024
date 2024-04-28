using Market.DAL.Repositories.Carts;
using Market.Exceptions;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("customers/{customerId:guid}/cart")]
public sealed class CartsControllers : ControllerBase
{
    public CartsControllers(ICartsRepository cartsRepository)
    {
        CartsRepository = cartsRepository;
    }

    private ICartsRepository CartsRepository { get; }
    
    [HttpGet]
    public async Task<ActionResult<Cart>> GetCartAsync([FromRoute] Guid customerId)
    {
        var cart = await CartsRepository.GetCartAsync(customerId);
        return cart ?? throw ErrorRegistry.NotFound("car", customerId);
    }
    
    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductAsync([FromRoute] Guid customerId, [FromBody] Guid productId)
    {
        await CartsRepository.AddProductToCartAsync(customerId, productId);
        return Ok();
    }
    
    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProductAsync(Guid customerId, [FromBody] Guid productId)
    {
        await CartsRepository.RemoveProductFromCartAsync(customerId, productId);
        return Ok();
    }
    
    [HttpPost("clear")]
    public async Task<IActionResult> ClearAsync(Guid customerId)
    {
        await CartsRepository.ClearAll(customerId);
        return Ok();
    }
}