using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Database.Entities;
using PFM.Database.Repositories;
using PFM.Mappings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace PFM.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<CategoryEntity> _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesService(IRepository<CategoryEntity> categoriesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoriesRepository = categoriesRepository;
        }
        public async Task<bool> AddCategories(HttpRequest request)
        {
            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, Encoding.UTF8);
            var parsedDataString = await reader.ReadToEndAsync().ConfigureAwait(false);

            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { "\n" });
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');

            CategoriesMappingModel csvMapper = new CategoriesMappingModel();

            CsvParser<CategoryEntity> csvParser = new CsvParser<CategoryEntity>(csvParserOptions, csvMapper);
            var result = csvParser
                         .ReadFromString(csvReaderOptions, parsedDataString)
                         .ToList();

            result.Remove(result[result.Count - 1]);

            List<CategoryEntity> list = new List<CategoryEntity>();
            for (int i = 3; i < result.Count; i++)
            {
                CategoryEntity dataForDb = new CategoryEntity
                {
                    Code = result[i].Result.Code,
                    ParentCode = result[i].Result.ParentCode,
                    Name = result[i].Result.Name
                };
                list.Add(dataForDb);
            }

            await _categoriesRepository.AddRange(list);
            var resultFromRepo = await _categoriesRepository.SaveAll();

            if (resultFromRepo)
            {
                // primer za return i poraka
                //await $"Created Categories on server with name {dataForDb.Name}",
                //    "With method: AddCategories ", user, ip, browser);
                return true;
            }

            return false;
        }

        public List<CategoryEntity> GetCategories(string parentId)
        {
            var query = _categoriesRepository.AsQueryable();
            if(!string.IsNullOrEmpty(parentId)){
                query =  query.Where(x => x.ParentCode.Contains(parentId));
            }
            query = query.OrderBy(x => x.Code);
            return query.ToList();
        }
    }
}

