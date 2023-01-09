using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;
using OwlRestaurant.WebApp.Models;
using static OwlRestaurant.WebApp.SD;

namespace OwlRestaurant.WebApp.Services;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public CartService(IBaseService baseService, IConfiguration configuration)
    {
        _baseService = baseService;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ServiceURLs:ShoppingCartAPI"];
    }

    public async Task<T> ApplyCoupon<T>(CartDTO cartDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts/apply-coupon",
            RequestType = RequestType.POST,
            Data = cartDTO
        });
    }
    
    public async Task<T> Checkout<T>(CartHeaderDTO cartHeaderDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts/checkout",
            RequestType = RequestType.POST,
            Data = cartHeaderDTO
        });
    }

    public async Task<T> CreateCartAsync<T>(CartDTO cartDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts",
            RequestType = RequestType.POST,
            Data = cartDTO
        });
    }

    public async Task<T> DeleteFromCartAsync<T>(Guid cardId)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts/{cardId}",
            RequestType = RequestType.DELETE
        });
    }

    public async Task<T> GetCartByUserIdAsync<T>(Guid userId)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts/{userId}",
            RequestType = RequestType.GET
        });
    }

    public async Task<T> RemoveCoupon<T>(Guid userId)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts/remove-coupon",
            RequestType = RequestType.DELETE,
            Data = userId
        });
    }

    public async Task<T> UpdateCartAsync<T>(CartDTO cartDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/carts",
            RequestType = RequestType.PUT,
            Data = cartDTO
        });
    }
}
