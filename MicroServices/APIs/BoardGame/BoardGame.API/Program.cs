using BoardGame.API.Extensions;
using BoardGame.API.Helpers;
using BoardGame.DataAccess;
using BoardGame.DataAccess.Models;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BoardGameDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IRepository<BoardGame.DataAccess.Models.BoardGame>, BoardGameRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<UnitOfWork>();


var app = builder.Build();

app.MapBoardGameEndPoints();

app.Run();
