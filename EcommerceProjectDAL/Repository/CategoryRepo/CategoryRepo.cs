using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.CategoryRepo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;

        public CategoryRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }
       
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _ecommerceProject.categories.AsNoTracking().ToListAsync();
        }
        public async Task<Category> GetCategoryById(int id)
        {
            return await _ecommerceProject.categories.FirstOrDefaultAsync(a => a.Id==id);
        }
        public async Task AddCategory(Category category)
        {
           await _ecommerceProject.categories.AddAsync(category);
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _ecommerceProject.categories.FindAsync(id);
            if(category !=null)
            {
                category.IsDeleted =true;
               // _ecommerceProject.Remove(category);
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task UpdateCategory(Category category)
        {

        }
        public async Task SaveChanges()
        {
            await _ecommerceProject.SaveChangesAsync();
        }
    }
}
