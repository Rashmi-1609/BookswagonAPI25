using Microsoft.EntityFrameworkCore;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.Infrastructure.Data;

// AppDbContext inherits from DbContext (the core EF class).
public class AppDbContext : DbContext
{
    // Constructor accepts options (like the connection string) passed from Program.cs via Dependency Injection.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet represents the collection of all Publisher entities in the database.
    // This is what our Repository will actually query against.
    public DbSet<Publisher> Publishers { get; set; }
}
