using GameStore.Api.Data;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Baskets;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Shared.Timing;
using Microsoft.AspNetCore.HttpLogging;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using GameStore.Api.Shared.Authorization;

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


builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters.RoleClaimType = "role";
                });


builder.AddGameStoreAuthorization();


builder.Services.AddHttpContextAccessor()
                .AddSingleton<FileUploader>();


var app = builder.Build();

app.UseStaticFiles();

app.UseAuthorization();
app.MapGameEndpoints();
app.MapGenreEndpoints();
app.MapBaskets();


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





