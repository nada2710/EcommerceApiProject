using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.OrderRepo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;

        public OrderRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }
        public async Task<Order> GetOrderId(int id, bool includeItems = false)
        {
            if (includeItems)
            {
                return await _ecommerceProject.orders
                    .Include(o => o.OrderItems)  
                    .ThenInclude(oi => oi.Product) 
                    .Include(o => o.Customer)  
                    .FirstOrDefaultAsync(o => o.Id == id);
            }

            return await _ecommerceProject.orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _ecommerceProject.orders.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllOrderByStatus(Status status)
        {
            var orders = await _ecommerceProject.orders
                            .Where(a => a.Status == status)
                            .AsNoTracking()
                            .ToListAsync();
            return orders;
        }
        //public async Task<Order> GetOrdersById(int id)
        //{
        //    return await _ecommerceProject.orders.FirstOrDefaultAsync(a => a.Id==id);
        //}
        public async Task AddOrders(Order order)
        {
            await _ecommerceProject.orders.AddAsync(order);
        }
        public async Task<bool> DeleteOrders(int id)
        {
            var orders = await _ecommerceProject.orders.FindAsync(id);
            if(orders !=null)
            {
                orders.IsDeleted=true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task UpdateOrders(Order order)
        {
            
        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }

       
    }
}
