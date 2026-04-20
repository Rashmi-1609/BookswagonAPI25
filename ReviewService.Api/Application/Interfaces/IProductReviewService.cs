using ReviewService.Api.Application.Common;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Interfaces;

public interface IProductReviewService
{
    Task<ServiceResult<ProductReviewSummary>> GetReviewSummaryAsync(int productId);
}
