using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Database.Repositories;
using PFM.Mappings;
using PFM.Models;
using TinyCsvParser;
using static PFM.Commands.QueryableExtensions;

namespace PFM.Services

{
    public class TransactionsService : ITransactionsService
    {
        private readonly IRepository<TransactionEntity> _transactionRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<SplitTransactions> _splitTransactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsService(IRepository<TransactionEntity> Repository,
                                     IMapper mapper, 
                                     IRepository<CategoryEntity> categoryRepository,
                                     IRepository<SplitTransactions> splitTransactionsRepository){
            _transactionRepository = Repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _splitTransactionsRepository = splitTransactionsRepository;
        }

        public async Task<bool> AddTransactions(HttpRequest request)
        {
            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, Encoding.UTF8);
            var parsedDataString = await reader.ReadToEndAsync().ConfigureAwait(false);

            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { "\n" });
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');

            TransactionsMappingModel csvMapper = new TransactionsMappingModel();

            CsvParser<TransactionEntity> csvParser = new CsvParser<TransactionEntity>(csvParserOptions, csvMapper);
            var result = csvParser
                         .ReadFromString(csvReaderOptions, parsedDataString)
                         .ToList();

            result.Remove(result[result.Count - 1]);

            List<TransactionEntity> list = new List<TransactionEntity>();
            for (int i = 3; i < result.Count; i++)
            {
                TransactionEntity dataForDb = new TransactionEntity
                {
                    Id = result[i].Result.Id,
                    BeneficiaryName = result[i].Result.BeneficiaryName,
                    Date = result[i].Result.Date,
                    Direction = result[i].Result.Direction,
                    Amount = result[i].Result.Amount,
                    Description = result[i].Result.Description,
                    Currency = result[i].Result.Currency,
                    Kind = result[i].Result.Kind,
                    mcc = result[i].Result.mcc
                };
                list.Add(dataForDb);
            }

          await _transactionRepository.AddRange(list);
          var resultFromRepo = await _transactionRepository.SaveAll();

            if (resultFromRepo)
            {
                // primer za return i poraka
                //await $"Created Transaction on server with name {dataForDb.Name}",
                //    "With method: AddTransactions ", user, ip, browser);
                return true;
            }

            return false;
        
        }

        public async Task<bool> SplitTransaction(string id, SplitTransactionCommand split)
        {
            var splits = split.splits;
            var transactionEntity = await _transactionRepository.GetById(id);
            var categoryQuery = _categoryRepository.AsQueryable();
            var splitTransactionsQuery = _splitTransactionsRepository.AsQueryable();

            if(transactionEntity == null)
                return false;

            if(transactionEntity.isSplited){
                double amountToGetBack = 0;
                splitTransactionsQuery = splitTransactionsQuery.Where(x => x.TransactionsId.Equals(transactionEntity.Id));
                var list = splitTransactionsQuery.ToList();
                
                foreach(var l in list){
                    amountToGetBack += l.Amount;
                }
                transactionEntity.Amount += amountToGetBack;
                _splitTransactionsRepository.DeleteRange(list);
                await _splitTransactionsRepository.SaveAll();
                transactionEntity.isSplited = false;
            }


            foreach(var s in splits){
                var categoryEntity = await _categoryRepository.GetById(s.catcode);
                if(categoryEntity != null)
                {
                    if(transactionEntity.Amount < s.Amount || transactionEntity.Amount <= 0){
                        throw new Exception();
                    }

                    SplitTransactions splitTransaction = new SplitTransactions {
                        Transactions  = transactionEntity,
                        Categories = categoryEntity,
                        CategoriesId = categoryEntity.Code,
                        TransactionsId = transactionEntity.Id,
                        Amount = s.Amount
                    };
                    await _splitTransactionsRepository.Add(splitTransaction);
                    transactionEntity.isSplited = true;
                    transactionEntity.Amount = transactionEntity.Amount - s.Amount;
                }
            }

            var status = await _splitTransactionsRepository.SaveAll();
            if(status)
                return true;
            return false;
        }


        public async Task<bool> CategorizeTransactions(string id, CategorizeTransaction catTrans)
        {
            var catcode = catTrans.catcode;
            var transactionEntity = await _transactionRepository.GetById(id);
            var categoryEntity = await _categoryRepository.GetById(catcode);
            
            if(categoryEntity != null && transactionEntity != null)
                {
                    if(transactionEntity.CategoryCode != null){
                        transactionEntity.CategoryCode = null;
                    }
                    transactionEntity.CategoryCode = catcode;
                }
            var result = await _transactionRepository.SaveAll();
            if(result)
                return true;

            return false;
        }

        public Task<ICollection<Transaction>> GetAllTransactions()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<TransactionEntity>> GetPagedListTransactions(QueryParams transactionsParams)
        {
            
            var query = _transactionRepository.AsQueryable();

            if(!string.IsNullOrEmpty(transactionsParams.Kind))
                query = query.Where(x => x.Kind.Equals(transactionsParams.Kind));

            if(!string.IsNullOrEmpty(transactionsParams.FromDate) && !string.IsNullOrEmpty(transactionsParams.ToDate)){
                var dateFrom = Convert.ToDateTime(transactionsParams.FromDate);
                var dateTo = Convert.ToDateTime(transactionsParams.ToDate);
                if (dateFrom <= dateTo) 
                    query = query.Where(x => (DateTime)(object)x.Date >= dateFrom && (DateTime)(object)x.Date <= dateTo);
            }
            
            if(!string.IsNullOrEmpty(transactionsParams.SortBy))
                {
                    query = query.OrderByDynamic(transactionsParams.SortBy, transactionsParams.SortOrder); 
                }
            return await PagedList<TransactionEntity>.ToPagedList(query, transactionsParams.PageNumber, transactionsParams.PageSize);      
        }
    }

}