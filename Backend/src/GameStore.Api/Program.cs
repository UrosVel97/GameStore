using System.ComponentModel.DataAnnotations;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.GetGameById;
using GameStore.Api.Models;
using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.UpdateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Genres.GetGenres;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

var app = builder.Build();


GameStoreData data = new();


app.MapGetGames(data);

app.MapGetGameById(data);

app.MapCreateGame(data);

app.MapUpdateGame(data);

app.MapDeleteGame(data);

app.MapGetGenres(data);


app.Run();





