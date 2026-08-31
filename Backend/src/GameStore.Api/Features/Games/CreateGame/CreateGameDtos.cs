using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Features.Games.CreateGame;

public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [StringLength(500)] string Description
);


public record GameSummaryDto(
    Guid Id,
    string Name,
    string GenreName,
    decimal Price,
    DateOnly ReleaseDate
);