namespace Model;

public class Hand
{
    public IList<Card> Cards { get; set; } = [];

    public static Hand CreateHand(IList<Card> cards)
    {
        var hand = new Hand() { Cards = cards };
        return hand;
    }

    public string DescribeHand()
    {
        var output = $"{string.Join(", ", Cards.Select(card => card.Name))}, adding up to {Cards.Select(card => card.NumberValue).Sum()}";
        if (Cards.Any(card => card.Value == CardValue.Ace))
            output += $" or {Cards.Select(card => card.Value == CardValue.Ace ? 11 : card.NumberValue).Sum()}";
        return output;
    }

    public string DescribeDealerHand()
    {
        if (!Cards.Any())
            return $"Dealer has {Cards.Count()} cards";
        else
            return $"Dealer has {Cards.Count()} cards, first one is {Cards.First().Name}";
    }

    public int NumberValue => Cards.Select(card => card.NumberValue).Sum();

    public void AddCard(Card card)
    {
        Cards.Add(card);
    }
}