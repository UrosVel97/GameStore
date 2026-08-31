namespace GameStore.Api.Features.Games.GetGameById;

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description
);

public record GameSummaryDto(
    Guid Id,
    string Name,
    string GenreName,
    decimal Price,
    DateOnly ReleaseDate
);