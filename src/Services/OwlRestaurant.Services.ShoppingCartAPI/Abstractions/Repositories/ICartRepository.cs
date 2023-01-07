using OwlRestaurant.Services.ShoppingCartAPI.DTOs;

namespace OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;

public interface ICartRepository
{
    Task<CartDTO> GetCartByUserIdAsync(Guid userId);
    Task<CartDTO> CreateUpdateCartAsync(CartDTO cartDTO);
    Task<bool> RemoveFromCartAsync(Guid cartDetailId);
    Task<bool> ClearCartByUserIdAsync(Guid userId);
    Task<bool> ApplyCoupon(Guid userId, string couponCode);
    Task<bool> RemoveCoupon(Guid userId);
}
