using OwlRestaurant.Services.ProductAPI.DTOs;

namespace OwlRestaurant.Services.ProductAPI.Abstractions.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDTO>> GetProducts();

    Task<ProductDTO> GetProductById(Guid productId);

    Task<ProductDTO> CreateUpdateProduct(ProductDTO product);

    Task<bool> DeleteProduct(Guid productId);
}
