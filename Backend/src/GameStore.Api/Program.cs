using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connectionString);

builder.Services.AddValidation();

builder.Services.AddSingleton<GameStoreData>();
builder.Services.AddTransient<GameDataLogger>();




var app = builder.Build();




app.MapGameEndpoints();
app.MapGenreEndpoints();

app.MigrateDb();

app.Run();





