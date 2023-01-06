namespace OwlRestaurant.WebApp.DTOs;

public class CartHeaderDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
    public double OrderTotal { get; set; }
}
