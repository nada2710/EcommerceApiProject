using EcommerceProjectBLL.Dto.ReviewDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ReviewManager
{
    public interface IReviewManger
    {
        Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviews();
        Task<ServiceResponse<ReviewReadDto>> GetReviewById(int id);
        Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviewsByCustomerId(string CustomerId);
        Task<ServiceResponse<List<ReviewReadDto>>> GetAllReviewsByProductId(int ProductId);
        Task<ServiceResponse<ReviewReadDto>> AddReview(CreateReviewDto createReviewDto);
        Task<ServiceResponse<bool>> UpdateReview(UpdateReviewDto updateReviewDto);
        Task<ServiceResponse<bool>> DeleteReview(int id);
        Task SaveChanges();
    }
}
