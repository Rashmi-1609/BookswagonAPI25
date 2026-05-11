using PublisherService.Application.Interfaces;
using PublisherService.Domain.Entities;
using PublisherService.Domain.Interfaces;


namespace PublisherService.Application.Services;

/// <summary>
/// Implements the core business rules for publishers before delegating to the repository.
/// </summary>
public class PublisherService(IPublisherRepository repository) : IPublisherService
{
    public async Task<Publisher?> GetPublisherByIdAsync(int id)
    {
        // Rule -> IDs must be positive
        if (id <= 0)
        {
            return null;
        }

        return await repository.GetPublisherByIdAsync(id);
    }

    public IQueryable<Publisher> GetPublishersByName(string name)
    {
        // Rule -> Don't search if name is empty
        if (string.IsNullOrWhiteSpace(name))
        {
            return Enumerable.Empty<Publisher>().AsQueryable();
        }

        return repository.GetPublishersByName(name);
    }
}
