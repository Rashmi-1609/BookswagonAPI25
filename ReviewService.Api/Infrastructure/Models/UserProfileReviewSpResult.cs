namespace ReviewService.Api.Infrastructure.Models;

/// <summary>
/// This is a flat "Loading Dock" class. It exists solely to catch the raw output 
/// of SProc_GetUserProfileReviews_WithImages.
/// </summary>
public class UserProfileReviewSpResult
{
    // Pagination / Row Info
    public long RowNum { get; set; }

    // User Profile Data
    public string? User_Name { get; set; }
    public string? Email_Id { get; set; }
    public int Id_CustProfile { get; set; }
    public int CustReaderType { get; set; }
    public string? ReaderLocation { get; set; }
    public int TotalReview { get; set; }

    // Review Data
    public int ID_ProductReview { get; set; }
    public int Rating { get; set; }
    public string? ReaderType { get; set; }
    public DateTime Date_Created { get; set; }
    public string? Review_Title { get; set; }
    public string? Description { get; set; }
    public string? ReaderSpoilers { get; set; }

    // Product Data
    public string? ISBN13 { get; set; }
    public string? Product_Title { get; set; }
    public string? Product_TitleUrl { get; set; }
    public string? Image_Location { get; set; }

    // The raw JSON string containing the images from the FOR JSON PATH subquery
    public string? ReviewImagesJson { get; set; }
}
