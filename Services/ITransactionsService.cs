using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Mappings;
using PFM.Models;

namespace PFM.Services

{
    public interface ITransactionsService 
    {
        Task<PagedList<TransactionEntity>> GetPagedListTransactions(QueryParams transactionsParams);
        Task<bool> AddTransactions(HttpRequest request);
        // Task<bool> DeleteTransactions(string id);
        // Task<bool> UpdateTransactions(Transaction transaction);
        Task<ICollection<Transaction>> GetAllTransactions();
        Task<bool> CategorizeTransactions(string id, CategorizeTransaction catTrans);
        Task<bool> SplitTransaction(string id, SplitTransactionCommand split);
    }
}