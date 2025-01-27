using FarmOnline.Models;
using System.Linq.Expressions;

namespace FarmOnline.Repositories.IRepository
{
    public interface IProduct:IRepository<Product>
    {
        Product Get(Expression<Func<Product, bool>> filter, string? includeProperties = null, bool tracked = false);

        void Update(Product product);

        IEnumerable<Product> Getproducts(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null);
    }
}
