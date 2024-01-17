using System.Text.Json;
using Blackjack.Model;

namespace Blackjack.Test;

public class TestModel
{
    [Fact]
    public void TestCreateInitialGameState()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = GameState.CreateInitialGameState(playerName, new DoNothingShuffler());

        // Assert
        Assert.Equal(playerName, game.PlayerName);
        Assert.Equal(52 - 2 - 2, game.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }

    [Fact]
    public void TestSerialization()
    {
        var game = new Game("tester");

        var gameJson = JsonSerializer.Serialize<Game>(game);
        var deserializedGame = JsonSerializer.Deserialize<Game>(gameJson);
        var deserializedGameJson = JsonSerializer.Serialize<Game>(deserializedGame!);

        Assert.Equal(gameJson, deserializedGameJson);
    }

    [Fact]
    public void TestCreateGame()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = new Game("tester", new DoNothingShuffler());
        
        // Assert
        Assert.True(game.Rounds.All(round => round.PlayerName == playerName));
        Assert.Equal(52 - 2 - 2, game.Rounds.First().Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }

    [Fact]
    public void TestAdvanceGameWithHit()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = new Game("tester", new DoNothingShuffler());
        game.Advance(PlayerDecision.Hit);

        // Assert
        Assert.Equal(2, game.Rounds.Count);
        Assert.True(game.Rounds.All(round => round.PlayerName == playerName));
        Assert.Equal(52 - 2 - 2, game.Rounds.First().Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
        Assert.Equal(52 - 2 - 2 - 2, game.Rounds.Last().Deck.Cards.Count); // After one round, one extra card is dealt to the player, one to the dealer

        Assert.Equal(GameStatus.Running, game.Status);
        Assert.Contains("deck", game.GameDescription);
        //Assert.Contains("dealer", game.DealersFinalHandDescription);
    }

    [Fact]
    public void TestAdvanceGameWithStand()
    {
        // Arrange
        var playerName = "tester";
        var game = new Game("tester", new DoNothingShuffler());

        // Act
        game.Advance(PlayerDecision.Stand);

        // Assert
        Assert.Equal(2, game.Rounds.Count);
        Assert.True(game.Rounds.All(round => round.PlayerName == playerName));
        Assert.Equal(52 - 2 - 2, game.Rounds.First().Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
        //Assert.Equal(52 - 2 - 2 - 2, game.Rounds.Last().Deck.Cards.Count); // After one round, one extra card is dealt to the player, one to the dealer
    }
}