using PublisherService.Application.Interfaces;
using PublisherService.Application.DTO;

namespace PublisherService.Api.GraphQL.Queries;

/// <summary>
/// The entry point for all read operations regarding Publishers.
/// </summary>
[ExtendObjectType("Query")]
public class PublisherQuery
{
    // --- QUERY: GET BY ID ---
    [GraphQLName("publisherById")]
    [GraphQLDescription("Fetches a single publisher by their unique ID.")]
    public async Task<PublisherDto> GetPublisherByIdAsync(
        int id,
        [Service] IPublisherService publisherService) // Method Injection!
    {
        var publisher = await publisherService.GetPublisherByIdAsync(id);

        if (publisher == null)
        {
            // it's a controlled GraphQL error here if not found.
            // Our GraphQLErrorFilter will let this pass through cleanly.
            throw new GraphQLException(new Error { Message = "Publisher not found or invalid ID." }.WithCode("NOT_FOUND"));
        }

        return publisher;
    }

    // --- QUERY: SEARCH BY NAME ---
    [GraphQLName("searchPublishers")]
    [GraphQLDescription("Searches for publishers by name with automatic pagination and filtering.")]
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    [UseProjection] // Allows the client to pick specific columns to fetch from the DB
    [UseFiltering]  // Allows the client to add custom 'where' clauses
    [UseSorting]    // Allows the client to sort the results
    public async Task<List<PublisherDto>> GetPublishersByName(
        string name,
        [Service] IPublisherService publisherService)
    {
        // If the service logic fails (e.g., empty string), it safely returns an empty IQueryable.
        // HotChocolate intercepts this IQueryable and translates it perfectly into a SQL LIMIT/OFFSET query!
        return await publisherService.GetPublishersByNameAsync(name);
    }
}
