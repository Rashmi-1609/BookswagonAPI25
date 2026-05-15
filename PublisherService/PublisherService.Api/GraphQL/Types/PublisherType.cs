using PublisherService.Application.DTO;

namespace PublisherService.Api.GraphQL.Types;

/// <summary>
/// Configures how the Publisher entity is exposed to the outside world via GraphQL.
/// </summary>
public class PublisherType : ObjectType<PublisherDto>
{
    protected override void Configure(IObjectTypeDescriptor<PublisherDto> descriptor)
    {
        descriptor.Description("Represents a book publisher in the Bookswagon system.");

        // Documenting specific fields for the frontend developers reading our schema
        descriptor.Field(p => p.PublisherId)
            .Description("The unique system identifier for the publisher.");

        descriptor.Field(p => p.CompanyName)
            .Description("The official registered name of the publishing company.");

        // Hot Chocolate automatically infers the rest of the fields (Description, MetaKeywords, etc.)
    }
}
