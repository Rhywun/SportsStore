using System.Linq;
using SportsStore.Models;

namespace SportsStore.Data
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}
