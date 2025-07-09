using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ShoppingCartItemRepo
{
    public class ShoppingCartItemRepo : IShoppingCartItemRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;
        public ShoppingCartItemRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItems()
        {
            return await _ecommerceProject.shoppingCartItems.AsNoTracking().ToListAsync();
        }
        public async Task<ShoppingCartItem> GetShoppingCartItemById(int id)
        {
            return await _ecommerceProject.shoppingCartItems.FirstOrDefaultAsync(a => a.Id==id);
        }
        public async Task AddShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            await _ecommerceProject.shoppingCartItems.AddAsync(shoppingCartItem);
        }
        public async Task<bool> DeleteShoppingCartItem(int id)
        {
            var shoppingCartItems = await _ecommerceProject.shoppingCartItems.FindAsync(id);
            if(shoppingCartItems !=null)
            {
                shoppingCartItems.IsDeleted = true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task UpdateShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            
        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }
      

    }
}
