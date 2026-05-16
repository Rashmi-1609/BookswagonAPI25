using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities;

/// <summary>
/// Represents helpfulness votes for a review.
/// </summary>
public class ReviewHelpFul
{
    [Key]
    public long ReviewHelpId { get; set; }
    
    public int Helpful { get; set; }
    
    public int NotHelpFul { get; set; }
    
    public int Reported { get; set; }
}
