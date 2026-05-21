using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CategoryService.Domain.Entities
{

    [Table("Table_CategoryTree")]
    public class CategoryTree
    {
        [Key]
        [Column("ID_CategoryTree")]
        public int CategoryTreeId { get; set; }

        [Column("ID_CategoryMapping")]
        public int CategoryMappingId { get; set; }

        [Column("ID_ParentCategory")]
        public int ParentCategoryId { get; set; }

        [Column("ID_ChildCategory")]
        public int ChildCategoryId { get; set; }

        [Column("Child_Name")]
        public string ChildName { get; set; }

        [Column("Child_URL")]
        public string? ChildUrl { get; set; }

        [Column("Level1_ID")]
        public int LevelId1 { get; set; }

        [Column("Level2_ID")]
        public int LevelId2 { get; set; }

        [Column("Level3_ID")]
        public int LevelId3 { get; set; }

        [Column("Level4_ID")]
        public int LevelId4 { get; set; }

        [Column("CategoryLevel")]
        public int CategoryLevel { get; set; }
    }



    [Table("Table_Category")] 
    public class Category
    {
        [Key]
        [Column("Id_Category")]
        public int CategoryId { get; set; }

        [Column("ThemaCode")]
        public string ThemaCode { get; set; }

        [Column("Category_Name")]
        public string CategoryName { get; set; }

        [Column("CategoryDescription")]

        public string CategoryDescription { get; set; }

        [Column("Category_DisplayName")]
        public string CategoryDisplayName { get; set; }

        [Column("CategoryUrl")]
        public string CategoryUrl { get; set; }

        [Column("Flag_Menu")]
        public bool Flag_Menu { get; set; }

        [Column("IsActive")]
        public bool FlagActive { get; set; }
    }
    

    /*
    //layered api 


    [Microsoft.EntityFrameworkCore.Keyless]
    public class GroupedMenuResult
    {
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string MenuName { get; set; }
        public string MenuLink { get; set; }
        public int MenuPosition { get; set; }
    }

    [Microsoft.EntityFrameworkCore.Keyless]
    public class TopCategoryResult
    {
        //public string SectionName { get; set; }
        public string Category_DisplayName { get; set; }
        public string CategoryUrl { get; set; }
    }

    [Microsoft.EntityFrameworkCore.Keyless]
    public class TopAuthorResult
    {
        public string Name { get; set; }
    }  
    */
}