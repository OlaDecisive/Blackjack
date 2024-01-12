Console.WriteLine("Blackjack!");

IEnumerable<Card> playerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);
IEnumerable<Card> dealerCards = Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 2);

PrintGameState();

var gameState = GameState.Running;
while (gameState == GameState.Running)
{
    Console.WriteLine($"[H]it, [S]tand?");
    var command = Console.ReadLine();

    if (command?.ToUpperInvariant().StartsWith("H") ?? false)
    {
        playerCards = playerCards.Concat(Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 1));
        PrintGameState();
    }
    else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
    {
        while (gameState == GameState.Running)
        {
            PrintGameState();
            var dealerSumStand = dealerCards.Select(card => (int)card).Sum();
            if (dealerSumStand < 17)
                dealerCards = dealerCards.Concat(Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 1));
            gameState = EvaluateGameState();
        }
    }
    else
    {
        Console.WriteLine("unknown command, try again");
        continue;
    }

    var dealerSum = dealerCards.Select(card => (int)card).Sum();
    if (dealerSum < 17)
        dealerCards = dealerCards.Concat(Random.Shared.GetItems<Card>(Enum.GetValues<Card>(), 1));
    gameState = EvaluateGameState();
}

Console.WriteLine($"Gamestate: {gameState}");
Console.WriteLine($"Dealer cards: {string.Join(", ", dealerCards)}, adding up to {dealerCards.Select(card => (int)card).Sum()}");

void PrintGameState()
{
    Console.WriteLine($"Player cards: {string.Join(", ", playerCards)}, adding up to {playerCards.Select(card => (int)card).Sum()}");
    Console.WriteLine($"Dealer has {dealerCards.Count()} cards, first one is {dealerCards.First()}");
}

GameState EvaluateGameState()
{
    // TODO: 'true' blackjack? ace and ten or picture card

    var playerSum = playerCards.Select(card => (int)card).Sum();
    var dealerSum = dealerCards.Select(card => (int)card).Sum();

    if (playerSum > 21 && dealerSum > 21)
        return GameState.Tie;
    if (playerSum > 21)
        return GameState.DealerWins;
    else if (dealerSum > 21)
        return GameState.PlayerWins;
    else if (dealerSum >= 17 && playerSum == dealerSum)
        return GameState.Tie;
    else if (playerSum > dealerSum && dealerSum >= 17)
        return GameState.PlayerWins;
    else 
        return GameState.Running;
}

enum GameState
{
    Running,
    PlayerWins,
    DealerWins,
    Tie,
}