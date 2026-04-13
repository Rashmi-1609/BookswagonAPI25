using AuthorService.Data.Models;
using AuthorService.Services;

namespace AuthorService.Api.GraphQL
{
    public class AuthorQuery
    {
        public Task<Author?> GetAuthorById([Service] IAuthorService svc, int id) => svc.GetByIdAsync(id);
        public Task<Author?> SearchAuthor([Service] IAuthorService svc, string name) => svc.GetAuthorByNameAsync(name);
        public Task<List<Author>> FeaturedAuthors([Service] IAuthorService svc, int top = 0) => svc.GetFeaturedAuthorsAsync(top);
    }
}
