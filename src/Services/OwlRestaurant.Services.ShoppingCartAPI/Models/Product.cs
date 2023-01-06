using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlRestaurant.Services.ShoppingCartAPI.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.Empty;
    
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
