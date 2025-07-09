using AutoMapper;
using Azure;
using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.ProductRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ProductManager
{
    public class ProductManger : IProductManger
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public ProductManger(IProductRepo productRepo,IMapper mapper)
        {
            _productRepo=productRepo;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<List<ProductReadDto>>> GetAllProducts()
        {
            var response = new ServiceResponse<List<ProductReadDto>>();
            var products = await _productRepo.GetAllProducts();
            if(products?.Any()!= true)
            {
                response.Message="No products found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<ProductReadDto>>(products);
            response.Message="Products retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<ProductReadDto>> GetProductById(int id)
        {
            var response = new ServiceResponse<ProductReadDto>();
            var product = await _productRepo.GetProductById(id);
            if(product is null)
            {
                response.Message="Product does not exist.";
                response.Success=false;
                return response;
            }
            response.Data =_mapper.Map<ProductReadDto>(product);
            return response;
        }
        public async Task<ServiceResponse<List<ProductReadDto>>> GetProductsByCategoryId(int categoryId)
        {
            var response = new ServiceResponse<List<ProductReadDto>>();
            var category = await _productRepo.GetProductsByCategoryId(categoryId);
            if (category?.Any()!= true)
            {
                response.Message="No products found";
                response.Success=false;
                return response;
            }
            response.Data=_mapper.Map<List<ProductReadDto>>(category);
            response.Message="Products retrieved successfully";
            return response;
        }
        public async Task<ServiceResponse<ProductReadDto>> AddProduct(ProductAddDto productAddDto, string webRootPath)
        {
            var response = new ServiceResponse<ProductReadDto>();
            if(productAddDto is null)
            {
                response.Message="Invalid product data provided";
                response.Success=false;
                return response;
            }
            try
            {
                string imagePath = null;
                if(productAddDto.Image!=null&& productAddDto.Image.Length>0)
                {
                    imagePath= await SaveImageAsync(productAddDto.Image, webRootPath);
                }
                var product = _mapper.Map<Product>(productAddDto);
                product.ImageUrl=imagePath;
                await _productRepo.AddProduct(product);
                await _productRepo.SaveChanges();

                var productRead = _mapper.Map<ProductReadDto>(product);
                response.Success=true;
                response.Message="Product added successfully.";
                response.Data= productRead;
            }
            catch(Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while adding the product : {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var product =await _productRepo.DeleteProduct(id);
                if(!product)
                {
                    response.Success=false;
                    response.Message= $"Product with ID {id} does not exist.";
                    response.Data=false;
                    return response;
                }
                response.Message="Product deleted successfully .";
            }
            catch(Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while deleting the product : {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateProduct(ProductUpdateDto productUpdateDto, string webRootPath)
        {
            var response = new ServiceResponse<bool>();
            string imagePath = null;
            if(productUpdateDto.Image!=null&& productUpdateDto.Image.Length>0)
            {
                imagePath= await SaveImageAsync(productUpdateDto.Image, webRootPath);
            }
            try
            {
                if(productUpdateDto is null)
                {
                    response.Message="Invalid product data provided";
                    response.Success=false;
                    return response;

                }
                var existProduct = await _productRepo.GetProductById(productUpdateDto.Id);
                if(existProduct == null)
                {
                    response.Message="product does not exist .";
                    response.Success=false;
                    return response;
                }
                var product =  _mapper.Map(productUpdateDto, existProduct);
                product.ImageUrl=imagePath;
                await _productRepo.SaveChanges();
                response.Message = "Product updated successfully.";
                response.Data = true;
            }
            catch(Exception ex)
            {
                response.Success=false;
                response.Message=$"An error occurred while updating the product : {ex.Message}";

            }
            return response;

        }
        public async Task SaveChanges()
        {
           await _productRepo.SaveChanges();
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


    }
}
