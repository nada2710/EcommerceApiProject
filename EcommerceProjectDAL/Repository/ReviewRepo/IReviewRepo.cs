using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ReviewRepo
{
    public interface IReviewRepo
    {
        Task<IEnumerable<Review>> GetAllReviews();
        Task<Review> GetReviewById(int id);
        Task<IEnumerable<Review>> GetAllReviewsByCustomerId(string CustomerId);
        Task<IEnumerable<Review>> GetAllReviewsByProductId(int ProductId);
        Task AddReview(Review review);
        Task UpdateReview(Review review);
        Task<bool> DeleteReview(int id);
        Task SaveChanges();
    }
}
