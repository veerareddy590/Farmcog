using FarmOnline.Models;
using System.Linq.Expressions;

namespace FarmOnline.Repositories.IRepository
{
    public interface IOrderHeader:IRepository<OrderHeader>
    {
        void Update();
        IEnumerable<OrderHeader> Getorderheader(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null);

        IEnumerable<OrderHeader> GetAllWithDetails();

        Task<OrderHeader> GetFirstOrDefaultAsync(Expression<Func<OrderHeader, bool>> filter, string includeProperties = null);



    }
}
