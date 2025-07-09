using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.ProductRepo
{
    public class ProductRepo : IProductRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;

        public ProductRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _ecommerceProject.products.AsNoTracking().ToListAsync();
        }
       
        public async Task<Product> GetProductById(int id)
        {
            return await _ecommerceProject.products.FirstOrDefaultAsync(a => a.Id ==id);
        }
        public async Task AddProduct(Product product)
        {
            await _ecommerceProject.products.AddAsync(product);
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var products = await _ecommerceProject.products.FindAsync(id);
            if(products !=null)
            {
                products.IsDeleted = true;
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            var category = await _ecommerceProject.categories.FindAsync(categoryId);
            if (category!=null)
            {
                return await _ecommerceProject.products.Where(p => p.CategoryId==categoryId).AsNoTracking().ToListAsync();

            }
            return Enumerable.Empty<Product>();
        }
        public async Task UpdateProduct(Product product)
        {
            
        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }

       
    }
}
