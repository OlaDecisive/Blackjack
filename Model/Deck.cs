using System.Collections.Immutable;

namespace Blackjack.Model;

public class Deck
{
    public IImmutableList<Card> Cards { get; }

    public Deck(IImmutableList<Card> cards) => Cards = cards;

    public static Deck CreateOrderedDeck()
    {
        var cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToArray();
        var deck = new Deck(cards.ToImmutableList<Card>());

        return deck;
    }

    public static Deck CreateShuffledDeck(IShuffler random)
    {
        var cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToArray();
        random.Shuffle(cards);
        var deck = new Deck(cards.ToImmutableList<Card>());
        
        return deck;
    }

    public Deck TakeCardFromTop(out Card selectedCard)
    {
        selectedCard = Cards.First();
        var nextCards = Cards.Remove(selectedCard);
        return new Deck(nextCards);
    }

    public Deck TakeCardsFromTop(int numberOfCards, out IImmutableList<Card> selectedCards)
    {
        selectedCards = Cards.Take(numberOfCards).ToImmutableList();
        var nextCards = Cards.RemoveRange(0, numberOfCards);
        return new Deck(nextCards);
    }
}