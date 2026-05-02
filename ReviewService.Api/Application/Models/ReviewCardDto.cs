namespace ReviewService.Api.Application.Models;

public class ReviewCardDto
{
    public int Rating { get; set; }
    public string? ReviewBy { get; set; }
    public DateTime DateCreated { get; set; }
    public string? ReviewTitle { get; set; }
    public string? Description { get; set; }
    public bool? RecommendThis { get; set; }
}
