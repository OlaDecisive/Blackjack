using Blackjack.Model;

Console.WriteLine("Blackjack!");

var game = new Game("player");

Console.WriteLine(game.GameDescription);

while (game.Status == GameStatus.Running)
{
    Console.WriteLine($"[H]it, [S]tand?");
    var command = Console.ReadLine();

    if (command?.ToUpperInvariant().StartsWith("H") ?? false)
    {
        game.Advance(PlayerDecision.Hit);
        Console.WriteLine(game.GameDescription);
    }
    else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
    {
        game.Advance(PlayerDecision.Stand);
        Console.WriteLine(game.GameDescription);
    }
    else
    {
        Console.WriteLine("unknown command, try again");
        continue;
    }
}

Console.WriteLine($"Dealers final hand: {game.DealersFinalHandDescription}");