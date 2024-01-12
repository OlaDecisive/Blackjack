Console.WriteLine("Blackjack!");

IEnumerable<Card> playerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);
IEnumerable<Card> dealerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);

Console.WriteLine($"Player cards: {string.Join(", ", playerCards)}, adding up to {playerCards.Select(card => (int)card).Sum()}");
Console.WriteLine($"Dealer has {dealerCards.Count()} cards, first one is {dealerCards.First()}");

Console.WriteLine($"[H]it, [S]tand?");

var command = Console.ReadLine();

if (command?.ToUpperInvariant().StartsWith("H") ?? false)
{
    playerCards = playerCards.Concat(Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 1));
    Console.WriteLine($"Player cards: {string.Join(", ", playerCards)}, adding up to {playerCards.Select(card => (int)card).Sum()}");
}
else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
{
    Console.WriteLine($"Player cards: {string.Join(", ", playerCards)}, adding up to {playerCards.Select(card => (int)card).Sum()}");
}
else
{
    Console.WriteLine("EH?");
}
