using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace AuthorService.Application.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing an author.
    /// </summary>
    public class AuthorDto
    {
        public long AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? InvertedName { get; set; }
        public string? AuthorDetail { get; set; }
        public string? AuthorImage { get; set; }
        public string? PageTitle { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Initializes a new instance of the AuthorDto class.
        /// </summary>
        public AuthorDto(long authorId, string name, string? firstName, string? lastName, string? invertedName, string? authorDetail, string? authorImage, string? pageTitle, string? metaKeywords, string? metaDescription)
        {
            AuthorId = authorId;
            AuthorName = name;
            FirstName = firstName;
            LastName = lastName;
            InvertedName = invertedName;
            AuthorDetail = authorDetail;
            AuthorImage = authorImage;
            PageTitle = pageTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
        }
    }
}
