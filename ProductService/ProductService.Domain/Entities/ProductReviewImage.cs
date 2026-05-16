using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities;

/// <summary>
/// Represents an image associated with a product review.
/// </summary>
public class ProductReviewImage
{
    [Key]
    [Column("ID_ProductReview")]
    public int ProductReviewId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string ImageLocation { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string ImageCaption { get; set; } = string.Empty;
}
