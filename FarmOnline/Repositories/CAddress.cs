using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Repositories
{
    public class CAddress : CRepository<Address>, IAddress
    {
        public ApplicationDbContext db { get; set; }
        internal DbSet<Address> dbset;
        public CAddress(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            this.dbset = db.Set<Address>();
        }

        public void update(Address address)
        {
            dbset.Update(address);
        }
    }
}
