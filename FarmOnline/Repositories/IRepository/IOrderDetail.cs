using FarmOnline.Models;
using System.Linq.Expressions;

namespace FarmOnline.Repositories.IRepository
{
    public interface IOrderDetail:IRepository<OrderDetail>
    {
        void Update();

        IEnumerable<OrderDetail> Getorders(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null);
    }
}
