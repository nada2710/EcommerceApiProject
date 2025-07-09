using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectBLL.Dto.OrderItemDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.OrderItemManager;
using EcommerceProjectDAL.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.OrderItemController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemManager _orderItemManager;
        public OrderItemController(IOrderItemManager orderItemManager)
        {
            _orderItemManager=orderItemManager;
        }

        [HttpGet("GetAllOrderItemByOrderId")]
        public async Task<ActionResult<ServiceResponse<OrderItemReadDto>>> GetAllOrderItemByOrderId(int OrderId)
        {
            var order = await _orderItemManager.GetAllOrderItemByOrderId(OrderId);
            if (order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }

        [HttpGet("GetOrderItemById/{id}")]
        public async Task<ActionResult<ServiceResponse<OrderItemReadDto>>> GetOrderItemById(int id)
        {
            var order = await _orderItemManager.GetOrderItemById(id);
            if (order.Success)
            {
                return Ok(order.Data);
            }
            return BadRequest(order.Message);
        }
       
        [HttpPost("AddCartItemsToOrder")]
        public async Task<IActionResult> AddCartItemsToOrder(string customerId, int orderId)
        {
            var result = await _orderItemManager.AddCartItemsToExistingOrder(customerId, orderId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteOrderItem/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteOrderItem(int id)
        {
            var order = await _orderItemManager.DeleteOrderItem(id);
            if (order.Success)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }

        [HttpPut("UpdateOrderItem")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateOrderItem(OrderItemUpdateDto orderItemUpdateDto)
        {
            var order = await _orderItemManager.UpdateOrderItem(orderItemUpdateDto);
            if (order.Success)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }

    }
}
