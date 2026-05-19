namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing detailed product review data.
/// </summary>
public class ProductReviewDto
{
    public int ProductReviewId { get; set; }
    public int ProductId { get; set; }
    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewBy { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReviewStatus { get; set; }
    public int Rating { get; set; }
    public int AvgRating { get; set; }
    public int TotalReview { get; set; }
    public string PostDate { get; set; } = string.Empty;
    public int RatingCount { get; set; }
    public string ReaderType { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int Vote { get; set; }
    public string ReviewTags { get; set; } = string.Empty;
    public int StarOne { get; set; }
    public int StarTwo { get; set; }
    public int StarThree { get; set; }
    public int StarFour { get; set; }
    public int StarFive { get; set; }
    public string RecommendThis { get; set; } = string.Empty;
    public string ReaderSpoiler { get; set; } = string.Empty;
    public int TotalRecord { get; set; }
    public int CustomerProfileId { get; set; }
    public List<ProductReviewImageDto> ProductReviewImages { get; set; } = new();
    public ReviewHelpFulDto ReviewHelpFul { get; set; } = new();
}
