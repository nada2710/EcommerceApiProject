using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ShoppingCartRepo
{
    public class ShoppingCartRepo :IShoppingCartRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;
        public ShoppingCartRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }
        public async Task<ShoppingCart> GetshoppingCartByCustomerId(string customerId)
        {
            return await _ecommerceProject.shoppingCarts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            return await _ecommerceProject.customers
                                 .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<bool> DeleteShoppingCartByCustomerIdAsync(string customerId)
        {
            var shoppingCarts = await _ecommerceProject.shoppingCarts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (shoppingCarts !=null)
            {
                shoppingCarts.IsDeleted = true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task AddShoppingCart(ShoppingCart shoppingCart)
        {
            await _ecommerceProject.shoppingCarts.AddAsync(shoppingCart);
        }
        public async Task<bool> DeleteShoppingCart(int id)
        {
            var shoppingCarts = await _ecommerceProject.shoppingCarts.FindAsync(id);
            if (shoppingCarts !=null)
            {
                shoppingCarts.IsDeleted = true;
                await SaveChanges();
                return true;
            }
            return false;
        }

        //public async Task UpdateShoppingCart(ShoppingCart shoppingCart)
        //{

        //}
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }
    }
}
