using Market.DAL.Repositories.Users;
using Market.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersControllers : ControllerBase
{
    public UsersControllers(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    private IUserRepository UserRepository { get; }
    
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