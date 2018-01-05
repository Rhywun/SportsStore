using System.Linq;
using SportsStore.Models;

namespace SportsStore.Data
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        void SaveOrder(Order order);
    }
}
