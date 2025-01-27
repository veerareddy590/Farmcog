using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace FarmOnline.Repositories
{
    public class COrderHeader : CRepository<OrderHeader>, IOrderHeader
    {
        ApplicationDbContext db;
        internal DbSet<OrderHeader> dbset;
        public COrderHeader(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<OrderHeader>();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderHeader> Getorderheader(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OrderHeader> query = dbset;
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

        public IEnumerable<OrderHeader> GetAllWithDetails()
        {
            return dbset
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.productFarmers)
                .ToList();
        }

    }
}
