using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Domain.Entities;

namespace ReviewService.Api.Infrastructure.Data;

// using primary constructor syntax to inject the DbContextOptions dependency
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ProductReview> ProductReviews { get; set; }
}
