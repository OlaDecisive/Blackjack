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
        var game = Game.CreateNewGame(playerName);

        // Assert
        Assert.Equal(playerName, game.PlayerName);
        Assert.Equal(52 - 2 - 2, game.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }
}