using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.CategoryManager
{
    public interface ICategoryManager
    {
        Task<ServiceResponse<List<CategoryReadDto>>> GetAllCategories();
        Task<ServiceResponse<CategoryReadDto>> GetCategoryById(int id);
        Task <ServiceResponse<CategoryReadDto>> AddCategory(CategoryAddDto categoryAddDto, string webRootPath);
        Task <ServiceResponse<bool>> UpdateCategory(CategoryUpdateDto categoryUpdateDto,string webRootPath);
        Task<ServiceResponse<bool>> DeleteCategory(int id);
        Task SaveChanges();
    }
}
