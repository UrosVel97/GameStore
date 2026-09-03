using System;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Games.GetGameById;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.UpdateGame;

namespace GameStore.Api.Features.Games;

public static class GameEndpoints
{
    public static void MapGameEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games");

        group.MapGetGames();
        group.MapGetGameById();
        group.MapCreateGame();
        group.MapUpdateGame();
        group.MapDeleteGame();
    }

}
