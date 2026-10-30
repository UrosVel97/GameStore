using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Shared.Timing;
using Microsoft.AspNetCore.HttpLogging;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

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

builder.Services.AddOpenApi();

builder.Services.AddAuthentication()
                .AddJwtBearer();


builder.Services.AddHttpContextAccessor()
                .AddSingleton<FileUploader>();


var app = builder.Build();

app.UseStaticFiles();
app.MapGameEndpoints();
app.MapGenreEndpoints();

app.UseHttpLogging();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.InitializeDbAsync();


app.Run();





