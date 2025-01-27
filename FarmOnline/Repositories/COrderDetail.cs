using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace FarmOnline.Repositories
{
    public class COrderDetail : CRepository<OrderDetail>, IOrderDetail
    {
        ApplicationDbContext db;
        internal DbSet<OrderDetail> dbset;
        public COrderDetail(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<OrderDetail>();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

       

        public IEnumerable<OrderDetail> Getorders(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OrderDetail> query = dbset;
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
