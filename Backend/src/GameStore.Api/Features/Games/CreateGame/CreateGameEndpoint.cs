using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;
namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    private const string DefaultImageUri = "https://placehold.co/100";

    public static void MapCreateGame(
        this IEndpointRouteBuilder app)
    {
        //POST /games
        app.MapPost("/", async (
           [FromForm] CreateGameDto gameDto,
            GameStoreContext dbContext,
            ILogger<Program> logger,
            FileUploader fileUploader,
            ClaimsPrincipal user) =>
        {

            if (user?.Identity?.IsAuthenticated == false)
            {
                return Results.Unauthorized();
            }

            var currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Results.Unauthorized();
            }

            var imageUri = DefaultImageUri;

            if (gameDto.ImageFile is not null)
            {
                var fileUploadResult = await fileUploader.UploadFileAsync(
                    gameDto.ImageFile,
                    StorageNames.GameImagesFolder);

                if (!fileUploadResult.IsSuccess)
                {
                    return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                }

                imageUri = fileUploadResult.FileUrl;


            }

            var genre = await dbContext.Genres.FindAsync(gameDto.GenreId);

            if (genre is null)
            {
                return Results.BadRequest("Invalid genre ID.");
            }


            var newGame = new Game
            {
                Id = Guid.NewGuid(),
                Name = gameDto.Name,
                GenreId = gameDto.GenreId,
                Genre = genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Description = gameDto.Description,
                ImageUri = imageUri!,
                LastUpdatedBy = currentUserId
            };

            dbContext.Games.Add(newGame);

            await dbContext.SaveChangesAsync();


            logger.LogInformation(
                "Created a game {GameName} with price {GamePrice}",
                newGame.Name,
                newGame.Price);

            return Results.CreatedAtRoute(
                            EndpointNames.GetGame,
                            new { id = newGame.Id },
                            new GameSummaryDto(
                                newGame.Id,
                                newGame.Name,
                                newGame.Genre.Name,
                                newGame.Price,
                                newGame.ReleaseDate,
                                newGame.ImageUri,
                                newGame.LastUpdatedBy
                            ));
        })
        .Produces<GameSummaryDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .DisableAntiforgery();

    }
}
