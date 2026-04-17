using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.Application.Interfaces;

// This is a CONTRACT - it defines WHAT operations exist
// The actual implementation is in PublisherRepository.cs
public interface IPublisherRepository
{
    Task<Publisher?> GetByIdAsync(int id);
    IQueryable<Publisher> GetByName(string name);
}
