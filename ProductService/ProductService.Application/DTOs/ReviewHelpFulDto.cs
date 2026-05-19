namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing helpfulness and report statistics for a review.
/// </summary>
public class ReviewHelpFulDto
{
    public long ReviewHelpId { get; set; }
    public int Helpful { get; set; }
    public int NotHelpFul { get; set; }
    public int Reported { get; set; }
}
