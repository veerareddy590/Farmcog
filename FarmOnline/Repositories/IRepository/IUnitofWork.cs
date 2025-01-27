using FarmOnline.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmOnline.Repositories.IRepository
{
    public interface IUnitofWork
    {
        IUser user { get; set; }
        IProduct product { get; set; }

        ICategory category { get; set; }

        ICart Cart { get; set; }

        IProductFarmer productFarmer { get; set; }

        IAddress address { get; set; }

        IOrderHeader orderHeader { get; set; }

        IOrderDetail orderDetail { get; set; }

        int save();

        
    }
}
