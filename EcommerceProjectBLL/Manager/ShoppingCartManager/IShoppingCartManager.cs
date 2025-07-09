using EcommerceProjectBLL.Dto.ShoppingCartDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ShoppingCartManger
{
    public interface IShoppingCartManager
    {
        Task<ServiceResponse<ShoppingCartReadDto>> GetshoppingCartByCustomerId(string customerId);
        Task<ServiceResponse<bool>> DeleteShoppingCartByCustomerIdAsync(string customerId);
        Task<ServiceResponse<ShoppingCartReadDto>> CreateShoppingCart(ShoppingCartAddDto shoppingCartAddDto);
        Task SaveChanges();

    }
}
