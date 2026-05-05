using AuthorService.Application.DTO;
using AuthorService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorService.Application.Interfaces
{
    /// <summary>
    /// Interface for author-related operations.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Gets a list of featured authors.
        /// </summary>
        /// <param name="topCount">The number of top authors to retrieve.</param>
        /// <returns>A list of <see cref="AuthorDto"/> representing the featured authors.</returns>
        Task<List<AuthorDto>> GetFeaturedAuthorsAsync(int topCount);

        /// <summary>
        /// Gets an author by their name.
        /// </summary>
        /// <param name="name">The name of the author to retrieve.</param>
        /// <returns>An <see cref="AuthorDto"/> representing the author, or null if not found.</returns>
        Task<AuthorDto?> GetAuthorByNameAsync(string name);

        /// <summary>
        /// Gets an author by their ID.
        /// </summary>
        /// <param name="id">The ID of the author to retrieve.</param>
        /// <returns>An <see cref="AuthorDto"/> representing the author, or null if not found.</returns>
        Task<AuthorDto?> GetAuthorByIdAsync(int id);
    }
}
