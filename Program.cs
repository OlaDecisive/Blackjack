Console.WriteLine("Blackjack!");

var deck = new Deck();

IEnumerable<Card> playerCards = [deck.TakeCardFromTop(), deck.TakeCardFromTop()];
IEnumerable<Card> dealerCards = [deck.TakeCardFromTop(), deck.TakeCardFromTop()];

PrintGameState();

var gameState = GameState.Running;
while (gameState == GameState.Running)
{
    Console.WriteLine($"[H]it, [S]tand?");
    var command = Console.ReadLine();

    if (command?.ToUpperInvariant().StartsWith("H") ?? false)
    {
        playerCards = playerCards.Append(deck.TakeCardFromTop());
        PrintGameState();
    }
    else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
    {
        while (gameState == GameState.Running)
        {
            PrintGameState();
            var dealerSumStand = dealerCards.Select(card => card.NumberValue).Sum();
            if (dealerSumStand < 17)
                dealerCards = dealerCards.Append(deck.TakeCardFromTop());

            var previousState = gameState;
            var previousDealerSum = dealerCards.Select(card => card.NumberValue).Sum();
            gameState = EvaluateGameState();
            if (gameState == previousState && previousDealerSum == dealerCards.Select(card => card.NumberValue).Sum())
            {
                throw new Exception("Unchanged game state detected, aborting");
            }
        }
    }
    else
    {
        Console.WriteLine("unknown command, try again");
        continue;
    }

    var dealerSum = dealerCards.Select(card => card.NumberValue).Sum();
    if (dealerSum < 17)
        dealerCards = dealerCards.Append(deck.TakeCardFromTop());
    gameState = EvaluateGameState();
}

Console.WriteLine($"Gamestate: {gameState}");
Console.WriteLine($"Dealer cards: {string.Join(", ", dealerCards.Select(card => card.Name))}, adding up to {dealerCards.Select(card => card.NumberValue).Sum()}");

void PrintGameState()
{
    Console.WriteLine($"Player cards: {string.Join(", ", playerCards.Select(card => card.Name))}, adding up to {playerCards.Select(card => card.NumberValue).Sum()}");
    if (playerCards.Any(card => card.Value == CardValue.Ace))
        Console.WriteLine($" or {playerCards.Select(card => card.Value == CardValue.Ace ? 11 : card.NumberValue).Sum()}");
    Console.WriteLine($"Dealer has {dealerCards.Count()} cards, first one is {dealerCards.First().Name}");
}

GameState EvaluateGameState()
{
    var playerSum = playerCards.Select(card => card.NumberValue).Sum();
    var dealerSum = dealerCards.Select(card => card.NumberValue).Sum();

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
    else if (playerSum < dealerSum && dealerSum >= 17)
        return GameState.DealerWins;
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