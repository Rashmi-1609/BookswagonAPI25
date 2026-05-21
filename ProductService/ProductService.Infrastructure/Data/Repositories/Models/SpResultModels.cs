using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Infrastructure.Data.Repositories.Models;

/// <summary>
/// Maps the result of the product rating count stored procedure.
/// </summary>
internal class ProductRatingCountSpResult
{
    public int? RatingCount { get; set; }
    public int? Rating { get; set; }
}

/// <summary>
/// Maps the detailed product review information from the stored procedure.
/// </summary>
internal class ProductReviewDetailSpResult
{
    [Column("ID_ProductReview")] public int ProductReviewId { get; set; }
    [Column("TotalRecord")] public int? TotalRecord { get; set; }
    [Column("User_Name")] public string? ReviewBy { get; set; }
    [Column("Email_Id")] public string? UserEmail { get; set; }
    [Column("ReaderType")] public string? ReaderType { get; set; }
    [Column("ReaderSpoilers")] public string? ReaderSpoiler { get; set; }
    [Column("TotalReview")] public int? TotalReview { get; set; }
    [Column("Vote")] public int? Vote { get; set; }
    [Column("ReviewTag_Name")] public string? ReviewTags { get; set; }
    [Column("Review_Title")] public string? ReviewTitle { get; set; }
    [Column("Description")] public string? Description { get; set; }
    [Column("Rating")] public int? Rating { get; set; }
    [Column("StarFive")] public int? StarFive { get; set; }
    [Column("StarFour")] public int? StarFour { get; set; }
    [Column("Id_CustProfile")] public int CustomerProfileId { get; set; }
    [Column("RecomendThis")] public string? RecommendThis { get; set; }
    [Column("Date_Created")] public DateTime? DateCreated { get; set; }
}

/// <summary>
/// Maps the user's specific profile reviews from the stored procedure.
/// </summary>
internal class UserProfileReviewSpResult
{
    [Column("ID_ProductReview")] public int ProductReviewId { get; set; }
    [Column("TotalReview")] public int? TotalRecord { get; set; }
    [Column("User_Name")] public string? ReviewBy { get; set; }
    [Column("Review_Title")] public string? ReviewTitle { get; set; }
    [Column("Description")] public string? Description { get; set; }
    [Column("Rating")] public int? Rating { get; set; }
    [Column("Date_Created")] public DateTime? DateCreated { get; set; }
    [Column("Email_Id")] public string? UserEmail { get; set; }
}

/// <summary>
/// Maps the helpfulness and reporting statistics for a review from the stored procedure.
/// </summary>
internal class ReviewHelpFulSpResult
{
    public int? Helpful { get; set; }
    public int? NotHelpFul { get; set; }
    public int? Reported { get; set; }
}

/// <summary>
/// Maps all reader types from the SP_GetReviewReaderType stored procedure.
/// </summary>
internal class ReviewReaderTypeSpResult
{
    [Column("Id")] public int? ReaderTypeId { get; set; }
    [Column("ReaderType")] public string? ReaderType { get; set; }
}

/// <summary>
/// Maps the product-specific reader types from the SProc_GetReviewReaderType stored procedure.
/// </summary>
internal class ProductReviewReaderTypeSpResult
{
    [Column("ReviewID")] public int? ReaderTypeId { get; set; }
    [Column("ReaderType")] public string? ReaderType { get; set; }
}

/// <summary>
/// Maps the review tags from the stored procedure.
/// </summary>
internal class ReviewTagNameSpResult
{
    [Column("Id_Tag")] public int? Id { get; set; }
    [Column("ReviewTag_Name")] public string? TagName { get; set; }
}

/// <summary>
/// Maps the images associated with a product review from the stored procedure.
/// </summary>
internal class ProductReviewImageSpResult
{
    [Column("ID_ProductReview")] public int? ProductReviewId { get; set; }
    [Column("Image_Location")] public string? ImageLocation { get; set; }
    [Column("ImgCaption")] public string? ImageCaption { get; set; }
}
