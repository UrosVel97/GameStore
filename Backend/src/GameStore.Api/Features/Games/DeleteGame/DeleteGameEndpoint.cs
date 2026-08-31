using System;
using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(
        this IEndpointRouteBuilder app,
        GameStoreData data)
    {
        // DELETE /games/{id}
        app.MapDelete("/games/{id}", (Guid id) =>
        {
            var existingGame = data.GetGame(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            data.RemoveGame(existingGame.Id);

            return Results.NoContent();

        });
    }
}
