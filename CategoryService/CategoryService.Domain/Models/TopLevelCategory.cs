using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CategoryService.Domain.Models
{
    [Keyless]
    public class TopLevelCategory
    {
        public int Level1_ID { get; set; }
        public string? Category_DisplayName { get; set; }
        public int ID_CategoryMapping { get; set; }
        public string? CategoryUrl { get; set; }
    }
}
