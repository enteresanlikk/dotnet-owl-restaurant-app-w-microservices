using OwlRestaurant.Integration.MessageBus;
using OwlRestaurant.Services.ShoppingCartAPI.DTOs;

namespace OwlRestaurant.Services.ShoppingCartAPI.Messages;

public class CheckoutHeaderDTO : BaseMessage
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
    public double OrderTotal { get; set; }

    public double DiscountTotal { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime PickupDateTime { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string ExpiryMonthYear { get; set; }
    public int TotalItems { get; set; }
    public IEnumerable<CartDetailDTO> CartDetails { get; set; }
}
