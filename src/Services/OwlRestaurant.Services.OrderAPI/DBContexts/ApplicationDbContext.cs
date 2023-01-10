using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.OrderAPI.Models;

namespace OwlRestaurant.Services.OrderAPI.DBContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
