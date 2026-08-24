using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Game> games =
[
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27)
    }
];

//GET /games
app.MapGet("/games", () => games);

//GET /games/{id}
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(u => u.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);

}).WithName(GetGameEndpointName);

//POST /games
app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);

    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new { id = game.Id },
        game);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (Guid id, Game game) =>
{
    var existingGame = games.Find(u => u.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = game.Name;
    existingGame.Genre = game.Genre;
    existingGame.Price = game.Price;
    existingGame.ReleaseDate = game.ReleaseDate;

    return Results.NoContent();

});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (Guid id) =>
{
    var existingGame = games.Find(u => u.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    games.Remove(existingGame);

    return Results.NoContent();

});

app.Run();
