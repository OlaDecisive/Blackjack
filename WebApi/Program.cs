using System.Text.Json;
using Blackjack.Model;

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

app.MapPost("/game/{playerName}", (string playerName, HttpContext context) =>
{
    var json = context.Session.GetString($"game_{playerName}");
    Game? game;
    if (json == null)
        game = new Game(playerName);
    else
        game = JsonSerializer.Deserialize<Game>(json);
    if (game == null)
         game = new Game(playerName);

    json = JsonSerializer.Serialize(game);
    context.Session.SetString($"game_{playerName}", json);

    return game.GameDescription;
});

app.MapPost("/game/{playerName}/Hit", async (string playerName, HttpContext context) =>
{
#if DEBUG
    context.Request.EnableBuffering();
    context.Request.Body.Position = 0;
    var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
#endif
    var json = context.Session.GetString($"game_{playerName}");
    var game = JsonSerializer.Deserialize<Game>(json);
    game.Advance(PlayerDecision.Hit);

    json = JsonSerializer.Serialize(game);
    context.Session.SetString($"game_{playerName}", json);

    return game.GameDescription;
});

app.MapPost("/game/{playerName}/Stand", async (string playerName, HttpContext context) =>
{
#if DEBUG
    context.Request.EnableBuffering();
    context.Request.Body.Position = 0;
    var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
#endif
    var json = context.Session.GetString($"game_{playerName}");
    var game = JsonSerializer.Deserialize<Game>(json);
    game.Advance(PlayerDecision.Stand);

    json = JsonSerializer.Serialize(game);
    context.Session.SetString($"game_{playerName}", json);

    return game.GameDescription;
});

app.Run();
