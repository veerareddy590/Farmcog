using FarmOnline.Models;
using System.Linq.Expressions;

namespace FarmOnline.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        List<T> GetAll();

        T search(T entity);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
    }
}
