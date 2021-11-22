using System;
using CsvHelper.Configuration.Attributes;
using PFM.Database.Entities;

namespace PFM.Models

{

    public class Transaction {

        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        public string Date { get; set; }

        public string Direction { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public int? mcc { get; set; }
      
        public string Currency { get; set; }

        public string Kind { get; set; }
        public MccCodes MccCode {get; set;}

        public string CategoryCode { get; set; }
        public bool isSplited { get; set; }

    }
}