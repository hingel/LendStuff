using LendStuff.Server.Data;
using LendStuff.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using LendStuff.DataAccess;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;

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

builder.Services.AddScoped<IRepository<BoardGame>, BoardGameRepository>();

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

app.MapGet("/all", async (IRepository<BoardGame> brepo) => await brepo.GetAll());
app.MapPost("/addGame", async (IRepository<BoardGame> brepo, BoardGame toAdd) => await brepo.AddItem(toAdd));
app.MapPatch("/updateGame", async(IRepository<BoardGame> brepo, BoardGame newToUpdate) => await brepo.Update(newToUpdate));
app.MapDelete("/deleteGame", async (IRepository<BoardGame> brepo, string idToDelete) => await brepo.Delete(idToDelete));

app.MapGet("/getByT", async (IRepository<BoardGame> brepo, string idToFind) =>
{
	//Varianter på denna func kan sen finnas i en service för att leta efter titel etc.
	Func<BoardGame, bool> filterFunc = (BoardGame b) => b.Id == idToFind;

	var result = await brepo.FindByKey(filterFunc);

	return result;
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
