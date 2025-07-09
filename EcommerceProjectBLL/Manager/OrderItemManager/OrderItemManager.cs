using AutoMapper;
using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.Dto.OrderItemDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.OrderItemRepo;
using EcommerceProjectDAL.Repository.OrderRepo;
using EcommerceProjectDAL.Repository.ProductRepo;
using EcommerceProjectDAL.Repository.ShoppingCartItemRepo;
using EcommerceProjectDAL.Repository.ShoppingCartRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.OrderItemManager
{
    public class OrderItemManager : IOrderItemManager
    {
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly IShoppingCartRepo _shoppingCartRepo;
        private readonly IShoppingCartItemRepo _shoppingCartItemRepo;

        public OrderItemManager(IOrderItemRepo orderItemRepo,IMapper mapper, IOrderRepo orderRepo, IProductRepo productRepo,IShoppingCartRepo shoppingCartRepo,IShoppingCartItemRepo shoppingCartItemRepo )
        {
            _orderItemRepo=orderItemRepo;
            _mapper=mapper;
            _orderRepo=orderRepo;
            _productRepo=productRepo;
            _shoppingCartRepo=shoppingCartRepo;
            _shoppingCartItemRepo=shoppingCartItemRepo;
        }
        public async Task<ServiceResponse<List<OrderItemReadDto>>> GetAllOrderItemByOrderId(int OrderId)
        {
            var response = new ServiceResponse<List<OrderItemReadDto>>();
            var orderItem = await _orderItemRepo.GetAllOrderItemByOrderId(OrderId);
            if (orderItem?.Any()!= true)
            {
                response.Message="No item found .";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<OrderItemReadDto>>(orderItem);
            response.Message="OrderItems retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<OrderItemReadDto>> GetOrderItemById(int id)
        {
            var response = new ServiceResponse<OrderItemReadDto>();
            var orderItem = await _orderItemRepo.GetOrderItemById(id);
            if(orderItem is null)
            {
                response.Message="No item found .";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<OrderItemReadDto>(orderItem);
            response.Message="OrderItem retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<bool>> AddCartItemsToExistingOrder(string customerId, int orderId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var order = await _orderRepo.GetOrderId(orderId);
                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                    return response;
                }
                var cart = await _shoppingCartRepo.GetshoppingCartByCustomerId(customerId);
                if (cart == null)
                {
                    response.Success = false;
                    response.Message = "Shopping cart not found.";
                    return response;
                }
                var cartItems = await _shoppingCartItemRepo.GetAllShoppingCartItems();
                if (!cartItems.Any())
                {
                    response.Success = false;
                    response.Message = "Shopping cart is empty.";
                    return response;
                }
                
                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity
                    };
                    await _orderItemRepo.AddOrderItem(orderItem);
                }

                await _orderItemRepo.SaveChanges();
                await _shoppingCartItemRepo.DeleteShoppingCartItem(cart.Id);
                await _shoppingCartRepo.SaveChanges();

                response.Success = true;
                response.Data = true;
                response.Message = "Cart items added to existing order successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while adding cart items to order: {ex.Message}";
            }
            return response;
        }

        #region oldadd
        //public async Task<ServiceResponse<OrderItemReadDto>> AddOrderItem(OrderItemAddDto orderItemAddDto)
        //{
        //    var response = new ServiceResponse<OrderItemReadDto>();
        //    if(orderItemAddDto is null)
        //    {
        //        response.Message="Invalid orderItem data provided.";
        //        response.Success=false;
        //        return response;
        //    }
        //    // Validate Order exists


        //    try
        //    {

        //        var orderItem = _mapper.Map<OrderItem>(orderItemAddDto);


        //        await _orderItemRepo.AddOrderItem(orderItem);
        //        await _orderItemRepo.SaveChanges();
        //        var createdItem = await _orderItemRepo.GetOrderItemById(orderItem.Id, true);
        //        var orderItemRead = _mapper.Map<OrderItemReadDto>(orderItem) ;



        //        response.Message="OrderItem added successfully.";
        //        response.Data=orderItemRead;
        //    }
        //    catch(Exception ex)
        //    {
        //        response.Success =false;
        //        response.Message=$"An error occurred while adding the orderItem : {ex.Message}";
        //    }
        //    return response;
        //}
        //        public async Task<ServiceResponse<OrderItemReadDto>> AddOrderItem(OrderItemAddDto orderItemAddDto)
        //{
        //    var response = new ServiceResponse<OrderItemReadDto>();
        //    if(orderItemAddDto is null)
        //    {
        //        response.Message = "Invalid orderItem data provided.";
        //        response.Success = false;
        //        return response;
        //    }

        //    try
        //    {

        //        var orderItem = _mapper.Map<OrderItem>(orderItemAddDto);

        //        var cart = await _shoppingCartRepo.GetCustomerByIdAsync(orderItemAddDto.CustomerId); 
        //        if(cart != null)
        //        {
        //            var cartItems = await _shoppingCartItemRepo.GetAllShoppingCartItems();
        //            if(cartItems.Any())
        //            {
        //                foreach(var cartItem in cartItems)
        //                {

        //                    var newOrderItem = new OrderItem
        //                    {
        //                        OrderId = orderItemAddDto.OrderId,
        //                        ProductId = cartItem.ProductId,
        //                        Quantity = cartItem.Quantity,
        //                       Product=cartItem.Product

        //                    };

        //                    await _orderItemRepo.AddOrderItem(newOrderItem);
        //                }

        //                       // object value = await _shoppingCartItemRepo.DeleteShoppingCartItem();
        //            }
        //        }
        //        else
        //        {

        //            await _orderItemRepo.AddOrderItem(orderItem);
        //        }

        //            await _orderItemRepo.SaveChanges();

        //            var createdItem = await _orderItemRepo.GetOrderItemById(orderItem.Id, true);
        //            var orderItemRead = _mapper.Map<OrderItemReadDto>(createdItem);

        //        response.Message = "OrderItem(s) added successfully from cart.";
        //        response.Data = orderItemRead;
        //        }
        //        catch(Exception ex)
        //        {
        //        response.Success = false;
        //        response.Message = $"An error occurred while adding the orderItem(s) : {ex.Message}";
        //    }
        //    return response;
        //} 
        #endregion

        public async Task<ServiceResponse<bool>> DeleteOrderItem(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var getorderItem = await _orderItemRepo.DeleteOrderItem(id);
                if (!getorderItem)
                {
                    response.Success =false;
                    response.Message=$"OrderItem does not exist.";
                    return response;
                }
                response.Message="OrderItem deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while deleting the orderItem : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateOrderItem(OrderItemUpdateDto orderItemUpdateDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                if (orderItemUpdateDto is null)
                {
                    response.Message="Invalid orderItem data provided.";
                    response.Success=false;
                    return response;
                }
                var existingOrderItem = await _orderItemRepo.GetOrderItemById(orderItemUpdateDto.Id,true);
                if (existingOrderItem is null)
                {
                    response.Success=false;
                    response.Message = $"OrderItem with ID {orderItemUpdateDto.Id} does not exist.";
                    response.Data = false;
                    return response;
                }
                var order = _mapper.Map(orderItemUpdateDto, existingOrderItem);
                await _orderItemRepo.SaveChanges();
             
                response.Success = true;
                response.Message = "OrderItem updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while updating the orderItem : {ex.Message}";
            }
            return response;

        }
        public async Task SaveChanges()
        {
            await _orderItemRepo.SaveChanges();
        }
    }
}
