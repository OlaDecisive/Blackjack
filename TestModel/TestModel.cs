using System.Text.Json;
using Blackjack.Model;

namespace Blackjack.Test;

public class TestModel
{
    [Fact]
    public void TestCreateInitialGameState()
    {
        // Arrange & Act
        var game = Game.CreateDeterministicGame("tester", DateTime.UtcNow);

        // Assert
        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer
    }

    [Fact]
    public void TestSerialization()
    {
        var game = Game.CreateDeterministicGame("tester", DateTime.UtcNow);

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
        var game = Game.CreateDeterministicGame(playerName, DateTime.UtcNow);
        
        // Assert
        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer

        // here we assume that the player is dealt the first card from the top of the deck
        // since we should have a deterministic deck, the first card should be Ace of Clubs (first Suit enum value)
        var firstPlayersCard = game.CurrentRound.PlayerHand.Cards.First();
        Assert.Equal(CardValue.Ace, firstPlayersCard.Value);
        Assert.Equal(Suit.Clubs, firstPlayersCard.Suit);

        var secondPlayersCard = game.CurrentRound.PlayerHand.Cards[1];
        Assert.Equal(CardValue.Two, secondPlayersCard.Value);
        Assert.Equal(Suit.Clubs, secondPlayersCard.Suit);

        var firstDealersCard = game.CurrentRound.DealerHand.Cards.First();
        Assert.Equal(CardValue.Three, firstDealersCard.Value);
        Assert.Equal(Suit.Clubs, firstDealersCard.Suit);

        var secondDealersCard = game.CurrentRound.DealerHand.Cards[1];
        Assert.Equal(CardValue.Four, secondDealersCard.Value);
        Assert.Equal(Suit.Clubs, secondDealersCard.Suit);
    }

