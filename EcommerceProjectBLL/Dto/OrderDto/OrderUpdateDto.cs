using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.OrderDto
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public string ShippingAddress { get; set; }
        public int? PaymentId { get; set; }
    }
}
