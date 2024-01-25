using System.Text.Json;
using Blackjack.Model;
using Microsoft.EntityFrameworkCore;

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

var sqliteDbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "blackjack.db");
builder.Services.AddSqlite<BlackjackContext>($"Data Source={sqliteDbPath}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.MapGet("/game/{playerName}", async (string playerName, HttpContext context) =>
{
    using (var client = new BlackjackContext())
    {
        var game = client.Games.Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.Deck)
                                   .ThenInclude(deck => deck.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.DealerHand)
                                   .ThenInclude(hand => hand.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.PlayerHand)
                                   .ThenInclude(hand => hand.Cards)
                         .SingleOrDefault(game => game.PlayerName == playerName && game.Status == GameStatus.Running);
        if (game == null)
        {
            var oldGames = client.Games.Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.Deck)
                                   .ThenInclude(deck => deck.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.DealerHand)
                                   .ThenInclude(hand => hand.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.PlayerHand)
                                   .ThenInclude(hand => hand.Cards)
                         .Where(game => game.PlayerName == playerName && game.Status != GameStatus.Running);
            if (oldGames?.Any() ?? false)
            {
                return $"Found {oldGames.Count()} finished games for {playerName}. {oldGames.Count(game => game.Status == GameStatus.PlayerWins)} wins, {oldGames.Count(game => game.Status == GameStatus.DealerWins)} losses, {oldGames.Count(game => game.Status == GameStatus.Tie)} ties";
            }
            else
            {
                return $"No games found for {playerName}";
            }
        }
        else
        {
            return game.GameDescription;
        }
    }
});

app.MapPost("/game/{playerName}", async (string playerName, HttpContext context) =>
{
    // var json = context.Session.GetString($"game_{playerName}");
    // Game? game;
    // if (json == null)
    //     game = new Game(playerName);
    // else
    //     game = JsonSerializer.Deserialize<Game>(json);
    // if (game == null)
    //      game = new Game(playerName);

    // json = JsonSerializer.Serialize(game);
    // context.Session.SetString($"game_{playerName}", json);

    using (var client = new BlackjackContext())
    {
        var game = client.Games.Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.Deck)
                                   .ThenInclude(deck => deck.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.DealerHand)
                                   .ThenInclude(hand => hand.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.PlayerHand)
                                   .ThenInclude(hand => hand.Cards)
                         .SingleOrDefault(game => game.PlayerName == playerName && game.Status == GameStatus.Running);
        if (game == null)
        {
            game = Game.CreateGameWithShuffledCards(playerName);
            client.Games.Add(game);
            await client.SaveChangesAsync();
        }
        return game.GameDescription;
    }
});

app.MapPost("/game/{playerName}/Hit", async (string playerName, HttpContext context) =>
{
// #if DEBUG
//     context.Request.EnableBuffering();
//     context.Request.Body.Position = 0;
//     var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
//     context.Request.Body.Position = 0;
// #endif
//     var json = context.Session.GetString($"game_{playerName}");
//     var game = JsonSerializer.Deserialize<Game>(json);
//     game.Advance(PlayerDecision.Hit);

//     json = JsonSerializer.Serialize(game);
//     context.Session.SetString($"game_{playerName}", json);
//     return game.GameDescription;

    using (var client = new BlackjackContext())
    {
        var game = client.Games.Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.Deck)
                                   .ThenInclude(deck => deck.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.DealerHand)
                                   .ThenInclude(hand => hand.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.PlayerHand)
                                   .ThenInclude(hand => hand.Cards)
                         .Single(game => game.PlayerName == playerName && game.Status == GameStatus.Running);
        game.Advance(PlayerDecision.Hit);
        await client.SaveChangesAsync();
        return game.GameDescription;
    }
});

app.MapPost("/game/{playerName}/Stand", async (string playerName, HttpContext context) =>
{
// #if DEBUG
//     context.Request.EnableBuffering();
//     context.Request.Body.Position = 0;
//     var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
//     context.Request.Body.Position = 0;
// #endif
//     var json = context.Session.GetString($"game_{playerName}");
//     var game = JsonSerializer.Deserialize<Game>(json);
//     game.Advance(PlayerDecision.Stand);

//     json = JsonSerializer.Serialize(game);
//     context.Session.SetString($"game_{playerName}", json);

//     return game.GameDescription;

    using (var client = new BlackjackContext())
    {
        var game = client.Games.Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.Deck)
                                   .ThenInclude(deck => deck.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.DealerHand)
                                   .ThenInclude(hand => hand.Cards)
                               .Include(game => game.CurrentRound)
                                   .ThenInclude(gameState => gameState.PlayerHand)
                                   .ThenInclude(hand => hand.Cards)
                         .Single(game => game.PlayerName == playerName && game.Status == GameStatus.Running);
        game.Advance(PlayerDecision.Stand);
        await client.SaveChangesAsync();
        return game.GameDescription;
    }
});

app.Run();

public partial class Program {}