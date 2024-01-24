using System.Text.Json;
using Blackjack.Model;

namespace Blackjack.Test;

public class TestModel
{
    [Fact]
    public void TestCreateInitialGameState()
    {
        // Arrange & Act
        var game = new Game("tester", new DoNothingShuffler());

        // Assert
        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }

    [Fact]
    public void TestSerialization()
    {
        var game = Game.CreateGameWithShuffledCards("tester");

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
        var game = new Game(playerName, new DoNothingShuffler());
        
        // Assert
        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }

    [Fact]
    public void TestAdvanceGameWithHit()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = new Game(playerName, new DoNothingShuffler());
        game.Advance(PlayerDecision.Hit);

        // Assert
        //Assert.Equal(2, game.Rounds.Count);
        //Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
        Assert.Equal(52 - 2 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // After one round, one extra card is dealt to the player, one to the dealer

        Assert.Equal(GameStatus.Running, game.Status);
        Assert.Contains("deck", game.GameDescription);
        //Assert.Contains("dealer", game.DealersFinalHandDescription);
    }

    [Fact]
    public void TestAdvanceGameWithStand()
    {
        // Arrange
        var playerName = "tester";
        var game = new Game(playerName, new DoNothingShuffler());

        // Act
        game.Advance(PlayerDecision.Stand);

        // Assert
        //Assert.Equal(2, game.Rounds.Count);
        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
        //Assert.Equal(52 - 2 - 2 - 2, game.Rounds.Last().Deck.Cards.Count); // After one round, one extra card is dealt to the player, one to the dealer
    }
}