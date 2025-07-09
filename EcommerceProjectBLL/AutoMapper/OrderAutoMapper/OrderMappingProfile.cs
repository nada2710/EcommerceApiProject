using AutoMapper;
using EcommerceProjectBLL.Dto.OrderDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.OrderAutoMapper
{
    public class OrderMappingProfile:Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderAddDto, Order>();
            CreateMap<Order, OrderReadDto>();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();
        }
    }
}
