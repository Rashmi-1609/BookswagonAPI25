using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing detailed product review data.
/// </summary>
public class ProductReviewDto
{
    [Column("ID_ProductReview")]
    public int ProductReviewId { get; set; }
    
    [Column("ID_Product")]
    public int ProductId { get; set; }
    
    [Column("Review_Title")]
    public string ReviewTitle { get; set; } = string.Empty;
    
    [Column("User_Name")] // SProc_GetReview uses User_Name, others use ReviewBy or User_Name
    public string ReviewBy { get; set; } = string.Empty;
    
    [Column("Description")]
    public string Description { get; set; } = string.Empty;
    
    [Column("Status")]
    public int ReviewStatus { get; set; }
    
    [Column("Rating")]
    public int Rating { get; set; }
    
    [NotMapped] // Populated manually
    public int AvgRating { get; set; }
    
    [Column("TotalReview")]
    public int TotalReview { get; set; }
    
    [Column("Date_Created")]
    public DateTime DateCreated { get; set; } // Changed from PostDate string to raw DateTime for frontend
    
    [Column("RatingCount")]
    public int RatingCount { get; set; }
    
    [Column("ReaderType")]
    public string ReaderType { get; set; } = string.Empty;
    
    [Column("Email_Id")]
    public string UserEmail { get; set; } = string.Empty;
    
    [Column("Vote")]
    public int Vote { get; set; }
    
    [Column("ReviewTag_Name")]
    public string ReviewTags { get; set; } = string.Empty;
    
    [Column("StarOne")]
    public int StarOne { get; set; }
    
    [Column("StarTwo")]
    public int StarTwo { get; set; }
    
    [Column("StarThree")]
    public int StarThree { get; set; }
    
    [Column("StarFour")]
    public int StarFour { get; set; }
    
    [Column("StarFive")]
    public int StarFive { get; set; }
    
    [Column("RecomendThis")]
    public string RecommendThis { get; set; } = string.Empty;
    
    [Column("ReaderSpoilers")]
    public string ReaderSpoiler { get; set; } = string.Empty;
    
    [Column("TotalRecord")]
    public int TotalRecord { get; set; }
    
    [Column("Id_CustProfile")]
    public int CustomerProfileId { get; set; }
    
    [NotMapped]
    public List<ProductReviewImageDto> ProductReviewImages { get; set; } = new();
    
    [NotMapped]
    public ReviewHelpFulDto ReviewHelpFul { get; set; } = new();
}
