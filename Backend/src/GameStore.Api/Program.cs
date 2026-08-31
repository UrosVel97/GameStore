using System.ComponentModel.DataAnnotations;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.GetGameById;
using GameStore.Api.Models;
using GameStore.Api.Features.Games.CreateGame;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

var app = builder.Build();


GameStoreData data = new();


app.MapGetGames(data);

app.MapGetGameById(data);

app.MapCreateGame(data);


// PUT /games/{id}
app.MapPut("/games/{id}", (Guid id, UpdateGameDto gameDto) =>
{
    var existingGame = data.GetGame(id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }


    var genre = data.GetGenre(gameDto.GenreId);
    if (genre is null)
    {
        return Results.BadRequest("Invalid genre ID.");
    }

    existingGame.Name = gameDto.Name;
    existingGame.Genre = genre;
    existingGame.Price = gameDto.Price;
    existingGame.ReleaseDate = gameDto.ReleaseDate;
    existingGame.Description = gameDto.Description;

    return Results.NoContent();

});

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

//GET /genres
app.MapGet("/genres", () =>
    data.GetGenres().Select(genre =>
        new GenreDto(
            genre.Id,
            genre.Name
        )
));

app.Run();



public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [StringLength(500)] string Description
);

public record GenreDto(Guid Id, string Name);

