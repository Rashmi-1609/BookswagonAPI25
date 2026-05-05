using AuthorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorService.Infrastructure.Data
{
    /// <summary>
    /// Database context for the AuthorService.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Authors DbSet.
        /// </summary>
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Author entity
          //  modelBuilder.Entity<Author>().ToTable("Table_Author");
            //modelBuilder.Entity<Author>().HasKey(a => a.AuthorId);
        }
    }
}
