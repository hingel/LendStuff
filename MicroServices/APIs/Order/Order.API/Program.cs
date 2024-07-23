using FastEndpoints;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Order.API.Consumers;
using Order.API.Helpers;
using Order.DataAccess;
using Order.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration["Auth0:Audience"],
        ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}/",
        ValidateLifetime = true
    };
});

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD");

var connectionString = $"Data Source={host};Initial Catalog={database};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<ClientFactory>();
builder.Services.AddFastEndpoints();
builder.Services.AddHttpClient();

builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<DeleteOrders>();

    c.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbit", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Use((ctx, next) =>
{
    var test = ctx.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

    return next();
});

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    context.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();

public partial class Program { }