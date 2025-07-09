using AutoMapper;
using EcommerceProjectBLL.Dto.PaymentDto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.PaymentAutoMapper
{
    public class AutoMapperMappingProfile :Profile
    {
        public AutoMapperMappingProfile()
        {
            // Mapping from PaymentIntent (Stripe response) to AddIntentPaymentResponseDto
            CreateMap<PaymentIntent, ConfirmPaymentDto>()
                .ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => src.Id));
                
        }
    }
}
