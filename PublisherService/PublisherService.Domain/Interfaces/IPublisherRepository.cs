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
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of matching publishers for deferred execution.</returns>
    Task<List<Publisher>> GetPublishersByNameAsync(string name, int pageNumber = 1, int pageSize = 10);
}
