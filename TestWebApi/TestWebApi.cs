using System.Net.Http.Json;
using Blackjack.Service;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestWebApi;

public class TestWebApi
{
    [Fact]
    public async Task TestCreateGame()
    {
        var playerName = "DeterministicTester";

        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var response = await client.PostAsync($"/game/{playerName}", null);
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("48 cards in deck", content);
    }

    [Fact]
    public async Task TestGetExistingGame()
    {
        var playerName = "DeterministicTester";

        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var newGameResponse = await client.PostAsync($"/game/{playerName}", null);
        var newGameContent = await newGameResponse.Content.ReadAsStringAsync();

        var readGameResponse = await client.GetAsync($"/game/{playerName}");
        var readGameContent = await readGameResponse.Content.ReadAsStringAsync();

        Assert.Equal(newGameContent, readGameContent);
    }

    [Fact]
    public async Task TestHit()
    {
        var playerName = "DeterministicTester";

        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var newGameResponse = await client.PostAsync($"/game/{playerName}", null);
        var newGameView = await newGameResponse.Content.ReadFromJsonAsync<GameView>();

        var gameAfterHitResponse = await client.PostAsync($"/game/{playerName}/hit", null);
        var gameViewAfterHit = await gameAfterHitResponse.Content.ReadFromJsonAsync<GameView>();

        var expectedPlayerCards = newGameView.PlayerHand.Cards.Count();

        Assert.Equal(expectedPlayerCards, gameViewAfterHit.PlayerHand.Cards.Count());
    }
}