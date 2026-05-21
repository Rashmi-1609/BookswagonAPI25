using CategoryService.Domain.Entities;
using CategoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CategoryService.Infrastructure.Data
{
    public class AppDbContext (DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<CategoryTree> CategoryTrees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TopLevelCategory> TopLevelCategoryResults { get; set; }


        //// Add these DbSets 
        //public DbSet<GroupedMenuResult> GroupedMenuResults { get; set; }
        //public DbSet<TopCategoryResult> TopCategoryResults { get; set; }
        //public DbSet<TopAuthorResult> TopAuthorResults { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API configurations can be added here if needed
            // For example, if you want to configure relationships or indexes


            //Explicit ModelBuilder configurations(For Category Tree)
            modelBuilder.Entity<CategoryTree>(entity =>
            {
                entity.ToTable("Table_CategoryTree");
                entity.HasKey(e => e.CategoryTreeId);
            });

            //For Category Menu
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
            });

            //modelBuilder.Entity<CategoryTree>().HasNoKey();

            // For Top Level Category Result (SP result) ->  configure the SP result as Keyless
            modelBuilder.Entity<TopLevelCategory>().HasNoKey();


           
        }


    }
}