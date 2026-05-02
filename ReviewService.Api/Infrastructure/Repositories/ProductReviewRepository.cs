using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;
using ReviewService.Api.Infrastructure.Data;

namespace ReviewService.Api.Infrastructure.Repositories;

public class ProductReviewRepository(AppDbContext context) : IProductReviewRepository
{
    public async Task<ProductReviewSectionDto?> GetReviewSummaryByProductIdAsync(int productId)
    {
        // base query with our strict business rules
        var query = context.ProductReviews
            .Where(p => p.ProductId == productId
                     && p.ReviewStatus == 1
                     && p.IsActive
                     && !p.IsDeleted);

        // here SQL Server calculates the total count
        var totalCount = await query.CountAsync();

        // Guard Clause: If there are no reviews, return an empty summary
        if (totalCount == 0)
        {
            return new ProductReviewSectionDto
            {
                TotalCount = 0,
                AvgRating = 0,
                Reviews = new()
            };
        }

        // here SQL Server calculate the average rating
        // (We do this after the count check because AverageAsync throws an error on empty sequences)
        var avgRating = await query.AverageAsync(p => p.Rating);

        // Map directly from Entity to the safe DTO in SQL
        var reviews = await query
            .Select(p => new ReviewCardDto
            {
                Rating = p.Rating,
                ReviewBy = p.ReviewBy,
                DateCreated = p.DateCreated,
                ReviewTitle = p.ReviewTitle,
                Description = p.Description,
                RecommendThis = p.RecommendThis
            })
            .ToListAsync();

        // Assemble and return the final DTO
        return new ProductReviewSectionDto
        {
            TotalRating = 5, // Hardcoded per your requirements
            AvgRating = Math.Round(avgRating, 1), // Rounding to 1 decimal place (e.g., 4.2)
            TotalCount = totalCount,
            Reviews = reviews
        };
    }
}
