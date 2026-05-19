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

    // Computed / SP fields (Not in Table)
    [NotMapped]
    public int AvgRating { get; set; }
    
    [NotMapped]
    public int TotalReview { get; set; }
    
    [NotMapped]
    [MaxLength(50)]
    public string PostDate { get; set; } = string.Empty;
    
    [NotMapped]
    public int RatingCount { get; set; }
    
    [NotMapped]
    [MaxLength(100)]
    public string ReaderType { get; set; } = string.Empty;
    
    [NotMapped]
    [MaxLength(255)]
    public string UserEmail { get; set; } = string.Empty;
    
    [NotMapped]
    public int Vote { get; set; }
    
    [NotMapped]
    [MaxLength(255)]
    public string ReviewTags { get; set; } = string.Empty;
    
    [NotMapped]
    public int StarOne { get; set; }
    
    [NotMapped]
    public int StarTwo { get; set; }
    
    [NotMapped]
    public int StarThree { get; set; }
    
    [NotMapped]
    public int StarFour { get; set; }
    
    [NotMapped]
    public int StarFive { get; set; }
    
    [NotMapped]
    [MaxLength(10)]
    public string RecommendThis { get; set; } = string.Empty;
    
    [NotMapped]
    [MaxLength(10)]
    public string ReaderSpoiler { get; set; } = string.Empty;
    
    [NotMapped]
    public int TotalRecord { get; set; }
    
    [NotMapped]
    public int CustomerProfileId { get; set; }

    // Relationships
    [NotMapped]
    public List<ProductReviewImage> ProductReviewImages { get; set; } = new();
    
    [NotMapped]
    public ReviewHelpFul ReviewHelpFul { get; set; } = new();

    [NotMapped]
    public List<ReviewTagName> ReviewTagNames { get; set; } = new();
}
