using AutoMapper;
using Azure;
using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.CategoryRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.CategoryManager
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryManager(ICategoryRepo categoryRepo,IMapper  mapper)
        {
            _categoryRepo=categoryRepo;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<List<CategoryReadDto>>> GetAllCategories()
        {
            // take object ServiceResponse => get it type because it is generic
            var response = new ServiceResponse<List<CategoryReadDto>>();
            var Categories = await _categoryRepo.GetAllCategories();
            if (Categories?.Any()!=true)
            {
                response.Message="No categories found.";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<CategoryReadDto>>(Categories);
            response.Message = "Categories retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<CategoryReadDto>> GetCategoryById(int id)
        {
            var response = new ServiceResponse<CategoryReadDto>();
            var categoryExists = await _categoryRepo.GetCategoryById(id);
            if(categoryExists is null)
            {
                response.Message="Category does not exist.";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<CategoryReadDto>(categoryExists);
            response.Message = "Category retrieved successfully.";
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<CategoryReadDto>> AddCategory(CategoryAddDto categoryAddDto, string webRootPath)
        {
            var response = new ServiceResponse<CategoryReadDto>();
            if(categoryAddDto is null)
            {
                response.Success=false;
                response.Message="Invalid category data provided.";
                return response;
            }
            try
            {
                string imagePath = null;
                if(categoryAddDto.Image!=null&&categoryAddDto.Image.Length>0)
                {
                    imagePath = await SaveImageAsync(categoryAddDto.Image, webRootPath);
                }
               
                var category = _mapper.Map<Category>(categoryAddDto);
                category.ImageUrl=imagePath;
                await _categoryRepo.AddCategory(category);
                await _categoryRepo.SaveChanges();

                var categoryRead = _mapper.Map<CategoryReadDto>(category);
                response.Message="Category added successfully.";
                response.Data=categoryRead;
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while adding the category : {ex.Message}";
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> DeleteCategory(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var getCategory = await _categoryRepo.DeleteCategory(id);
                if (!getCategory)
                {
                    response.Success=false;
                    response.Message= $"Category with ID {id} does not exist.";
                    response.Data=false;
                    return response;
                }
                
               // await _categoryRepo.SaveChanges();
                response.Success=true;
                response.Message="Category deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while deleting the category : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateCategory(CategoryUpdateDto categoryUpdateDto, string webRootPath)
        {
            string imagePath = null;
            if (categoryUpdateDto.Image!=null&&categoryUpdateDto.Image.Length>0)
            {
                imagePath = await SaveImageAsync(categoryUpdateDto.Image, webRootPath);
            }
            var response = new ServiceResponse<bool>();
            try 
            {
                if (categoryUpdateDto is null)
                {
                    response.Success = false;
                    response.Message = "Invalid category data provided.";
                    response.Data = false;
                    return response;
                }
                var existingCategory = await _categoryRepo.GetCategoryById(categoryUpdateDto.Id);
                if (existingCategory is null)
                {
                    response.Success=false;
                    response.Message = $"Category with ID {categoryUpdateDto.Id} does not exist.";
                    response.Data = false;
                    return response;
                }
                var category=_mapper.Map(categoryUpdateDto, existingCategory);
                category.ImageUrl=imagePath;
                await _categoryRepo.SaveChanges();
                response.Success = true;
                response.Message = "Category updated successfully.";
                response.Data = true;
            }
            catch
            {
                response.Success = false;
                response.Message = $"An error occurred while updating the category";
                response.Data = false;
            }
            return response;
        }
        private async Task<string> SaveImageAsync(IFormFile image, string webRootPath)
        {
            string uploadsFolder = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string uniqueFileNme = Guid.NewGuid().ToString()+"_"+Path.GetFileName(image.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileNme);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return Path.Combine("uploads", uniqueFileNme);
        }
        public async Task SaveChanges()
        {
            await _categoryRepo.SaveChanges();
        }

       
    }
}
