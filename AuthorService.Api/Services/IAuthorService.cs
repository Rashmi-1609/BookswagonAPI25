using AuthorService.Data.Models;

namespace AuthorService.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetFeaturedAuthorsAsync(int topCount);
        Task<Author?> GetAuthorByNameAsync(string name);
        Task<Author?> GetByIdAsync(int id);
    }
}
