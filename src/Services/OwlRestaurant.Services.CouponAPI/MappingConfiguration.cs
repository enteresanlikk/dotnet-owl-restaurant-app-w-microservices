using AutoMapper;
using OwlRestaurant.Services.CouponAPI.DTOs;
using OwlRestaurant.Services.CouponAPI.Models;

namespace OwlRestaurant.Services.CouponAPI;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Coupon, CouponDTO>().ReverseMap();
    }
}
