using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublisherService.Api.Domain.Entities;

// [Table] maps this class specifically to the legacy SQL table name.
[Table("Table_Publisher")]
public class Publisher
{

    // [Key] tells EF Core this is the primary key.
    // [Column] maps our standard C# property to the legacy DB column name.
    [Key]
    [Column("ID_Publisher")]
    public int PublisherId { get; set; }

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
