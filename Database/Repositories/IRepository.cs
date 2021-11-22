using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Database.Entities;
using PFM.Models;
using System.Linq;


namespace PFM.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByCode(int catcode);     
        Task<T> GetById(string id);     
        Task<ICollection<T>> List();
        Task AddRange(ICollection<T> collection);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(ICollection<T> collection);
        IQueryable<T> AsQueryable();
        Task<bool> SaveAll();
        IEnumerable<T> AsEnumerable();
    }
}