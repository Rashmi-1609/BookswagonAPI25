namespace ReviewService.Api.Application.Models;

public class ProductReviewSectionDto
{
    public int TotalRating { get; set; } = 5;
    public double AvgRating { get; set; }
    public int TotalCount { get; set; }

    // The Composition List
    public List<ReviewCardDto> Reviews { get; set; } = new();
}
