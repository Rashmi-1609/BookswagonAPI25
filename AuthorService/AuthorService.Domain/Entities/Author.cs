using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AuthorService.Domain.Entities
{
    /// <summary>
    /// Represents an author entity in the domain.
    /// </summary>
    [Table("Table_Author")]
    public class Author
    {
        [Column("ID_Author")]
        public long AuthorId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Column("Flag_TopAuthor")]
        public bool? FlagTopAuthor { get; set; }

        public int? DisplayOrder { get; set; }

        [Column("Flag_Active")]
        public bool? FlagActive { get; set; }

        [Column("Flag_Delete")]
        public bool? FlagDelete { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? InvertedName { get; set; }

        [Column("Flag_Approve")]
        public bool? FlagApprove { get; set; }

        public string? AuthorDetail { get; set; }
        public string? AuthorImage { get; set; }
        public string? PageTitle { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
    }

}
