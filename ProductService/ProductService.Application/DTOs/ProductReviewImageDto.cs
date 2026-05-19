namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing an image attached to a product review.
/// </summary>
public class ProductReviewImageDto
{
    public int ProductReviewId { get; set; }
    public string ImageLocation { get; set; } = string.Empty;
    public string ImageCaption { get; set; } = string.Empty;
}
