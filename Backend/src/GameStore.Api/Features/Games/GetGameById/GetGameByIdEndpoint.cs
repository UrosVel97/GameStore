using System;
using System.Diagnostics;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using Microsoft.Data.Sqlite;

namespace GameStore.Api.Features.Games.GetGameById;

public static class GetGameByIdEndpoint
{
    public static void MapGetGameById(
        this IEndpointRouteBuilder app)
    {
        //GET /games/{id}
        app.MapGet("/{id}", async (
            Guid id,
            GameStoreContext dbContext,
            ILogger<Program> logger) =>
        {

            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() :
                Results.Ok(new GameDetailsDto(
                            game.Id,
                            game.Name,
                            game.GenreId,
                            game.Price,
                            game.ReleaseDate,
                            game.Description,
                            game.ImageUri,
                            game.LastUpdatedBy
                        ));


        }).WithName(EndpointNames.GetGame);
    }

}
