using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities;

/// <summary>
/// Represents a tag name for a review.
/// </summary>
public class ReviewTagName
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string TagName { get; set; } = string.Empty;
}
