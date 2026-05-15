using Microsoft.EntityFrameworkCore;
using PublisherService.Domain.Entities;

namespace PublisherService.Infrastructure.Data;

/// <summary>
/// Database context for the PublisherService.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // This creates the physical bridge between our C# entity and the SQL Table
    /// <summary>
    /// Gets or sets the Publishers DbSet.
    /// </summary>
    public DbSet<Publisher> Publishers => Set<Publisher>();
}
