using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.Services.ShoppingCartAPI.Models;

public class CartHeader
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
}
