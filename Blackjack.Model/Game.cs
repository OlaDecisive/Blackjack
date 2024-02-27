using System.Text.Json.Serialization;

namespace Blackjack.Model;

public enum GameStatus
{
    Unknown,
    Running,
    PlayerWins,
    DealerWins,
    Tie,
}

public enum PlayerDecision
{
    Unknown,
    Hit,
    Stand,
}

public class Game
{
    public Guid Id { get; set; }
    public string PlayerName { get; set; }
    public GameState CurrentRound { get; set; }
    public PlayerDecision LastRoundPlayerDecision { get; set; }
    
    public Game() 
    {
        PlayerName = default!;
        CurrentRound = default!;
    }

    public static Game CreateGameWithShuffledCards(string playerName)
    {
        return new Game(playerName, new RandomShuffler(), DateTime.UtcNow);
    }

    public static Game CreateDeterministicGame(string playerName, DateTime timestamp)
    {
        return new Game(playerName, new DoNothingShuffler(), timestamp);
    }

    public Game(string playerName, IShuffler random, DateTime timestamp)
    {
        PlayerName = playerName;
        CurrentRound = GameState.CreateInitialGameState(this, random, timestamp);
    }

    public string GetPlayerDescription()
    {
        var output = $"{PlayerName} cards: {CurrentRound.PlayerHand.GetHandDescription()}";
        return output;
    }

    public void Advance(PlayerDecision playerDecision)
    {
        if (playerDecision == PlayerDecision.Hit && LastRoundPlayerDecision == PlayerDecision.Stand)
            throw new Exception("Cannot Hit when player has previously chosen Stand");
        
        LastRoundPlayerDecision = playerDecision;

        CurrentRound.DealCards(playerDecision);
    }

    [JsonIgnore]
    public GameStatus Status
    {
        get
        {
            return CurrentRound?.DetermineGameStatus() ?? GameStatus.Unknown;
        }
        private set {}
    }

    [JsonIgnore]
    public string GameDescription => string.Join("\n", GetPlayerDescription(), CurrentRound.GetGameStateDescription());
    [JsonIgnore]
    public string DealersFinalHandDescription => CurrentRound.DealerHand.GetHandDescription();
}
