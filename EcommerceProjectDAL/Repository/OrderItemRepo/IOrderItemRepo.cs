using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.OrderItemRepo
{
    public interface IOrderItemRepo
    {
        //Task<IEnumerable<Data.Models.OrderItem>> GetAllOrderItem();
        Task<Data.Models.OrderItem> GetOrderItemById(int id, bool includeProduct = false);//
        Task<IEnumerable<Data.Models.OrderItem>> GetAllOrderItemByOrderId(int OrderId);
        Task AddOrderItem(Data.Models.OrderItem orderItem);
        Task UpdateOrderItem(Data.Models.OrderItem orderItem);
        Task<bool> DeleteOrderItem(int id);
        Task<bool> ExistsAsync(int id);//
        Task SaveChanges();

    }
}
