using System.Globalization;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient httpClient)
{
    public async Task<GamesPage> GetGamesAsync(int pageNumber = 1, string? name = null)
        => await httpClient.GetFromJsonAsync<GamesPage>(
            $"games?pageNumber={pageNumber}&name={Uri.EscapeDataString(name ?? string.Empty)}")
            ?? throw new Exception("Could not load games!");

    public async Task<CommandResult> AddGameAsync(GameDetails game, IFormFile? imageFile = null)
    {
        using var content = CreateGameContent(game, imageFile);
        using var response = await httpClient.PostAsync("games", content);
        return await response.HandleAsync();
    }

    public async Task<GameDetails> GetGameAsync(Guid id)
        => await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}")
            ?? throw new Exception("Could not find game!");

    public async Task<CommandResult> UpdateGameAsync(GameDetails updatedGame, IFormFile? imageFile = null)
    {
        using var content = CreateGameContent(updatedGame, imageFile);
        using var response = await httpClient.PutAsync($"games/{updatedGame.Id}", content);
        return await response.HandleAsync();
    }

    public async Task<CommandResult> DeleteGameAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"games/{id}");
        return await response.HandleAsync();
    }

    private static MultipartFormDataContent CreateGameContent(GameDetails game, IFormFile? imageFile)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(game.Name), nameof(game.Name) },
            { new StringContent(game.GenreId?.ToString() ?? string.Empty), nameof(game.GenreId) },
            { new StringContent(game.Price.ToString(CultureInfo.InvariantCulture)), nameof(game.Price) },
            { new StringContent(game.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)), nameof(game.ReleaseDate) },
            { new StringContent(game.Description), nameof(game.Description) }
        };

        if (imageFile is not null)
        {
            content.Add(new StreamContent(imageFile.OpenReadStream()), "ImageFile", Path.GetFileName(imageFile.FileName));
        }

        return content;
    }
}
