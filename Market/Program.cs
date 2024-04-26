using Market.Controllers;
using Market.DAL;
using Market.DAL.Repositories.Carts;
using Market.DAL.Repositories.Orders;
using Market.DAL.Repositories.Products;
using Market.DAL.Repositories.Users;
using Market.Filters;
using Market.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(c =>
{
    c.Filters.Add<ExceptionFilter>();
});



builder.Services
    .AddDbContext<RepositoryContext>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<ICartsRepository, CartsRepository>()
    .AddScoped<IProductsRepository, ProductsRepository>()
    .AddScoped<IOrdersRepository, OrdersRepository>();

builder.Services
    .AddScoped<UsersControllers>()
    .AddScoped<ProductsController>()
    .AddScoped<OrdersControllers>()
    .AddScoped<CartsControllers>();

builder.Services
    .AddScoped<UserAuthenticator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();