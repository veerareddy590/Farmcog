using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FarmOnline.Repositories
{
    public class CProduct : CRepository<Product>, IProduct
    {
        ApplicationDbContext db;
        internal DbSet<Product> dbset;
        public CProduct(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<Product>();
        }

        public Product Get(Expression<Func<Product, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Product> query;
            if (tracked)
            {
                query = dbset;

            }
            else
            {
                query = dbset.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public void Update(Product product) {
            dbset.Update(product);
        }

        public IEnumerable<Product> Getproducts(Expression<Func<Product, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<Product> query = dbset;
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
