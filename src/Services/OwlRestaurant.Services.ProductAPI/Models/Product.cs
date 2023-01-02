using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.Services.ProductAPI.Models;

public class Product : BaseEntity
{
    [Required]
    public string Name { get; set; }

    [Range(1, 100000)]
    public double Price { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public string CategoryName { get; set; }
}
