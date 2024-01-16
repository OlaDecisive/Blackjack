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
    public Hand PlayerHand { get; set; } = new Hand();
    public Hand DealerHand { get; set; } = new Hand();

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

        game.PlayerHand = Hand.CreateHand([game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()]);
        game.DealerHand = Hand.CreateHand([game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()]);


        return game;
    }

    public string GetGameState()
    {
        var output = $"{Deck.Cards.Count} cards in deck";
        output += $"\n{PlayerName} cards: {PlayerHand.DescribeHand()}";
        output += $"\nDealer cards: {DealerHand.DescribeDealerHand()}";
        output += $"\nGame state is: {State}";
        return output;
    }

    public GameState EvaluateGameState()
    {
        var playerSum = PlayerHand.NumberValue;
        var dealerSum = DealerHand.NumberValue;

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
        if (State != GameState.Running)
            return;
            
        if (playerDecision == PlayerDecision.Hit)
        {
            PlayerHand.AddCard(Deck.TakeCardFromTop());
        }
        else if (playerDecision == PlayerDecision.Stand)
        {
            // dealer pulls cards until bust or win
            // TODO: could this inner while loop be replaced by recursive calls to ProgressGameState?
            while (State == GameState.Running)
            {
                var dealerSumStand = DealerHand.NumberValue;
                if (dealerSumStand < 17)
                     DealerHand.AddCard(Deck.TakeCardFromTop());

                var previousState = State;
                var previousDealerSum = DealerHand.NumberValue;
                State = EvaluateGameState();
                if (State == previousState && previousDealerSum == DealerHand.NumberValue)
                {
                    throw new Exception("Unchanged game state detected, aborting");
                }
            }
        }

        var dealerSum = DealerHand.NumberValue;
        if (dealerSum < 17)
             DealerHand.AddCard(Deck.TakeCardFromTop());
        State = EvaluateGameState();
    }
}