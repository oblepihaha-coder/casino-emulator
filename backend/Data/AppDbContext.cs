using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Bet> Bets => Set<Bet>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=casino.db");

        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        Name = "Олена",
        Balance = 1000,
        Role = "Player"
    },
    new User
    {
        Id = 2,
        Name = "Іван",
        Balance = 1200,
        Role = "Player"
    },
    new User
    {
        Id = 3,
        Name = "Admin",
        Balance = 99999,
        Role = "Admin"
    }
);

    modelBuilder.Entity<User>()
            .HasMany(u => u.Bets)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .Property(u => u.Balance)
            .HasDefaultValue(1000);

        modelBuilder.Entity<Bet>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}