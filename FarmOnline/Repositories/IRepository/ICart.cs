using FarmOnline.Models;
using System.Linq.Expressions;

namespace FarmOnline.Repositories.IRepository
{
    public interface ICart:IRepository<Cart>
    {
        void update();
        IEnumerable<Cart> Getcart(Expression<Func<Cart, bool>>? filter = null, string? includeProperties = null);

        void RemoveRange(IEnumerable<Cart> entities);
    }


}
