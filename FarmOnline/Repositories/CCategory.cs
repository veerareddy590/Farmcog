
using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Repositories
{
    public class CCategory : CRepository<Category>, ICategory   
    {
        ApplicationDbContext db;
        internal DbSet<Category> dbSet;
        public CCategory(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            dbSet = db.Set<Category>();

        }

        public bool update()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> Getall()
        {
            return dbSet.ToList();
        }
    }
}
