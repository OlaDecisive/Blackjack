using System.Text.Json;
using Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => 
{
    options.Cookie.Name = ".Blackjack.Game";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.MapPost("/game", (HttpContext context) =>
{
    var game = Game.CreateNewGame();

    var json = JsonSerializer.Serialize(game);
    context.Session.SetString("game", json);

    return game.GetGameState();
});

app.MapPost("/game/Hit", async (HttpContext context) =>
{
    //var game = await context.Request.ReadFromJsonAsync<Game>();
    //game?.ProgressGameState(PlayerDecision.Hit);
    //return game;

    var json = context.Session.GetString("game");
    var game = JsonSerializer.Deserialize<Game>(json);
    game.ProgressGameState(PlayerDecision.Hit);

    json = JsonSerializer.Serialize(game);
    context.Session.SetString("game", json);

    return game.GetGameState();
});

app.MapPost("/game/Stand", async (HttpContext context) =>
{
#if DEBUG
    context.Request.EnableBuffering();
    context.Request.Body.Position = 0;
    var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
#endif

    //var game = await context.Request.ReadFromJsonAsync<Game>();
    //game?.ProgressGameState(PlayerDecision.Stand);
    //return game;

    var json = context.Session.GetString("game");
    var game = JsonSerializer.Deserialize<Game>(json);
    game.ProgressGameState(PlayerDecision.Stand);

    json = JsonSerializer.Serialize(game);
    context.Session.SetString("game", json);

    return game.GetGameState();
});

app.Run();
