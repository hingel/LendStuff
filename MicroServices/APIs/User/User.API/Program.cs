using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MassTransit;
using Messages.API.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using User.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD");

var connectionString = $"Data Source={host};Initial Catalog={database};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthorization(); //b => b.AddPolicy("RolePolicy", p => { p.RequireRole("user"); })); //Rollen �r inte aktiverad.

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

builder.Services.AddMassTransit(c =>
{
    c.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        //cfg.ConfigureEndpoints();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        context.Database.Migrate();
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("newUser", async (UserDbContext userDbContext, UserDto newUser) =>
{
    var user = await userDbContext.Users.FirstOrDefaultAsync(u =>
        u.UserName.ToLower() == newUser.UserName.ToLower() || u.Email.ToLower() == newUser.Email.ToLower());

    if (user != null)
        return new ServiceResponse<UserDto> { Message = "User already exists", Success = false };

    var userToAdd = new User.DataAccess.Models.User { UserName = newUser.UserName, Email = newUser.Email };

    userDbContext.Users.Add(userToAdd);
    await userDbContext.SaveChangesAsync();

    return new ServiceResponse<UserDto> { Data = DtoConverters.UserToDto(userToAdd), Message = $"User added{userToAdd.Id}", Success = true };
});

app.MapDelete("/{userId}", async (string userId, UserDbContext userDbContext, IBus bus, HttpContext httpContext) =>
{
    var activeUserName = httpContext.User.Identity?.Name;
    var userToDelete = await userDbContext.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

    if (userToDelete == null ||
        userToDelete.UserName != activeUserName ||
        userToDelete.ActiveOrders.Count > 0) 
        return new ServiceResponse<string> {Message = "Delete of user not allowed", Success = false};
    
    userDbContext.Remove(userToDelete);
    await userDbContext.SaveChangesAsync();

    await bus.Publish(new DeleteMessages(Guid.Parse(userId)));
    return new ServiceResponse<string> { Message = $"User {userId} deleted", Success = true };
}).RequireAuthorization();

app.Run();