    [Fact]
    public void TestAdvanceGameWithHit()
    {
        // Arrange
        var playerName = "tester";
        
        // Act
        var game = Game.CreateDeterministicGame(playerName, DateTime.UtcNow);

        Assert.Equal(52 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // At start of game, two cards are dealt to player, two to dealer

        game.Advance(PlayerDecision.Hit);

        // Assert
        Assert.Equal(52 - 2 - 2 - 2, game.CurrentRound.Deck.Cards.Count); // After one round, one extra card is dealt to the player, one to the dealer

        Assert.Equal(GameStatus.Running, game.Status);
        Assert.Contains("deck", game.GameDescription);
        
        // here we assume that the player is dealt the first card from the top of the deck
        // since we should have a deterministic deck, the first card should be Ace of Clubs (first Suit enum value)
        var firstPlayersCard = game.CurrentRound.PlayerHand.Cards.First();
        Assert.Equal(CardValue.Ace, firstPlayersCard.Value);
        Assert.Equal(Suit.Clubs, firstPlayersCard.Suit);

        var secondPlayersCard = game.CurrentRound.PlayerHand.Cards[1];
        Assert.Equal(CardValue.Two, secondPlayersCard.Value);
        Assert.Equal(Suit.Clubs, secondPlayersCard.Suit);

        var firstDealersCard = game.CurrentRound.DealerHand.Cards.First();
        Assert.Equal(CardValue.Three, firstDealersCard.Value);
        Assert.Equal(Suit.Clubs, firstDealersCard.Suit);

        var secondDealersCard = game.CurrentRound.DealerHand.Cards[1];
        Assert.Equal(CardValue.Four, secondDealersCard.Value);
        Assert.Equal(Suit.Clubs, secondDealersCard.Suit);

        var thirdPlayersCard = game.CurrentRound.PlayerHand.Cards[2];
        Assert.Equal(CardValue.Five, thirdPlayersCard.Value);
        Assert.Equal(Suit.Clubs, thirdPlayersCard.Suit);

        var thirdDealersCard = game.CurrentRound.DealerHand.Cards[2];
        Assert.Equal(CardValue.Six, thirdDealersCard.Value);
        Assert.Equal(Suit.Clubs, thirdDealersCard.Suit);

    }

    [Fact]
    public void TestAdvanceGameWithStand()
    {
        // Arrange
        var playerName = "tester";
        var game = Game.CreateDeterministicGame(playerName, DateTime.UtcNow);

        // Act
        game.Advance(PlayerDecision.Stand);

        // Assert
        Assert.Equal(52 - 2 - 2 - 1, game.CurrentRound.Deck.Cards.Count); // After one round with stand, no extra card is dealt to the player, one to the dealer

        // here we assume that the player is dealt the first card from the top of the deck
        // since we should have a deterministic deck, the first card should be Ace of Clubs (first Suit enum value)
        var firstPlayersCard = game.CurrentRound.PlayerHand.Cards.First();
        Assert.Equal(CardValue.Ace, firstPlayersCard.Value);
        Assert.Equal(Suit.Clubs, firstPlayersCard.Suit);

        var secondPlayersCard = game.CurrentRound.PlayerHand.Cards[1];
        Assert.Equal(CardValue.Two, secondPlayersCard.Value);
        Assert.Equal(Suit.Clubs, secondPlayersCard.Suit);

        var firstDealersCard = game.CurrentRound.DealerHand.Cards.First();
        Assert.Equal(CardValue.Three, firstDealersCard.Value);
        Assert.Equal(Suit.Clubs, firstDealersCard.Suit);

        var secondDealersCard = game.CurrentRound.DealerHand.Cards[1];
        Assert.Equal(CardValue.Four, secondDealersCard.Value);
        Assert.Equal(Suit.Clubs, secondDealersCard.Suit);

        //var thirdPlayersCard = game.CurrentRound.PlayerHand.Cards[2];
        //Assert.Equal(CardValue.Five, thirdPlayersCard.Value);
        //Assert.Equal(Suit.Clubs, thirdPlayersCard.Suit);
        Assert.Equal(2, game.CurrentRound.PlayerHand.Cards.Count);

        var thirdDealersCard = game.CurrentRound.DealerHand.Cards[2];
        Assert.Equal(CardValue.Five, thirdDealersCard.Value);
        Assert.Equal(Suit.Clubs, thirdDealersCard.Suit);
    }

    [Fact]
    public void TestCannotHitAfterStand()
    {
        // Arrange
        var playerName = "tester";
        var game = Game.CreateDeterministicGame(playerName, DateTime.UtcNow);

        // Act
        game.Advance(PlayerDecision.Stand);

        // Assert
        Assert.ThrowsAny<Exception>(() => game.Advance(PlayerDecision.Hit));
    }

    [Fact]
    public void TestDealerWinsAfterStand()
    {
        // Arrange
        var playerName = "tester";
        var game = Game.CreateDeterministicGame(playerName, DateTime.UtcNow);

        // Act
        while (game.Status == GameStatus.Running)
            game.Advance(PlayerDecision.Stand);

        // Assert
        Assert.Equal(GameStatus.DealerWins, game.Status);
        Assert.Equal(3+4+5+6, game.CurrentRound.DealerHand.NumberValue);
    }

    [Theory]
    [InlineData(1, 1, GameStatus.Running)]
    [InlineData(21, 1, GameStatus.PlayerWins)]
    [InlineData(22, 1, GameStatus.DealerWins)]
    [InlineData(1, 21, GameStatus.DealerWins)]
    [InlineData(21, 21, GameStatus.Tie)]
    [InlineData(20, 18, GameStatus.PlayerWins)]
    [InlineData(18, 20, GameStatus.DealerWins)]
    [InlineData(17, 17, GameStatus.Tie)]
    [InlineData(16, 17, GameStatus.DealerWins)]
    [InlineData(16, 16, GameStatus.Running)]
    public void TestDetermineGameStatus(int playerSum, int dealerSum, GameStatus expectedStatus)
    {
        var gameStatus = GameState.DetermineGameStatus(playerSum, dealerSum);
        Assert.Equal(expectedStatus, gameStatus);
    }
}