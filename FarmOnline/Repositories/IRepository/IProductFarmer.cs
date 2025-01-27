using FarmOnline.Models;

namespace FarmOnline.Repositories.IRepository
{
    public interface IProductFarmer:IRepository<ProductFarmer>
    {
        void Update(ProductFarmer productFarmer);
    }
}
