using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.Server.Extensions;
using LendStuff.Shared;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using LendStuff.DataAccess.Data;
using LendStuff.Server.Services;

//using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors(options => 
{
	options.AddPolicy("myCorsSpec",
		policy =>
		{
			policy.WithOrigins("http://localhost:5173")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
}); //TODO: Detta måste styras upp på något annat sätt.


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>(
options =>
{
	//	options.IdentityResources["openid"].UserClaims.Add("username"); 
	//	options.ApiResources.Single().UserClaims.Add("username"); 
	options.IdentityResources["openid"].UserClaims.Add("role");
	options.ApiResources.Single().UserClaims.Add("role");
});
//TODO: Vilken typ av roll genererare är aktiv?

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role"); //Står nedan

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddAuthorizationBuilder()
	//.AddPolicy("admin_access", policy => policy.RequireUserName("c@cr.se"));
	.AddPolicy("admin_access", policy => policy.RequireRole("admin"));

//Injectade services

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IRepository<ApplicationUser>, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
//builder.Services.AddScoped<IProfileService, IdentityClaimsService>(); //Detta är tillagd för test med rollerna och namn.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role"); //Behövdes för att den inte ska mapp om rollen igen eller liknande. Som alternativ till det på rad: 44-50

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

//app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndPoints();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

//app.MapPost("/hej", (IHttpContextAccessor httpContextAccessor) => httpContextAccessor.HttpContext?.User.IsInRole("admin"));

//app.MapPost("/fillData/{userName}", async ([FromServices] IProfileService claimsService, [FromServices] RoleManager<IdentityRole> rolemgt, [FromServices] UserManager<ApplicationUser> usrMngr, string userName) =>
//{
//	//using (var scope = app.Services.CreateScope())
//	//{
//		//var rolemgt = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//		//var usrMngr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//		var role = await rolemgt.Roles.FirstOrDefaultAsync(r => r.Name == "admin");
		
//		await rolemgt.CreateAsync(new IdentityRole() { Name = "admin" });
//		role = await rolemgt.Roles.FirstOrDefaultAsync(r => r.Name == "admin");

//		var user = await usrMngr.FindByNameAsync(userName);

//		if (user is null)
//		{
//			return Results.Ok(new ServiceResponse<List<string>>()
//			{
//				Message = "User not found",
//				Success = false
//			});
//		}

//		if (await usrMngr.IsInRoleAsync(user, role.Name))
//		{
//			return Results.Ok(new ServiceResponse<List<string>>()
//			{
//				Message = "User already is in role",
//				Success = false
//			});
//		}

//		await usrMngr.AddToRoleAsync(user, role.Name);

//		return Results.Ok(new ServiceResponse<List<string>>()
//		{
//			Message = "OK",
//			Success = true
//		});
//	//}
	
//});




app.Run();
