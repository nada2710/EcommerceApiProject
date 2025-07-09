using EcommerceProjectBLL.Dto.ProductDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.OrderItemDto
{
    public class OrderItemReadDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => ProductPrice * Quantity;
        //public int Quantity { get; set; }
        //// public decimal GetTotalPrice =>Quantity * ProductPrice ;
        //public decimal TotalPrice { get; set; }
        //public int OrderId { get; set; }
        //public int ProductId { get; set; }
        //public decimal ProductPrice { get; set; }
    }
}
