using System.ComponentModel.DataAnnotations;

namespace PFM.Database.Entities
{
    public class CategoryEntity
    {
        [Key]
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }
    }
}