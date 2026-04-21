using Microsoft.EntityFrameworkCore;
using DemoAPI.Data;
using DemoAPI.Models;
using DemoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DemoDB"));

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Id = 1, Name = "Laptop Gaming", Category = "Elektronik", Price = 15000000, Stock = 10 },
            new Product { Id = 2, Name = "Mouse Wireless", Category = "Aksesoris", Price = 250000, Stock = 50 },
            new Product { Id = 3, Name = "Keyboard Mechanical", Category = "Aksesoris", Price = 850000, Stock = 25 }
        );
        context.SaveChanges();
    }
}

app.Run();
