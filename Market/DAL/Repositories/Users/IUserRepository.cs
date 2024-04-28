using Market.Models;

namespace Market.DAL.Repositories.Users;

public interface IUserRepository
{
    public  Task<Guid> CreateUser(string name, string login, string password);

    public Task<User?> GetUser(string login);

    public  Task SetSellerState(Guid userId, bool newState);
}