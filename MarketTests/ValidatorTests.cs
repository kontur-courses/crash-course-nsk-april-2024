using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Market.Controllers;
using Market.DAL.Repositories.Products;
using Market.DTO;
using Market.Enums;
using Market.Models;
using Market.Validators;
using NSubstitute;

namespace MarketTests;

public class Tests
{
    private IValidator<ProductDto> _validator;
    private ProductDto _productDto;
    private IValidator<ProductDto> _fakeValidator;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _validator = new ProductDtoValidator();
        
        _fakeValidator = Substitute.For<IValidator<ProductDto>>();
        _fakeValidator.Validate(null!).ReturnsForAnyArgs(new ValidationResult());
    }

    [SetUp]
    public void Setup()
    {
        _productDto = GenerateValidProductDto();
    }

    [Test]
    public void EmptyProductDtoShouldFail()
    {
        // arrange
        _productDto.Name = string.Empty;
        _productDto.Description = string.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    
    [Test]
    public void ProductDtoShouldPass()
    {
        // arrange
        var productDto = new ProductDto
        {
            Category = ProductCategory.Food,
            Description = Guid.NewGuid().ToString(),
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            PriceInRubles = 1,
            SellerId = Guid.NewGuid()
        };
        //act
        var validateResult = _validator.Validate(productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }
    [Test]
    public void EmptyIdPropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.Id = Guid.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ValidIdPropertyForProductDtoShouldPass()
    {
        // arrange
        _productDto.Id = Guid.NewGuid();
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }
    [Test]
    public void EmptyNamePropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.Name = string.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ShortLengthNamePropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.Name = new string('a', 1);
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ValidLengthNamePropertyForProductDtoShouldPass()
    {
        // arrange
        _productDto.Name = new string('a', 50);
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }
    [Test]
    public void EmptyDescriptionPropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.Description = string.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ShortLengthDescriptionPropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.Description = string.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ValidLengthDescriptionPropertyForProductDtoShouldPass()
    {
        // arrange
        _productDto.Description = new string('a', 200);
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }
    [Test]
    public void EmptySellerIdPropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.SellerId = Guid.Empty;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ValidSellerIdPropertyForProductDtoShouldPass()
    {
        // arrange
        _productDto.SellerId = Guid.NewGuid();
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }
    [Test]
    public void NotPositivePriceInRublesPropertyForProductDtoShouldFail()
    {
        // arrange
        _productDto.PriceInRubles = -150;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsFalse(validateResult.IsValid);
    }
    [Test]
    public void ValidPriceInRublesPropertyForProductDtoShouldPass()
    {
        // arrange
        _productDto.PriceInRubles = 150;
        //act
        var validateResult = _validator.Validate(_productDto);
        //assert
        Assert.IsTrue(validateResult.IsValid);
    }

    [Test]
    public void OnlyOneDBQueryShouldPass()
    {
        /*
        //arrange
        var productRepository = Substitute.For<IProductsRepository>();

        productRepository.GetProductsAsync().ReturnsForAnyArgs(new List<Product>());
        
        var mainValidator = Substitute.For<IEmailValidator>();

        var productController = new ProductsController(productRepository, mainValidator);
        //act
        await productController.searchProductsAsynk(new SearchProductRequestDto(null, null, null));
        //assert
        await productRepository.GetProductsAsync().ReceivedWithAnyArgs(1);
        */
    }

    private static ProductDto GenerateValidProductDto()
    {
        var productDto = new ProductDto
        {
            Category = ProductCategory.Food,
            Description = Guid.NewGuid().ToString(),
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            PriceInRubles = 1,
            SellerId = Guid.NewGuid()
        };
        return productDto;
    }
}