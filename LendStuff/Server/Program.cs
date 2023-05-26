using LendStuff.Server.Data;
using LendStuff.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using LendStuff.DataAccess;
using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Extensions;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using static Duende.IdentityServer.Models.IdentityResources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.AddAuthorizationBuilder().AddPolicy("user_access", policy => policy.RequireUserName("c@cr.se"));

//Injectade services
builder.Services.AddScoped<IRepository<BoardGame>, BoardGameRepository>(); //Kan jag ha den injectad och även ingå i UnitofWork? Jepp, det är ok.
builder.Services.AddScoped<IRepository<ApplicationUser>, UserRepository>();
//builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<InternalMessage>, InternalMessageRepository>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<BoardGameService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapBoardGameEndPoints();
app.MapUserEndPoints();
app.MapOrderEndPoints();
app.MapMessageEndPoints();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
