using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.ProductManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.ProductController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManger _productManger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductManger productManger,IWebHostEnvironment webHostEnvironment)
        {
            _productManger=productManger;
            _webHostEnvironment=webHostEnvironment;
        }
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ServiceResponse<ProductReadDto>>> GetAllProducts()
        {
            var response = await _productManger.GetAllProducts();
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ServiceResponse<ProductReadDto>>> GetProductById(int id)
        {
            var response = await _productManger.GetProductById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetProductsByCategoryId")]
        public async Task<ActionResult<ServiceResponse<ProductReadDto>>> GetProductsByCategoryId( int id)
        {
            var response = await _productManger.GetProductsByCategoryId(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);

        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateProduct(ProductAddDto productAddDto)
        {
            var response = await _productManger.AddProduct(productAddDto,_webHostEnvironment.WebRootPath);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var response = await _productManger.DeleteProduct(id);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);

        }
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            var response = await _productManger.UpdateProduct(productUpdateDto, _webHostEnvironment.WebRootPath);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }



    }
}
