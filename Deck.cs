using System.Collections.ObjectModel;

enum CardValue
{
    Ace = 1,
    Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King,
}

enum Suit
{
    Clubs,
    Hearts,
    Spades,
    Diamonds
}

class Card
{
    public CardValue Value { get; }
    Suit suit;

    public Card(CardValue value, Suit suit)
    {
        this.Value = value;
        this.suit = suit;
    }

    public int NumberValue => Value switch
    {
        CardValue.Jack => 10,
        CardValue.Queen => 10,
        CardValue.King => 10,
        _ => (int)Value
    };

    public string Name => $"{Value} of {suit}";
}

class Deck
{
    List<Card> cards;

    public Deck()
    {
        cards = Enum.GetValues<Suit>().Select(suit => Enum.GetValues<CardValue>().Select(value => new Card(value: value, suit: suit))).SelectMany(card => card).ToList();
        cards = cards.OrderBy(card => Random.Shared.Next()).ToList(); // shuffle cards
    }

    public Card Pop()
    {
        var selectedCard = cards.First();
        cards.Remove(selectedCard);
        return selectedCard;
    }
}