using CsvHelper.Configuration.Attributes;

namespace PFM.Models

{
    public class Category {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
    }
}