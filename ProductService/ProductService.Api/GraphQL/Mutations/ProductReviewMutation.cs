using HotChocolate;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.Api.GraphQL.Mutations;

/// <summary>
/// GraphQL mutations for Product Reviews.
/// </summary>
public class ProductReviewMutation
{
    /// <summary>
    /// Adds a new product review along with tags and images.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="input">The review input details.</param>
    /// <returns>The generated review ID, or 0 if failed.</returns>
    public async Task<int> PostReview([Service] IProductReviewService svc, ProductReviewInputDto input)
    {
        if (string.IsNullOrEmpty(input.ReviewTitle))
        {
            throw new GraphQLException("ReviewTitle is required.");
        }
        return await svc.AddProductReviewAsync(input);
    }
}
