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
builder.Services.AddScoped<IRepository<BoardGame>, BoardGameRepository>();
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
app.UseAuthorization();


#region Förtesting

//För spelen:
app.MapGet("/allGames", async (BoardGameService brepo) => await brepo.GetAll());
app.MapPost("/addGame", async (BoardGameService brepo, BoardGameDto toAdd) => await brepo.AddTitle(toAdd));
app.MapPatch("/updateGame", async(BoardGameService brepo, BoardGameDto newToUpdate) => await brepo.UpdateBoardGame(newToUpdate));
app.MapDelete("/deleteGame", async (BoardGameService brepo, string idToDelete) => await brepo.DeleteBoardGame(idToDelete));
app.MapGet("/getGameByT", async (BoardGameService service, string title) => await service.FindByTitle(title));

//För användaren:
app.MapGet("/getUsersGames", async (UserService service, string email) => await service.GetUsersGames(email));
app.MapGet("/getById", async (UserService service, string id) => await service.FindUserById(id));
app.MapPatch("/addBoardGameToUser", async (UserService service, string boardGameToAddId, string userEmail) => await service.AddBoardGameToUserCollection(boardGameToAddId, userEmail));

//Orders:
app.MapGet("/getOrders", async (OrderService service) => await service.GetAllOrders());
app.MapGet("/allUserOrders", async (OrderService service, string userDtoId) => await service.GetAllUserOrders(userDtoId));
app.MapPost("/postOrder", async (OrderService service, OrderDto newOrderDto) => await service.AddOrder(newOrderDto));
app.MapPatch("/updateOrder", async (OrderService service, OrderDto orderToUpdate) => await service.UpdateOrder(orderToUpdate));
app.MapDelete("/deleteOrder", async (OrderService service, string orderToDelete) => await service.DeleteOrder(orderToDelete));


//Messages:
//TODO: lägg till getAll. Men vet inte om den kommer att användas.
app.MapPost("/addMessage", async (MessageService service, MessageDto newMessage) => await service.AddMessage(newMessage));
app.MapGet("/getUsersMessages", async (MessageService service, string id) => await service.GetUserMessages(id));
app.MapDelete("/deleteMessage", async(MessageService service, int id) => await service.DeleteMessage(id));
app.MapPatch("/updateMessage", async (MessageService service, MessageDto messageToUpdate) => await service.UpdateMessage(messageToUpdate));

#endregion



app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
