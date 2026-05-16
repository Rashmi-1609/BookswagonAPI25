using HotChocolate.Types;
using ProductService.Application.DTOs;

namespace ProductService.Api.GraphQL.Types;

/// <summary>
/// GraphQL type for ProductReviewImageDto.
/// </summary>
public class ProductReviewImageType : ObjectType<ProductReviewImageDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductReviewImageDto> descriptor)
    {
        descriptor.Field(i => i.ProductReviewId).Type<NonNullType<IntType>>();
        descriptor.Field(i => i.ImageLocation).Type<StringType>();
        descriptor.Field(i => i.ImageCaption).Type<StringType>();
    }
}
