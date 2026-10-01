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
        app.MapPost("/", async (CreateGameDto game, GameStoreContext dbContext) =>
        {

            var genre = await dbContext.Genres.FindAsync(game.GenreId);

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

            await dbContext.SaveChangesAsync();

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
