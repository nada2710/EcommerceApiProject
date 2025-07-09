using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.OrderItemDto
{
    public class OrderItemAddDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string CustomerId { get; set; }
    }
}
