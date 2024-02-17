using FastEndpoints;
using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;
using Order.DataAccess;
using Order.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository<Order.DataAccess.Models.Order>, OrderRepository>();

builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseFastEndpoints();


app.Run();
