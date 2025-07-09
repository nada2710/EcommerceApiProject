using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.CategoryManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.CategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(ICategoryManager categoryManager, IWebHostEnvironment environment)
        {
            _categoryManager=categoryManager;
            _environment=environment;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ServiceResponse<CategoryReadDto>>>GetAllCategories()
        {
            var response = await _categoryManager.GetAllCategories();
            if(response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);

        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<ServiceResponse<CategoryReadDto>>> GetCategoryById(int id)
        {
            var response = await _categoryManager.GetCategoryById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ServiceResponse<CategoryReadDto>>> AddCategory(CategoryAddDto categoryAddDto)
        {
            var response = await _categoryManager.AddCategory(categoryAddDto, _environment.WebRootPath);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCategory(int id)
        {
            var response = await _categoryManager.DeleteCategory(id);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);

        }
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var response = await _categoryManager.UpdateCategory(categoryUpdateDto,_environment.WebRootPath);
            if(response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }

    }
}
