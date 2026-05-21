using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities;

/// <summary>
/// Represents a product review within the domain.
/// </summary>
[Table("Table_ProductReview")]
public class ProductReview
{
    [Key]
    [Column("ID_ProductReview")]
    public int ProductReviewId { get; set; }
    
    [Required]
    [Column("ID_Product")]
    public int ProductId { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Column("Review_Title")]
    public string ReviewTitle { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string ReviewBy { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Column("Status")]
    public int ReviewStatus { get; set; }
    
    public int Rating { get; set; }
    
    [Column("Flag_Active")]
    public bool IsActive { get; set; }
    
    [Column("Flag_Delete")]
    public bool IsDeleted { get; set; }
    
    [Column("Date_Created")]
    public DateTime DateCreated { get; set; }


}
