using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PfmDbContext _dbContex;
        private readonly DbSet<T> entities;

        public Repository(PfmDbContext dbContex)
        {
            _dbContex = dbContex;
            entities = _dbContex.Set<T>();
        }
        public async Task Add(T entity)
        {
            await entities.AddAsync(entity);
        }

        public async Task AddRange(ICollection<T> collection)
        {
            await entities.AddRangeAsync(collection);
        }

        public IEnumerable<T> AsEnumerable()
        {
            return entities.AsEnumerable();
        }

        public IQueryable<T> AsQueryable()
        {
            return entities.AsQueryable<T>();
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
        }
        public void DeleteRange(ICollection<T> collection)
        {
            entities.RemoveRange(collection);
        }


        public async Task<T> GetByCode(int catcode)
        {
            return await entities.FindAsync(catcode);
        }

        public async Task<T> GetById(string id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<ICollection<T>> List()
        {
            return await entities.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _dbContex.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            entities.Attach(entity).State = EntityState.Modified;
        }
    }
}