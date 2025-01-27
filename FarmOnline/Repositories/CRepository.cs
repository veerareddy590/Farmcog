using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FarmOnline.Repositories
{
    public class CRepository<T> : IRepository<T> where T : class
    {
        public ApplicationDbContext db;
        internal DbSet<T> dbset;
        public CRepository(ApplicationDbContext db)
        {
            this.db = db;
            this.dbset = db.Set<T>();
        }

        public void Add(T entity)
        {
            
            dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public List<T> GetAll()
        {
            return dbset.ToList();
        }

        public T search(T entity)
        {
            return dbset.Find(entity);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return dbset.FirstOrDefault(filter);
        }

        
    }
}
