using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.ProductManager
{
    public interface IProductManger
    {
        Task<ServiceResponse<List<ProductReadDto>>> GetAllProducts();
        Task<ServiceResponse<ProductReadDto>> GetProductById(int id);
        Task<ServiceResponse<ProductReadDto>> AddProduct(ProductAddDto productAddDto,string webRootPath);
        Task<ServiceResponse<bool>> UpdateProduct(ProductUpdateDto productUpdateDto, string webRootPath);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
        Task<ServiceResponse<List<ProductReadDto>>> GetProductsByCategoryId(int categoryId);

        Task SaveChanges();
    }
}
