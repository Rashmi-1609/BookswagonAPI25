using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Interfaces;

public interface IProductReviewRepository
{
    Task<ProductReviewSectionDto?> GetReviewSummaryByProductIdAsync(int productId);
}
