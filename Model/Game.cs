using System.Text.Json.Serialization;

namespace Model;

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
    public IList<GameState> Rounds { get; }
    
    public Game(string playerName)
    {
        Rounds = [GameState.CreateInitialGameState(playerName)];
    }

    [JsonConstructor]
    public Game(IList<GameState> rounds)
    {
        Rounds = rounds;
    }

    public void Advance(PlayerDecision playerDecision)
    {
        var nextRound = Rounds.Last().GetNextGameState(playerDecision);
        Rounds.Add(nextRound);

        // If player stands, dealer hits until dealer cards are >= 17 or greater than players cards
        if (playerDecision == PlayerDecision.Stand &&
            nextRound.DealerHand.NumberValue < 17 && 
            nextRound.DealerHand.NumberValue <= nextRound.PlayerHand.NumberValue)
        {
            Advance(playerDecision);
        }
    }

    [JsonIgnore]
    public GameStatus Status => Rounds.Last().DetermineGameStatus();
    [JsonIgnore]
    public string GameDescription => Rounds.Last().GetGameDescription();
    [JsonIgnore]
    public string DealersFinalHandDescription => Rounds.Last().DealerHand.GetHandDescription();
}
