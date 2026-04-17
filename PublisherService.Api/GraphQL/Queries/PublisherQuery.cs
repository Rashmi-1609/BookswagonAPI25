using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using PublisherService.Api.Application.Interfaces;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.GraphQL.Queries;

public class PublisherQuery
{
    // --- QUERY 1: GET BY ID ---
    [GraphQLName("publisherById")]
    [GraphQLDescription("Fetches a single publisher by their unique ID.")]
    public async Task<Publisher?> GetPublisherByIdAsync(
        int id,
        [Service] IPublisherService publisherService) // Method Injection!
    {
        var result = await publisherService.GetPublisherByIdAsync(id);

        if (!result.IsSuccess)
        {
            // Translates our Service error into a standard GraphQL error
            throw new GraphQLException(result.ErrorMessage ?? "Unknown error occurred.");
        }

        return result.Data;
    }

    // --- QUERY 2: SEARCH BY NAME (WITH HIGH PERFORMANCE IQUERYABLE) ---
    [GraphQLName("searchPublishers")]
    [GraphQLDescription("Searches for publishers by name with automatic pagination and filtering.")]
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] 
    [UseProjection] // Allows the client to pick specific columns to fetch from the DB
    [UseFiltering]  // Allows the client to add custom 'where' clauses
    [UseSorting]    // Allows the client to sort the results
    public IQueryable<Publisher> GetPublishersByName(
        string name,
        [Service] IPublisherService publisherService) // Method Injection!
    {
        var result = publisherService.GetPublishersByName(name);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.ErrorMessage ?? "Unknown error occurred.");
        }

        // We return the raw query. The [UsePaging] attribute intercepts this right before
        // it hits the database and safely applies the LIMIT/OFFSET to the SQL command!
        return result.Data!;
    }
}
