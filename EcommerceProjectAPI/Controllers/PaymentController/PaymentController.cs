using EcommerceProjectBLL.Dto.PaymentDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.PaymentManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe.V2;

namespace EcommerceProjectAPI.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentManger _paymentManger;

        public PaymentController(IPaymentManger paymentManger)
        {
            _paymentManger=paymentManger;
        }
        [HttpPost("create-intent/{orderId}")]
        public async Task <ActionResult<ServiceResponse<PaymentResponseDto>>> CreateIntent(int orderId,decimal amount)
        {
            var result = await _paymentManger.CreatePaymentIntentAsync(orderId, amount);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("confirmpayment")]
        public async Task<ActionResult<ServiceResponse<ConfirmPaymentDto>>> ConfirmPayment(ConfirmPaymentDto dto)
        {
            var result =await _paymentManger.ConfirmPaymentAsync(dto.PaymentIntentId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
