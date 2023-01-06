using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.Services.ShoppingCartAPI.DTOs;

public class CartHeaderDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
}
