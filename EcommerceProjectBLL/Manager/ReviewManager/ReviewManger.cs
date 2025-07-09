using AutoMapper;
using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectBLL.Dto.ReviewDto;
using EcommerceProjectBLL.Dto.ShoppingCartDto;
using EcommerceProjectBLL.Dto.ShoppingCartItemDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.ReviewRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ReviewManager
{
    public class ReviewManger : IReviewManger
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IMapper _mapper;
        public ReviewManger(IReviewRepo reviewRepo,IMapper mapper)
        {
            _reviewRepo=reviewRepo;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviews()
        {
            var response = new ServiceResponse<List<ReviewReadDto>>();
            var review = await _reviewRepo.GetAllReviews();
            if (review?.Any()!= true)
            {
                response.Message="No reviews found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<ReviewReadDto>>(review);
            response.Message="Reviews retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<ReviewReadDto>> GetReviewById(int id)
        {
            var response = new ServiceResponse<ReviewReadDto>();
            var review = await _reviewRepo.GetReviewById(id);
            if (review is null)
            {
                response.Message="No review found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<ReviewReadDto>(review);
            response.Message="Review retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviewsByCustomerId(string CustomerId)
        {
            var response = new ServiceResponse<List<ReviewReadDto>>();
            var review = await _reviewRepo.GetAllReviewsByCustomerId(CustomerId);
            if (review?.Any()!= true)
            {
                response.Message="No reviews found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<ReviewReadDto>>(review);
            response.Message="Reviews retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviewsByProductId(int ProductId)
        {
            var response = new ServiceResponse<List<ReviewReadDto>>();
            var review = await _reviewRepo.GetAllReviewsByProductId(ProductId);
            if (review?.Any()!= true)
            {
                response.Message="No reviews found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<ReviewReadDto>>(review);
            response.Message="Reviews retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<ReviewReadDto>> AddReview(CreateReviewDto createReviewDto)
        {
            var response = new ServiceResponse<ReviewReadDto>();
            if (createReviewDto is null)
            {
                response.Message="Invalid data provided.";
                response.Success=false;
                return response;
            }
            try
            {
                var data = _mapper.Map<Review>(createReviewDto);
                await _reviewRepo.AddReview(data);
                await _reviewRepo.SaveChanges();
                response.Data =_mapper.Map<ReviewReadDto>(data);
                response.Message="Review added successfully";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while Adding the Review \tInner Exception: {ex.InnerException.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteReview(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var Review = await _reviewRepo.DeleteReview(id);
                if (!Review)
                {
                    response.Success=false;
                    response.Message= $"Review with ID {id} does not exist.";
                    response.Data=false;
                    return response;
                }
                response.Message="Review deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while deleting the Review : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateReview(UpdateReviewDto updateReviewDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                if (updateReviewDto is null)
                {
                    response.Message="Invalid data provided.";
                    response.Success=false;
                    return response;
                }
                var existingReview = await _reviewRepo.GetReviewById(updateReviewDto.Id);
                if (existingReview is null)
                {
                    response.Success=false;
                    response.Message = $"Review does not exist.";
                    response.Data = false;
                    return response;
                }
                var item = _mapper.Map(updateReviewDto, existingReview);
                await _reviewRepo.SaveChanges();
                response.Success = true;
                response.Message = "Review updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success =false;
                response.Message=$"An error occurred while Update the Review : {ex.Message}";
            }
            return response;
        }
        public async Task SaveChanges()
        {
            await _reviewRepo.SaveChanges();
        }
    }
}
