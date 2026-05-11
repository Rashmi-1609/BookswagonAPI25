using PublisherService.Domain.Entities;

namespace PublisherService.Domain.Interfaces;

/// <summary>
/// This is a CONTRACT - it defines WHAT operations exist for Publisher entities.
/// </summary>
public interface IPublisherRepository
{
    /// <summary>
    /// Fetches a single publisher by their unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the publisher.</param>
    /// <returns>The Publisher entity if found; otherwise, null.</returns>
    Task<Publisher?> GetPublisherByIdAsync(int id);

    /// <summary>
    /// Searches for publishers based on a fuzzy match of their company name.
    /// </summary>
    /// <param name="name">The search string to match against company names.</param>
    /// <returns>An IQueryable of matching publishers for deferred execution.</returns>
    IQueryable<Publisher> GetPublishersByName(string name);
}
