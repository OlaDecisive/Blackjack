Console.WriteLine("Blackjack!");

var playerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);
var dealerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);

Console.WriteLine($"Player got cards {string.Join(", ", playerCards)}");
Console.WriteLine($"Dealer got cards {string.Join(", ", dealerCards)}");