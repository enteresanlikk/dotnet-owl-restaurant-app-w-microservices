namespace OwlRestaurant.Services.CouponAPI.DTOs;

public class CouponDTO
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public double DiscountAmount { get; set; }
}
