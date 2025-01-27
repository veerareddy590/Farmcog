using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace FarmOnline.Repositories
{
    public class CCart : CRepository<Cart>, ICart
    {
        public ApplicationDbContext db { get; set; }
        internal DbSet<Cart> dbset;
        public CCart(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<Cart>();
        }

        public void update()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cart> Getcart(Expression<Func<Cart, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<Cart> query = (IQueryable<Cart>)dbset;
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

        public void RemoveRange(IEnumerable<Cart> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
