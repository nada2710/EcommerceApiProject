using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.Dto.OrderItemDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.OrderItemManager
{
    public interface IOrderItemManager
    {
        Task<ServiceResponse<List<OrderItemReadDto>>> GetAllOrderItemByOrderId(int OrderId);
        Task<ServiceResponse<OrderItemReadDto>> GetOrderItemById(int id);
        //Task<ServiceResponse<OrderItemReadDto>> AddOrderItem(OrderItemAddDto orderItemAddDto);
        Task<ServiceResponse<bool>> AddCartItemsToExistingOrder(string customerId, int orderId);
        Task<ServiceResponse<bool>> UpdateOrderItem(OrderItemUpdateDto orderItemUpdateDto);
        Task<ServiceResponse<bool>> DeleteOrderItem(int id);
        Task SaveChanges();

    }
}
