using AuthorService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorService.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<List<Author>> GetFeaturedAuthorsAsync(int topCount);
        Task<Author?> GetAuthorByNameAsync(string name);
    }
}

