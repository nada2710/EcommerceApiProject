using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.OrderRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.OrderManager
{
    public class OrderManger : IOrderManger
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;
        public OrderManger(IOrderRepo orderRepo,IMapper mapper)
        {
            _orderRepo=orderRepo;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<List<OrderReadDto>>> GetAllOrders()
        {
            var response = new ServiceResponse<List<OrderReadDto>>();
            var orders = await _orderRepo.GetAllOrders();
            if(orders?.Any()!=true)
            {
                response.Message="No orders found.";
                response.Success=false;
                return response;
            }
            response.Data= _mapper.Map<List<OrderReadDto>>(orders);
            response.Message = "Orders retrieved successfully.";
            return response;
        }
        public async Task<ServiceResponse<List<OrderReadDto>>> GetAllOrderByStatus(Status status)
        {
            var response = new ServiceResponse<List<OrderReadDto>>();
            var orders = await _orderRepo.GetAllOrderByStatus(status);
            if (orders?.Any()!=true)
            {
                response.Message="No orders found.";
                response.Success=false;
                return response;
            }
            response.Data= _mapper.Map<List<OrderReadDto>>(orders);
            response.Message = "Orders retrieved successfully.";
            return response;
        }
        public async Task<ServiceResponse<OrderReadDto>> GetOrderId(int id)
        {
            var response = new ServiceResponse<OrderReadDto>();
            var orders = await _orderRepo.GetOrderId(id,includeItems:true);
            if (orders is null)
            {
                response.Message="Order does not exist.";
                response.Success=false;
                return response;
            }
            response.Data= _mapper.Map<OrderReadDto>(orders);
            response.Message = "Order retrieved successfully.";
            return response;
        }
        public async Task<ServiceResponse<OrderReadDto>> AddOrder(OrderAddDto orderAddDto)
        {
          
            var response = new ServiceResponse<OrderReadDto>();
            if(orderAddDto is null)
            {
                response.Message="Invalid order data provided.";
                response.Success=false;
                return response;
            }
            
            try
            {
                var order = _mapper.Map<Order>(orderAddDto);
                order.CreatedDate = DateTime.UtcNow;
                order.Status = Status.Pending; // Set initial status
                await _orderRepo.AddOrders(order);
                await _orderRepo.SaveChanges();
                var createdOrder = await _orderRepo.GetOrderId(order.Id, includeItems: true);

                var orderRead = _mapper.Map<OrderReadDto>(order);
                response.Message="Order added successfully.";
                response.Data=orderRead;
            }
            catch(Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while adding the order : {ex.InnerException}";
                //response.Message= ex.InnerException?.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteOrder(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var getorder = await _orderRepo.DeleteOrders(id);
                if(!getorder)
                {
                    response.Success =false;
                    response.Message=$"Order does not exist.";
                    return response;
                }
                response.Message="Order deleted successfully.";
            }
            catch(Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while deleting the order : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                if(orderUpdateDto is null)
                {
                    response.Message="Invalid order data provided.";
                    response.Success=false;
                    return response;
                }
                var existingOrder = await _orderRepo.GetOrderId(orderUpdateDto.Id);
                if(existingOrder is null)
                {
                    response.Success=false;
                    response.Message = $"Order with ID {orderUpdateDto.Id} does not exist.";
                    response.Data = false;
                    return response;
                }
                var order = _mapper.Map(orderUpdateDto, existingOrder);
                await _orderRepo.SaveChanges();
                response.Success = true;
                response.Message = "Order updated successfully.";
            }
            catch(Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while updating the order : {ex.Message}";
            }
            return response;
        }
        public async Task SaveChanges()
        {
            await _orderRepo.SaveChanges();
        }
    }
}
