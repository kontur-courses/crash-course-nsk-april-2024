using Market.Enums;
using Market.Helpers;
using Market.Misc;
using Market.Models;

namespace Market.DAL;

internal class DataInitializer
{
    private static readonly Random Random = Random.Shared;
    private static readonly ProductCategory[] Categories = Enum.GetValues<ProductCategory>();

    private static Product[] _products;
    private static Cart[] _carts;
    private static User[] _users;
    
    public DataInitializer()
    {
        InitializeData();
    }
    
    public Product[] GetSeedProducts() => _products;
    public User[] GetSeedUsers() => _users;
    public Cart[] GetSeedCarts() => _carts;

    private static void InitializeData()
    {
        _users = CreateUsers(10);
        _carts = CreateCarts(_users);
        _products = CreateProducts(_users, 100);
    }

    private static User[] CreateUsers(int count)
    {
        var resultUsers = new User[count];
        foreach (var index in Enumerable.Range(0, count))
        {
            var salt = Guid.NewGuid().ToString();
            var password = string.Join("", Enumerable.Repeat(index, 3));
            var passHash = PasswordHelper.GetPasswordHash(password, salt);

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Login = "user-" + index,
                Name = "user name " + index,
                Salt = salt,
                PasswordHash = passHash,
                IsSeller = false,
            };

            resultUsers[index] = user;
        }

        return resultUsers;
    }
    
    private static Cart[] CreateCarts(IEnumerable<User> users) => 
        users.Select(u => new Cart { CustomerId = u.Id, ProductIds = new List<Guid>() }).ToArray();

    private static Product[] CreateProducts(User[] users, int totalCount)
    {
        var resultProducts = new Product[totalCount];
        foreach (var index in Enumerable.Range(0, totalCount))
        {
            var user = users[Random.Next(0, users.Length)];
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = $"Product-{index}",
                Description = $"Some description for product-{index}",
                PriceInRubles = (decimal)Random.NextDouble(100, 10000),
                Category = Categories[Random.Next(Categories.Length)],
                SellerId = user.Id
            };

            resultProducts[index] = product;
        }

        return resultProducts;
    }
}