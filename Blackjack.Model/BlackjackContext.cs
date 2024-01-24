using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blackjack.Model;

public class BlackjackContext : DbContext
{
    public string DbPath { get; }

    public DbSet<Game> Games { get; set; }
    public DbSet<GameState> GameStates { get; set; }
    public DbSet<Hand> Hands { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Card> Cards { get; set; }
    //public DbSet<CardValue> CardValues { get; set; }
    //public DbSet<Suit> Suits { get; set; }


    public BlackjackContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blackjack.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Deck>(buildAction => 
        {
            //buildAction.OwnsOne(deck => deck.Cards)
            //buildAction.OwnsMany<Card>("ownedTypeName", "navigationName");
            buildAction.OwnsMany<Card>(deck => deck.Cards, deck => 
            {
                return deck.Cards;
            });
        });*/
        base.OnModelCreating(modelBuilder);
    }
}