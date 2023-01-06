namespace OwlRestaurant.Services.ShoppingCartAPI.DTOs;

public class CartDTO
{
    public CartHeaderDTO CartHeader { get; set; }
    public IEnumerable<CartDetailDTO> CartDetails { get; set; }
}
