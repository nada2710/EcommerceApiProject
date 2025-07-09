using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.ReviewDto
{
    public class CreateReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
    }
}
