using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectBLL.Dto.ReviewDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.ReviewManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.ReviewController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewManger _reviewManger;

        public ReviewController(IReviewManger reviewManger)
        {
            _reviewManger=reviewManger;
        }
        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<ServiceResponse<ReviewReadDto>>> GetAllReviews()
        {
            var response = await _reviewManger.GetAllReviews();
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetReviewById/{id}")]
        public async Task<ActionResult<ServiceResponse<ReviewReadDto>>> GetReviewById(int id)
        {
            var response = await _reviewManger.GetReviewById(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetAllReviewsByCustomerId")]
        public async Task<ActionResult<ServiceResponse<ReviewReadDto>>> GetAllReviewsByCustomerId(string CustomerId)
        {
            var response = await _reviewManger.GetAllReviewsByCustomerId(CustomerId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpGet("GetAllReviewsByProductId")]
        public async Task<ActionResult<ServiceResponse<ReviewReadDto>>> GetAllReviewsByProductId(int ProductId)
        {
            var response = await _reviewManger.GetAllReviewsByProductId(ProductId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpPost("AddReview")]
        public async Task<ActionResult<ServiceResponse<ReviewReadDto>>> AddReview(CreateReviewDto createReviewDto)
        {
            var response = await _reviewManger.AddReview(createReviewDto);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }
        [HttpDelete("DeleteReview/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteReview(int id)
        {
            var response = await _reviewManger.DeleteReview(id);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);

        }
        [HttpPut("UpdateReview")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateReview(UpdateReviewDto updateReviewDto)
        {
            var response = await _reviewManger.UpdateReview(updateReviewDto);
            if (response.Success)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }
    }
}
