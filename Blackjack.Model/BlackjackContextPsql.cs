using Microsoft.EntityFrameworkCore;

namespace Blackjack.Model;

/// <summary>
/// To avoid generating EFCore migrations for SQLite, this class can be used:
/// 'dotnet ef migrations add <name> --context BlackjackContextPsql'
/// </summary>
public class BlackjackContextPsql : BlackjackContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var pgsqlConnectionString = System.Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_");
        optionsBuilder.UseNpgsql(pgsqlConnectionString);
    }
}