using System;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.FileUpload;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(
        this IEndpointRouteBuilder app)
    {
        // DELETE /games/{id}
        app.MapDelete("/{id}", async (
            Guid id,
            GameStoreContext dbContext,
            FileUploader fileUploader) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();

            fileUploader.DeleteFile(existingGame.ImageUri, StorageNames.GameImagesFolder);
            await transaction.CommitAsync();

            return Results.NoContent();

        });
    }
}
