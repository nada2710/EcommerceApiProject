using AutoMapper;
using Azure;
using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.Dto.ShoppingCartItemDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.OrderRepo;
using EcommerceProjectDAL.Repository.ShoppingCartItemRepo;
using EcommerceProjectDAL.Repository.ShoppingCartRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ShoppingCartItemManager
{
    public class ShoppingCartItemManager : IShoppingCartItemManager
    {
        private readonly IShoppingCartItemRepo _shoppingCartItemRepo;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingCartRepo _shoppingCartRepo;

        public ShoppingCartItemManager(IShoppingCartItemRepo shoppingCartItemRepo,IMapper mapper, IHttpContextAccessor httpContextAccessor,IShoppingCartRepo shoppingCartRepo)
        {
            _shoppingCartItemRepo=shoppingCartItemRepo;
            _mapper=mapper;
            _httpContextAccessor=httpContextAccessor;
            _shoppingCartRepo=shoppingCartRepo;
        }
        public async Task<ServiceResponse<List<ShoppingCartItemReadDto>>> GetAllItemInTheCart()
        {
            var response = new ServiceResponse<List<ShoppingCartItemReadDto>>();
            var item = await _shoppingCartItemRepo.GetAllShoppingCartItems();
            if(item?.Any()!=true)
            {
                response.Success=false;
                response.Message="No item in cart";
                return response;
            }
            response.Data=_mapper.Map<List<ShoppingCartItemReadDto>>(item);
            response.Message="Items retrieved successfully.";
            return response;
        }
        public async Task<ServiceResponse<ShoppingCartItemReadDto>> GetItemByItemId(int id)
        {
            var response = new ServiceResponse<ShoppingCartItemReadDto>();
            var item = await _shoppingCartItemRepo.GetShoppingCartItemById(id);
            if(item is null)
            {
                response.Success=false;
                response.Message="Item does not exist";
                return response;
            }
            response.Data=_mapper.Map<ShoppingCartItemReadDto>(item);
            response.Message="Item retrieved successfully.";
            return response;

        }
        public async Task<ServiceResponse<ShoppingCartItemReadDto>> AddItemToCart(ShoppingCartItemAddDto shoppingCartItemAddDto)
        {
            var response = new ServiceResponse<ShoppingCartItemReadDto>();
            try
            {
                if(shoppingCartItemAddDto is null )
                {

                    response.Message="Invalid data provided.";
                    response.Success=false;
                    return response;

                }
             
                var item = _mapper.Map<ShoppingCartItem>(shoppingCartItemAddDto);
                
                //item.ShoppingCartId = cart.Id;
                
                await _shoppingCartItemRepo.AddShoppingCartItem(item);
                await _shoppingCartItemRepo.SaveChanges();
                response.Data =_mapper.Map<ShoppingCartItemReadDto>(item);
                response.Message="Item added successfully.";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while Adding the Item \tInner Exception: {ex.InnerException.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteItem(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var item = await _shoppingCartItemRepo.DeleteShoppingCartItem(id);
                if (!item)
                {
                    response.Success= false;
                    response.Message="Item does not exist.";
                    return response;
                }
                response.Message="Item deleted successfully.";
            }
            catch(Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while deleting the Item : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateQuantity(ShoppingCartItemUpdateDto shoppingCartItemUpdateDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                if(shoppingCartItemUpdateDto is null)
                {
                    response.Message="Invalid data provided.";
                    response.Success=false;
                    return response;
                }
                var existingItem = await _shoppingCartItemRepo.GetShoppingCartItemById(shoppingCartItemUpdateDto.Id);
                if(existingItem is null)
                {
                    response.Success=false;
                    response.Message = $"Item does not exist.";
                    response.Data = false;
                    return response;
                }
                var item=_mapper.Map(shoppingCartItemUpdateDto, existingItem);
                await _shoppingCartItemRepo.SaveChanges();
                response.Success = true;
                response.Message = "Quantity updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while Update the Item : {ex.Message}";
            }
            return response;
        }
        public async Task SaveChanges()
        {
           await _shoppingCartItemRepo.SaveChanges();
        }
    }
}
