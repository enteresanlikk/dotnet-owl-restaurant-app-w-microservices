using System.ComponentModel.DataAnnotations;

namespace OwlRestaurant.Services.CouponAPI.Models;

public class Coupon
{
    [Key]
    public Guid Id { get; set; }
    public string Code { get; set; }
    public double DiscountAmount { get; set; }
}
