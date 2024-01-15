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
    public GameState State { get; private set; }

    Deck deck;
    IEnumerable<Card> playerCards;
    IEnumerable<Card> dealerCards;

    public Game()
    {
        State = GameState.Running;

        deck = new Deck();

        playerCards = [deck.TakeCardFromTop(), deck.TakeCardFromTop()];
        dealerCards = [deck.TakeCardFromTop(), deck.TakeCardFromTop()];
    }

    public string GetGameState()
    {
        var output = $"Player cards: {string.Join(", ", playerCards.Select(card => card.Name))}, adding up to {playerCards.Select(card => card.NumberValue).Sum()}";
        if (playerCards.Any(card => card.Value == CardValue.Ace))
            output += $" or {playerCards.Select(card => card.Value == CardValue.Ace ? 11 : card.NumberValue).Sum()}";
        output += $"\nDealer has {dealerCards.Count()} cards, first one is {dealerCards.First().Name}";

        return output;
    }

    public string GetDealerState()
    {
        return $"Dealer cards: {string.Join(", ", dealerCards.Select(card => card.Name))}, adding up to {dealerCards.Select(card => card.NumberValue).Sum()}";
    }

    public GameState EvaluateGameState()
    {
        var playerSum = playerCards.Select(card => card.NumberValue).Sum();
        var dealerSum = dealerCards.Select(card => card.NumberValue).Sum();

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
            playerCards = playerCards.Append(deck.TakeCardFromTop());
        }
        else if (playerDecision == PlayerDecision.Stand)
        {
            // dealer pulls cards until bust or win
            // TODO: could this inner while loop be replaced by recursive calls to ProgressGameState?
            while (State == GameState.Running)
            {
                var dealerSumStand = dealerCards.Select(card => card.NumberValue).Sum();
                if (dealerSumStand < 17)
                    dealerCards = dealerCards.Append(deck.TakeCardFromTop());

                var previousState = State;
                var previousDealerSum = dealerCards.Select(card => card.NumberValue).Sum();
                State = EvaluateGameState();
                if (State == previousState && previousDealerSum == dealerCards.Select(card => card.NumberValue).Sum())
                {
                    throw new Exception("Unchanged game state detected, aborting");
                }
            }
        }

        var dealerSum = dealerCards.Select(card => card.NumberValue).Sum();
        if (dealerSum < 17)
            dealerCards = dealerCards.Append(deck.TakeCardFromTop());
        State = EvaluateGameState();
    }
}