using Model;

Console.WriteLine("Blackjack!");

var game = Game.CreateNewGame("player");

Console.WriteLine(game.GetGameDescription());

while (game.Status == GameStatus.Running)
{
    Console.WriteLine($"[H]it, [S]tand?");
    var command = Console.ReadLine();

    if (command?.ToUpperInvariant().StartsWith("H") ?? false)
    {
        game.ProgressGame(PlayerDecision.Hit);
        Console.WriteLine(game.GetGameDescription());
    }
    else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
    {
        game.ProgressGame(PlayerDecision.Stand);
        Console.WriteLine(game.GetGameDescription());
    }
    else
    {
        Console.WriteLine("unknown command, try again");
        continue;
    }
}

Console.WriteLine(game.DealerHand.GetHandDescription());
Console.WriteLine($"Gamestate: {game.Status}");