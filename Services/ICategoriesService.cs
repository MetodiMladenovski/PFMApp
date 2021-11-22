using Microsoft.AspNetCore.Http;
using PFM.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services
{
    public interface ICategoriesService
    {
        Task<bool> AddCategories(HttpRequest request);
        List<CategoryEntity> GetCategories(string parentId);
    }
}
