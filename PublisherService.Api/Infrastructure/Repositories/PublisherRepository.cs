using Microsoft.EntityFrameworkCore;
using PublisherService.Api.Domain.Entities;
using PublisherService.Api.Application.Interfaces;
using PublisherService.Api.Infrastructure.Data;

namespace PublisherService.Api.Infrastructure.Repositories;

// using primary constructor syntax to inject the AppDbContext dependency
public class PublisherRepository(AppDbContext context) : IPublisherRepository
{
    public async Task<Publisher?> GetByIdAsync(int id)
    {
        // Find one publisher by ID
        return await context.Publishers
            .FirstOrDefaultAsync(p => p.PublisherId == id);
    }

    public IQueryable<Publisher> GetByName(string name)
    {
        // Fuzzy search using LIKE
        return context.Publishers
            .Where(p => EF.Functions.Like(p.CompanyName, $"%{name}%"))
            .OrderBy(p => p.CompanyName);
    }
}
