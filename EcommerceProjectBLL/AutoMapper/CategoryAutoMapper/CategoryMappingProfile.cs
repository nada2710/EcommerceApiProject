using AutoMapper;
using EcommerceProjectBLL.Dto.CategoryDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.CategoryAutoMapper
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryAddDto,Category>()
                .ForMember(dest=>dest.ImageUrl,opt=>opt.Ignore());
            CreateMap<Category, CategoryReadDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

        }
    }
}
