using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services.AddSingleton<GameStoreData>();
builder.Services.AddTransient<GameDataLogger>();

var app = builder.Build();




app.MapGameEndpoints();
app.MapGenreEndpoints();


app.Run();





