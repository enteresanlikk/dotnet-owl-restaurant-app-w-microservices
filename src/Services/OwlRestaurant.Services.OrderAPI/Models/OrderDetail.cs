using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlRestaurant.Services.OrderAPI.Models;

public class OrderDetail
{
    [Key]
    public Guid Id { get; set; }
    public Guid OrderHeaderId { get; set; }

    [ForeignKey("OrderHeaderId")]
    public virtual OrderHeader CartHeader { get; set; }
    public Guid ProductId { get; set; }
    public int Count { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
