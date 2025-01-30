using FarmOnline.Models;

namespace FarmOnline.Repositories.IRepository
{
    public interface IUser :  IRepository<ApplicationUser>
    {
        ApplicationUser searchLogin(String entity);
    }
}
