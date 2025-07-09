using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.ReviewDto
{
    public class ReviewReadDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
