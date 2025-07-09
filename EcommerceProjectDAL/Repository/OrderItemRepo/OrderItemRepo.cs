using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.OrderItemRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.OrderItem
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;
        public OrderItemRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }

        //public async Task<IEnumerable<Data.Models.OrderItem>> GetAllOrderItem()
        //{
        //    return await _ecommerceProject.orderItems.AsNoTracking().ToListAsync();
        //}
        public async Task<IEnumerable<Data.Models.OrderItem>> GetAllOrderItemByOrderId(int OrderId)
        {

            var orderItem =await _ecommerceProject.orders.FindAsync(OrderId);
            if (orderItem!=null)
            {
                return await _ecommerceProject.orderItems.Where(p => p.OrderId==OrderId).Include(o=>o.Product).AsNoTracking().ToListAsync();

            }
            return Enumerable.Empty<Data.Models.OrderItem>();
        }
        public async Task<Data.Models.OrderItem> GetOrderItemById(int id, bool includeProduct = false)
        {
            // return await _ecommerceProject.orderItems.Include(o=>o.Product).FirstOrDefaultAsync(a => a.Id ==id);
            if (includeProduct)
            {
                return await _ecommerceProject.orderItems
                    .Include(oi => oi.Product)
                    .FirstOrDefaultAsync(oi => oi.Id == id);
            }
            return await _ecommerceProject.orderItems.FindAsync(id);
        }
        public async Task AddOrderItem(Data.Models.OrderItem orderItem)
        {
            await _ecommerceProject.orderItems.AddAsync(orderItem);
        }
        public async Task<bool> DeleteOrderItem(int id)
        {
            var orderIems = await _ecommerceProject.orderItems.FindAsync(id);
            if(orderIems !=null)
            {
                orderIems.IsDeleted = true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task UpdateOrderItem(Data.Models.OrderItem orderItem)
        {
          
        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _ecommerceProject.orderItems.AnyAsync(oi => oi.Id == id && !oi.IsDeleted);
        }


    }
}
