using AutoMapper;
using Azure;
using EcommerceProjectBLL.Dto.PaymentDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectDAL.Repository.OrderRepo;
using EcommerceProjectDAL.Repository.PaymentRepo;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.PaymentManager
{
    public class PaymentManger :IPaymentManger
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public PaymentManger(IPaymentRepo paymentRepo,IOrderRepo orderRepo, IConfiguration configuration,IMapper mapper)
        {
            _paymentRepo=paymentRepo;
            _orderRepo=orderRepo;
            _configuration = configuration;
            _mapper=mapper;
        }
        public async Task<ServiceResponse<ConfirmPaymentDto>> ConfirmPaymentAsync(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var response = new ServiceResponse<ConfirmPaymentDto>();

            try
            {
                var service = new PaymentIntentService();
                var intent = await service.ConfirmAsync(paymentIntentId);  

                if (intent is not null)
                {
                    response.Data = _mapper.Map<ConfirmPaymentDto>(intent); 
                    response.Message = "Payment confirmed successfully.";
                    return response;
                }

                response.Success = false;
                response.Message = "Payment confirmation failed.";
                return response;
            }
            catch (StripeException e)
            {
                response.Success = false;
                response.Message = $"Stripe Error: {e.StripeError.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Server-side Error: {ex.Message}";
                return response;
            }
        }
        public async Task<ServiceResponse<PaymentResponseDto>> CreatePaymentIntentAsync(int orderId, decimal amount)
        {
            var response = new ServiceResponse<PaymentResponseDto>();
            var order = await _orderRepo.GetOrderId(orderId);
            if(order is null)
            {
                response.Success=false;
                response.Message="Order not found";
                return response;
            }
            if (amount < 0.5m)
            {
                response.Success=false;
                response.Message="Amount must be at least $0.50";
                return response;
            }
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true, AllowRedirects = "never" }
            };
            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            var payment = new Payment
            {
                TransactionId = intent.Id,
                OrderId = order.Id,
                Amount = amount,
                Status = PaymentStatus.Pending,
                Notes = "Stripe intent created",
            };
            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveChangesAsync();
            response.Data = new PaymentResponseDto
            {
                ClientSecret = intent.ClientSecret,
                 PaymentIntentId= intent.Id
            };
            response.Message ="success";
            return response;
        }
    }
}
