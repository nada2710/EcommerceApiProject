using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.OrderManager
{
    public interface IOrderManger
    {
        Task<ServiceResponse<List<OrderReadDto>>> GetAllOrders();
        Task<ServiceResponse<OrderReadDto>> GetOrderId(int id);
        Task<ServiceResponse<List<OrderReadDto>>> GetAllOrderByStatus(Status status);
        Task<ServiceResponse<OrderReadDto>> AddOrder(OrderAddDto orderAddDto);
        Task<ServiceResponse<bool>> DeleteOrder(int id);
        Task<ServiceResponse<bool>> UpdateOrder(OrderUpdateDto orderUpdateDto);
        Task SaveChanges();



    }
}
