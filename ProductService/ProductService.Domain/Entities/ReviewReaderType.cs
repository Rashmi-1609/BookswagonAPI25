using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities;

/// <summary>
/// Represents a reader type for a review.
/// </summary>
public class ReviewReaderType
{
    [Key]
    public int ReaderTypeId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string ReaderType { get; set; } = string.Empty;
}
