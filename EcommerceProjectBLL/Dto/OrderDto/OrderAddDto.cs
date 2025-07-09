using EcommerceProjectBLL.Dto.OrderItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.OrderDto
{
    public class OrderAddDto
    {
        public string CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        //public List<OrderItemAddDto> OrderItems { get; set; } = new();
    }
}
