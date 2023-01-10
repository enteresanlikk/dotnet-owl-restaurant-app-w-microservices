using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlRestaurant.Services.OrderAPI.Messages;

public class CartDetailDTO
{
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    public Guid ProductId { get; set; }
    public virtual ProductDTO Product { get; set; }
    public int Count { get; set; }
}
