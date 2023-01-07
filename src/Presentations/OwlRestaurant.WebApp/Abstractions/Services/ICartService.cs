using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Abstractions.Services;

public interface ICartService
{
    Task<T> GetCartByUserIdAsync<T>(Guid userId);
    Task<T> CreateCartAsync<T>(CartDTO cartDTO);
    Task<T> UpdateCartAsync<T>(CartDTO cartDTO);
    Task<T> DeleteFromCartAsync<T>(Guid cardId);
    Task<T> ApplyCoupon<T>(CartDTO cartDTO);
    Task<T> RemoveCoupon<T>(Guid userId);
}
