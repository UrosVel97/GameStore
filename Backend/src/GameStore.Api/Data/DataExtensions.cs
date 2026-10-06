using System;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace GameStore.Api.Data;

public static class DataExtensions
{

    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await app.MigrateDbAsync();
        await app.SeedDataAsync();

        app.Logger.LogInformation(18, "The database has been initialized successfully.");

    }

    private static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        GameStoreContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        GameStoreContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<GameStoreContext>();

        if (!dbContext.Genres.Any())
        {
            dbContext.Genres.AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Kids and family" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Roleplaying" },
                new Genre { Name = "Sports" }
            );
        }

        await dbContext.SaveChangesAsync();
    }
}
