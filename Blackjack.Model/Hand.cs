namespace Blackjack.Model;

public class Hand
{
    public Guid Id { get; set; }
    public List<Card> Cards { get; private set; }

    private Hand() { Cards = default!; }

    public Hand(List<Card> cards) => Cards = cards;

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

    public void AddCard(Card card)
    {
        Cards.Add(card);
    }
}