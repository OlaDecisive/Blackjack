using Model;

Console.WriteLine("Blackjack!");

var game = new Game();

Console.WriteLine(game.GetGameState());

while (game.State == GameState.Running)
{
    Console.WriteLine($"[H]it, [S]tand?");
    var command = Console.ReadLine();

    if (command?.ToUpperInvariant().StartsWith("H") ?? false)
    {
        game.ProgressGameState(PlayerDecision.Hit);
        Console.WriteLine(game.GetGameState());
    }
    else if (command?.ToUpperInvariant().StartsWith("S") ?? false)
    {
        game.ProgressGameState(PlayerDecision.Stand);
        Console.WriteLine(game.GetGameState());
    }
    else
    {
        Console.WriteLine("unknown command, try again");
        continue;
    }
}

Console.WriteLine(game.GetDealerState());
Console.WriteLine($"Gamestate: {game.State}");