using Market.DAL.Repositories.Users;
using Market.Helpers;

namespace Market.Services;

internal class UserAuthenticator : IUserAuthenticator
{
    private readonly IUserRepository _userRepository;

    public UserAuthenticator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Guid?> AuthenticateUser(string login, string password)
    {
        var user = await _userRepository.GetUser(login);
        if (user == null)
            return null;

        var passwordHash = PasswordHelper.GetPasswordHash(password, user.Salt);
        return passwordHash == user.PasswordHash ? user.Id : null;
    }
}