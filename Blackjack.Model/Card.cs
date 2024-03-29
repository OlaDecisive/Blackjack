using Microsoft.EntityFrameworkCore;

namespace Blackjack.Model;

public enum CardValue
{
    Ace = 1,
    Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King,
}

public enum Suit
{
    Clubs,
    Hearts,
    Spades,
    Diamonds
}

public class Card
{
    public Guid Id { get; set; }
    public CardValue Value { get; private set; }
    public Suit Suit { get; private set; }

    public Card(CardValue value, Suit suit)
    {
        this.Value = value;
        this.Suit = suit;
    }

    public int NumberValue => Value switch
    {
        CardValue.Jack => 10,
        CardValue.Queen => 10,
        CardValue.King => 10,
        _ => (int)Value
    };

    public string Name => $"{Value} of {Suit}";
}