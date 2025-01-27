using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;

namespace FarmOnline.Repositories
{
    public class CUnitofWork : IUnitofWork
    {
        public IUser user { get; set; }
        public ICategory category { get; set; }
        public IProduct product { get; set; }

        public IProductFarmer  productFarmer { get; set; }

        public ICart Cart { get; set; }

        public IAddress address { get; set; }

        public IOrderHeader orderHeader { get; set; }

        public IOrderDetail orderDetail { get; set; }


        //public User IUser => throw new NotImplementedException();

        public ApplicationDbContext _db;

        public CUnitofWork(ApplicationDbContext db) { 

            _db = db;
            user = new CUser(db);
            category = new CCategory(db);
            product = new CProduct(db);
            Cart = new CCart(db);
            productFarmer = new CProductFarmer(db);
            address = new CAddress(db);
            orderHeader = new COrderHeader(db);
            orderDetail = new COrderDetail(db);
        
        }
        public int save()
        {
           return _db.SaveChanges();
        }
    }
}

