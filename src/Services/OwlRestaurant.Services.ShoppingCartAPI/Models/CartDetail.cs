using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlRestaurant.Services.ShoppingCartAPI.Models;

public class CartDetail
{
    [Key]
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public virtual CartHeader CartHeader { get; set; }

    public Guid ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    public int Count { get; set; }
}
