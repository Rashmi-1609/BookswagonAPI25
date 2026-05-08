using AuthorService.Infrastructure.Data;
using Shared.Authentication.Entities;
using Shared.Authentication.Interfaces;

namespace AuthorService.Infrastructure.Data.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTokenAsync(UserToken userToken)
        {
            await _dbContext.UserTokens.AddAsync(userToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}