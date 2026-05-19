namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a review tag name.
/// </summary>
public class ReviewTagNameDto
{
    public int Id { get; set; }
    public string TagName { get; set; } = string.Empty;
}
