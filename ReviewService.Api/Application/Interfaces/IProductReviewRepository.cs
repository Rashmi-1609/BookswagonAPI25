using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Interfaces;

public interface IProductReviewRepository
{
    Task<ProductReviewSummary?> GetReviewSummaryByProductIdAsync(int productId);
}
