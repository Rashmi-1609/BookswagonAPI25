using HotChocolate.Types;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.GraphQL.Types;

public class PublisherType : ObjectType<Publisher>
{
    protected override void Configure(IObjectTypeDescriptor<Publisher> descriptor)
    {
        descriptor.Description("Represents a book publisher in the Bookswagon system.");

        // We can explicitly describe individual fields here. 
        // This shows up as hints in the Banana Cake Pop playground!
        descriptor.Field(p => p.PublisherId)
            .Description("The unique system identifier for the publisher.");

        descriptor.Field(p => p.CompanyName)
            .Description("The official registered name of the publishing company.");

        // The rest of the fields (PublisherImage, Description, etc.) will be automatically 
        // inferred by Hot Chocolate, so we only need to map the ones we want to document.
    }
}
