using MareksGym.Api.Application.Exercises;
using MareksGym.Api.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Infrastructure.Persistence;

// This class represents a session with the database and is used to query and save entities
public class ApplicationDbContext : DbContext
{
    // Constructor gets database configuration from ASP.NET (connection string, provider, etc.)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // This represents the MacroCalculationHistories table in the database
    public DbSet<MacroCalculationHistory> MacroCalculationHistories { get; set; }
    // This represents the Exercises table in the database
    public DbSet<Exercise> Exercises { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // I’m adding an index on the Name column of the Exercises table. Without this index, SQL Server would need to scan the entire table
        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.Name);

        // This type is only used to map stored procedure results, so it does not have a primary key
        modelBuilder.Entity<ExerciseSearchRow>()
            .HasNoKey();

        // I call the base method once so EF Core can apply its default conventions
        base.OnModelCreating(modelBuilder);
    }
}
