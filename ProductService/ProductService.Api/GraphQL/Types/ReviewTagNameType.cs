using HotChocolate.Types;
using ProductService.Application.DTOs;

namespace ProductService.Api.GraphQL.Types;

/// <summary>
/// GraphQL type for ReviewTagNameDto.
/// </summary>
public class ReviewTagNameType : ObjectType<ReviewTagNameDto>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewTagNameDto> descriptor)
    {
        descriptor.Field(r => r.Id).Type<NonNullType<IntType>>();
        descriptor.Field(r => r.TagName).Type<StringType>();
    }
}
