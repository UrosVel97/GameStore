using System;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(
        this IEndpointRouteBuilder app,
        GameStoreData data)
    {
        //POST /games
        app.MapPost("/", (CreateGameDto game) =>
        {

            var genre = data.GetGenre(game.GenreId);

            if (genre is null)
            {
                return Results.BadRequest("Invalid genre ID.");
            }
            var newGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Genre = genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description
            };

            data.AddGame(newGame);

            return Results.CreatedAtRoute(
                EndpointNames.GetGame,
                new { id = newGame.Id },
                new GameSummaryDto(
                    newGame.Id,
                    newGame.Name,
                    newGame.Genre.Name,
                    newGame.Price,
                    newGame.ReleaseDate
                ));
        });

    }
}
