using EcommerceProjectBLL.Dto.PaymentDto;
using EcommerceProjectBLL.HandlerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.PaymentManager
{
    public interface IPaymentManger
    {
        Task<ServiceResponse<PaymentResponseDto>> CreatePaymentIntentAsync(int orderId, decimal amount);
        Task<ServiceResponse<ConfirmPaymentDto>> ConfirmPaymentAsync(string paymentIntentId);
        //Task CreatePaymentIntentAsync(int orderId);
    }
}
