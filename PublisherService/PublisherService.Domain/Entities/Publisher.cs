using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PublisherService.Domain.Entities;

/// <summary>
/// Represents a book publisher in the Bookswagon ecosystem.
/// </summary>
 
[Table("Table_Publisher")]
public class Publisher
{
    [Key]
    [Column("ID_Publisher")]
    public int PublisherId { get; set; }

    [Required]
    [Column("Company_Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Column("PublisherImage")]
    public string? PublisherImage { get; set; }

    [Column("Description")]
    public string? Description { get; set; }

    [Column("PageTitle")]
    public string? PageTitle { get; set; }

    [Column("MetaDescription")]
    public string? MetaDescription { get; set; }

    [Column("MetaKeywords")]
    public string? MetaKeywords { get; set; }
}
