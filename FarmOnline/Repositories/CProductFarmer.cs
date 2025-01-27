using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Repositories
{
    public class CProductFarmer : CRepository<ProductFarmer>, IProductFarmer
    {
        ApplicationDbContext db;
        internal DbSet<ProductFarmer> dbset;
        public CProductFarmer(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<ProductFarmer>();
            
        }

        public void Update(ProductFarmer productFarmer)
        {
            dbset.Update(productFarmer);
        }
    }
}

