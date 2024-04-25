using Market.DAL.Repositories.Users;
using Market.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("users")]
public class UsersControllers : ControllerBase
{
    public UsersControllers()
    {
        UserRepository = new UserRepository();
    }

    private UserRepository UserRepository { get; }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequestDto userInfo)
    {
        return await UserRepository.CreateUser(userInfo.Name, userInfo.Login, userInfo.Password);
    }
    
    [HttpPost("{userId:guid}/set-as-seller")]
    public async Task<IActionResult> SetUserAsSeller([FromRoute] Guid userId)
    {
        await UserRepository.SetSellerState(userId, true);
        return Ok();
    }
}