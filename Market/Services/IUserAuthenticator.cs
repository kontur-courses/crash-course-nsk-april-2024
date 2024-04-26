namespace Market.Services;

public interface IUserAuthenticator
{
    public Task<Guid?> AuthenticateUser(string login, string password);
}