using HotChocolate.Types;
using ProductService.Application.DTOs;

namespace ProductService.Api.GraphQL.Types;

/// <summary>
/// GraphQL type for ProductReviewDto.
/// </summary>
public class ProductReviewType : ObjectType<ProductReviewDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductReviewDto> descriptor)
    {
        descriptor.Field(p => p.ProductReviewId).Type<NonNullType<IntType>>();
        descriptor.Field(p => p.ProductId).Type<NonNullType<IntType>>();
        descriptor.Field(p => p.ReviewTitle).Type<StringType>();
        descriptor.Field(p => p.ReviewBy).Type<StringType>();
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.ReviewStatus).Type<IntType>();
        descriptor.Field(p => p.Rating).Type<IntType>();
        descriptor.Field(p => p.AvgRating).Type<IntType>();
        descriptor.Field(p => p.TotalReview).Type<IntType>();
        descriptor.Field(p => p.DateCreated).Type<DateTimeType>();
        descriptor.Field(p => p.RatingCount).Type<IntType>();
        descriptor.Field(p => p.ReaderType).Type<StringType>();
        descriptor.Field(p => p.UserEmail).Type<StringType>();
        descriptor.Field(p => p.Vote).Type<IntType>();
        descriptor.Field(p => p.ReviewTags).Type<StringType>();
        descriptor.Field(p => p.StarOne).Type<IntType>();
        descriptor.Field(p => p.StarTwo).Type<IntType>();
        descriptor.Field(p => p.StarThree).Type<IntType>();
        descriptor.Field(p => p.StarFour).Type<IntType>();
        descriptor.Field(p => p.StarFive).Type<IntType>();
        descriptor.Field(p => p.RecommendThis).Type<StringType>();
        descriptor.Field(p => p.ReaderSpoiler).Type<StringType>();
        descriptor.Field(p => p.TotalRecord).Type<IntType>();
        descriptor.Field(p => p.CustomerProfileId).Type<IntType>();
        descriptor.Field(p => p.ProductReviewImages).Type<ListType<ProductReviewImageType>>();
        descriptor.Field(p => p.ReviewHelpFul).Type<ReviewHelpFulType>();
    }
}
