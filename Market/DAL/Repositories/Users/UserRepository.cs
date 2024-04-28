using System.Security.Cryptography;
using Market.Exceptions;
using Market.Helpers;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories.Users;

internal sealed class UserRepository : IUserRepository
{
    private static MD5 _md5 = MD5.Create();

    private readonly RepositoryContext _context;

    public UserRepository(RepositoryContext repositoryContext)
    {
        _context = repositoryContext;
    }

    public async Task<Guid> CreateUser(string name, string login, string password)
    {
        var salt = Guid.NewGuid().ToString();
        var passwordHash = PasswordHelper.GetPasswordHash(password, salt);
        ;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = login,
            Name = name,
            Salt = salt,
            PasswordHash = passwordHash,
            IsSeller = false
        };

        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ErrorRegistry.InternalServerError();
        }
    }

    public Task<User?> GetUser(string login) =>
        _context.Users.FirstOrDefaultAsync(s => s.Login == login);

    public async Task SetSellerState(Guid userId, bool newState)
    {
        var user = await _context.Users.FirstOrDefaultAsync(s => s.Id == userId);
        if (user == null)
            throw ErrorRegistry.NotFound("user", userId);

        user.IsSeller = newState;

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
}