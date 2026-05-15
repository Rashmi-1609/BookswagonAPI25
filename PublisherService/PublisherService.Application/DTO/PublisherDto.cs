namespace PublisherService.Application.DTO;

/// <summary>
/// A safe, frontend-facing representation of a Publisher, stripping away internal database fields.
/// </summary>
public class PublisherDto
{
    public int PublisherId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? PublisherImage { get; set; }
    public string? Description { get; set; }
    public string? MetaKeywords { get; set; }
    public string? MetaDescription { get; set; }
    public string? PageTitle { get; set; }
}
