namespace ProductService.Application.DTOs;

/// <summary>
/// Input DTO representing details for creating a new product review.
/// </summary>
public class ProductReviewInputDto
{
    public int ProductId { get; set; }
    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewBy { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReviewStatus { get; set; }
    public int Rating { get; set; }
    public string RecommendThis { get; set; } = string.Empty;
    public string ReaderType { get; set; } = string.Empty;
    public string ReaderSpoiler { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<ReviewTagNameDto>? ReviewTagNames { get; set; }
    public List<ProductReviewImageDto>? ProductReviewImages { get; set; }
}
