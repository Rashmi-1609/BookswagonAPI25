using ReviewService.Api.Domain.Entities;

namespace ReviewService.Api.Application.Models;

public class ProductReviewSummary
{
    // Hardcoded to 5 per current requirements
    public int TotalRating { get; set; } = 5;

    // Calculated average across all reviews
    public double AvgRating { get; set; }

    // Total count of reviewers
    public int TotalCount { get; set; }

    // The list of individual user reviews
    public List<ProductReview> Reviews { get; set; } = new();
}
