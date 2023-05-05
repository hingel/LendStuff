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
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

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

//Injectade services
//builder.Services.AddScoped<IRepository<BoardGame>, BoardGameRepository>();
builder.Services.AddScoped<IRepository<ApplicationUser>, UserRepository>();
//builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<BoardGameService>();
builder.Services.AddScoped<OrderService>();

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
app.UseAuthorization();


#region Förtesting

//För spelen:
app.MapGet("/allGames", async (BoardGameService brepo) => await brepo.GetAll());
app.MapPost("/addGame", async (BoardGameService brepo, BoardGameDto toAdd) => await brepo.AddTitle(toAdd));
app.MapPatch("/updateGame", async(BoardGameService brepo, BoardGameDto newToUpdate) => await brepo.UpdateBoardGame(newToUpdate));
app.MapDelete("/deleteGame", async (BoardGameService brepo, string idToDelete) => await brepo.DeleteBoardGame(idToDelete));

//app.MapGet("/getByT", async (IRepository<BoardGame> brepo, string idToFind) =>
//{
//	//Varianter på denna func kan sen finnas i en service för att leta efter del i titel etc.
//	Func<BoardGame, bool> filterFunc = (BoardGame b) => b.Id == idToFind;

//	var result = await brepo.FindByKey(filterFunc);

//	return result;
//});

//För användaren:

app.MapGet("/getUsersGames", async (UserService service, string email) =>
{
	var response = await service.GetUsersGames(email);

	return response;
});

//app.MapPatch("/patchUser", async (UserService service, IRepository<BoardGame> brepo, string email) =>
//{
//	var result = await brepo.GetAll();
//	var gameToAdd = result.FirstOrDefault();
	
//	var response = service.AddBoardGameToUserCollection(gameToAdd, email);
//});

app.MapPost("/postOrder", async (OrderService service, OrderDto newOrderDto) => await service.AddOrder(newOrderDto));
app.MapGet("/getOrders", async (OrderService service) => await service.GetAllOrders());


#endregion



app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
