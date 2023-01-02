using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.Services.ProductAPI.Models;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.Empty;
}
