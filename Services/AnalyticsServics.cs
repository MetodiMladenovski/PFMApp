using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Database.Repositories;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using PFM.Mappings;
using PFM.Models;
using TinyCsvParser;
using static PFM.Commands.QueryableExtensions;


namespace PFM.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IRepository<TransactionEntity> _transactionRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<SplitTransactions> _splitTransactionsRepository;
        private readonly IMapper _mapper;

        public AnalyticsService(IRepository<TransactionEntity> Repository,
                                     IMapper mapper, 
                                     IRepository<CategoryEntity> categoryRepository,
                                     IRepository<SplitTransactions> splitTransactionsRepository){
            _transactionRepository = Repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _splitTransactionsRepository = splitTransactionsRepository;
        }

        public ICollection<AnalyticsModel> GetAnalytics(string catcode, string startDate, string endDate, string direction)
        {
            var splitsQuery = _splitTransactionsRepository.AsQueryable();
            var transactionsQuery = _transactionRepository.AsQueryable();
            List<AnalyticsModel> analytics = new List<AnalyticsModel>();

            if(!string.IsNullOrEmpty(catcode)){
                splitsQuery = splitsQuery.Where(x => x.CategoriesId.Equals(catcode));
                transactionsQuery = transactionsQuery.Where(x => x.CategoryCode.Equals(catcode));
            }
            else if(!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate)){
                var dateFrom = Convert.ToDateTime(startDate);
                var dateTo = Convert.ToDateTime(endDate);
                if (dateFrom <= dateTo) 
                    splitsQuery = splitsQuery.Where(x => (DateTime)(object)x.Transactions.Date >= dateFrom && (DateTime)(object)x.Transactions.Date <= dateTo);
                    transactionsQuery = transactionsQuery.Where(x => (DateTime)(object)x.Date >= dateFrom && (DateTime)(object)x.Date <= dateTo);
            }
            else if(!string.IsNullOrEmpty(direction)){
                splitsQuery.Where(x => x.Transactions.Direction.Contains(direction));
                transactionsQuery.Where(x => x.Direction.Contains(direction));
            }
            foreach(var obj in transactionsQuery){
                if(string.IsNullOrEmpty(obj.CategoryCode)){
                    continue;
                }
                var checkIfExists = analytics.Where(x => x.catcode.Equals(obj.CategoryCode));
                if(checkIfExists.Count() != 0){
                   analytics.Where(x => x.catcode.Equals(obj.CategoryCode)).First().Amount += obj.Amount;
                   analytics.Where(x => x.catcode.Equals(obj.CategoryCode)).First().Count++;
                }
                else{
                    AnalyticsModel mAdd = new AnalyticsModel{
                    catcode = obj.CategoryCode,
                    Amount = obj.Amount,
                    Count = 1
                };
                    analytics.Add(mAdd);
                }
            }

            foreach(var obj in splitsQuery){
                var checkIfExists = analytics.Where(x => x.catcode.Equals(obj.CategoriesId));
                if(checkIfExists.Count()!=0){
                   analytics.Where(x => x.catcode.Equals(obj.CategoriesId)).First().Amount += obj.Amount;
                   analytics.Where(x => x.catcode.Equals(obj.CategoriesId)).First().Count++;
                }else{
                    AnalyticsModel mAdd = new AnalyticsModel{
                    catcode = obj.CategoriesId,
                    Amount = obj.Amount,
                    Count = 1
                };
                    analytics.Add(mAdd);
                }
            }
            
            return analytics;
        }
    }
}