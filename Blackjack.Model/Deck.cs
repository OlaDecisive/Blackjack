namespace Blackjack.Model;

public class Deck
{
    public Guid Id { get; set; }
    public List<Card> Cards { get; private set; }

    private Deck() { Cards = default!; }

    public Deck(List<Card> cards) => Cards = cards;

    public static Deck CreateShuffledDeck(IShuffler random)
    {
        var cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToArray();
        random.Shuffle(cards);
        var deck = new Deck(cards.ToList<Card>());
        
        return deck;
    }

    public Card TakeCardFromTop()
    {
        var selectedCard = Cards.First();
        Cards.Remove(selectedCard);
        return selectedCard;
    }

    public List<Card> TakeCardsFromTop(int numberOfCards)
    {
        var selectedCards = Cards.Take(numberOfCards).ToList();
        Cards.RemoveRange(0, numberOfCards);
        return selectedCards;
    }
}