using AuthorService.Data;
using AuthorService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorService.Services
{

    public class AuthorService:IAuthorService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public AuthorService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<Author?> GetAuthorByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            try
            {
                await using var db = await _dbFactory.CreateDbContextAsync();
                return await db.Authors
                    .Where(a => (a.FlagActive == true) && (a.FlagDelete == false) && EF.Functions.Like(a.Name, $"%{name}%"))
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Author>> GetFeaturedAuthorsAsync(int topCount = 0)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var q = db.Authors.Where(a => (a.FlagActive == true) && (a.FlagDelete == false) && (a.FlagTopAuthor == true)).OrderBy(a => a.Name);
            return topCount > 0 ? await q.Take(topCount).ToListAsync() : await q.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Authors.FirstOrDefaultAsync(a => a.AuthorId == id && (a.FlagActive == true) && (a.FlagDelete == false));
        }
    }
}
