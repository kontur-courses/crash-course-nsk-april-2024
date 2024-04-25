namespace Market.DTO;

public class CartDto
{
    public Guid CustomerId { get; set; }

    public List<Guid> ProductIds { get; set; } = new();
}