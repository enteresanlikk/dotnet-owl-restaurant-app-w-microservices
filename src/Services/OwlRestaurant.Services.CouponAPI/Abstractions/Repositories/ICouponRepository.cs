using OwlRestaurant.Services.CouponAPI.DTOs;

namespace OwlRestaurant.Services.CouponAPI.Abstractions.Repositories;

public interface ICouponRepository
{
    Task<CouponDTO> GetCouponByCode(string couponCode);
}
