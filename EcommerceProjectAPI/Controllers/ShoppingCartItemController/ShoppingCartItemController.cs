using Azure;
using EcommerceProjectBLL.Dto.ShoppingCartItemDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.ShoppingCartItemManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.ShoppingCartItemController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly IShoppingCartItemManager _shoppingCartItemManager;

        public ShoppingCartItemController(IShoppingCartItemManager shoppingCartItemManager)
        {
            _shoppingCartItemManager=shoppingCartItemManager;
        }
        [HttpGet("GetAllItemInTheCart")]
        public async Task<ActionResult<ServiceResponse<ShoppingCartItemReadDto>>> GetAllItemInTheCart()
        {
            var response =await _shoppingCartItemManager.GetAllItemInTheCart();
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetItemByItemId/{id}")]
        public async Task<ActionResult<ServiceResponse<ShoppingCartItemReadDto>>> GetItemByItemId(int id)
        {
            var response = await _shoppingCartItemManager.GetItemByItemId(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<ServiceResponse<ShoppingCartItemReadDto>>> AddItemToCart(ShoppingCartItemAddDto shoppingCartItemAddDto)
        {
            var response = await _shoppingCartItemManager.AddItemToCart(shoppingCartItemAddDto);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpDelete("DeleteItem/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteItem(int id)
        {
            var response = await _shoppingCartItemManager.DeleteItem(id);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }
        [HttpPut("UpdateQuantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(ShoppingCartItemUpdateDto shoppingCartItemUpdateDto)
        {
            var response = await _shoppingCartItemManager.UpdateQuantity(shoppingCartItemUpdateDto);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }

    }
}
