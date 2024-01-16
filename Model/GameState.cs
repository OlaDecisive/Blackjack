namespace Model;

public class GameState
{
    public string PlayerName { get; }
    public Deck Deck { get; }
    public Hand PlayerHand { get; }
    public Hand DealerHand { get; }

    public GameState(string playerName, Deck deck, Hand playerHand, Hand dealerHand)
    {
        (PlayerName, Deck, PlayerHand, DealerHand) = (playerName, deck, playerHand, dealerHand);
        VerifyInvariants();
    }

    public static GameState CreateInitialGameState(string playerName)
    {
        var intialDeck = Deck.CreateShuffledDeck();
        var playerDealtDeck = intialDeck.TakeCardsFromTop(2, out var playerCards);
        var dealerDealtDeck = playerDealtDeck.TakeCardsFromTop(2, out var dealerCards);

        var playerHand = Hand.CreateHand(playerCards);
        var dealerHand = Hand.CreateHand(dealerCards);

        return new GameState(playerName, dealerDealtDeck, playerHand, dealerHand);
    }

    public string GetGameDescription()
    {
        var output = $"{Deck.Cards.Count} cards in deck";
        output += $"\n{PlayerName} cards: {PlayerHand.GetHandDescription()}";
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

    public GameState GetNextGameState(PlayerDecision playerDecision)
    {
        if (DetermineGameStatus() != GameStatus.Running)
            return this;
            
        var nextDeck = Deck;
        var nextPlayerHand = PlayerHand;
        var nextDealerHand = DealerHand;

        if (playerDecision == PlayerDecision.Hit)
        {
            nextDeck = nextDeck.TakeCardFromTop(out var cardForPlayer);
            nextPlayerHand = nextPlayerHand.AddCard(cardForPlayer);
        }

        if (nextDealerHand.NumberValue < 17 && nextDealerHand.NumberValue <= nextPlayerHand.NumberValue)
        {
            nextDeck = nextDeck.TakeCardFromTop(out var cardForDealer);
            nextDealerHand = nextDealerHand.AddCard(cardForDealer);
        }
        return new GameState(PlayerName, nextDeck, nextPlayerHand, nextDealerHand);
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