using System.ComponentModel.DataAnnotations;
using Market.Enums;

namespace Market.DTO;

public record SearchProductRequestDto(
    string? ProductName,
    SortType? SortType,
    ProductCategory? Category,
    bool Ascending = true,
    int Skip = 0,
    int Take = 50);


public class Positive : ValidationAttribute
{
    public Positive() : base("Value should not be negative.")
    {
    }
    
    public override bool IsValid(object? value)
    {
        if (value is not int intValue)
            return true;

        return intValue >= 0;
    }
}