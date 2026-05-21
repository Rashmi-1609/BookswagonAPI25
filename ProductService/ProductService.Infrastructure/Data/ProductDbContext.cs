using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Data;

/// <summary>
/// Database context for Product service operations.
/// </summary>
public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<ProductReview> ProductReviews { get; set; }


    /// <summary>
    /// Configures the model mapping using the model builder.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Data annotations in Domain entities handle the mappings now.
    }
}
