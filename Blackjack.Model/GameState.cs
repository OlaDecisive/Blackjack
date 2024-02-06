namespace Blackjack.Model;

public class GameState
{
    public Guid Id { get; set; }
    public Deck Deck { get; private set; }
    public Hand PlayerHand { get; private set; }
    public Hand DealerHand { get; private set; }
    public DateTime Timestamp { get; private set; }

    private GameState() 
    {
        Deck = default!;
        PlayerHand = default!;
        DealerHand = default!;
        Timestamp = default!;
    }

    public GameState(Deck deck, Hand playerHand, Hand dealerHand, DateTime timestamp)
    {
        (Deck, PlayerHand, DealerHand, Timestamp) = (deck, playerHand, dealerHand, timestamp);
        VerifyInvariants();
    }

    public static GameState CreateInitialGameState(Game game, IShuffler random, DateTime timestamp)
    {
        var deck = Deck.CreateShuffledDeck(random);
        var playerCards = deck.TakeCardsFromTop(2);
        var dealerCards = deck.TakeCardsFromTop(2);

        var playerHand = new Hand(playerCards);
        var dealerHand = new Hand(dealerCards);

        return new GameState(deck, playerHand, dealerHand, timestamp);
    }

    public string GetGameStateDescription()
    {
        var output = $"{Deck.Cards.Count} cards in deck";
        output += $"\nDealer cards: {DealerHand.GetDealersHandDescription()}";
        output += $"\nGame status is: {DetermineGameStatus()}";
        return output;
    }

    public GameStatus DetermineGameStatus()
    {
        var playerSum = PlayerHand.NumberValue;
        var dealerSum = DealerHand.NumberValue;

        // TODO: playerAction and dealerAction should both be Stand before it makes sense to compare card sums
        if (playerSum > 21 && dealerSum > 21)
            return GameStatus.Tie;
        if (playerSum > 21)
            return GameStatus.DealerWins;
        else if (dealerSum > 21)
            return GameStatus.PlayerWins;
        else if (dealerSum >= 21 && playerSum == dealerSum)
            return GameStatus.Tie;
        else if (playerSum > dealerSum && dealerSum >= 17)
            return GameStatus.PlayerWins;
        else if (playerSum < dealerSum && dealerSum >= 17)
            return GameStatus.DealerWins;
        else 
            return GameStatus.Running;
    }   

    public void DealCards(PlayerDecision playerDecision)
    {
        if (DetermineGameStatus() != GameStatus.Running)
            return;

        var nextPlayerHand = PlayerHand;
        var nextDealerHand = DealerHand;

        if (playerDecision == PlayerDecision.Hit)
        {
            var cardForPlayer = Deck.TakeCardFromTop();
            PlayerHand.AddCard(cardForPlayer);
        }

        if (nextDealerHand.NumberValue < 17 && nextDealerHand.NumberValue <= nextPlayerHand.NumberValue)
        {
            var cardForDealer = Deck.TakeCardFromTop();
            DealerHand.AddCard(cardForDealer);
        }
    }

    private void VerifyInvariants()
    {
        var allCards = Deck.Cards.Concat(PlayerHand.Cards).Concat(DealerHand.Cards);

        var duplicateCardGroups = allCards.GroupBy(card => (card.Suit, card.Value)).Where(cardGroup => cardGroup.Count() > 1);
        if (duplicateCardGroups.Any())
            throw new Exception($"Found duplicate cards: {string.Join(", ", duplicateCardGroups.Select(cardGroup => cardGroup.First().Name))}");

        var totalNumberOfCards = Deck.Cards.Count + PlayerHand.Cards.Count + DealerHand.Cards.Count;
        if (totalNumberOfCards != 52)
            throw new Exception($"Number of cards is {totalNumberOfCards}, should be 52. {Deck.Cards.Count} cards in deck, {PlayerHand.Cards.Count} cards in players hand, {DealerHand.Cards.Count} cards in dealers hand");
    }
}