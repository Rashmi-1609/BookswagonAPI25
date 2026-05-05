using AuthorService.Application.DTO;
using AuthorService.Application.Interfaces;
using AuthorService.Domain.Entities;
using AuthorService.Domain.Interfaces;

namespace AuthorService.Application.Services
{
    /// <summary>
    /// Service implementation for author-related operations.
    /// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;

        /// <summary>
        /// Initializes a new instance of the AuthorService class.
        /// </summary>
        /// <param name="repository">The repository for accessing author data.</param>
        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<List<AuthorDto>> GetFeaturedAuthorsAsync(int topCount)
        {
            var authors = await _repository.GetFeaturedAuthorsAsync(topCount);
            return authors.Select(a => new AuthorDto(a.AuthorId, a.Name, a.FirstName, a.LastName, a.InvertedName, a.AuthorDetail, a.AuthorImage, a.PageTitle, a.MetaKeywords, a.MetaDescription)).ToList();
        }

        /// <inheritdoc />
        public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _repository.GetAuthorByIdAsync(id);
            return author == null ? null : new AuthorDto(author.AuthorId, author.Name, author.FirstName, author.LastName, author.InvertedName, author.AuthorDetail, author.AuthorImage, author.PageTitle, author.MetaKeywords, author.MetaDescription);
        }

        /// <inheritdoc />
        public async Task<AuthorDto?> GetAuthorByNameAsync(string name)
        {
            var author = await _repository.GetAuthorByNameAsync(name);
            return author == null ? null : new AuthorDto(author.AuthorId, author.Name, author.FirstName, author.LastName, author.InvertedName, author.AuthorDetail, author.AuthorImage, author.PageTitle, author.MetaKeywords, author.MetaDescription);
        }
    }
}
