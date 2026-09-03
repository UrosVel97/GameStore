using System;
using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(
        this IEndpointRouteBuilder app)
    {
        // DELETE /games/{id}
        app.MapDelete("/{id}", (Guid id, GameStoreData data) =>
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
