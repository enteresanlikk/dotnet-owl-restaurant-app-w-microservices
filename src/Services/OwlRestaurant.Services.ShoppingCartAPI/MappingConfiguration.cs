using AutoMapper;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;
using OwlRestaurant.Services.ShoppingCartAPI.Models;

namespace OwlRestaurant.Services.ShoppingCartAPI;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Cart, CartDTO>().ReverseMap();
        CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
        CreateMap<CartDetail, CartDetailDTO>().ReverseMap();
    }
}
