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
    Hit,
    Stand,
}

public class Game
{
    public Guid Id { get; set; }
    public string PlayerName { get; set; }
    public GameState CurrentRound { get; set; }
    
    public Game() 
    {
        PlayerName = default!;
        CurrentRound = default!;
    }

    //public Game(string playerName) : this(playerName, new RandomShuffler()) {}
    public static Game CreateGameWithShuffledCards(string playerName)
    {
        return new Game(playerName, new RandomShuffler());
    }

    public Game(string playerName, IShuffler random)
    {
        PlayerName = playerName;
        CurrentRound = GameState.CreateInitialGameState(this, random);
    }

    public string GetPlayerDescription()
    {
        var output = $"{PlayerName} cards: {CurrentRound.PlayerHand.GetHandDescription()}";
        return output;
    }

    public void Advance(PlayerDecision playerDecision)
    {
        CurrentRound.DealCards(playerDecision);
        
        // If player stands, dealer hits until dealer cards are >= 17 or greater than players cards
        // TODO: dealer shouldn't know about players hand?
        if (playerDecision == PlayerDecision.Stand &&
            CurrentRound.DealerHand.NumberValue < 17 && 
            CurrentRound.DealerHand.NumberValue <= CurrentRound.PlayerHand.NumberValue)
        {
            Advance(playerDecision);
        }
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
