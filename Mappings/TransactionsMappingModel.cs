using System;
using PFM.Database.Entities;
using PFM.Models;
using TinyCsvParser.Mapping;



namespace PFM.Mappings
{
    public class TransactionsMappingModel : CsvMapping<TransactionEntity>
    {
        public TransactionsMappingModel()
        : base()
        {
            MapProperty(0, m => m.Id);
            MapProperty(1, m => m.BeneficiaryName);
            MapProperty(2, m => m.Date);
            MapProperty(3, m => m.Direction);
            MapProperty(4, m => m.Amount);
            MapProperty(5, m => m.Description);
            MapProperty(6, m => m.Currency);
            MapProperty(7, m => m.mcc);
            MapProperty(8, m => m.Kind);
        }
    }
}