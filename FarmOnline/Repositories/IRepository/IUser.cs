using FarmOnline.Models;

namespace FarmOnline.Repositories.IRepository
{
    public interface IUser :  IRepository<User>
    {
        User searchLogin(String entity);
    }
}
