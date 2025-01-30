using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Repositories
{
    public class CUser : CRepository<ApplicationUser>, IUser
    {
        ApplicationDbContext db;
        public CUser(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public ApplicationUser searchLogin(string entity)
        {
            DbSet<ApplicationUser> users = db.applicationUsers;
            return users.Find(entity);
        }
    }
}
