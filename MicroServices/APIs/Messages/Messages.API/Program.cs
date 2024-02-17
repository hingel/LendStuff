using LendStuff.Shared;
using Messages.API.Extensions;
using Messages.DataAccess;
using Messages.DataAccess.Models;
using Messages.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MessageDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IRepository<InternalMessage>, InternalMessageRepository>();


var app = builder.Build();


app.MapMessageEndPoints();

app.Run();
