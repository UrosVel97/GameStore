using System;
using GameStore.Api.Data;
using Microsoft.AspNetCore.Mvc;
using GameStore.Api.Shared.FileUpload;
using GameStore.Api.Features.Games.Constants;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(
        this IEndpointRouteBuilder app)
    {
        // PUT /games/{id}
        app.MapPut("/{id}", async (
            Guid id,
            [FromForm] UpdateGameDto gameDto,
            GameStoreContext dbContext,
            FileUploader fileUploader) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            var previousImageUri = existingGame.ImageUri;

            if (gameDto.ImageFile is not null)
            {
                var fileUploadResult = await fileUploader.UploadFileAsync(
                    gameDto.ImageFile,
                    StorageNames.GameImagesFolder);

                if (!fileUploadResult.IsSuccess)
                {
                    return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                }

                existingGame.ImageUri = fileUploadResult.FileUrl!;
            }

            existingGame.Name = gameDto.Name;
            existingGame.GenreId = gameDto.GenreId;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;

            var updateSucceeded = false;
            try
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                await dbContext.SaveChangesAsync();

                if (gameDto.ImageFile is not null)
                {
                    fileUploader.DeleteFile(previousImageUri, StorageNames.GameImagesFolder);
                }

                await transaction.CommitAsync();
                updateSucceeded = true;
            }
            finally
            {
                if (!updateSucceeded && gameDto.ImageFile is not null)
                {
                    fileUploader.DeleteFile(existingGame.ImageUri, StorageNames.GameImagesFolder);
                }
            }

            return Results.NoContent();

        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .DisableAntiforgery();

    }
}
