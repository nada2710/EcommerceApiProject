using AutoMapper;
using EcommerceProjectBLL.Dto.ShoppingCartDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.ShoppingCartManger;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.ShoppingCartRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ShoppingCartManager
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly IShoppingCartRepo _shoppingCart;
        private readonly IMapper _mapper;
        public ShoppingCartManager(IShoppingCartRepo shoppingCart,IMapper mapper)
        {
            _shoppingCart=shoppingCart;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<ShoppingCartReadDto>> GetshoppingCartByCustomerId(string customerId)
        {
            var response = new ServiceResponse<ShoppingCartReadDto>();
            var existhoppingCart = await _shoppingCart.GetshoppingCartByCustomerId(customerId);
            if(existhoppingCart is null)
            {
                response.Message="ShoppingCart does not exist.";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<ShoppingCartReadDto>(existhoppingCart);
            response.Message="ShoppingCart retrieved successfully.";
            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteShoppingCartByCustomerIdAsync(string customerId)
        {
            var response = new ServiceResponse<bool>();
            var existhoppingCart = await _shoppingCart.DeleteShoppingCartByCustomerIdAsync(customerId);
            if (!existhoppingCart)
            {
                response.Message="ShoppingCart does not exist.";
                response.Success=false;
                return response;
            }
            response.Message="ShoppingCart deleted successfully.";
            return response;
        }
        public async Task<ServiceResponse<ShoppingCartReadDto>> CreateShoppingCart(ShoppingCartAddDto shoppingCartAddDto)
        {
            var response = new ServiceResponse<ShoppingCartReadDto>();
            if(shoppingCartAddDto is null)
            {
                response.Message="Invalid data provided.";
                response.Success=false;
                return response;
            }
          
            try
            {
                var data = _mapper.Map<ShoppingCart>(shoppingCartAddDto);
                await _shoppingCart.AddShoppingCart(data);
                await _shoppingCart.SaveChanges();

                response.Data =_mapper.Map<ShoppingCartReadDto>(data);
                response.Message="Cart added successfully";
            }
            catch(Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while Adding the cart \tInner Exception: {ex.InnerException.Message}";
            }
            return response;
        }
        public async Task SaveChanges()
        {
            await _shoppingCart.SaveChanges();
        }

       
    }
}
