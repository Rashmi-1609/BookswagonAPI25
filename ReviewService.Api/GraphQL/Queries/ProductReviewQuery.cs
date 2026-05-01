using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class ProductReviewQuery
{
    [GraphQLName("productReviewDetail")]
    [GraphQLDescription("Retrieves the aggregated summary and list of active product reviews by Product ID.")]
    public async Task<ProductReviewSummary?> GetProductReviewDetailAsync(
        int productId,
        [Service] IProductReviewService reviewService) // Method Injection!
    {
        var result = await reviewService.GetReviewSummaryAsync(productId);

        if (!result.IsSuccess)
        {
            // HotChocolate will automatically convert this exception into a clean GraphQL error array
            throw new GraphQLException(result.ErrorMessage ?? "An unknown error occurred.");
        }

        return result.Data;
    }
}
