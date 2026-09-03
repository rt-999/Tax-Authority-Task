using Microsoft.EntityFrameworkCore;
using BonusSystem.Api.Models.Entities;

namespace BonusSystem.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<PremiumMethod> PremiumMethods => Set<PremiumMethod>();
    public DbSet<Measure> Measures => Set<Measure>();
    public DbSet<IngestionHistory> IngestionHistories => Set<IngestionHistory>();
    public DbSet<IngestedDataRow> IngestedDataRows => Set<IngestedDataRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IngestedDataRow>()
            .HasIndex(r => r.IngestionHistoryId);

        // הגדרת עמודת JSON SQLite/SQL Server
        modelBuilder.Entity<IngestedDataRow>()
            .Property(r => r.DataJson)
            .HasColumnType("TEXT"); // ב-SQLite משתמשים ב-TEXT עבור JSON
    }
}