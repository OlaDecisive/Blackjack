using System.Linq;
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
        var pgsqlConnectionString = System.Environment.GetEnvironmentVariable("PSQL_CONNECTIONSTRING");
        if (string.IsNullOrEmpty(pgsqlConnectionString))
        {
            var varnames = System.Environment.GetEnvironmentVariables().Keys;
            List<string> envvars = new();

            foreach (var v in varnames)
                envvars.Add(v + " = " + System.Environment.GetEnvironmentVariable((string)v));

            throw new Exception($"Empty psql connstring, dumping env vars:\n{string.Join("\n", envvars)}");
        }
        optionsBuilder.UseNpgsql(pgsqlConnectionString);
        #endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
