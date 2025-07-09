using EcommerceProjectBLL.Dto.ShoppingCartDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.ShoppingCartManger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.ShoppingCartController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartManager _shoppingCartManager;

        public ShoppingCartController(IShoppingCartManager shoppingCartManager)
        {
            _shoppingCartManager=shoppingCartManager;
        }
        [HttpGet("GetshoppingCartByCustomerId/{customerId}")]
        public async Task<ActionResult<ServiceResponse<ShoppingCartReadDto>>> GetshoppingCartByCustomerId(string customerId)
        {
            var response =await _shoppingCartManager.GetshoppingCartByCustomerId(customerId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpPost("CreateCart")]
        public async Task<ActionResult<ServiceResponse<ShoppingCartReadDto>>> CreateCart(ShoppingCartAddDto shoppingCartAddDto)
        {
            var response = await _shoppingCartManager.CreateShoppingCart(shoppingCartAddDto);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);

        }
        [HttpDelete("DeleteShoppingCartByCustomerId/{customerId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteShoppingCartByCustomerId(string customerId)
        {
            var response = await _shoppingCartManager.DeleteShoppingCartByCustomerIdAsync(customerId);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }

    }
}
