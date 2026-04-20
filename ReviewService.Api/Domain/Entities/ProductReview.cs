using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewService.Api.Domain.Entities;

// [Table] maps this class specifically to the legacy SQL table name.
[Table("Table_ProductReview")]
public class ProductReview
{
    // [Key] tells EF Core this is the primary key.
    // [Column] maps our standard C# property to the legacy DB column name.
    [Key]
    [Column("ID_ProductReview")]
    public int Id { get; set; }

    [Column("ID_Product")]
    public int ProductId { get; set; }

    [Column("RecomendThis")]
    public bool RecommendThis { get; set; }

    [Column("Status")]
    public int ReviewStatus { get; set; }

    [Column("Flag_Active")]
    public bool IsActive { get; set; }

    [Column("Flag_Delete")]
    public bool IsDeleted { get; set; }

    // 1. The REAL database column
    [Column("Date_Created")]
    public DateTime DateCreated { get; set; }

    // 2. The CALCULATED UI property
    [NotMapped]
    public string PostDate => $"{(DateTime.Now - DateCreated).Days} Days Ago";

    public int Rating { get; set; }
    public string ReviewBy { get; set; } = string.Empty;
    public string ReviewTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
