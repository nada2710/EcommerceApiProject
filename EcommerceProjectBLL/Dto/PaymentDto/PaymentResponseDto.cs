using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.PaymentDto
{
    public class PaymentResponseDto
    {
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
