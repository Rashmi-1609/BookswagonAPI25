using Microsoft.EntityFrameworkCore;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.Infrastructure.Data;

// using primary constructor syntax to inject the DbContextOptions dependency
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSet represents the collection of all Publisher entities in the database.
    // This is what our Repository will actually query against.
    public DbSet<Publisher> Publishers { get; set; }
}
