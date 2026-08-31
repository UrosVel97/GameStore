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
        this IEndpointRouteBuilder app,
        GameStoreData data)
    {
        var group = app.MapGroup("/games");

        group.MapGetGames(data);

        group.MapGetGameById(data);

        group.MapCreateGame(data);

        group.MapUpdateGame(data);

        group.MapDeleteGame(data);
    }

}
