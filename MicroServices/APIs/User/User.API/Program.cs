using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD");

var connectionString = $"Data Source={host};Initial Catalog={database};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthorization(b => b.AddPolicy("RolePolicy", p
    => { p.RequireRole("user"); }));

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

        //options.Audience = "https://localhost:7272";
        //options.TokenValidationParameters = new TokenValidationParameters()
        //{
        //    ValidateIssuer = true,
        //    ValidIssuer = "lendstuff", //"https://localhost:7272",
        //    ValidateAudience = true,
        //    ValidAudience = "http://localhost:5000",
        //    ValidateIssuerSigningKey = true,
        //    IssuerSigningKeys = new List<SecurityKey>() { new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!&/(()=()=)")) },
        //    //IssuerSigningKeys = new List<SecurityKey>() { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder!.Configuration["Authentication:GitHub:ClientSecret"])) },
        //    ValidateLifetime = true
        //};
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

app.MapPost("/login/{userName}", (UserDbContext dbContext, HttpContext httpContext, string userName) =>
{
    //Denna är till för om vi skulle ha egen chech om autentiserad.

    var subClaim = new Claim("sub", Guid.NewGuid().ToString());
    var nameClaim = new Claim("name", userName);
    var roleClaim = new Claim("role", "user");
    //var claims = new ClaimsIdentity(new List<Claim> { claim });

    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!&/(()=()=)")); //TODO: Skulle kunna lägga denna i secrets ändå!!
    //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:GitHub:ClientSecret"]));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    var tokenOptions = new JwtSecurityToken(
        issuer: "lendstuff",//"https://localhost:7272",
        audience: "https://localhost:7272",
        claims: new List<Claim> { subClaim, nameClaim, roleClaim },
        expires: DateTime.Now.AddMinutes(10),
        signingCredentials: signinCredentials);

    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

    //var claimIdentity = new ClaimsIdentity(new List<Claim> { claim }, BearerTokenDefaults.AuthenticationScheme);

    //context.User.AddIdentities(new List<ClaimsIdentity>() { claimIdentity });


    return token;
});

app.MapGet("/test", () => "Hello!").RequireAuthorization();

app.Run();
