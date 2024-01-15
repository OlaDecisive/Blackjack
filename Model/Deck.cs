namespace Model;

public class Deck
{
    public List<Card> Cards { get; set; } = [];

    public Deck()
    {
    }

    public static Deck CreateShuffledDeck()
    {
        var deck = new Deck();
        
        deck.Cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToList();
        deck.Cards = deck.Cards.OrderBy(card => Random.Shared.Next()).ToList(); // shuffle cards
        
        return deck;
    }

    public Card TakeCardFromTop()
    {
        var selectedCard = Cards.First();
        Cards.Remove(selectedCard);
        return selectedCard;
    }
}