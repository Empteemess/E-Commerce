
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserSagaData> UserSagaData { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSagaData>().HasKey(x => x.CorrelationId);

        modelBuilder.Entity<OutboxMessage>().HasIndex(x => new { x.IsProcessed, x.ProcessedAt });

        base.OnModelCreating(modelBuilder);
    }
}