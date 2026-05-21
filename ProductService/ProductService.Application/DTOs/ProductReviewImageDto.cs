using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing an image attached to a product review.
/// </summary>
public class ProductReviewImageDto
{
    [Column("ID_ProductReview")]
    public int ProductReviewId { get; set; }
    
    [Column("Image_Location")]
    public string ImageLocation { get; set; } = string.Empty;
    
    [Column("ImgCaption")]
    public string ImageCaption { get; set; } = string.Empty;
}
