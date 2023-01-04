using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;
using OwlRestaurant.WebApp.Models;
using static OwlRestaurant.WebApp.SD;

namespace OwlRestaurant.WebApp.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> CreateProductAsync<T>(ProductDTO productDTO)
    {
        return await SendAsync<T>(new APIRequest()
        {
            Url = $"{SD.ProductAPIBase}/api/products",
            RequestType = RequestType.POST,
            Data = productDTO
        });
    }

    public async Task<T> DeleteProductAsync<T>(Guid id)
    {
        return await SendAsync<T>(new APIRequest()
        {
            Url = $"{SD.ProductAPIBase}/api/products/{id}",
            RequestType = RequestType.DELETE
        });
    }

    public async Task<T> GetProductByIdAsync<T>(Guid id)
    {
        return await SendAsync<T>(new APIRequest()
        {
            Url = $"{SD.ProductAPIBase}/api/products/{id}",
            RequestType = RequestType.GET
        });
    }

    public async Task<T> GetAllProductsAsync<T>()
    {
        return await SendAsync<T>(new APIRequest()
        {
            Url = $"{SD.ProductAPIBase}/api/products",
            RequestType = RequestType.GET
        });
    }

    public async Task<T> UpdateProductAsync<T>(ProductDTO productDTO)
    {
        return await SendAsync<T>(new APIRequest()
        {
            Url = $"{SD.ProductAPIBase}/api/products",
            RequestType = RequestType.PUT,
            Data = productDTO
        });
    }
}
