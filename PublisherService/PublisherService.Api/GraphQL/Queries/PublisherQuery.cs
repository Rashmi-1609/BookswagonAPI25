using PublisherService.Application.DTO;
using PublisherService.Application.Interfaces;
using HotChocolate;

namespace PublisherService.Api.GraphQL.Queries;

/// <summary>
/// GraphQL query class for publisher-related operations.
/// </summary>
public class PublisherQuery
{
    /// <summary>
    /// Retrieves a publisher by their ID.
    /// </summary>
    /// <param name="svc">The publisher service to handle the query.</param>
    /// <param name="id">The ID of the publisher to retrieve.</param>
    /// <returns>A PublisherDto object representing the publisher, or null if not found.</returns>
    public Task<PublisherDto?> GetPublisherById([Service] IPublisherService svc, int id) => svc.GetPublisherByIdAsync(id);

    /// <summary>
    /// Retrieves publishers by their name.
    /// </summary>
    /// <param name="svc">The publisher service to handle the query.</param>
    /// <param name="name">The name of the publishers to retrieve.</param>
    /// <param name="pageNumber">The page number for pagination (1-indexed).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A list of PublisherDto objects representing the publishers.</returns>
    public Task<List<PublisherDto>> GetPublishersByName([Service] IPublisherService svc, string name, int pageNumber = 1, int pageSize = 10) => svc.GetPublishersByNameAsync(name, pageNumber, pageSize);
}
