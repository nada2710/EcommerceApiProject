using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.OrderRepo
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetAllOrders();
       // Task<Order> GetOrdersById(int id);
        Task<Order> GetOrderId(int id, bool includeItems = false);
        Task<IEnumerable<Order>> GetAllOrderByStatus(Status status);
        Task AddOrders(Order order);
        Task UpdateOrders(Order order);
        Task<bool> DeleteOrders(int id);

        Task SaveChanges();

    }
}
