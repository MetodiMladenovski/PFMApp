using PFM.Database.Entities;
using TinyCsvParser.Mapping;



namespace PFM.Mappings
{
    public class CategoriesMappingModel : CsvMapping<CategoryEntity>
    {
        public CategoriesMappingModel()
        : base()
        {
            MapProperty(0, m => m.Code);
            MapProperty(1, m => m.ParentCode);
            MapProperty(2, m => m.Name);
        }
    }
}