using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ShoppingCartItemRepo
{
    public interface IShoppingCartItemRepo
    {
        Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItems();
        Task<ShoppingCartItem> GetShoppingCartItemById(int id);
        Task AddShoppingCartItem(ShoppingCartItem shoppingCartItem);
       
        Task UpdateShoppingCartItem(ShoppingCartItem shoppingCartItem);
        Task<bool> DeleteShoppingCartItem(int id);
        Task SaveChanges();
    }
}
