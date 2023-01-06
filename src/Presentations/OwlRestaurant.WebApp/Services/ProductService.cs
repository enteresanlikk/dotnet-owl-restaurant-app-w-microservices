using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;
using OwlRestaurant.WebApp.Models;
using static OwlRestaurant.WebApp.SD;

namespace OwlRestaurant.WebApp.Services;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public ProductService(IBaseService baseService, IConfiguration configuration)
    {
        _baseService = baseService;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ServiceURLs:ProductAPI"];
    }

    public async Task<T> CreateProductAsync<T>(ProductDTO productDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/products",
            RequestType = RequestType.POST,
            Data = productDTO
        });
    }

    public async Task<T> DeleteProductAsync<T>(Guid id)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/products/{id}",
            RequestType = RequestType.DELETE
        });
    }

    public async Task<T> GetProductByIdAsync<T>(Guid id)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/products/{id}",
            RequestType = RequestType.GET
        });
    }

    public async Task<T> GetAllProductsAsync<T>()
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/products",
            RequestType = RequestType.GET
        });
    }

    public async Task<T> UpdateProductAsync<T>(ProductDTO productDTO)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            Url = $"{_apiBaseUrl}/api/products",
            RequestType = RequestType.PUT,
            Data = productDTO
        });
    }
}
