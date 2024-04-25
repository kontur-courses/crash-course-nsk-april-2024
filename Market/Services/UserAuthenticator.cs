using Market.DAL.Repositories.Users;
using Market.Helpers;

namespace Market.Services;

public class UserAuthenticator
{
    private readonly UserRepository _userRepository;

    public UserAuthenticator()
    {
        _userRepository = new UserRepository();
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