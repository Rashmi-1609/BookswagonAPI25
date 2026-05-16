using Microsoft.EntityFrameworkCore;
using PublisherService.Domain.Entities;
using PublisherService.Domain.Interfaces;

namespace PublisherService.Infrastructure.Data.Repositories;

/// <summary>
/// EF Core implementation of the IPublisherRepository.
/// </summary>
public class PublisherRepository(ApplicationDbContext context) : IPublisherRepository
{
    public async Task<Publisher?> GetPublisherByIdAsync(int id)
    {
        return await context.Publishers
            .FirstOrDefaultAsync(p => p.PublisherId == id);
    }

    public async Task<List<Publisher>> GetPublishersByNameAsync(string name, int pageNumber = 1, int pageSize = 10)
    {
        // Search using LIKE matching your uploaded logic
        return await context.Publishers
            .Where(p => EF.Functions.Like(p.CompanyName, $"%{name}%"))
            .OrderBy(p => p.CompanyName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
