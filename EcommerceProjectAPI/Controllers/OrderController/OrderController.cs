using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.OrderManager;
using EcommerceProjectDAL.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.OrderController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManger _orderManger;
        public OrderController(IOrderManger orderManger)
        {
            _orderManger=orderManger;
        }
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<ServiceResponse<OrderReadDto>>> GetAllOrders()
        {
            var order = await _orderManger.GetAllOrders();
            if(order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }
        [HttpGet("GetAllOrdersByStatus/{status}")]
        public async Task<ActionResult<ServiceResponse<OrderReadDto>>> GetAllOrdersByStatus(Status status)
        {
            var order = await _orderManger.GetAllOrderByStatus(status);
            if (order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<ServiceResponse<OrderReadDto>>> GetOrderById(int id)
        {
            var order = await _orderManger.GetOrderId(id);
            if (order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ServiceResponse<OrderReadDto>>> CreateOrder(OrderAddDto orderAddDto)
        {
            var order = await _orderManger.AddOrder(orderAddDto);
            if (order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteOrder(int id)
        {
            var order = await _orderManger.DeleteOrder(id);
            if(order.Success)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }
        [HttpPut("UpdateOrder")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            var order = await _orderManger.UpdateOrder(orderUpdateDto);
            if (order.Success)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }

    }
}
