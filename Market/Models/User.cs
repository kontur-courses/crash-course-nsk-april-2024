namespace Market.Models;

public class User
{
    public string Name { get; set; }
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsSeller { get; set; }
}