using AuthorService.Domain.Entities;
using AuthorService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthorService.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for accessing author data.
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        /// <summary>
        /// Initializes a new instance of the AuthorRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AuthorRepository(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        /// <inheritdoc />
        public async Task<List<Author>> GetFeaturedAuthorsAsync(int topCount)
        {
            return await _dbcontext.Authors
                .Where(a => a.FlagTopAuthor == true)
                .Take(topCount)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Author?> GetAuthorByNameAsync(string name)
        {
            return await _dbcontext.Authors
                 .FirstOrDefaultAsync(a => a.Name == name);
        }

        /// <inheritdoc />
        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _dbcontext.Authors
                .FirstOrDefaultAsync(a => a.AuthorId == id);
        }
    }
}
