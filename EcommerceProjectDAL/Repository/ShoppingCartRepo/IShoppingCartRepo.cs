using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ShoppingCartRepo
{
    public interface IShoppingCartRepo
    {
        Task<ShoppingCart> GetshoppingCartByCustomerId(string customerId);
        Task<bool> DeleteShoppingCartByCustomerIdAsync(string customerId);
        Task<Customer> GetCustomerByIdAsync(string customerId);
        Task SaveChanges();
        //Task<ShoppingCart> GetShoppingCartById(int id);
        Task AddShoppingCart(ShoppingCart shoppingCart);
        //Task UpdateShoppingCart(ShoppingCart shoppingCart);
    }
}
