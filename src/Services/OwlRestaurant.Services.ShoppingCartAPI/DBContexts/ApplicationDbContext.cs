using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.ShoppingCartAPI.Models;

namespace OwlRestaurant.Services.ShoppingCartAPI.DBContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
