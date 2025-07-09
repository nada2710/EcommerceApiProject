using AutoMapper;
using EcommerceProjectBLL.Dto.ShoppingCartItemDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.ShoppingCartItemMapper
{
    public class ShoppingCartItemMappingProfile:Profile
    {
        public ShoppingCartItemMappingProfile()
        {
            CreateMap<ShoppingCartItemAddDto, ShoppingCartItem>();
            CreateMap<ShoppingCartItem, ShoppingCartItemReadDto>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemUpdateDto>().ReverseMap();
        }
    }
}
