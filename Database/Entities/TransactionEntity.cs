using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PFM.Models;

namespace PFM.Database.Entities

{
    public class TransactionEntity
    { 
        [Key]
        public string Id { get; set; }
        [MaxLength(255)]
        public string BeneficiaryName { get; set; }
        [Required]  
        public string Date { get; set; }
        [Required]
        public string Direction { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public double Amount { get; set; }
        [ForeignKey("MccCodes")]
        public int? mcc { get; set; }
        [Required, MaxLength(3), MinLength(3)]
        public string Currency { get; set; }
        [Required]
        public string Kind { get; set; }
        public MccCodes MccCode {get; set;}
        public string CategoryCode { get; set; }
        public bool isSplited { get; set; }

    }
}