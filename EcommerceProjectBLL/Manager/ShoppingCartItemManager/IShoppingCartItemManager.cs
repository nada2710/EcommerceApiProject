using EcommerceProjectBLL.Dto.ShoppingCartItemDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ShoppingCartItemManager
{
    public interface IShoppingCartItemManager
    {
        Task<ServiceResponse<List<ShoppingCartItemReadDto>>> GetAllItemInTheCart();
        Task<ServiceResponse<ShoppingCartItemReadDto>> GetItemByItemId(int id);
        Task<ServiceResponse<ShoppingCartItemReadDto>> AddItemToCart(ShoppingCartItemAddDto shoppingCartItemAddDto);
        Task<ServiceResponse<bool>> DeleteItem(int id);
        Task<ServiceResponse<bool>> UpdateQuantity(ShoppingCartItemUpdateDto shoppingCartItemUpdateDto);


    }
}
