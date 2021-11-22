using System.ComponentModel.DataAnnotations;

namespace PFM.Database.Entities
{
    public class MccCodes
    {
        [Key]
        public int Code { get; set; }
        public string MerchanTtype { get; set; }
    }
}