using Newtonsoft.Json;
using OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;

namespace OwlRestaurant.Services.ShoppingCartAPI.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _httpClient;

    public CouponRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CouponDTO> GetCoupon(string couponCode)
    {
        var httpResponse = await _httpClient.GetAsync($"/api/coupons/{couponCode}");
        var content = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ResponseDTO>(content);

        if (response is not null && response.Success)
        {
            return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Data));
        }
        return new CouponDTO();
    }
}
