using Blackjack.Model;
using Blackjack.Service;

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

using (var service = new GameService())
{
    await service.Context.Database.EnsureCreatedAsync();
}

app.MapGet("/game/{playerName}", async (string playerName, HttpContext context) =>
{
    using var service = new GameService();
    return await service.GetRunningGameViewAsync(playerName);
});

app.MapPost("/game/{playerName}", async (string playerName, HttpContext context) =>
{
    using var service = new GameService();
    var deterministicGame = playerName == "DeterministicTester";
    var runningGame = await service.GetRunningGameViewAsync(playerName) ?? await service.CreateNewGameAsync(playerName, deterministicGame);
    return runningGame;
});

app.MapPost("/game/{playerName}/Hit", async (string playerName, HttpContext context) =>
{
    using var service = new GameService();
    await service.AdvanceGameAsync(playerName, PlayerDecision.Hit);
    return await service.GetRunningGameViewAsync(playerName);
});

app.MapPost("/game/{playerName}/Stand", async (string playerName, HttpContext context) =>
{
    using var service = new GameService();
    await service.AdvanceGameAsync(playerName, PlayerDecision.Stand);
    return await service.GetRunningGameViewAsync(playerName);
});

app.Run();

public partial class Program {}