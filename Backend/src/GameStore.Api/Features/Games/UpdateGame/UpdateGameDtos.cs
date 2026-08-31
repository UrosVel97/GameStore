using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Features.Games.UpdateGame;

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [StringLength(500)] string Description
);
