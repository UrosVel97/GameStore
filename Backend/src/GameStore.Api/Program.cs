using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Genre> genres =
[
    new Genre
    {
        Id = new Guid("e256264a-1377-42cb-9c1f-29f42f0c8094"),
        Name = "Fighting"
    },
    new Genre
    {
        Id = new Guid("80d4c16e-1b18-4041-9a76-3433a8b902f0"),
        Name = "Kids and family"
    },
    new Genre
    {
        Id = new Guid("4d5738c0-aaa5-41f3-bfdd-e620fa8c7c5d"),
        Name = "Racing"
    },
        new Genre
    {
        Id = new Guid("7f33722f-dbb4-44cf-a10a-320e38e4582c"),
        Name = "Roleplaying"
    },
        new Genre
    {
        Id = new Guid("214d9091-9b1a-4008-b10a-dc2bcf8f17d8"),
        Name = "Sports"
    }
];

List<Game> games =
[
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "Street Fighter II is a landmark competitive fighting game where players choose from a diverse roster of martial artists, master unique special moves, and battle opponents from around the world in fast-paced one-on-one matches."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[3],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description = "Final Fantasy XIV is a sprawling online role-playing adventure set in the realm of Eorzea, where players create their own heroes, experience an evolving story, explore dangerous dungeons, and join others in large-scale battles."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = genres[4],
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description = "FIFA 23 delivers an authentic football experience with licensed clubs, leagues, and stadiums from around the world, along with refined match mechanics and a variety of competitive single-player and multiplayer modes."
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
