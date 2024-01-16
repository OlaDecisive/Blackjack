using System.Text.Json;
using Model;

namespace Test;

public class TestModel
{
    [Fact]
    public void TestNewGame()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = GameState.CreateInitialGameState(playerName);

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
}