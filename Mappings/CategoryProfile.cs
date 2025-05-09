using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProductsApi.DTOs.Requests;
using ProductsApi.DTOs.Responses;
using ProductsApi.Models;

namespace ProductsApi.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            //Request to Model
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            //Model to Response
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => p.Name).ToList()));
        }
    }
}