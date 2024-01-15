namespace Model;

public class Deck
{
    List<Card> cards;

    public Deck()
    {
        cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToList();
        cards = cards.OrderBy(card => Random.Shared.Next()).ToList(); // shuffle cards
    }

    public Card TakeCardFromTop()
    {
        var selectedCard = cards.First();
        cards.Remove(selectedCard);
        return selectedCard;
    }
}