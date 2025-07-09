using AutoMapper;
using EcommerceProjectBLL.Dto.OrderItemDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.OrderItemAutoMapper
{
    public class OrderItemMappingProfile:Profile
    {
        public OrderItemMappingProfile()
        {
            CreateMap<OrderItemAddDto, OrderItem>();
            CreateMap<OrderItem, OrderItemReadDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
    .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));
            CreateMap<OrderItem, OrderItemUpdateDto>().ReverseMap();
        }
    }
}
 