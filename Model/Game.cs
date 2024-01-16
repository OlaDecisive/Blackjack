namespace Model;

public enum GameStatus
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
    public GameStatus Status { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public Deck Deck { get; set; } = new Deck();
    public Hand PlayerHand { get; set; } = new Hand();
    public Hand DealerHand { get; set; } = new Hand();

    public static Game CreateNewGame(string playerName)
    {
        var game = new Game
        {
            PlayerName = playerName,
            Status = GameStatus.Running,
            Deck = Deck.CreateShuffledDeck()
        };

        game.PlayerHand = Hand.CreateHand([game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()]);
        game.DealerHand = Hand.CreateHand([game.Deck.TakeCardFromTop(), game.Deck.TakeCardFromTop()]);

        game.VerifyInvariants();

        return game;
    }

    public string GetGameDescription()
    {
        var output = $"{Deck.Cards.Count} cards in deck";
        output += $"\n{PlayerName} cards: {PlayerHand.GetHandDescription()}";
        output += $"\nDealer cards: {DealerHand.GetDealersHandDescription()}";
        output += $"\nGame status is: {Status}";
        return output;
    }

    public GameStatus DetermineGameStatus()
    {
        VerifyInvariants();
        
        var playerSum = PlayerHand.NumberValue;
        var dealerSum = DealerHand.NumberValue;

        if (playerSum > 21 && dealerSum > 21)
            return GameStatus.Tie;
        if (playerSum > 21)
            return GameStatus.DealerWins;
        else if (dealerSum > 21)
            return GameStatus.PlayerWins;
        else if (dealerSum >= 17 && playerSum == dealerSum)
            return GameStatus.Tie;
        else if (playerSum > dealerSum && dealerSum >= 17)
            return GameStatus.PlayerWins;
        else if (playerSum < dealerSum && dealerSum >= 17)
            return GameStatus.DealerWins;
        else 
            return GameStatus.Running;
    }

    public void ProgressGame(PlayerDecision playerDecision)
    {
        if (Status != GameStatus.Running)
            return;
            
        if (playerDecision == PlayerDecision.Hit)
        {
            PlayerHand.AddCard(Deck.TakeCardFromTop());
        }
        else if (playerDecision == PlayerDecision.Stand)
        {
            // dealer pulls cards until bust or win
            // TODO: could this inner while loop be replaced by recursive calls to ProgressGame?
            while (Status == GameStatus.Running)
            {
                var dealerSumStand = DealerHand.NumberValue;
                if (dealerSumStand < 17)
                     DealerHand.AddCard(Deck.TakeCardFromTop());

                var previousStatus = Status;
                var previousDealerSum = DealerHand.NumberValue;
                Status = DetermineGameStatus();
                if (Status == previousStatus && previousDealerSum == DealerHand.NumberValue)
                {
                    throw new Exception("Unchanged game state detected, aborting");
                }
            }
        }

        var dealerSum = DealerHand.NumberValue;
        if (dealerSum < 17)
             DealerHand.AddCard(Deck.TakeCardFromTop());
        Status = DetermineGameStatus();
    }

    private void VerifyInvariants()
    {
        var allCards = Deck.Cards.Concat(PlayerHand.Cards).Concat(DealerHand.Cards);

        var duplicateCardGroups = allCards.GroupBy(card => (card.suit, card.Value)).Where(cardGroup => cardGroup.Count() > 1);
        if (duplicateCardGroups.Any())
            throw new Exception($"Found duplicate cards: {string.Join(", ", duplicateCardGroups.Select(cardGroup => cardGroup.First().Name))}");

        var totalNumberOfCards = Deck.Cards.Count + PlayerHand.Cards.Count + DealerHand.Cards.Count;
        if (totalNumberOfCards != 52)
            throw new Exception($"Number of cards is {totalNumberOfCards}, should be 52. {Deck.Cards.Count} cards in deck, {PlayerHand.Cards.Count} cards in players hand, {DealerHand.Cards.Count} cards in dealers hand");
    }
}