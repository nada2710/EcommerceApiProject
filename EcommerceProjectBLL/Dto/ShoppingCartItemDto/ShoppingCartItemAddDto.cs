using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.ShoppingCartItemDto
{
    public class ShoppingCartItemAddDto
    {
        public int Quantity { get; set; }
       // public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public int ShoppingCartId { get; set; }

    }
}
