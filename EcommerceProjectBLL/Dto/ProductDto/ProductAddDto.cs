using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.ProductDto
{
    public class ProductAddDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}
