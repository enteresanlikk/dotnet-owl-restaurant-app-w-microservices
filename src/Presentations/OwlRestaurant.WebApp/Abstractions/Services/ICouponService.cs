using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Abstractions.Services;

public interface ICouponService
{
    Task<T> GetCoupon<T>(string couponCode);
}
