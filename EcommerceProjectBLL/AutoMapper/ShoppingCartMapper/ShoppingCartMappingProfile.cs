using AutoMapper;
using EcommerceProjectBLL.Dto.ShoppingCartDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.ShoppingCartMapper
{
    public class ShoppingCartMappingProfile :Profile
    {
        public ShoppingCartMappingProfile()
        {
            CreateMap<ShoppingCartAddDto,ShoppingCart>();
            CreateMap<ShoppingCart, ShoppingCartReadDto>().ReverseMap();
        }

    }
}
