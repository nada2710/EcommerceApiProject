using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.PaymentDto
{
    public class ConfirmPaymentDto
    {
        [Required]
        public string PaymentIntentId { get; set; }
    }
}
