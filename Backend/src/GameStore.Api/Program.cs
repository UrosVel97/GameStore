using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

var app = builder.Build();


GameStoreData data = new();

app.MapGameEndpoints(data);
app.MapGenreEndpoints(data);


app.Run();





