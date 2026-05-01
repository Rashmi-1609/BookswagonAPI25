using System.Text.Json.Serialization;

namespace ReviewService.Api.Application.Models;

// 1. The Parent "Folder" (The User Profile)
public class UserProfileReviewDto
{
    public string? ReviewBy { get; set; }
    public string? UserEmail { get; set; }
    public int TotalReview { get; set; }
    public string? ReaderType { get; set; }

    // Nested array of reviewed books
    public List<ReviewedBookDto> ReviewedBooks { get; set; } = new();
}

// 2. The Individual Book Review 
public class ReviewedBookDto
{
    public string? ISBN13 { get; set; }
    public string? ProductTitle { get; set; }
    public string? ProductTitleUrl { get; set; }
    public string? ProductImageLocation { get; set; }

    public int Rating { get; set; }
    public string? DateCreated { get; set; } // We will format this nicely in the Repo (e.g., "Oct 12, 2023")
    public string? ReaderSpoiler { get; set; }
    public string? ReviewTitle { get; set; }
    public string? Description { get; set; }

    // Nested array of images
    public List<ProductReviewImageDto> ProductReviewImages { get; set; } = new();
}

// 3. The Uploaded Images of Reviewed Book (if any)
public class ProductReviewImageDto
{
    [JsonPropertyName("ImgCaption")]
    public string? ImageCaption { get; set; }

    [JsonPropertyName("Image_Location")]
    public string? ImageLocation { get; set; }
}
