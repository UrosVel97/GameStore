using System;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.GetGameById;

public static class GetGameByIdEndpoint
{
    public static void MapGetGameById(
        this IEndpointRouteBuilder app)
    {
        //GET /games/{id}
        app.MapGet("/{id}", (Guid id, GameStoreData data) =>
        {
            Game? game = data.GetGame(id);

            return game is null ? Results.NotFound() :
            Results.Ok(new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.Genre.Id,
                        game.Price,
                        game.ReleaseDate,
                        game.Description
                    ));

        }).WithName(EndpointNames.GetGame);
    }
}
