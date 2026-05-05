using AuthorService.Application.DTO;
using AuthorService.Application.Interfaces;
using HotChocolate;

namespace AuthorService.Api.GraphQL
{
    /// <summary>
    /// GraphQL query class for author-related operations.
    /// </summary>
    public class AuthorQuery
    {
        /// <summary>
        /// Retrieves an author by their ID.
        /// </summary>
        /// <param name="svc">The author service to handle the query.</param>
        /// <param name="id">The ID of the author to retrieve.</param>
        /// <returns>An AuthorDto object representing the author, or null if not found.</returns>
        public Task<AuthorDto?> GetAuthorById([Service] IAuthorService svc, int id) => svc.GetAuthorByIdAsync(id);

        /// <summary>
        /// Retrieves an author by their name.
        /// </summary>
        /// <param name="svc">The author service to handle the query.</param>
        /// <param name="name">The name of the author to retrieve.</param>
        /// <returns>An AuthorDto object representing the author, or null if not found.</returns>
        public Task<AuthorDto?> GetAuthorsByName([Service] IAuthorService svc, string name) => svc.GetAuthorByNameAsync(name);

        /// <summary>
        /// Retrieves a list of featured authors.
        /// </summary>
        /// <param name="svc">The author service to handle the query.</param>
        /// <param name="top">The maximum number of featured authors to retrieve. Defaults to 0 (all).</param>
        /// <returns>A list of AuthorDto objects representing the featured authors.</returns>
        public Task<List<AuthorDto>> GetFeaturedAuthors([Service] IAuthorService svc, int top = 0) => svc.GetFeaturedAuthorsAsync(top);
    }
}
