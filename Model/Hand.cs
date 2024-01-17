using System.Collections.Immutable;

namespace Blackjack.Model;

public class Hand
{
    public IImmutableList<Card> Cards { get; }

    public Hand(IImmutableList<Card> cards) => Cards = cards;

    public static Hand CreateHand(IImmutableList<Card> cards)
    {
        var hand = new Hand(cards);
        return hand;
    }

    public string GetHandDescription()
    {
        var output = $"{string.Join(", ", Cards.Select(card => card.Name))}, adding up to {Cards.Select(card => card.NumberValue).Sum()}";
        if (Cards.Any(card => card.Value == CardValue.Ace))
            output += $" or {Cards.Select(card => card.Value == CardValue.Ace ? 11 : card.NumberValue).Sum()}";
        return output;
    }

    public string GetDealersHandDescription()
    {
        string output = $"Dealer has {Cards.Count} cards";
        if (Cards.Any())
            output += $", first one is {Cards.First().Name}";
        return output;
    }

    public int NumberValue => Cards.Select(card => card.NumberValue).Sum();

    public Hand AddCard(Card card)
    {
        return new Hand(Cards.Append(card).ToImmutableList());
    }
}