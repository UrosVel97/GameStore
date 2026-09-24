using System;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(
        this IEndpointRouteBuilder app)
    {
        //POST /games
        app.MapPost("/", (CreateGameDto game, GameStoreContext dbContext) =>
        {

            var genre = dbContext.Genres.Find(game.GenreId);

            if (genre is null)
            {
                return Results.BadRequest("Invalid genre ID.");
            }


            var newGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                GenreId = game.GenreId,
                Genre = genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description
            };

            dbContext.Games.Add(newGame);

            dbContext.SaveChanges();

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
        })
        .Produces<GameSummaryDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

    }
}
