using OwlRestaurant.WebApp.DTOs;

namespace OwlRestaurant.WebApp.Abstractions.Services;

public interface IProductService
{
    Task<T> GetAllProductsAsync<T>();
    Task<T> GetProductByIdAsync<T>(Guid id);
    Task<T> CreateProductAsync<T>(ProductDTO productDTO);
    Task<T> UpdateProductAsync<T>(ProductDTO productDTO);
    Task<T> DeleteProductAsync<T>(Guid id);
}
