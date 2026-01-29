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
}
