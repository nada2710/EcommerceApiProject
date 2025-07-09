using AutoMapper;
using EcommerceProjectBLL.Dto.ReviewDto;
using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.AutoMapper.ReviewAutoMapper
{
    public class ReviewMappingProfile:Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Review, UpdateReviewDto>().ReverseMap();
            CreateMap<Review, ReviewReadDto>().ReverseMap();
        }
    }
}
