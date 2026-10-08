using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Shared.Timing;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connectionString);

builder.Services.AddValidation();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;

});

var app = builder.Build();


app.MapGameEndpoints();
app.MapGenreEndpoints();

app.UseHttpLogging();

await app.InitializeDbAsync();


app.Run();





