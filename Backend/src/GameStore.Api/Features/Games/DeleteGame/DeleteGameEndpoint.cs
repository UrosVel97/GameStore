using System;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(
        this IEndpointRouteBuilder app)
    {
        // DELETE /games/{id}
        app.MapDelete("/{id}", (Guid id, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDelete();

            return Results.NoContent();

        });
    }
}
