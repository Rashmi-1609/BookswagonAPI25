namespace ProductService.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a reader profile type for reviews.
/// </summary>
public class ReviewReaderTypeDto
{
    public int ReaderTypeId { get; set; }
    public string ReaderType { get; set; } = string.Empty;
}
