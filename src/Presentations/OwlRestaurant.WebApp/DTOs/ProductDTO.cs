using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.WebApp.DTOs;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryName { get; set; }
}
