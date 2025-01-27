using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Repositories
{
    public class CUser : CRepository<User>, IUser
    {
        ApplicationDbContext db;
        public CUser(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public User searchLogin(string entity)
        {
            DbSet<User> users = db.user;
            return users.Find(entity);
        }
    }
}
