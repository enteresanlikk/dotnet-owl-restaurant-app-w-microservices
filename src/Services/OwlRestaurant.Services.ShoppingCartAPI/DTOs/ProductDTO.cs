using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlRestaurant.Services.ShoppingCartAPI.DTOs;

public class ProductDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryName { get; set; }
}
