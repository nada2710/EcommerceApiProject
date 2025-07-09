using EcommerceProjectBLL.Dto.OrderItemDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.OrderDto
{
    public class OrderReadDto
    {
        public DateTime CreatedBy { get; set; }
        public Status Status { get; set; }
        public string ShippingAddress { get; set; }
        public string StatusAsString => Enum.GetName(typeof(Status), Status);
        public ICollection<OrderItemReadDto> orderItems { get; set; } = new HashSet<OrderItemReadDto>();
        public decimal TotalAmount { get;set; }
        public string CustomerId { get; set; }
        public int PaymentId { get; set; }
    }
}
