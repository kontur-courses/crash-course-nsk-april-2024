using Market.DAL;
using Market.DAL.Repositories.Products;
using Market.DTO;
using Market.Enums;
using Market.Exceptions;
using Market.Filters;
using Market.Models;
using Market.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductsController : ControllerBase
{
    private readonly MainValidator _validator;

    public ProductsController()
    {
        ProductsRepository = new ProductsRepository();
        _validator = new MainValidator();
    }

    private ProductsRepository ProductsRepository { get; }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid productId)
    {
        var product = await ProductsRepository.GetProductAsync(productId);
        return product != null 
            ? ProductDto.FromModel(product) 
            : throw ErrorRegistry.NotFound("product", productId);
    }

    [HttpPost("search")]
    public async Task<ActionResult<List<ProductDto>>> SearchProductsAsync([FromBody] SearchProductRequestDto requestInfo)
    {
        await _validator.Validate(requestInfo);
        
        var products =
            await ProductsRepository.GetProductsAsync(
                requestInfo.ProductName,
                category: requestInfo.Category,
                skip: requestInfo.Skip,
                take: requestInfo.Take);

        if (!requestInfo.SortType.HasValue)
            return products.Select(ProductDto.FromModel).ToList();

        return SortProducts(products, requestInfo.SortType.Value, requestInfo.Ascending)
            .Select(ProductDto.FromModel)
            .ToList();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetSellerProductsAsync(
        [FromQuery] Guid sellerId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var products = await ProductsRepository.GetProductsAsync(
            sellerId: sellerId, 
            skip: skip, 
            take: take);
        return products.Select(ProductDto.FromModel).ToList();
    }

    [HttpPost]
    [CheckAuthFilter]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductDto product)
    {
        await _validator.Validate(product);
        await ProductsRepository.CreateProductAsync(new Product
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            PriceInRubles = product.PriceInRubles,
            SellerId = product.SellerId
        });

        return Ok();
    }

    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId,
        [FromBody] UpdateProductRequestDto requestInfo)
    {
        await ProductsRepository.UpdateProductAsync(productId, new ProductUpdateInfo
        {
            Name = requestInfo.Name,
            Description = requestInfo.Description,
            Category = requestInfo.Category,
            PriceInRubles = requestInfo.PriceInRubles
        });

        return Ok();
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
        await ProductsRepository.DeleteProductAsync(productId);
        return Ok();
    }

    private static IEnumerable<Product> SortProducts(IEnumerable<Product> products, SortType sortType,
        bool ascending)
    {
        switch (sortType)
        {
            case SortType.Name:
                return ascending
                    ? products.OrderBy(p => p.Name)
                    : products.OrderByDescending(p => p.Name);
            case SortType.Price:
            default:
                return ascending
                    ? products.OrderBy(p => p.PriceInRubles)
                    : products.OrderByDescending(p => p.PriceInRubles);
        }
    }
}