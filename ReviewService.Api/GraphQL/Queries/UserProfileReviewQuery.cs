using HotChocolate;
using HotChocolate.Types;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.GraphQL.Queries;

// We use ExtendObjectType so we can split our massive Query across multiple files
[ExtendObjectType("Query")]
public class UserProfileReviewQuery
{
    // The exact schema name the frontend will use
    [GraphQLName("getUserProfileReviews")]
    [GraphQLDescription("Fetches a paginated list of product reviews written by a specific customer, including associated books and images.")]
    public async Task<UserProfileReviewDto?> GetUserProfileReviewsAsync(
        [GraphQLDescription("The unique ID of the customer profile.")] int customerProfileId,
        [GraphQLDescription("The page number for pagination (starts at 1).")] int pageNo,
        [GraphQLDescription("The number of rows to return per page (max 50).")] int noOfRow,
        [Service] IUserProfileReviewService reviewService) // Method Injection!
    {
        var result = await reviewService.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);

        if (!result.IsSuccess)
        {
            // Hot Chocolate catches this and formats it beautifully in the "errors" JSON array
            throw new GraphQLException(result.ErrorMessage ?? "An unknown error occurred.");
        }

        return result.Data;
    }
}
