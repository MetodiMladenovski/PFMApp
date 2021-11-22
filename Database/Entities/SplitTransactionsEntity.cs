using System.ComponentModel.DataAnnotations;

namespace PFM.Database.Entities

{
    public class SplitTransactions
    {
        [Key]
        public int Id { get; set; }
        public double Amount { get; set; }
        [Required]
        public string CategoriesId { get; set; }
        public CategoryEntity Categories { get; set; }
        [Required]
        public string TransactionsId { get; set; }
        public TransactionEntity Transactions { get; set; }

    }
}