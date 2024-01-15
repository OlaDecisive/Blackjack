using System.Text.Json.Serialization;

namespace Model;

public enum GameState
{
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
    public GameState State { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public Deck Deck { get; set; } = new Deck();
    public IEnumerable<Card> PlayerCards  { get; set; } = [];
    public IEnumerable<Card> DealerCards  { get; set; } = [];

    public Game()
    {
    }

    public static Game CreateNewGame(string playerName)
    {
        var game = new Game
        {
            PlayerName = playerName,
            State = GameState.Running,
            Deck = Deck.CreateShuffledDeck()
        };

        game.PlayerCards = [game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()];
        game.DealerCards = [game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()];

        return game;
    }

    public string GetGameState()
    {
        var output = $"{PlayerName} cards: {string.Join(", ", PlayerCards.Select(card => card.Name))}, adding up to {PlayerCards.Select(card => card.NumberValue).Sum()}";
        if (PlayerCards.Any(card => card.Value == CardValue.Ace))
            output += $" or {PlayerCards.Select(card => card.Value == CardValue.Ace ? 11 : card.NumberValue).Sum()}";
        output += $"\nDealer has {DealerCards.Count()} cards, first one is {DealerCards.First().Name}";
        output += $"\nGame state is: {State}";
        return output;
    }

    public string GetDealerState()
    {
        return $"Dealer cards: {string.Join(", ", DealerCards.Select(card => card.Name))}, adding up to {DealerCards.Select(card => card.NumberValue).Sum()}";
    }

    public GameState EvaluateGameState()
    {
        var playerSum = PlayerCards.Select(card => card.NumberValue).Sum();
        var dealerSum = DealerCards.Select(card => card.NumberValue).Sum();

        if (playerSum > 21 && dealerSum > 21)
            return GameState.Tie;
        if (playerSum > 21)
            return GameState.DealerWins;
        else if (dealerSum > 21)
            return GameState.PlayerWins;
        else if (dealerSum >= 17 && playerSum == dealerSum)
            return GameState.Tie;
        else if (playerSum > dealerSum && dealerSum >= 17)
            return GameState.PlayerWins;
        else if (playerSum < dealerSum && dealerSum >= 17)
            return GameState.DealerWins;
        else 
            return GameState.Running;
    }

    public void ProgressGameState(PlayerDecision playerDecision)
    {
        if (playerDecision == PlayerDecision.Hit)
        {
            PlayerCards = PlayerCards.Append(Deck.TakeCardFromTop());
        }
        else if (playerDecision == PlayerDecision.Stand)
        {
            // dealer pulls cards until bust or win
            // TODO: could this inner while loop be replaced by recursive calls to ProgressGameState?
            while (State == GameState.Running)
            {
                var dealerSumStand = DealerCards.Select(card => card.NumberValue).Sum();
                if (dealerSumStand < 17)
                    DealerCards = DealerCards.Append(Deck.TakeCardFromTop());

                var previousState = State;
                var previousDealerSum = DealerCards.Select(card => card.NumberValue).Sum();
                State = EvaluateGameState();
                if (State == previousState && previousDealerSum == DealerCards.Select(card => card.NumberValue).Sum())
                {
                    throw new Exception("Unchanged game state detected, aborting");
                }
            }
        }

        var dealerSum = DealerCards.Select(card => card.NumberValue).Sum();
        if (dealerSum < 17)
            DealerCards = DealerCards.Append(Deck.TakeCardFromTop());
        State = EvaluateGameState();
    }
}