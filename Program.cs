Console.WriteLine("Blackjack!");

var playerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);
var dealerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);

Console.WriteLine($"Player got cards {string.Join(", ", playerCards)}, adding up to {playerCards.Select(card => (int)card).Sum()}");
Console.WriteLine($"Dealer got {dealerCards.Length} cards, first one is {dealerCards.First()}");