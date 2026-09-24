using System;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace GameStore.Api.Data;

public static class DataExtensions
{

    public static void InitializeDb(this WebApplication app)
    {
        app.MigrateDb();
        app.SeedData();
    }

    private static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        GameStoreContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    private static void SeedData(this WebApplication app)
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

        dbContext.SaveChanges();
    }
}
