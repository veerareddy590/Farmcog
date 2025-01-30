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

        public IEnumerable<T> GatAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            //IQueryable<T> query = dbSet.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

    }
}
