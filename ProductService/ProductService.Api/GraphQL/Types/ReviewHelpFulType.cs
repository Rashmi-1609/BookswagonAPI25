using HotChocolate.Types;
using ProductService.Application.DTOs;

namespace ProductService.Api.GraphQL.Types;

/// <summary>
/// GraphQL type for ReviewHelpFulDto.
/// </summary>
public class ReviewHelpFulType : ObjectType<ReviewHelpFulDto>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewHelpFulDto> descriptor)
    {
        descriptor.Field(r => r.ReviewHelpId).Type<NonNullType<IntType>>();
        descriptor.Field(r => r.Helpful).Type<IntType>();
        descriptor.Field(r => r.NotHelpFul).Type<IntType>();
        descriptor.Field(r => r.Reported).Type<IntType>();
    }
}
