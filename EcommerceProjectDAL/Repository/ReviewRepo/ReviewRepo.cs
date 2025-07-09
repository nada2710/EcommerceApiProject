using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ReviewRepo
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;

        public ReviewRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }

        public async Task<IEnumerable<Review>> GetAllReviews()
        {
            return await _ecommerceProject.reviews.AsNoTracking().ToListAsync();
        }
        public async Task<Review> GetReviewById(int id)
        {
            return await _ecommerceProject.reviews.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Review>> GetAllReviewsByCustomerId(string CustomerId)
        {
            var customer = await _ecommerceProject.customers.FindAsync(CustomerId);
            if(customer!=null)
            {
                return await _ecommerceProject.reviews.Where(p => p.CustomerId==CustomerId).AsNoTracking().ToListAsync();
            }
            return Enumerable.Empty<Review>();
        }
        public async Task<IEnumerable<Review>> GetAllReviewsByProductId(int ProductId)
        {
            var customer = await _ecommerceProject.products.FindAsync(ProductId);
            if (customer!=null)
            {
                return await _ecommerceProject.reviews.Where(p => p.ProductId==ProductId).AsNoTracking().ToListAsync();
            }
            return Enumerable.Empty<Review>();
        }
        public async Task AddReview(Review review)
        {
            await _ecommerceProject.reviews.AddAsync(review);
        }
        public async Task<bool> DeleteReview(int id)
        {
            var review = await _ecommerceProject.reviews.FindAsync(id);
            if(review !=null)
            {
                review.IsDeleted=true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task UpdateReview(Review review)
        {
            
        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }

       
    }
}
