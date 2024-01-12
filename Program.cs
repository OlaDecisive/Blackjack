Console.WriteLine("Blackjack!");

var playerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);

Console.WriteLine($"Player got card {string.Join(", ", playerCards)} with values {string.Join(", ", playerCards.Select(card => (int)card))}");