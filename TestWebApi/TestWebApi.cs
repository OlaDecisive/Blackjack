using Microsoft.AspNetCore.Mvc.Testing;

namespace TestWebApi;

public class TestWebApi
{
    [Fact]
    public async Task TestNewGame()
    {
        var playerName = "tester";

        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var response = await client.PostAsync($"/game/{playerName}", null);
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("48 cards in deck", content);
    }
}