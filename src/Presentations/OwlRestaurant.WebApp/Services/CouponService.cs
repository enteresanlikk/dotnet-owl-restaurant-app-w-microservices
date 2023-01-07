using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.Models;
using static OwlRestaurant.WebApp.SD;

namespace OwlRestaurant.WebApp.Services;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public CouponService(IBaseService baseService, IConfiguration configuration)
    {
        _baseService = baseService;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ServiceURLs:CouponAPI"];
    }
    
    public async Task<T> GetCoupon<T>(string couponCode)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/coupons/{couponCode}",
            RequestType = RequestType.GET
        });
    }
}
