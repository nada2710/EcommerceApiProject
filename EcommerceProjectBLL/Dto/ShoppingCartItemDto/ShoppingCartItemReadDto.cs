using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.ShoppingCartItemDto
{
    public class ShoppingCartItemReadDto
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
    }
}
