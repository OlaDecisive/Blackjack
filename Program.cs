Console.WriteLine("Blackjack!");

var playerCards = Random.Shared.GetItems<Cards>(Enum.GetValues<Cards>(), 2);

Console.WriteLine($"Player got card {string.Join(", ", playerCards)} with values {string.Join(", ", playerCards.Select(card => (int)card))}");