using AutoMapper;
using OwlRestaurant.Services.ProductAPI.DTOs;
using OwlRestaurant.Services.ProductAPI.Models;

namespace OwlRestaurant.Services.ProductAPI;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
