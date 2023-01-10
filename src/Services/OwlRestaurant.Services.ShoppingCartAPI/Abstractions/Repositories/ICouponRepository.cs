using OwlRestaurant.Services.ShoppingCartAPI.DTOs;

namespace OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;


public interface ICouponRepository
{
    Task<CouponDTO> GetCoupon(string couponCode);
}
