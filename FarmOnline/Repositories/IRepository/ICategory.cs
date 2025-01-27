
using FarmOnline.Models;

namespace FarmOnline.Repositories.IRepository
{
    public interface ICategory:IRepository<Category>
    {
        bool update();

        IEnumerable<Category> Getall();
    }
}
