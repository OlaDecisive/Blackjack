using Blackjack.Model;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Service;

/// <summary>
/// Front-facing game model - should not include data that player should not see, like dealers hand
/// </summary>
public class GameView
{
    public string Description { get; set; }
    public string PlayerName { get; set; }
    public Hand PlayerHand { get; set; }
    public GameStatus Status { get; set; }
}

public class GameService : IDisposable
{
    public BlackjackContext Context { get; set; }

    public GameService()
    {
        Context = new BlackjackContext();
    }

    public async Task<GameView?> GetRunningGameViewAsync(string playerName)
    {
        var game = await GetRunningGameAsync(playerName);
        if (game == null)
        {
            return null;
        }
        else
        {
            return new GameView() { Description = game.GameDescription, PlayerName = playerName, PlayerHand = game.CurrentRound.PlayerHand };
        }
    }

    public async Task<GameView> CreateNewGameAsync(string playerName, bool deterministicGame = false)
    {
        if (await GetRunningGameAsync(playerName) != null)
            throw new Exception("Cannot create new game when there is already a running game");

        Game game;
        if (deterministicGame)
            game = Blackjack.Model.Game.CreateDeterministicGame(playerName, DateTime.UtcNow);
        else
            game = Blackjack.Model.Game.CreateGameWithShuffledCards(playerName);
            
        Context.Games.Add(game);
        await Context.SaveChangesAsync();
        return (await GetRunningGameViewAsync(playerName))!;
    }

    public List<GameView>? GetFinishedGames(string playerName)
    {
        var oldGames = Context.Games.Include(game => game.CurrentRound)
                            .ThenInclude(gameState => gameState.Deck)
                            .ThenInclude(deck => deck.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                        .Include(game => game.CurrentRound)
                            .ThenInclude(gameState => gameState.DealerHand)
                            .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                        .Include(game => game.CurrentRound)
                            .ThenInclude(gameState => gameState.PlayerHand)
                            .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                    .Where(game => game.PlayerName == playerName && game.Status != GameStatus.Running && game.Status != GameStatus.Unknown);
        if (oldGames?.Any() ?? false)
        {
            return oldGames.Select(game => new GameView() { Description = game.GameDescription, PlayerName = playerName, PlayerHand = game.CurrentRound.PlayerHand }).ToList();
        }
        else
        {
            return null;
        }
    }

    public async Task AdvanceGameAsync(string playerName, PlayerDecision playerDecision)
    {
        var runningGame = await GetRunningGameAsync(playerName);
        runningGame!.Advance(playerDecision);

        await Context.SaveChangesAsync();
    }

    private async Task<Game?> GetRunningGameAsync(string playerName)
    {
        return await Context.Games.Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.Deck)
                                .ThenInclude(deck => deck.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                            .Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.DealerHand)
                                .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                            .Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.PlayerHand)
                                .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                        .SingleOrDefaultAsync(game => game.PlayerName == playerName && game.Status == GameStatus.Running);
    }

    private async Task<Game?> GetLatestGameAsync(string playerName)
    {
        return await Context.Games.Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.Deck)
                                .ThenInclude(deck => deck.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                            .Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.DealerHand)
                                .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                            .Include(game => game.CurrentRound)
                                .ThenInclude(gameState => gameState.PlayerHand)
                                .ThenInclude(hand => hand.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Value))
                        .OrderByDescending(game => game.CurrentRound.Timestamp)
                        .FirstOrDefaultAsync(game => game.PlayerName == playerName);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
