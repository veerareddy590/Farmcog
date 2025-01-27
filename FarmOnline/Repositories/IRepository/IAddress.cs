using FarmOnline.Models;

namespace FarmOnline.Repositories.IRepository
{
    public interface IAddress:IRepository<Address>
    {
        void update(Address address);
    }
}
