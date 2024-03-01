using System.Linq;
using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Model;

public class BlackjackContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<GameState> GameStates { get; set; }
    public DbSet<Hand> Hands { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Card> Cards { get; set; }
    //public DbSet<CardValue> CardValues { get; set; }
    //public DbSet<Suit> Suits { get; set; }

    public BlackjackContext()
    {
    }

    public BlackjackContext(DbContextOptions<BlackjackContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        #if DEBUG
        optionsBuilder.UseSqlite("Data Source=file::memory:?cache=shared");
        #else
        var pgsqlConnectionString = System.Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_PSQL_CONNECTIONSTRING_MANAGED");
        if (string.IsNullOrEmpty(pgsqlConnectionString))
        {
            var varnames = System.Environment.GetEnvironmentVariables().Keys;
            List<string> envvars = new();

            foreach (var v in varnames)
                envvars.Add(v + " = " + System.Environment.GetEnvironmentVariable((string)v));

            throw new Exception($"Empty psql connstring, dumping env vars:\n{string.Join("\n", envvars)}");
        }
        optionsBuilder.UseNpgsql(pgsqlConnectionString, npgsqlDbContextOptionsBuilder => 
        {
            npgsqlDbContextOptionsBuilder.ProvidePasswordCallback((host, port, database, username) => GetAccessToken());
        });
        #endif
    }

    private static string GetAccessToken()
    {
        var tokenRequestContext = new TokenRequestContext(new[] { "https://ossrdbms-aad.database.windows.net" });
        var credential = new DefaultAzureCredential(); // Set ManagedIdentityClientId if you want to use User Assigned Managed Identity
        var accessToken = credential.GetToken(tokenRequestContext);

        return accessToken.Token;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
