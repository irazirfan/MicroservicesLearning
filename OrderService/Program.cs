using Microsoft.EntityFrameworkCore;
using OrderService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite("Data Source=orders.db"));

builder.Services.AddControllers();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();