using ReviewService.Api.Application.Common;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Services;

public class ProductReviewService(IProductReviewRepository repository) : IProductReviewService
{
    public async Task<ServiceResult<ProductReviewSummary>> GetReviewSummaryAsync(int productId)
    {
        // 1. Guard Clause: Validate the input
        if (productId <= 0)
        {
            return ServiceResult<ProductReviewSummary>.Failure("Invalid Product ID provided.");
        }

        // 2. Fetch the data from the repository
        var summary = await repository.GetReviewSummaryByProductIdAsync(productId);

        // 3. Handle unexpected nulls (though our repo should return an empty summary, it's good practice)
        if (summary == null)
        {
            return ServiceResult<ProductReviewSummary>.Failure("Failed to retrieve product reviews.");
        }

        // 4. Return the successful payload
        return ServiceResult<ProductReviewSummary>.Success(summary);
    }
}
