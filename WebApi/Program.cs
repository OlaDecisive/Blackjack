using System.Text.Json;
using Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/game", () =>
{
    var game = Game.CreateNewGame();
    return game;
});

app.MapPost("/game/Hit", async (HttpContext context) =>
{
    var game = await context.Request.ReadFromJsonAsync<Game>();
    game?.ProgressGameState(PlayerDecision.Hit);
    return game;
});

app.MapPost("/game/Stand", async (HttpContext context) =>
{
#if DEBUG
    context.Request.EnableBuffering();
    context.Request.Body.Position = 0;
    var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
#endif

    var game = await context.Request.ReadFromJsonAsync<Game>();
    game?.ProgressGameState(PlayerDecision.Stand);
    return game;
});

app.Run();
