using AuthorService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorService.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>().ToTable("Table_Author");
            modelBuilder.Entity<Author>().HasKey(a => a.AuthorId);
        }
    }
}
